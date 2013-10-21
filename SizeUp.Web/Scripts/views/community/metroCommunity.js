(function () {
    sizeup.core.namespace('sizeup.views.community');
    sizeup.views.community.metroCommunity = function (opts) {


        var defaults = {
            mapSettings: sizeup.maps.mapOptions.getDefaults(),
            styles: sizeup.maps.mapStyles.getDefaults()
        };
        var me = {};

        me.opts = $.extend(true, defaults, opts);

        me.data = {};
        me.container = $('#community');
        var templates = new sizeup.core.templates(me.container);
        var notifier = new sizeup.core.notifier(function () { init(); });


        me.content = {};

        sizeup.api.data.getBoundingBox({ geographicLocationId: opts.CurrentPlace.Metro.Id }, notifier.getNotifier(function (data) { me.data.BoundingBox = data; }));
        sizeup.api.data.getCentroid({ geographicLocationId: opts.CurrentPlace.Metro.Id }, notifier.getNotifier(function (data) { me.data.CityCenter = new sizeup.maps.latLng({ lat: data.Lat, lng: data.Lng }); }));
        
        var init = function () {

            var bounds = new sizeup.maps.latLngBounds();
            bounds.extend(new sizeup.maps.latLng({ lat: me.data.BoundingBox.SouthWest.Lat, lng: me.data.BoundingBox.SouthWest.Lng }));
            bounds.extend(new sizeup.maps.latLng({ lat: me.data.BoundingBox.NorthEast.Lat, lng: me.data.BoundingBox.NorthEast.Lng }));



            me.content.map = new sizeup.maps.map({
                container: me.container.find('.map')
            });

            var borderOverlay = new sizeup.maps.overlay({
                attribute: sizeup.api.tiles.overlayAttributes.geographyBoundary,
                tileParams: {
                    geographicLocationId: opts.CurrentPlace.Metro.Id
                }
            });

            me.content.map.setCenter(me.data.CityCenter);
            me.content.map.fitBounds(bounds);
            me.content.map.addOverlay(borderOverlay);

        };

        var publicObj = {

        };
        return publicObj;

    };
})();