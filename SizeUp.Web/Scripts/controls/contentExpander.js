(function () {
    sizeup.core.namespace('sizeup.controls.contentExpander');
    sizeup.controls.contentExpander = function (opts) {

        var me = {};
        me.opts = opts;
        me.data = {};

        var init = function () {
            me.button = opts.button;
            me.contentPanel = opts.contentPanel;
            me.contentPanel.hide().removeClass('hidden');
            me.button.click(function () { onClick(); });
        };


        var onClick = function () {
            var text = me.button.attr('data-toggleText');
            if (typeof text !== 'undefined' && text !== false) {
                me.button.attr('data-toggleText', me.button.html());
                me.button.html(text);
            }
            var expandedClass = me.contentPanel.attr('data-expandedClass');
            if (typeof expandedClass !== 'undefined' && expandedClass !== false) {
                if (!me.contentPanel.is(':visible')) {
                    me.contentPanel.addClass(expandedClass);
                }
                else {
                    me.contentPanel.removeClass(expandedClass);
                }
            }
            me.contentPanel.slideToggle();
        };

        var publicObj = {

        };
        init();
        return publicObj;

    };
})();