(function () {
    sizeup.core.namespace('sizeup.controls');
    sizeup.controls.rangeLabel = function (opts) {

        var defaults = {
            container: $('<div></div>')
        };
        var me = {};
        me.opts = $.extend(true, defaults, opts);
        me.templates = new sizeup.core.templates();


        var init = function () {
            me.container = me.opts.container;
            me.labels = {};
            
            me.labels['range'] = me.container.attr('data-range');
            me.labels['min'] = me.container.attr('data-min');
            me.labels['max'] = me.container.attr('data-max');
            me.labels['value'] = me.container.attr('data-value');
            me.labels['off'] = me.container.attr('data-off');
        };

        
        var setValues = function (values) {
            var html = '';
            if (values != null && values.min != null && values.max != null) {
                html = me.labels['range'];
            }
            else if (values != null && values.min != null) {
                html = me.labels['min'];
            }
            else if (values != null && values.max != null) {
                html = me.labels['max'];
            }
            else if (values != null && values.value != null) {
                html = me.labels['value'];
            }
            else {
                html = me.labels['off'];
            }
            var text = me.templates.bind(html, values);
            var delay = function () { me.container.html(text); };
            //this is done becuase ie is a real piece of crap. turns out it doesnt like to update the dom so we have to
            //do this stupid setTimeout shenanigans
            setTimeout(delay ,1);
        };
        
        var show = function () {
            me.container.show();
        };

        var hide = function () {
            me.container.hide();
        };

        var getContainer = function () {
            return me.container;
        };


        var publicObj = {
            setValues: function (values) {
                setValues(values);
            },
            show: function () {
                show();
            },
            hide: function () {
                hide();
            },
            getContainer: function () {
                return getContainer();
            }
        };
        init();
        return publicObj;

    };
})();