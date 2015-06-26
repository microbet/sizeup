(function () {
    sizeup.core.namespace('sizeup.controls');
    sizeup.controls.toggleButton = function (opts) {

        var defaults = {
            onClick: function () { }
        };
        var me = {};
        me.opts = $.extend(true, defaults, opts);
        

        var init = function () {
            me.button = opts.button;
            me.button.click(function () { onClick(); });
        };

        var toggle = function () {
            var text = me.button.attr('data-toggleText');
            if (typeof text !== 'undefined' && text !== false) {
                me.button.attr('data-toggleText', me.button.html());
                me.button.html(text);
            }
            var cls = me.button.attr('data-toggleClass');
            if (typeof cls !== 'undefined' && cls !== false) {
                me.button.toggleClass(cls);
            }
        };

        var onClick = function () {
            toggle();
            me.opts.onClick();
        };

        var publicObj = {
            toggle: function () {
                toggle();
            }
        };
        init();
        return publicObj;

    };
})();