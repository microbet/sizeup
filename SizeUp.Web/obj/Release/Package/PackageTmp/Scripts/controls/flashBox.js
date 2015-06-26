(function () {
    sizeup.core.namespace('sizeup.controls');
    sizeup.controls.flashBox = function (opts) {

        var defaults = {
            container: $('<div></div>'),
            flashLength: 5000,
            fadeLength:1000
        };
        var me = {};
        me.opts = $.extend(true, defaults, opts);
        me.data = {};

        var init = function () {
            me.opts.container.hide().removeClass('hidden');
        };

        var flash = function () {
            me.opts.container.show().delay(me.opts.flashLength).fadeOut(me.opts.fadeLength);
        };


        var show = function () {
            me.opts.container.show();
        };

        var hide = function () {
            me.opts.container.hide();
        };
        

        var publicObj = {
            flash: function () {
                flash();
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