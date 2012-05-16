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

        var publicObj = {
            getNative: function () {
                return me._native;
            },
            extend: function (latLng) {
                extend(latLng);
            }
        };
        return publicObj;

    };
})();

















