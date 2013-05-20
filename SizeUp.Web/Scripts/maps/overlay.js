﻿(function () {
    sizeup.core.namespace('sizeup.maps');
    sizeup.maps.overlay = function (opts) {



        var defaults = {
            opacity: 1
        };
        var me = {};
        me.opts = $.extend(true, defaults, opts);
        var pointer = 0;
        var params = {
            getTileUrl: function (point, zoom) {
                var params = $.extend({
                    x: point.x,
                    y: point.y,
                    zoom: zoom
                },
                me.opts.tileParams);
                if (pointer == 2) {
                    pointer = 0;
                }
                pointer++;
                var urlBase = window.location.protocol + '//t' + pointer + '.api.' + window.location.hostname.replace('www.', '') + '/';
                jQuery.ajaxSettings.traditional = true;
                var url = jQuery.param.querystring(urlBase + me.opts.tileUrl, params);
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

