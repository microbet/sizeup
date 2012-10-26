(function () {
    sizeup.core.namespace('sizeup.maps');
    sizeup.maps.mapStyles = sizeup.maps.mapStyles || (function () {

        var mapStyles = {
            blackAndWhite: [
                    {
                        featureType: "all",
                        stylers: [
                        { saturation: -100 },
                        { lightness: 0 }
                        ]
                    },
                    {
                        featureType: "all",
                        elementType: "labels",
                        stylers: [
                            { saturation: -100 },
                            { lightness: 50 }
                        ]
                    },
                    {
                        featureType: "poi.park",
                        elementType: "geometry",
                        stylers: [
                            { saturation: -100 },
                            { lightness: 50 }
                        ]
                    }/*,
                    {
                        featureType: "water",
                        elementType: "geometry",
                        stylers: [
                            { color: '#BBDAFA' }
                        ]
                    }*/
            ]
        };


        var getDefaults = function () {
            return mapStyles.blackAndWhite;
        }
        var publicObj = {
            getDefaults: function () {
                return getDefaults();
            }
        };
        return publicObj;


    })();
})();






