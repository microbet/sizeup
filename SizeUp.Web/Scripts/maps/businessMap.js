(function () {
    sizeup.core.namespace('sizeup.maps');
    sizeup.maps.businessMap = function (opts) {

        var defaults = {
            mapSettings: sizeup.maps.mapOptions.getDefaults(),
            styles: sizeup.maps.mapStyles.getDefaults()
        };
        var dataLayer = new sizeup.core.data();
        var templates = new sizeup.core.templates();
        var me = {};
        me.opts = $.extend(true, defaults, opts);
        me.container = opts.container;
        me.overlays = [];
        me.currentOverlayIndex = null;

        var init = function () {

            me.map = new sizeup.maps.map({
                container: me.container,
                mapSettings: me.opts.mapSettings,
                styles: me.opts.styles
            });
            //buildOverlays();
            //me.map.addEventListener('zoom_changed', zoomChanged);
            //setOverlay();
        };

        var buildOverlays = function () {
            for (var x in me.opts.overlays) {
                var func = function () {
                    var opts = me.opts.overlays[x];
                    return function (point, zoom) {
                        var url = opts.tileUrl;
                        var params = {
                            industryId: opts.industryId,
                            x: point.x,
                            y: point.y,
                            zoom: zoom,
                            colors: opts.colors.join(',')
                        };
                        if (opts.boundingEntityId) {
                            params.boundingEntityId = opts.boundingEntityId;
                        }
                        return jQuery.param.querystring(url, params);
                    };
                };

                var overlay =
                {
                    legendSource: me.opts.overlays[x].legendSource,
                    legendTitle: me.opts.overlays[x].legendTitle,
                    legendFormat: me.opts.overlays[x].legendFormat,
                    minZoom: me.opts.overlays[x].minZoom,
                    maxZoom: me.opts.overlays[x].maxZoom,
                    colors: me.opts.overlays[x].colors,
                    imageMap: new google.maps.ImageMapType({
                        getTileUrl: func(),
                        tileSize: new google.maps.Size(256, 256)
                    })
                }
                me.overlays.push(overlay);
            }
        };

        var setOverlay = function () {
     

          
       
        };

       
        var addMarker = function (marker) {
            me.map.addMarker(marker);
        };

        var triggerEvent = function (event) {
            me.map.triggerEvent(event);
        };



        var zoomChanged = function () {
            setOverlay();
        };



        var publicObj = {
            getContainer: function () {
                return me.container;
            },
            addMarker: function (marker) {
                addMarker(marker);
            },
            addPin: function (pin) {

            },
            triggerEvent: function (event) {
                triggerEvent(event);
            }
        };
        init();
        return publicObj;

    };
})();

















