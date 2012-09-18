(function () {
    sizeup.core.namespace('sizeup.maps');
    sizeup.maps.heatMap = function (opts) {

        var defaults = {
            mapSettings: sizeup.maps.mapOptions.getDefaults(),
            styles: sizeup.maps.mapStyles.getDefaults(),
            colors: [],
            overlays: [],
            borderId: null,
            showBorders: false,
            borderUrl: ''
        };
        var dataLayer = new sizeup.core.data();
        var templates = new sizeup.core.templates();
        var me = {};
        me.opts = $.extend(true, defaults, opts);
        me.container = opts.container;
        me.overlays = [];
        me.currentOverlayIndex=null;
        me.borderOverlay = null;

        var init = function () {

            me.legend = me.container.find('.legendContainer');
            me.legend.parent().hide();
            me.title = me.container.find('.title').hide();
            me.map = new sizeup.maps.map({
                container: me.container.find('.container'),
                mapSettings: me.opts.mapSettings,
                styles: me.opts.styles
            });
            me.textAlternative = me.container.find('.textAlternative');
            me.textAlternative.click(function () { textAlternativeClicked(); });
            buildBorderOverlay();
            buildOverlays(me.opts.overlays);
            me.map.addEventListener('zoom_changed', zoomChanged);
            setOverlay();
        };

        var textAlternativeClicked = function () {
            var overlay = me.overlays[me.currentOverlayIndex];
            var url = overlay.textAlternativeUrl;
            var bounds = me.map.getBounds();
            var data = {
                industryId: overlay.industryId,
                boundingEntityId: overlay.boundingEntityId,
                southWest: bounds.getSouthWest().toString(),
                northEast: bounds.getNorthEast().toString()
            };

            window.open(jQuery.param.querystring(url, data), '_blank');

        };

        var buildBorderOverlay = function () {
            var func = function () {
                return function (point, zoom) {
                    var url = me.opts.borderUrl;
                    var params = {
                        x: point.x,
                        y: point.y,
                        zoom: zoom,
                        entityId: me.opts.borderId
                    };
                    return jQuery.param.querystring(url, params);
                };
            };

            me.borderOverlay = new google.maps.ImageMapType({
                getTileUrl: func(),
                tileSize: new google.maps.Size(256, 256)
            });

        };


        var buildOverlays = function (overlays) {
            for (var x in overlays) {
                var func = function(){
                    var opts = overlays[x];
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
                    industryId: overlays[x].industryId,
                    boundingEntityId: overlays[x].boundingEntityId,
                    textAlternativeUrl: overlays[x].textAlternativeUrl,
                    legendSource: overlays[x].legendSource,
                    legendTitle: overlays[x].legendTitle,
                    legendFormat: overlays[x].legendFormat,
                    minZoom: overlays[x].minZoom,
                    maxZoom: overlays[x].maxZoom,
                    colors: overlays[x].colors,
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
                if (z >= me.overlays[x].minZoom && z <= me.overlays[x].maxZoom) {
                    newOverlay = {
                        overlay: me.overlays[x],
                        index: x
                    };
                }
            }
            me.map.getNative().overlayMapTypes.clear();
            if (newOverlay != null) {
                
                me.map.getNative().overlayMapTypes.push(newOverlay.overlay.imageMap);
                me.currentOverlayIndex = newOverlay.index;
                newOverlay.overlay.legendSource(function (data) {
                    updateLegend({ overlay: newOverlay.overlay, data: data });
                });
            }
            if (me.opts.showBorders && me.opts.borderId != null) {
                me.map.getNative().overlayMapTypes.push(me.borderOverlay);
            }
        };

        var updateLegend = function (data) {

            me.title.html(data.overlay.legendTitle);

            var list = [];
            if (data.data.length > 0) {
                var colors = data.overlay.colors;
                if (data.data.length < colors.length) {
                    for (var x = 0; x < data.data.length; x++) {
                        var t = me.opts.legendItemTemplate;
                        list.push(templates.bind(t, { color: colors[x], label: data.overlay.legendFormat(data.data[x].Min) }));
                    }
                }
                else {
                    for (var x = 0; x < data.data.length; x++) {
                        var t = me.opts.legendItemTemplate;
                        list.push(templates.bind(t, { color: colors[x], label: data.overlay.legendFormat(data.data[x].Min) + ' - ' + data.overlay.legendFormat(data.data[x].Max) }));
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

        var hideLegend = function (duration) {
            if (duration) {
                me.legend.parent().hide(duration);
                me.title.hide(duration);
            }
            else {
                me.legend.parent().hide();
                me.title.hide();
            }
        };

        var showLegend = function (duration) {
            if (duration) {
                me.legend.parent().show(duration);
                me.title.show(duration);
            }
            else {
                me.legend.parent().show();
                me.title.show();
            }
        };

        var clearOverlays = function () {
            me.overlays = [];
            me.currentOverlayIndex = null;
            me.map.getNative().overlayMapTypes.clear();
            hideLegend();
        };

        var setOverlays = function (overlays) {
            showLegend();
            me.currentOverlayIndex = null;
            buildOverlays(overlays);
            setOverlay();
        };

        var triggerEvent = function (event) {
            me.map.triggerEvent(event);
        };

        var addMarker = function (marker) {
            me.map.addMarker(marker);
        };

        var setZoom = function (zoom) {
            me.map.setZoom(zoom);
        };

        var showCityBorder = function () {
            me.opts.showBorders = true;
            setOverlay();
        };

        var hideCityBorder = function () {
            me.opts.showBorders = false;
            setOverlay();
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
            },
            hideLegend: function (duration) {
                hideLegend(duration);
            },
            showLegend: function (duration) {
                showLegend(duration);
            },
            clearOverlays: function () {
                clearOverlays();
            },
            setOverlays: function (overlays) {
                setOverlays(overlays);
            },
            triggerEvent: function (event) {
                triggerEvent(event);
            },
            addMarker: function (marker) {
                addMarker(marker);
            },
            setZoom: function (zoom) {
                setZoom(zoom);
            },
            showCityBorder: function () {
                showCityBorder();
            },
            hideCityBorder: function () {
                hideCityBorder();
            }
        };
        init();
        return publicObj;

    };
})();

















