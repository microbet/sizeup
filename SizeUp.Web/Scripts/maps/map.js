(function () {
    sizeup.core.namespace('sizeup.maps.map');
    sizeup.maps.map = function (opts) {

        var defaults = {
            mapSettings: sizeup.maps.mapOptions.getDefaults(),
            styles: sizeup.maps.mapStyles.getDefaults()
        };
        var me = {};
        me.opts = $.extend(true, defaults, opts);
        me.container = opts.container;
        
        var init = function () {

            me.map = new google.maps.Map(me.container.get(0), me.opts.mapSettings);
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



        var publicObj = {
            getContainer: function(){
                return me.container;
            }
        };
        init();
        return publicObj;

    };
})();

















