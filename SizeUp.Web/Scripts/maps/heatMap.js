(function () {
    sizeup.core.namespace('sizeup.maps');
    sizeup.maps.heatMap = function (opts) {

        var defaults = {
            mapSettings: sizeup.maps.mapOptions.getDefaults(),
            styles: sizeup.maps.mapStyles.getDefaults(),
            colors: [],
            overlays:[]
        };
        var dataLayer = new sizeup.core.data();
        var me = {};
        me.opts = $.extend(true, defaults, opts);
        me.container = opts.container;
        me.overlays = [];
        me.currentOverlayIndex=null;

        var init = function () {

            me.legend = me.container.find('.legendContainer');
            me.legend.parent().hide();
            me.title = me.container.find('.title').hide();
            me.map = new sizeup.maps.map({
                container: me.container.find('.container'),
                mapSettings: me.opts.mapSettings,
                styles: me.opts.styles
            });
            buildOverlays();
            me.map.addEventListener('zoom_changed', zoomChanged);
            setOverlay();
        };

        var buildOverlays = function () {
            for (var x in me.opts.overlays) {
                var func = function(){
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
                        getTileUrl:func(),
                        tileSize: new google.maps.Size(256, 256)
                    })
                }
                me.overlays.push(overlay);
            }
        };
        
        var setOverlay = function () {
            var z = me.map.getNative().getZoom();
            var newOverlay = null;
            for(var x in me.overlays){
                if (z >= me.overlays[x].minZoom && z <= me.overlays[x].maxZoom && me.currentOverlayIndex != x) {
                    newOverlay = {
                        overlay: me.overlays[x],
                        index: x
                    };
                }
            }

            if (newOverlay != null) {
                me.map.getNative().overlayMapTypes.clear();
                me.map.getNative().overlayMapTypes.push(newOverlay.overlay.imageMap);
                me.currentOverlayIndex = newOverlay.index;
                newOverlay.overlay.legendSource(function (data) {
                    updateLegend({ overlay: newOverlay.overlay, data: data });
                });
            }
        };

        var updateLegend = function (data) {

            me.title.html(data.overlay.legendTitle);

            var list = [];
            if (data.data.length > 0) {
                if (data.data.length < data.overlay.colors.length) {
                    for (var x = 0; x < data.data.length; x++) {
                        var t = me.opts.templates.get('legendItem');
                        list.push(me.opts.templates.bind(t, { color: data.overlay.colors[x], label: data.overlay.legendFormat(data.data[x].Min) }));
                    }
                }
                else {
                    for (var x = 0; x < data.data.length; x++) {
                        var t = me.opts.templates.get('legendItem');
                        list.push(me.opts.templates.bind(t, { color: data.overlay.colors[x], label: data.overlay.legendFormat(data.data[x].Min) + ' - ' + data.overlay.legendFormat(data.data[x].Max) }));
                    }
                }
                me.legend.html(list.reverse().join(''));

                me.legend.parent().show();
                me.title.show();
            }
            else {
                me.legend.parent().hide();
                me.title.hide();
            }

         

        };

        var zoomChanged = function () {
            setOverlay();
        };

        var fitBounds = function (latLngBounds) {
            me.map.fitBounds(latLngBounds);
        };

        var setCenter = function (latLng) {
            me.map.setCenter(latLng);
        };

        var publicObj = {
            getContainer: function () {
                return me.container;
            },
            fitBounds: function (bounds) {
                fitBounds(bounds);
            },
            setCenter: function (latLng) {
                setCenter(latLng);
            }
        };
        init();
        return publicObj;

    };
})();

















