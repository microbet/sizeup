(function () {
    sizeup.core.namespace('sizeup.maps');
    sizeup.maps.heatMap = function (opts) {

        var defaults = {
            mapSettings: sizeup.maps.mapOptions.getDefaults(),
            styles: sizeup.maps.mapStyles.getDefaults()
        };
        var me = {};
        me.opts = $.extend(true, defaults, opts);
        me.container = opts.container;

        var init = function () {

            me.legend = me.container.find('.legend');
            me.map = new sizeup.maps.map({
                container: me.container.find('.container'),
                mapSettings: me.opts.mapSettings,
                styles: me.opts.styles
            });


        };



        var publicObj = {
            getContainer: function () {
                return me.container;
            }
        };
        init();
        return publicObj;

    };
})();

















