(function () {
    window.sizeup = window.sizeup || {};
    window.sizeup.api = window.sizeup.api || {};

    window.sizeup.api.util = (function () {

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
    window.sizeup.api.loader = (function () {

        var me = {};
        var pub = {
            getData: function (url, params, success, error) {
                return getJsonp(url, params, success, error);
            },
            getSourceLocation: function(){
                return me.currentLocation;
            },
            buildTokenUrl: function(src, params){
                return buildTokenUrl(src, params);
            }
        };
        me.head = document.getElementsByTagName('head')[0];
        me.scriptQueue = [];
        me.callbackIndex = 0;
        me.jsonpPrefix = 'cbb';
        me.sessionId = '7kuvw29ohlwfhe25e6posgt0h';
        me.apiToken = 'utZOqvvO8s2KUngekSFKXPsZjTrtpXw6iGm4rJj6SAiCxVx/nelq3Tx/XxAWLZaK';
        // me.apiToken = 'utZOqvvO8s2KUngekSFKXGCs8Xxxh9jIHzZcuNNyuRMKMEb3ORj95aB75AH0B+dM';
        me.instanceId = '7kuvw29ohlwfhe25e6posgt0h';
        me.widgetToken = '';
        sizeup.api[me.jsonpPrefix] = {};
        me.callbackComplete = {};

        var buildTokenUrl = function(src, params){
            if (src.indexOf('?') < 0) {
                src = src + '?';
            }
            else {
                src = src + '&';
            }
            params['o'] = document.location.hostname;
            params['s'] = me.sessionId;
            params['t'] = me.apiToken;
            params['i'] = me.instanceId;
            if(me.widgetToken != ''){
                params['wt'] = me.widgetToken;
            }
            src = src + sizeup.api.util.formatParams(params);
            return src;
        };

        var getThisScript = function () {
            var scripts = document.getElementsByTagName("script");
            var index = scripts.length - 1;
            me.currentScript = scripts[index];
        };

        var getScriptLocation = function () {
            var script = me.currentScript;
            var src = script.src;
            var chop = function (str) {
                var hash = {};
                str = str.replace(' ', '');
                if (str.length > 0) {
                    var vars = str.split("&");
                    for (var i = 0; i < vars.length; i++) {
                        var pair = vars[i].split("=");
                        hash[pair[0]] = unescape(pair[1]);
                    }
                }
                return hash;
            };
            var match = src.match(/^((https?|ftp):\/)?\/?((.*?)(:(.*?)|)@)?((www\.)?([^:\/\s]+))(:([^\/]*))?((\/\w+)*\/)([-\w.]+[^#?\s]*)?(\?([^#]*))?(#(.*))?$/);
            match = match || new Array(20);
            me.currentLocation = {
                protocol: match[2],
                host: match[7],
                domain: match[9],
                port: match[11],
                path: match[12] + match[14],
                query: chop(match[16] ? match[16] : ''),
                hash: chop(match[18] ? match[18] : '')
            };
        };


        var loadScript = function (item) {
            var script = document.createElement('script');
            script.type = 'text/javascript';
            var src = me.currentLocation.protocol + '://' + me.currentLocation.host + '/' + item.url;
            script.src = buildTokenUrl(src, {});
            script.onload = script.onreadystatechange = function() {
                if ( !item.loaded && (!this.readyState || this.readyState === "loaded" || this.readyState === "complete") ) {
                    item.loaded = true;
                    ready();
                    // Handle memory leak in IE
                    script.onload = script.onreadystatechange = null;
                    if ( me.head && script.parentNode ) {
                        me.head.removeChild( script );
                    }
                }
            };
            me.head.appendChild(script);
        };

        var fillQueue = function () {
            me.scriptQueue.push({ url: '/js/data', loaded: false });
            me.scriptQueue.push({ url: '/js/granularity', loaded: false });
            me.scriptQueue.push({ url: '/js/range', loaded: false });
            me.scriptQueue.push({ url: '/js/overlay', loaded: false });
            me.scriptQueue.push({ url: '/js/overlayAttributes', loaded: false });
            me.scriptQueue.push({ url: '/js/attributes', loaded: false });
        };

        var loadQueue = function () {
            for (var x = 0; x < me.scriptQueue.length; x++) {
                loadScript(me.scriptQueue[x]);
            }
        };

        var isScriptLoadingComplete = function () {
            var complete = true;
            for (var x = 0; x < me.scriptQueue.length; x++) {
                complete = complete && me.scriptQueue[x].loaded;
            }
            return complete && me.apiToken != null && me.windowLoaded != null;
        };

        var getNextCallback = function(){
            if(me.callbackIndex > 999999){
                me.callbackIndex = 0;
            }
            me.callbackIndex ++;
            return 'cb' + me.callbackIndex;
        };

        var getJsonp = function (url, params, success, error) {
            var cb = getNextCallback();
            var opts = {
                aborted: false
            };
            var p = Math.floor((Math.random()*2) + 1);
            var script = document.createElement('script');
            sizeup.api[me.jsonpPrefix][cb] = function (data) {
                if (success && !opts.aborted) {
                    success(data);
                }
                cleanup();
            };

            script.onerror = function (e) {
                if (error && !opts.aborted) {
                    error(e);
                }
                cleanup();
            };

            var cleanup = function () {
                delete sizeup.api[me.jsonpPrefix][cb];
                me.head.removeChild(script);
            };


            script.type = 'text/javascript';
            var src = me.currentLocation.protocol + '://' + 'a' + p + '-' + me.currentLocation.domain + url;
            params['cb']  = 'sizeup.api.' + me.jsonpPrefix + '.' + cb;
            script.src = buildTokenUrl(src, params);
            me.head.appendChild(script);

            var request = {
                abort: function(){
                    opts.aborted = true;
                }
            };
            return request;
        };

        var updateToken = function (callback) {
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

        var ready = function () {
            if (isScriptLoadingComplete()) {
                window[me.currentLocation.query['callback']]();
            }
        };



        window.onload = function () { me.windowLoaded = true; ready(); }
        getThisScript();
        getScriptLocation();
        setInterval(updateToken, 1000 * 60 * 5);
        fillQueue();
        loadQueue();
        return pub;
    })();
})();
