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
                var url = new sizeup.api.tiles.overlay(me.opts.tileParams, me.opts.attribute).getTileUrl(point, zoom);
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

