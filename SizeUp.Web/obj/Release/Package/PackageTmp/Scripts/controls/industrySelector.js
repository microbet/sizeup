(function () {

    sizeup.core.namespace('sizeup.controls');
    window.sizeup.controls.industrySelector = function (opts) {

        var me = {};
        me.textbox = opts.textbox;
        me.promptText = me.textbox.attr('data-prompt');
        me.maxResults = opts.maxResults || 35;
        me.minLength = opts.minLength || 1;
        me.revertToSelection = opts.revertToSelection || false;
        me.onChange = opts.onChange || function () { };
        me.onBlur = opts.onBlur || function () { };
        me.onFocus = opts.onFocus || function () { };
        me.selection = null;
        me.changed = false;
        me.hasFocus = false;

        var wrap = function (text, q) {
            var qs = $.trim(q).replace('|', '\|').split(' ').join('|');
            text = text.replace(new RegExp(qs, 'gi'), '<strong>' + "$&" + '</strong>');
            return text;
        };

        var init = function () {
            me.container = $('<div class="autoComplete-container"></div>');
            me.textbox.after(me.container);
            me.textbox.detach();
            me.container.append(me.textbox);

            me.textbox.val(me.promptText);
            me.textbox.addClass('blank');
            var id = me.textbox.attr('id');
            if (id) {
                me.container.attr('id', 'autoComplete-' + id);
            }

            me.textbox.autocomplete({
                appendTo: me.container,
                source: function (request, response) {

                    var callback = function (data) {
                        me.container.removeClass('loading');
                        response($.map(data, function (item) {
                            return {
                                label: wrap(item.Name, request.term),
                                value: item
                            };
                        }));
                    };
                    me.container.addClass('loading');
                    sizeup.api.data.findIndustry({ term: request.term, maxResults: me.maxResults }, callback);
                },
                minLength: me.minLength,
                select: function (event, ui) {
                    setSelection(ui.item.value);
                    me.onChange(ui.item.value);
                    me.changed = false;
                    me.hasFocus = false;
                    me.textbox.blur();
                    return false;
                },
                change: function(event, ui){
                    if (ui.item == null && me.changed) {
                        if (!me.revertToSelection) {
                            me.selection = null;
                        }
                        setSelection(me.selection);
                        me.onChange(me.selection);
                        me.changed = false;
                        me.hasFocus = false;
                        me.onBlur(me.selection);
                    }
                    me.changed = false;
                },
                focus: function (event, ui) {
                    return false;
                },
                open: function (event, ui) {
                    $("ul.ui-autocomplete.ui-menu .ui-menu-item:even").addClass('odd');
                }
            }).data('autocomplete')._renderItem = function (ul, item) {
                return $("<li></li>")
                        .data("item.autocomplete", item)
                        .append('<a>' + item.label + '</a>')
                        .appendTo(ul);
            };

            me.container.on('focus', 'input[type=text]', onFocus);
            me.container.on('blur', 'input[type=text]', onBlur);
            me.container.on('keyup', 'input[type=text]', onKeyup);
        };

        var onKeyup = function () {
            me.changed = true;
        };

        var onBlur = function () {
            if ($.trim(me.textbox.val()) == '') {
                if (!me.revertToSelection) {
                    me.selection = null;
                }
                me.hasFocus = false;
                me.changed = false;
                setSelection(me.selection);
            }
            if (!me.changed) {
                me.hasFocus = false;
                me.onBlur(me.selection);
            }
        };

        var onFocus = function () {
            me.hasFocus = true;
            if (me.selection == null) {
                me.textbox.val('');
            }
            else if(!me.changed){
                setTimeout(function () { me.textbox.select(); }, 0);
            }
            me.textbox.removeClass('blank');
            me.onFocus();
        };

        var clearTextbox = function () {
            me.textbox.addClass('blank');
            me.textbox.val(me.promptText);
        };


        var setSelection = function (item) {
            me.changed = false;
            me.selection = item;
            if (item != null) {
                me.textbox.val(item.Name);
                me.textbox.removeClass('blank');
            }
            else {
                clearTextbox();
            }
        };

        var getSelection = function () {
            return me.selection;
        };



        var publicObj = {
            getSelection: function () {
                return getSelection();
            },
            setSelection: function (item) {
                setSelection(item);
            },
            hasFocus: function () {
                return me.hasFocus;
            }
        };
        init();
        return publicObj;
    };
})();




