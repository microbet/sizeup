(function () {
    window.sizeup = window.sizeup || {};
    window.sizeup.api = window.sizeup.api || {};
    window.sizeup.api.data = function (opts) {
        
        var prefix = 'szup_' + new Date().getTime();
        var me = {};       
        var init = function () {
            me.scriptContainer = document.createElement('div');
            me.scriptContainer.style["display"] = 'none';
            document.body.appendChild(me.scriptContainer);
        };

        var getData = function (url, success, error) {
            var cb = prefix + '_' + new Date().getTime();
            var script = document.createElement('script');
            script.type = 'text/javascript';
            script.src = url + '&jsoncallback=' + cb;

            var cleanup = function () {
                delete window[cb];
                me.scriptContainer.removeChild(script);
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
            me.scriptContainer.appendChild(script);
        };

        var publicObj = {
            get: function (url, success, error) {
                getData(url, success, error);
            }
        };
        init();
        return publicObj;
    };
})();