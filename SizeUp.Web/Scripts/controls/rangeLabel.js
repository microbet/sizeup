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
        };

        
        var setValues = function (values) {
            var html = '';
            if (values && values.min && values.max) {
                html = unescape(me.container.attr('data-range'));
            }
            else if (values && values.min) {
                html = unescape(me.container.attr('data-min'));
            }
            else if (values && values.max) {
                html = unescape(me.container.attr('data-max'));
            }
            else if (values && values.value) {
                html = unescape(me.container.attr('data-value'));
            }
            else {
                html = unescape(me.container.attr('data-off'));
            }
            me.container.html(me.templates.bind(html, values));
        };
        
        var show = function () {
            me.container.show();
        };

        var hide = function () {
            me.container.hide();
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
            }
        };
        init();
        return publicObj;

    };
})();