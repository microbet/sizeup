(function () {
    sizeup.core.namespace('sizeup.maps');
    sizeup.maps.latLng = function (opts) {

        var defaults = {
            
        };
        var me = {};
        me.opts = $.extend(true, defaults, opts);
        me.latLng = new google.maps.LatLng(parseFloat(me.opts.lat), parseFloat(me.opts.lng));
        me._native = me.latLng;

        var publicObj = {
            getNative: function () {
                return me._native;
            }
        };
        return publicObj;

    };
})();

















