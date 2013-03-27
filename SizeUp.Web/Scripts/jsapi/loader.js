(function () {
    window.sizeup = window.sizeup || {};
    window.sizeup.api = window.sizeup.api || {};
    window.sizeup.api.loader = (function () {

        var me = {};
        var pub = {};

        var load = function (url) {    
            var script = document.createElement('script');
            script.type = 'text/javascript';
            script.src = url;

            script.onload = function () {

            };

            script.onerror = function () {

            };
            document.head.appendChild(script);
        };


        return pub;
    })();
})();