(function () {
    sizeup.core.namespace('sizeup.maps');
    sizeup.maps.polygon = function (opts) {

        var defaults = {
            
        };
        var me = {};
        me.opts = $.extend(true, defaults, opts);
        me.poly = new google.maps.Polygon(me.opts);
        me._native = me.poly;


        var setVisible = function (v) {
            me.poly.setVisible(v);
        };

        var publicObj = {
            getNative: function () {
                return me._native;
            },
            setVisible: function (v) {
                setVisible(v);
            }
        };
        return publicObj;

    };
})();

















