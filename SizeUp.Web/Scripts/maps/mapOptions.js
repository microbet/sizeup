(function () {

    sizeup.core.namespace('sizeup.maps');
    sizeup.maps.mapOptions = sizeup.maps.mapOptions || (function () {

        var mapOptions = {
            defaults: function () {
                return {
                    zoom: 12,
                    minZoom: 4,
                    center: new google.maps.LatLng(37.012, -122.01),
                    mapTypeControl: false,
                    streetViewControl: false,
                    scrollwheel: false,
                    zoomControl: true,
                    mapTypeId: google.maps.MapTypeId.ROADMAP,
                    mapTypeControlOptions: { mapTypeIds: [google.maps.MapTypeId.ROADMAP, "mapStyle"] }
                };
            }
        };

        var getDefaults = function () {
            return mapOptions.defaults();
        };

        var publicObj = {
            getDefaults: function () {
                return getDefaults();
            }
        };
        return publicObj;
    })();
})();








