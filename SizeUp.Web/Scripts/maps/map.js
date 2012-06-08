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


        var addPolygon = function (p) {
            p.getNative().setMap(me.map);
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

        var fitBounds = function (latLngBounds) {
            me.map.fitBounds(latLngBounds.getNative());
        };
        var setCenter = function (latLng) {
            me.map.setCenter(latLng.getNative());
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
            addPolygon: function (p) {
                addPolygon(p);
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
            }
        };
        init();
        return publicObj;

    };
})();

















