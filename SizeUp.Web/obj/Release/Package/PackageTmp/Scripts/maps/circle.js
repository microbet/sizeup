(function () {
    sizeup.core.namespace('sizeup.maps');
    sizeup.maps.circle = function (opts) {

        var defaults = {

        };
        var me = {};
        me.opts = $.extend(true, defaults, opts);
        
        me._native = new google.maps.Circle({ center: me.opts.center.getNative(), radius: me.opts.radius, strokeColor: me.opts.strokeColor, strokeWeight: me.opts.strokeWeight, strokeOpacity: me.opts.strokeOpacity, fillOpacity: me.opts.fillOpacity });

        var publicObj = {
            getNative: function () {
                return me._native;
            }
        };
        return publicObj;

    };
})();

















