(function () {
    sizeup.core.namespace('sizeup.maps');
    sizeup.maps.legend = function (opts) {

        var defaults = {
            templates: new sizeup.core.templates(),
            colors: [
                '#F5F500',
                '#F5CC00',
                '#F5A300',
                '#F57A00',
                '#F55200',
                '#F52900',
                '#F50000'
            ],
            title: '',
            items:[],
            format: function (val) { return val; }
        };

    

        var me = {};
        me.opts = $.extend(true, defaults, opts);

        me.legendContainer =  $(me.opts.templates.get('legendContainer'));
        me.titleContainer = $(me.opts.templates.get('legendTitle'));
        me.title = me.titleContainer.find('.title .text');
        me.legend = me.legendContainer.find('.legendContainer');

        
        var list = [];
        var t = me.opts.templates.get('legendItem');
        if (me.opts.items.length == 0) {
            list.push(me.opts.templates.bind(t, { color: '#C0C0C0', label: 'No data (zoom out)' }));
        }
        else {
            list.push(me.opts.templates.bind(t, { color: '#C0C0C0', label: 'No data available' }));
        }

        if (me.opts.items.length < me.opts.colors.length) {
            for (var x = 0; x < me.opts.items.length; x++) {
                t = me.opts.templates.get('legendItem');
                list.push(me.opts.templates.bind(t, { color: me.opts.colors[x], label: me.opts.format(me.opts.items[x].Min) }));
            }
        }
        else {
            for (var x = 0; x < me.opts.items.length; x++) {
                t = me.opts.templates.get('legendItem');
                list.push(me.opts.templates.bind(t, { color: me.opts.colors[x], label: me.opts.format(me.opts.items[x].Min) + ' - ' + me.opts.format(me.opts.items[x].Max) }));
            }
        }
        me.legend.html(list.reverse().join(''));
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

















