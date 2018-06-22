// Adapted from extant sizeup code

var request = require('request');

var util = require('./util');


module.exports = function makeGetData(apiKey) {
  // Factory function (encapsulating an authentication context per apiKey) that
  // returns a function that gets data from the SizeUp data API via JSONP, authenticating as needed


  var me = {};
  me.sessionId  = null;
  me.apiToken   = null;
  me.instanceId = null;
  me.widgetToken = '';
  me.callbackComplete = {};
  me.currentLocation = {
    protocol: 'https',
    domain: 'api.sizeup.com'
  };


  var getData = function (path, params, onSuccess, onError) {
    var opts = { aborted: false };
    function abort() {
      opts.aborted = true;  // NOTE: why not null the success and error functions so they may be GC'd?
    }

    var r;
    if (!onSuccess && !onError) {
      r = new Promise(function(resolve, reject) {
        onSuccess = resolve;
        onError   = reject;
      })
      r.abort = abort;
    } else {
      r = { abort:abort };    // historical
    }

    // If auth is missing or old, and not already underway ...
    if ((!me.sessionId || me.authdAt && me.authdAt + 5*60*1000 < +new Date()) && !me.getDataQueue) {
      // ... Authenticate (async); Queue requests until auth is available.
      //  Alternately client could use a callback for auth.  Burden belongs here.
      //  TODO: if we could compute auth values from the apiKey client side, none of this would be needed; but I think server must establish session.
      //  TODO: Why doesn't server just accept key on each request? How are sessions used/implemented on server?

      me.getDataQueue = [];
      authenticate()
        .then(function () {
          var q = me.getDataQueue;
          delete me.getDataQueue;

          for (var i = 0; i < q.length; i++) {  // TODO: move to es6??
            getData(q[i].path, q[i].params, q[i].onSuccess, q[i].onError);  // TODO: move to es6??
          }
        })
        .catch(function (message) {
          var q = me.getDataQueue;
          delete me.getDataQueue;

          for (var i = 0; i < q.length; i++) {  // TODO: move to es6??
            q[i].onError(message);
          }
        })
    }
    if (me.getDataQueue) {
      me.getDataQueue.push({
        path:       path,
        params:     params,
        onSuccess:  onSuccess,
        onError:    onError
      });
      return r;
    }

    // console.log("getData", path, params);
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


  var authenticate = function () {
    var authUrl = me.currentLocation.protocol + '://' + me.currentLocation.domain + '/js/?apikey=' + apiKey;
    // console.log('***** AUTH', authUrl);
    return new Promise(function (resolve, reject) {
      request(authUrl, function (error, response, body) {
        if (!error && (response||{}).statusCode == 200) {
          me.authdAt = +new Date();

          // console.log(body);
          var re = /me.(sessionId|apiToken|instanceId)\s=\s['"](.*)['"];/g;
          for (var a; a = re.exec(body); ) {
            // console.log(a.slice(1));
            me[a[1]] = a[2];
          }

          resolve();   // TODO: might be clearer to resolve with the auth'd context as argument... or not

        } else {
          reject("Error attempting to authenticate: " + (
              error
              ? "Failed to contact sizeup server: " + error
              : "Auth error (invalid key?): "
              + (response ? ("status code: "+response.statusCode+"; "+response.statusMessage) : "unknown") ) );
        }
      })
    });
  };


  var buildTokenUrl = function (src, params){
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


  return getData;
}
