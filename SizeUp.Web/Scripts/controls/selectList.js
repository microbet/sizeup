(function () {
    sizeup.core.namespace('sizeup.controls');
    sizeup.controls.selectList = function (opts) {

        var defaults = {
            onOpen: function () { },
            onChange: function () { },
            onClose: function () { },
            select: $('<select></select>')
        };
        var me = {};
        me.opts = $.extend(true, defaults, opts);
        

        var init = function () {
            me.opts.select.hide();
            me.container = $('<div class="selectList-container"></div>');
            me.selector = $('<a class="selectList-selector" href="javascript:void(0);"></a>');
            me.list = $('<ul class="selectList-list"></ul>');

            var id = me.opts.select.attr('id');
            if (id) {
                me.container.attr('id', 'selectList-' + id);
            }
            me.list.hide();
            me.list.delegate('li.selectList-listItem', 'mouseenter', onMouseEnterListItem);
            me.list.delegate('li.selectList-listItem', 'mouseleave', onMouseLeaveListItem);
            me.list.delegate('li.selectList-listItem', 'click', onClickListItem);
            $('body').click(menuBlur);
            me.selector.click(selectorClicked);
            me.container.append(me.selector);
            me.container.append(me.list);
            createList();

            me.opts.select.after(me.container);
        };

        var selectorClicked = function (e) {
            if (!me.list.is(':visible')) {
                me.list.show();
                me.list.css('left', '').css('right', '');
                var bodyWidth = me.container.parent().outerWidth();
                if (bodyWidth < me.list.outerWidth() + me.container.position().left) {
                    me.list.css('right', bodyWidth - (me.container.position().left + me.container.outerWidth()));
                }
                me.opts.onOpen();
            }
        };

        var menuBlur = function (e) {
            if (!$(e.target).parents().is(me.container) && me.list.is(':visible')) {
                me.list.hide();
                me.opts.onClose();
            }
        };

        var onMouseEnterListItem = function (e) {
            var item = $(this);
            item.addClass('selectList-hover');
        };

        var onMouseLeaveListItem = function (e) {
            var item = $(this);
            item.removeClass('selectList-hover');
        };

        var onClickListItem = function (e) {
            var item = $(this);
            setValue(item.attr('data-value'));
            me.list.hide();
            me.opts.onChange();
        };

        var createList = function () {
            var list = visitNode(me.opts.select);
            me.list.html(list);
            setValue(me.opts.select.val());
        };

        var visitNode = function (node) {
            var str = '';
            var children = node.children();
            if (node.is('select')) {
                children.each(function (index) {
                    str = str + visitNode($(this));
                });
            }
            else if(node.is('option')){
                str = str + createListItem(node);
            }
            else if (node.is('optgroup')) {
                str = str + createListGroup(node);
                children.each(function (index) {
                    str = str + visitNode($(this));
                });
            }
            return str;
        };

        createListItem = function (node) {
            return '<li class="selectList-listItem" data-value="' + node.attr('value') + '">' + node.html() + '</li>';
        };

        createListGroup = function (node) {
            return '<li class="selectList-listGroup">' + node.attr('label') + '</li>';
        };

        var updateList = function () {
            createList();
        };

        var getValue = function () {
            return me.opts.select.val();
        };

        var setValue = function (val) {
            me.opts.select.val(val);
            me.list.find('li.selectList-listItem').removeClass('selectList-selected');
            var item = me.list.find('li.selectList-listItem[data-value="' + val + '"]');
            item.addClass('selectList-selected');
            me.selector.html(item.html());
        };

        var getName = function () {
            var val = me.opts.select.val();
            var item = me.list.find('li.selectList-listItem[data-value="' + val + '"]');
            return item.html();
        };

        var getSelectList = function () {
            return me.opts.select;
        };

        var publicObj = {
            getName: function(){
                return getName();
            },
            getValue: function () {
                return getValue();
            },
            setValue: function (val) {
                setValue(val);
            },
            updateList: function () {
                updateList();
            },
            getSelectList: function () {
                return getSelectList();
            }
        };
        init();
        return publicObj;

    };
})();