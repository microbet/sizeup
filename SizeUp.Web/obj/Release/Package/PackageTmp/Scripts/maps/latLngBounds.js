(function () {
    sizeup.core.namespace('sizeup.maps');
    sizeup.maps.latLngBounds = function (opts) {

        var defaults = {

        };
        var me = {};
        me.opts = $.extend(true, defaults, opts);
        me.bounds = new google.maps.LatLngBounds();
        me._native = me.bounds;

        for (var x in me.opts.latLngs) {
            me.bounds.extend(me.opts.latLngs[x].getNative());
        }

        var extend = function (latLng) {
            me.bounds.extend(latLng.getNative());
        };

        var getSouthWest = function () {
            var sw = me._native.getSouthWest();
            return new sizeup.maps.latLng({ lat: sw.lat(), lng: sw.lng() });
        };


        var getNorthEast = function () {
            var sw = me._native.getNorthEast();
            return new sizeup.maps.latLng({ lat: sw.lat(), lng: sw.lng() });
        };


        var publicObj = {
            getNative: function () {
                return me._native;
            },
            extend: function (latLng) {
                extend(latLng);
            },
            getSouthWest: function () {
                return getSouthWest();
            },
            getNorthEast: function () {
                return getNorthEast();
            }
        };
        return publicObj;

    };
})();

















