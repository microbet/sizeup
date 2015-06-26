(function () {
    sizeup.core.namespace('sizeup.maps');
    sizeup.maps.heatmapColors = function (opts) {

        var defaults = {
            bands: 5,
            startColor: '000000',
            endColor: 'ffffff'
        };
        var me = {};
        me.opts = $.extend(true, defaults, opts);

        me.startColor = tinycolor(me.opts.startColor);
        me.endColor = tinycolor(me.opts.endColor);

        var getColors = function () {
            var colors = [];
            var midColors = Math.max(me.opts.bands - 2, 0);
            var c1 = me.startColor.toRgb();
            var c2 = me.endColor.toRgb();

            var deltaR = (c2.r - c1.r) / (midColors + 1);
            var deltaG = (c2.g - c1.g) / (midColors + 1);
            var deltaB = (c2.b - c1.b) / (midColors + 1);

            colors[0] = me.startColor;
            for (var x = 1; x <= midColors; x++) {
                var rgb = { r: Math.ceil(c1.r + (x * deltaR)), g: Math.ceil(c1.g + (x * deltaG)), b: Math.ceil(c1.b + (x * deltaB)) };
                var c = tinycolor(rgb);
                colors[x] = c;
            }
            colors[colors.length] = me.endColor;


            var output = [];
            for (var x = 0; x < me.opts.bands; x++) {
                output[x] = colors[x].toHex();
            }
            return output;
        };


        var publicObj = {
            getColors: function () {
                return getColors();
            }
        };
        return publicObj;

    };
})();

