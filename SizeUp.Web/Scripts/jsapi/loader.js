(function () {
    window.sizeup = window.sizeup || {};
    window.sizeup.api = window.sizeup.api || {};
    window.sizeup.api.loader = (function () {

        var me = {};
        var pub = {
            getData: function (url, params, success, error) {
                if (url.indexOf('?') < 0) {
                    url = url + '/?';
                }
                else {
                    url = url + '&';
                }
                params.apitoken = me.apiToken;
                url = url + formatParams(params);
                getJsonp(url, success, error);
            }
        };
        me.scriptQueue = [];
        me.callbackIndex = 0;
        me.domainIndex = 0;
        me.jsonpPrefix = 'szup_' + Math.floor(Math.random() * 999999);
        sizeup.api[me.jsonpPrefix] = {};
        me.callbackComplete = {};
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
                        else if (params[x]._type && params[x]._type() == 'sizeup.api.range') {
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
            script.src = me.currentLocation.protocol + '://' + me.currentLocation.host + '/scripts' + item.url;
            script.onload = function () {
                item.loaded = true;
                ready();
            };
            document.head.appendChild(script);
        };

        var fillQueue = function () {
            me.scriptQueue.push({ url: '/jsapi/data.js', loaded: false });
            me.scriptQueue.push({ url: '/jsapi/granularity.js', loaded: false });
            me.scriptQueue.push({ url: '/jsapi/range.js', loaded: false });
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

        var getJsonp = function (url, success, error) {
            var cb = getNextCallback();
            if (me.domainIndex > 2) {
                me.domainIndex = 0;
            }
            me.domainIndex++;
            var script = document.createElement('script');
            sizeup.api[me.jsonpPrefix][cb] = function (data) {
                if (success) {
                    success(data);
                }
                cleanup();
            };

            script.onerror = function () {
                if (error) {
                    error();
                }
                cleanup();
            };

            var cleanup = function () {
                delete sizeup.api[me.jsonpPrefix][cb];
                document.head.removeChild(script);
            };


            script.type = 'text/javascript';
            var src = me.currentLocation.protocol + '://' + 'api' + me.domainIndex + '.' + me.currentLocation.domain + url;
            if (src.indexOf('?') < 0) {
                src = src + '/?';
            }
            else {
                src = src + '&';
            }
            src = src + 'jsoncallback=' + 'sizeup.api.' + me.jsonpPrefix + '.' + cb + '&origin=' + document.location.hostname + '&s=' + me.jsonpPrefix;
            script.src = src;
            document.head.appendChild(script);
        };

        var updateToken = function (callback) {
            getJsonp('/api/token?apikey=' + me.currentLocation.query['apikey'], function (data) {
                me.apiToken = data;
                if (callback) {
                    callback();
                }
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
        setInterval(updateToken, 1000 * 60 * 30);
        fillQueue();
        updateToken(function () { ready(); });
        loadQueue();
        return pub;
    })();
})();