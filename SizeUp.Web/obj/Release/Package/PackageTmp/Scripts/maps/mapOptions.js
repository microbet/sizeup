(function () {

    sizeup.core.namespace('sizeup.maps');
    sizeup.maps.mapOptions = sizeup.maps.mapOptions || (function () {

        var mapOptions = {
            defaults: function () {
                return {
                    zoom: 4,
                    minZoom: 3,
                    maxZoom:24,
                    center: new google.maps.LatLng(38.272689, -95.800781),
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








