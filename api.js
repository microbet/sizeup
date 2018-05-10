var request = require('request');

module.exports = function () {
    global.sizeup = {};  // Yikes! NOTE: CONSIDER REVISING: loaded scripts depend on sizeup being global, or at least otherwise injected
	global.window = {sizeup:sizeup};  // see?
    // TODO: if we modify data.js we needn't use global.sizeup, BUT then we can't use fresh and unmodifief data.js in the future
    sizeup.api = {};  // yep: that's global.sizeup

    require('./data');  // installs window.sizeup.api.data

    sizeup.api.util = (function () {

        var me = {};

        var mergeParams = function(primary, secondary){
            var obj = {};
            primary = primary || {};
            secondary = secondary || {};
            for (var x in primary) {
                    if (primary.hasOwnProperty(x)) {
                        obj[x] = primary[x];
                    }
            }

            for (var x in secondary) {
                    if (secondary.hasOwnProperty(x)) {
                        obj[x] = secondary[x];
                    }
            }
            return obj;
        };

        var formatParams = function (params) {
            var output = '';
            var formattedParams = [];
            if (params != null) {
                for (var x in params) {
                    if (params.hasOwnProperty(x)) {
                        if (Object.prototype.toString.call(params[x]) === '[object Array]') {
                            for (var y = 0; y < params[x].length; y++) {
                                formattedParams.push(x + '=' + encodeURIComponent(params[x][y]));
                            }
                        }
                        else if (params[x] && params[x]._type && params[x]._type() == 'sizeup.api.range') {
                            var p = [params[x].min(), params[x].max()];
                            for (var y = 0; y < p.length; y++) {
                                formattedParams.push(x + '=' + encodeURIComponent(p[y]));
                            }
                        }
                        else {
                            formattedParams.push(x + '=' + encodeURIComponent(params[x]));
                        }
                    }
                }
                if (formattedParams.length > 0) {
                    output = formattedParams.join('&');
                }
            }
            return output;
        };

        var wrapAsArray = function(val){
            if( Object.prototype.toString.call( val ) === '[object Array]' ) {
                return val;
            }
            else{
                return [val];
            }
        };

        var pub = {
            mergeParams: function(primary, secondary){
                return mergeParams(primary, secondary);
            },
            formatParams: function(params){
                return formatParams(params);
            },
            wrapAsArray: function(val){
                return wrapAsArray(val);
            }
        };
        return pub;
    })();

///////////////////////////////////////////////////////////////////////////////////////////////
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
        me.sessionId = 'winr345x7i4k0mg7jt5khtltg';  // TODO
        me.apiToken = 'utZOqvvO8s2KUngekSFKXHQ7vmhVazr8RBY+Ve3SfvJXMUHoUypcTNho6W4DyJdY'; // TODO
        me.instanceId = '2gmexw5dasdck5m2bcvhclwby'; // TODO
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
            if(me.widgetToken != ''){
                params['wt'] = me.widgetToken;
            }
            src = src + sizeup.api.util.formatParams(params);
            return src;
        };


        var getJsonp = function (path, params, onSuccess, onError) {
            var opts = { aborted: false };
            var p = Math.floor((Math.random()*2) + 1); // NOTE: 1 or 2

            var url = me.currentLocation.protocol + '://' + 'a' + p + '-' + me.currentLocation.domain + path;
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
                    return onError(error || response);
                }

                try {
                    var result = JSON.parse(/^JSONP_WRAPPER\((.*)\)$/.exec(body)[1]);
                } catch (e) {
                    return onError(e);
                }

                onSuccess(result);
            });

            return {
                abort: function(){
                    opts.aborted = true;  // NOTE: why not null the success and error functions so they may be GC'd?
                }
            };;
        };

        /*
        var updateToken = function (callback) {
			// NOTE http://a2-api.sizeup.com/token?cb=sizeup.api.cbb.cb34&o=sizeup.com&s=1f4uhh94x0968t1x46oox9z3j&t=utZOqvvO8s2KUngekSFKXGCs8Xxxh9jIHzZcuNNyuROLRHA4MFBr%2BiqIWuk4Z39E&i=re6ktch2yfdd3wb3xocdi8zrh
            getJsonp('/token',{} , function (data) {
                me.apiToken = data;
                if (callback) {
                    callback();
                }
            },
            function(){
                location.reload();
            });
        };
        */
        // setInterval(updateToken, 1000 * 60 * 5);

        return pub;
    })();
};
