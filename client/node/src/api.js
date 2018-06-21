var request = require('request');

var util = require('./util');

if (!global.sizeup || !global.window.sizeup)  throw new Error("Must have global sizeup already");  // TODO deglobalize



module.exports = function (apiKey) {
  if (!apiKey)  throw new Error("Need apiKey to authenticate");

  sizeup.api.loader = (function () {

    var me = {};
    var pub = {
      getData: function (path, params, success, error) {  // NOTE used extensively by data script!
        return getJsonp(path, params, success, error);
      },
      /*
      getSourceLocation: function(){  // NOTE used only by overlay script, not data.
        return me.currentLocation;
      },
      buildTokenUrl: function(src, params){  // NOTE used only by overlay script, not data.
        return buildTokenUrl(src, params);
      }
      */
    };
    me.sessionId  = null;
    me.apiToken   = null;
    me.instanceId = null;
    me.widgetToken = '';
    me.callbackComplete = {};
    me.currentLocation = {
      protocol: 'https',
      domain: 'api.sizeup.com'
    };


    var buildTokenUrl = function(src, params){
      if (src.indexOf('?') < 0) {
        src = src + '?';
      }
      else {
        src = src + '&';
      }
      params['o'] = 'sizeup.com';  // TODO: unused??
      params['s'] = me.sessionId;
      params['t'] = me.apiToken;
      params['i'] = me.instanceId;
      if(me.widgetToken != '') {
        params['wt'] = me.widgetToken;
      }
      src = src + util.formatParams(params);
      return src;
    };


    var getJsonp = function (path, params, onSuccess, onError) {
      var opts = { aborted: false };
      function abort() {
        opts.aborted = true;  // NOTE: why not null the success and error functions so they may be GC'd?
      }

      var r;
      if (!onSuccess && !onError) {
        r = new Promise(function(resolve, reject) {  // TODO: require some Promise implementation module?
          onSuccess = resolve;
          onError   = reject;
        })
        r.abort = abort;
      } else {
        r = { abort:abort };
      }

      // Authenticate, async: Queue requests until auth is available
      //  Alternately client could use a callback for auth.  Burden belongs here.
      //  TODO: if we could compute auth values from the apiKey client side, none of this would be needed; but I think server must establish session.  Why doesn't server just accept key on each request? How are sessions used/implemented on server?
      if ((!me.sessionId || me.authdAt && me.authdAt + 5*60*1000 < +new Date()) && !me.getJsonpQueue) {
        me.getJsonpQueue = [];
        var authUrl = me.currentLocation.protocol + '://' + me.currentLocation.domain + '/js/?apikey=' + apiKey;
        // console.log('***** AUTH', authUrl);
        request(authUrl, function (error, response, body) {
          var q = me.getJsonpQueue;
          delete me.getJsonpQueue;

          if (error || !response || response.statusCode!==200) {
            var message = "Error attempting to authenticate: " + (
              error
                ? "Failed to contact sizeup server: " + error
                : "Auth error (invalid key?): "
                  + (response ? ("status code: "+response.statusCode+"; "+response.statusMessage) : "unknown") );

            for (var i = 0; i < q.length; i++) {  // TODO: discuss JS/node version support with Travis
              q[i].onError(message);
            }
          } else {
            me.authdAt = +new Date();

            // console.log(body);
            var re = /me.(sessionId|apiToken|instanceId)\s=\s['"](.*)['"];/g;
            for (var a; a = re.exec(body); ) {
              // console.log(a.slice(1));
              me[a[1]] = a[2];
            }

            for (var i = 0; i < q.length; i++) {  // TODO: discuss JS/node version support with Travis
              getJsonp(q[i].path, q[i].params, q[i].onSuccess, q[i].onError);  // TODO: discuss JS/node version support with Travis
            }
          }
        });
      }
      if (me.getJsonpQueue) {
        me.getJsonpQueue.push({
          path:       path,
          params:     params,
          onSuccess:  onSuccess,
          onError:    onError
        });
        return r;
      }

      // console.log("getJsonp", path, params);
      var serverNum = Math.floor((Math.random()*2) + 1); // NOTE: 1 or 2

      var url = me.currentLocation.protocol + '://' + 'a' + serverNum + '-' + me.currentLocation.domain + path;
      params['cb'] = 'JSONP_WRAPPER';
      var tokenUrl = buildTokenUrl(url, params);
      // NOTE: E.g., http://a2-api.sizeup.com/data/place/search/?term=fresno&maxResults=10&cb=sizeup.api.cbb.cb33&o=sizeup.com&s=1f4uhh94x0968t1x46oox9z3j&t=utZOqvvO8s2KUngekSFKXGCs8Xxxh9jIHzZcuNNyuROLRHA4MFBr%2BiqIWuk4Z39E&i=re6ktch2yfdd3wb3xocdi8zrh
      // NOTE: E.g., http://a2-api.sizeup.com/data/place/search/?term=fresno&maxResults=10
      //   &cb=CB
      //   &o=   document.location.hostname
      //   &s=   me.sessionId
      //   &t=   me.apiToken
      //   &i=   me.instanceId

      request(tokenUrl, function (error, response, body) {
        if (error || !response || response.statusCode!==200) {
          return onError(
            error    ? "Network error: " + error :
            response ? "Query failed (bad query?): "+response.statusMessage+"\n"+response.body :
            'unknown'
          );
        }

        try {
          // console.log(body);
          var unJsonpWrapped = /^JSONP_WRAPPER\((.*)\)$/.exec(body)[1];
          var result = unJsonpWrapped ? JSON.parse(unJsonpWrapped) : null;
        } catch (e) {
          return onError(e);
        }

        onSuccess(result);
      });

      return r;
    };

    return pub;
  })();
};
