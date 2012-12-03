(function () {
    sizeup.core.namespace('sizeup.controls');
    sizeup.controls.promptBox = function (opts) {

        var defaults = {
            textbox: $('<input type="text" />')
        };
        var me = {};
        me.opts = $.extend(true, defaults, opts);
        me.data = {};

        var init = function () {
            me.data.promptText = me.opts.textbox.val();
            me.opts.textbox.blur(onBlur);
            me.opts.textbox.focus(onFocus);
        };

        var onBlur = function () {

        };

        var onFocus = function () {

        };

        var getValue = function () {
            return me.opts.textbox.val();
        };

        var setValue = function (val) {
            me.opts.textbox.val(val);
        };

        var publicObj = {
            getValue: function () {
                return getValue();
            },
            setValue: function (val) {
                setValue(val);
            }
        };
        init();
        return publicObj;

    };
})();