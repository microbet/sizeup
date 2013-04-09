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
        me.jsonpPrefix = 'szup_' + new Date().getTime();

        var formatParams = function (params) {
            var output = '';
            var formattedParams = [];
            if (params != null) {
                for (var x in params) {
                    if (params.hasOwnProperty(x)) {
                        formattedParams.push(x + '=' + encodeURIComponent(params[x]));
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
                if (isScriptLoadingComplete()) {
                    if (window[me.currentLocation.query['callback']]) {
                        window[me.currentLocation.query['callback']]();
                    }
                }
            };
            document.head.appendChild(script);
        };

        var fillQueue = function () {
            me.scriptQueue.push({ url: '/jsapi/data.js', loaded: false });
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
            return complete && me.apiToken != null;;
        };

        var getJsonp = function (url, success, error) {
            var cb = me.jsonpPrefix + '_' + new Date().getTime();
            var script = document.createElement('script');
            script.type = 'text/javascript';
            var src = me.currentLocation.protocol + '://' + me.currentLocation.host + url;
            if (src.indexOf('?') < 0) {
                src = src + '/?';
            }
            else {
                src = src + '&';
            }
            src = src + 'jsoncallback=' + cb;
            script.src = src;
            var cleanup = function () {
                delete window[cb];
                document.head.removeChild(script);
            };

            window[cb] = function (data) {
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

        var initToken = function () {
            updateToken(function () {
                if (isScriptLoadingComplete()) {
                    if (window[me.currentLocation.query['callback']]) {
                        window[me.currentLocation.query['callback']]();
                    }
                }
            });
        };


        getThisScript();
        getScriptLocation();
        setInterval(updateToken, 1000 * 60 * 30);      
        fillQueue();
        initToken();
        loadQueue();
        return pub;
    })();
})();