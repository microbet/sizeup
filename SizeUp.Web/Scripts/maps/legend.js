(function () {
    sizeup.core.namespace('sizeup.maps');
    sizeup.maps.legend = function (opts) {

        var defaults = {
            container: $('<div></div>'),
            templates: new sizeup.core.templates(),
            legendItemTemplateId: '',
            colors: [
                '#F5F500',
                '#F5CC00',
                '#F5A300',
                '#F57A00',
                '#F55200',
                '#F52900',
                '#F50000'
            ]
        };

        var me = {};
        me.opts = $.extend(true, defaults, opts);
        
        me.title = me.opts.container.find('.title .text');
        me.legendContainer = me.opts.container.find('.legendContainer');

        var getContainer = function () {
            return me.opts.container;
        };

        var setLegend = function (items) {
            var list = [];
            if (items.length < me.opts.colors.length) {
                for (var x = 0; x < items.length; x++) {
                    var t = me.opts.templates.get(me.opts.legendItemTemplateId);
                    list.push(me.opts.templates.bind(t, { color: me.opts.colors[x], label: items[x].Min }));
                }
            }
            else {
                for (var x = 0; x < items.length; x++) {
                    var t = me.opts.templates.get(me.opts.legendItemTemplateId);
                    list.push(me.opts.templates.bind(t, { color: me.opts.colors[x], label: items[x].Min + ' - ' + items[x].Max }));
                }
            }
            me.legendContainer.html(list.reverse().join(''));
        };

        var setTitle = function (title) {
            me.title.html(title);
        };

        var publicObj = {
            getContainer: function () {
                return getContainer();
            },
            setLegend: function (items) {
                setLegend(items);
            },
            setTitle: function (title) {
                setTitle(title);
            }

        };
        return publicObj;

    };
})();

















