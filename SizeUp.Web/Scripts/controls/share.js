(function () {
    sizeup.core.namespace('sizeup.controls');
    sizeup.controls.share = function (opts) {

        var defaults = {
            button: $('<a></a>'),
            container: $('<div></div>'),
            options: {}
        };
        var me = {};
        me.opts = $.extend(true, defaults, opts);
        

        var init = function () {

            me.button = new sizeup.controls.toggleButton(
                {
                    button: me.opts.container.find('.shareButton'),
                    onClick: function () { shareToggle(); }
                });

            me.container = me.opts.container;

            me.contentContainer = me.opts.container.find('.container').hide().removeClass('hidden');
            me.menu = me.opts.container.find('.menu');
            me.content = me.opts.container.find('.content');
            me.default = null;
            me.tabs = {};

            for (var o in me.opts.options) {
                me.tabs[o] = {
                    getCode: me.opts.options[o].getCode || function () { },
                    menuItem: me.opts.options[o].menuItem || $('<div></div>'),
                    contentItem: me.opts.options[o].contentItem || $('<div</div>')
                };
                me.tabs[o].codeBox = me.opts.options[o].contentItem.find('.code');
                me.tabs[o].codeBox.bind('focus', o, function (e) {contentFocus(e.data); });
                me.tabs[o].codeBox.bind('blur', o, function (e) { contentBlur(e.data); });
                me.tabs[o].codeBox.bind('click', o, function (e) { contentClick(e.data); });
                me.tabs[o].menuItem.find('a').bind('click', o, function (e) { activateTab(e.data); });
                if(me.default==null){
                    me.default = o;
                }
            };


            $('body').click(onBodyClicked);

        };

        var onBodyClicked = function (e) {
            if (!$(e.target).parents().is(me.container) && me.contentContainer.is(':visible')) {
                me.contentContainer.hide();
            }
        };


        var shareToggle = function () {
            var activate = !me.contentContainer.is(':visible');
            me.contentContainer.toggle();
            if (activate) {
                activateTab(me.default);
            }
        };

        var activateTab = function (index) {
            hideTabs();
            me.tabs[index].menuItem.addClass('active');
            me.tabs[index].contentItem.show();
            contentFocus(index);
        };

        var contentBlur = function (index) {
            me.tabs[index].codeBox.get(0).selectionStart = -1;
            me.tabs[index].codeBox.get(0).selectionEnd = -1;
        };

        var contentFocus = function (index) {
            me.tabs[index].codeBox.val(me.tabs[index].getCode());
            setTimeout(function () {
                me.tabs[index].codeBox.get(0).select();
            }, 1);
        };

        var contentClick = function (index) {
            setTimeout(function () {
                me.tabs[index].codeBox.get(0).select();
            }, 1);
        };

        var hideTabs = function () {
            for (var x in me.tabs) {
                me.tabs[x].menuItem.removeClass('active');
                me.tabs[x].contentItem.hide();
            }
        };

        var publicObj = {
            
        };
        init();
        return publicObj;

    };
})();