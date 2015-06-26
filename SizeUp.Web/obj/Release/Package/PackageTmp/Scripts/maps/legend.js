(function () {
    sizeup.core.namespace('sizeup.maps');
    sizeup.maps.legend = function (opts) {

        var defaults = {
            templates: new sizeup.core.templates(),
            startColor: '000000',
            endColor: 'ffffff',
            title: '',
            items:[],
            format: function (val) { return val; }
        };

    

        var me = {};
        me.opts = $.extend(true, defaults, opts);

        var heatmapOpts = { startColor: me.opts.startColor, endColor: me.opts.endColor, bands: me.opts.items.length };
        var heatmapColors = new sizeup.maps.heatmapColors(heatmapOpts);
        me.opts.colors = heatmapColors.getColors();

        me.legendContainer =  $(me.opts.templates.get('legendContainer'));
        me.titleContainer = $(me.opts.templates.get('legendTitle'));
        me.title = me.titleContainer.find('.title .text');
        me.legend = me.legendContainer.find('.legendContainer');

        var isRange = function (data) {
            var bool = true;
            for (var x = 0; x < data.length; x++) {
                bool = bool && data[x].Min != data[x].Max;
            }
            return bool;
        };


        var list = [];
        var t = me.opts.templates.get('legendItem');

        if (!isRange(me.opts.items)) {
            for (var x = 0; x < me.opts.items.length; x++) {
                t = me.opts.templates.get('legendItem');
                list.push(me.opts.templates.bind(t, { color: '#' + me.opts.colors[x], label: me.opts.format(me.opts.items[x].Max) }));
            }
        }
        else {
            for (var x = 0; x < me.opts.items.length; x++) {
                t = me.opts.templates.get('legendItem');
                list.push(me.opts.templates.bind(t, { color: '#' + me.opts.colors[x], label: me.opts.format(me.opts.items[x].Min) + ' - ' + me.opts.format(me.opts.items[x].Max) }));
            }
        }

        if (me.opts.items.length == 0) {
            list.push(me.opts.templates.bind(t, { color: '#C0C0C0', label: 'No data (zoom out)' }));
        }
        else {
            list.push(me.opts.templates.bind(t, { color: '#C0C0C0', label: 'No data available' }));
        }
        me.legend.html(list.join(''));
        me.title.html(me.opts.title);
      
      
       


       


        var publicObj = {
            getLegend: function () {
                return me.legendContainer;
            },
            getTitle: function () {
                return me.titleContainer;
            }

        };
        return publicObj;

    };
})();

















