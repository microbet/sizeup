(function () {
    sizeup.core.namespace('sizeup.maps');
    sizeup.maps.overlay = function (opts) {



        var defaults = {
            opacity: 1
        };
        var me = {};
        me.opts = $.extend(true, defaults, opts);

        var params = {
            getTileUrl: function (point, zoom) {
                var params = $.extend({
                    x: point.x,
                    y: point.y,
                    zoom: zoom
                },
                me.opts.tileParams);
                var url = jQuery.param.querystring(me.opts.tileUrl, params);
                if (zoom > me.opts.maxZoom || zoom < me.opts.minZoom) {
                    url = null;
                }
                return url;
            },
            tileSize: new google.maps.Size(256, 256),
            opacity: me.opts.opacity
        };

        me._native = new google.maps.ImageMapType(params)

        var publicObj = {
            getNative: function () {
                return me._native;
            }
        };
        return publicObj;

    };
})();

