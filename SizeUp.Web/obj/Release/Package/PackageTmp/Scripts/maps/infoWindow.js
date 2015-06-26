(function () {
    sizeup.core.namespace('sizeup.maps');
    sizeup.maps.infoWindow = function (opts) {

        var defaults = {
            content:'',
            marker: null
        };
        var me = {};
        me.opts = $.extend(true, defaults, opts);

        


        me._native = new google.maps.InfoWindow({
            content: opts.content
        });

        
        var open = function (map, marker) {
            me._native.open(map.getNative(), marker.getNative());
        };

        var close = function () {
            me._native.close();
        };
       
        var setPosition = function (position) {
            me._native.setPosition(position.getNative());
        };

        var publicObj = {
            getNative: function () {
                return me._native;
            },
            open: function (map, marker) {
                open(map, marker);
            },
            setPosition: function (position) {
                setPosition(position);
            },
            close: function () {
                close();
            }
           
        };
        return publicObj;

    };
})();

















