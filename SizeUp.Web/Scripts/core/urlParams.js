(function () {
    sizeup.core.namespace('sizeup.core');
    sizeup.core.urlParams =  sizeup.core.urlParams || (function () {

        var me = {};
        var add = function (obj) {
            jQuery.bbq.pushState(obj);
        };

        var replace = function (obj) {
            jQuery.bbq.pushState(obj, 2);
        };

        var getParams = function () {
            return jQuery.bbq.getState();
        };

        var publicObj = {
            add: function (obj) {
                add(obj);
            },
            replace: function (obj) {
                replace(obj);
            },
            getParams: function () {
                return getParams();
            }
        };
        return publicObj;
    })();
})();