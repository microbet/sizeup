(function () {
    sizeup.core.namespace('sizeup.controls');
    sizeup.controls.promptBox = function (opts) {

        var defaults = {
            textbox: $('<input type="text" data-prompt="" />'),
            onChange: function () { },
            changeDelay: 250
        };
        var me = {};
        me.opts = $.extend(true, defaults, opts);
        me.data = {};

        var init = function () {
            me.data.promptText = me.opts.textbox.attr('data-prompt');
            me.opts.textbox.val(me.data.promptText);
            me.opts.textbox.addClass('blank');
            me.opts.textbox.blur(onBlur);
            me.opts.textbox.focus(onFocus);
            me.opts.textbox.keydown(onKeydown);
        };

        var onChange = function () {
            me.opts.onChange();
        };

        var onKeydown = function (e) {
            clearTimeout(me.data.interval);
            me.data.interval = setTimeout(onChange, me.opts.changeDelay);
        };

        var onBlur = function () {
            var val = me.opts.textbox.val();
            if ($.trim(val) == '') {
                me.opts.textbox.val(me.data.promptText).addClass('blank');
            }
        };

        var onFocus = function () {
            var val = me.opts.textbox.val();
            if ($.trim(val) == me.data.promptText) {
                me.opts.textbox.val('').removeClass('blank');
            }
            else {
                me.opts.textbox.select();
            }
        };

        var getValue = function () {
            var val = $.trim(me.opts.textbox.val());
            if(val == me.data.promptText)
            {
                val = '';
            }
            return val;
        };

        var setValue = function (val) {
            if ($.trim(val) == '') {
                me.opts.textbox.val(me.data.promptText).addClass('blank');
            }
            else {
                me.opts.textbox.val(val).removeClass('blank');
            }
        };

        var getTextbox = function () {
            return me.opts.textbox;
        };

        var publicObj = {
            getValue: function () {
                return getValue();
            },
            setValue: function (val) {
                setValue(val);
            },
            getTextbox: function () {
                return getTextbox();
            }
        };
        init();
        return publicObj;

    };
})();