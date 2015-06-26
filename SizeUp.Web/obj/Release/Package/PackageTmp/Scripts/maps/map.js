(function () {
    sizeup.core.namespace('sizeup.maps');
    sizeup.maps.map = function (opts) {

        var defaults = {
            mapSettings: sizeup.maps.mapOptions.getDefaults(),
            styles: sizeup.maps.mapStyles.getDefaults()
        };
        var me = {};
        me.opts = $.extend(true, defaults, opts);
        me.container = opts.container;
        me.map = new google.maps.Map(me.container.get(0), me.opts.mapSettings);
        me._native = me.map;
        me.overlays = [];
        me.legend = null;

        var init = function () {
            google.maps.event.trigger(me.map, 'resize');
            if (me.opts.styles) {
                var mapStyle = new google.maps.StyledMapType(me.opts.styles, { name: "mapStyle" });
                me.map.mapTypes.set("mapStyle", mapStyle);
                me.map.setMapTypeId("mapStyle");
            }

            var copyrightDiv = document.createElement("div");
            copyrightDiv.id = "map-copyright";
            copyrightDiv.style.fontSize = "10px";
            copyrightDiv.style.fontFamily = "Arial, sans-serif";
            copyrightDiv.style.height = "15px";
            copyrightDiv.style.padding = "2px";
            copyrightDiv.style.whiteSpace = "nowrap";
            copyrightDiv.style.color = "#444444";
            $(copyrightDiv).css('background', 'rgba(255,255,255,0.5)');

            me.map.controls[google.maps.ControlPosition.RIGHT_BOTTOM].push(copyrightDiv);
            copyrightDiv.innerHTML = "Business Intelligence Provided By: SizeUp";
        };


        var clearOverlays = function () {
            me.overlays = [];
            me._native.overlayMapTypes.clear();
        };

        var addOverlay = function (overlay, index) {
            //all this complicated stuff is to create a z indexing for overlays
            //we cache each overlay in me.overlays where me.overlays[x] where x is the z-index. multiple overlays can be placed on the same zindex and
            //the order that they are on top of each other is arbitrary. However, all overlays with a zindex of 0 are GARAUNTEED to be below those with a zindex of 1
            if (index == null) {
                index = 0;
            }

            if (typeof (me.overlays[index]) == "undefined") {
                me.overlays[index] = [];
            }
            me.overlays[index].push(overlay);
            me._native.overlayMapTypes.clear();
            for (var x = 0;x < me.overlays.length; x++) {
                if (me.overlays[x] != null) {
                    for (var y = 0; y < me.overlays[x].length; y++) {
                        me._native.overlayMapTypes.push(me.overlays[x][y].getNative());
                    }
                }
            }
        };

        var removeOverlay = function (overlay) {
            var overlays = me._native.overlayMapTypes.getArray();
            var index = -1;
            for (var x = 0; x < overlays.length; x++) {
                if (overlays[x] === overlay.getNative()) {
                    index = x;
                }
            }
            if(index >=0){
                me._native.overlayMapTypes.removeAt(index);
            }

            //remove from cache too
            for (var x = 0; x < me.overlays.length; x++) {
                if (me.overlays[x] != null) {
                    var i = $.inArray(overlay, me.overlays[x]);
                    if (i >= 0) {
                        me.overlays[x].splice(i, 1);
                        if (me.overlays[x].length == 0) {
                            delete me.overlays[x];
                        }
                    }
                }
            }



        };

        var addPolygon = function (p) {
            p.getNative().setMap(me.map);
        };

        var addCircle = function (c) {
            c.getNative().setMap(me.map);
        };

        var removeCircle = function (c) {
            c.getNative().setMap(null);
        };


        var addEventListener = function(event, handler){
            google.maps.event.addListener(me.map, event, handler);
        };

        var triggerEvent = function (event) {
            google.maps.event.trigger(me.map, event);
        };

        var getBounds = function () {
            var nbounds = me.map.getBounds();
            var b = new sizeup.maps.latLngBounds();
            b.getNative().union(nbounds);
            return b;
        };

        var getZoom = function () {
            return me.map.getZoom();
        };

        var setZoom = function (zoom) {
            me.map.setZoom(zoom);
        };

        var fitBounds = function (latLngBounds) {
            me.map.fitBounds(latLngBounds.getNative());
        };
        var setCenter = function (latLng) {
            me.map.setCenter(latLng.getNative());
        };

        var addMarker = function (marker) {
            marker.getNative().setMap(me.map);
        };

        var removeMarker = function (marker) {
            marker.getNative().setMap(null);
        };

        var addLegend = function (legend) {
            if (me.legend == null) {
                me.legend = {
                    title: $('<div class="titleContainer"></div>'),
                    legend: $('<div class="legendContainer"></div>')
                }
                me.map.controls[google.maps.ControlPosition.RIGHT_TOP].setAt(0, me.legend.title.get(0));
                me.map.controls[google.maps.ControlPosition.RIGHT_TOP].setAt(1, me.legend.legend.get(0));
                replaceLegend(legend);
            }
            else {
                me.map.controls[google.maps.ControlPosition.RIGHT_TOP].removeAt(0);
                me.map.controls[google.maps.ControlPosition.RIGHT_TOP].removeAt(0);
                me.map.controls[google.maps.ControlPosition.RIGHT_TOP].setAt(0, me.legend.title.get(0));
                me.map.controls[google.maps.ControlPosition.RIGHT_TOP].setAt(1, me.legend.legend.get(0));
                replaceLegend(legend);

            }
        };

        var replaceLegend = function (legend) {
            me.legend.title.html(legend.getTitle());
            me.legend.legend.html(legend.getLegend());
        };

        var clearLegend = function () {
            me.map.controls[google.maps.ControlPosition.RIGHT_TOP].clear();
            me.legend = null;
        };

        var getWidth = function () {
            return me.container.width();
        };


        var publicObj = {
            getContainer: function () {
                return me.container;
            },
            getNative: function () {
                return me._native;
            },
            getBounds: function(){
                return getBounds();
            },
            getZoom: function(){
                return getZoom();
            },
            setZoom: function(zoom){
                setZoom(zoom);
            },
            addPolygon: function (p) {
                addPolygon(p);
            },
            addCircle: function (c) {
                addCircle(c);
            },
            removeCircle: function (c) {
                removeCircle(c);
            },
            addEventListener: function (event, handler) {
                addEventListener(event, handler);
            },
            triggerEvent: function(event){
                triggerEvent(event);
            },
            fitBounds: function (latLngBounds) {
                fitBounds(latLngBounds);
            },
            setCenter: function (latLng) {
                setCenter(latLng);
            },
            addMarker: function (marker) {
                addMarker(marker);
            },
            removeMarker: function (marker) {
                removeMarker(marker);
            },
            addOverlay: function (overlay, index) {
                addOverlay(overlay, index);
            },
            removeOverlay: function (overlay) {
                removeOverlay(overlay);
            },
            clearOverlays: function(){
                clearOverlays();
            },
            setLegend: function (control) {
                addLegend(control);
            },
            clearLegend: function () {
                clearLegend();
            },
            getWidth: function () {
                return getWidth();
            }

        };
        init();
        return publicObj;

    };
})();

















