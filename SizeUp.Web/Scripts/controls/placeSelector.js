(function () {

    sizeup.core.namespace('sizeup.controls');
    window.sizeup.controls.placeSelector = function (opts) {

        var me = {};
        var dataLayer = new sizeup.core.data();
        me.textbox = opts.textbox;
        me.promptText = me.textbox.attr('data-prompt');
        me.maxResults = opts.maxResults || 35;
        me.minLength = opts.minLength || 1;
        me.onChange = opts.onChange || function () { };
        me.selection = null;
       


        var wrap = function (text, q) {
            var qs = $.trim(q).replace('|', '\|').split(' ').join('|');
            text = text.replace(new RegExp(qs, 'gi'), '<strong>' + "$&" + '</strong>');
            return text;
        };

        var init = function () {
            me.textbox.val(me.promptText);
            me.textbox.addClass('blank');
        };

        var onBlur = function () {
            if ($.trim(me.textbox.val()) == '' || $.trim(me.textbox.val()) == me.promptText) {
                me.textbox.val(me.promptText);
                me.textbox.addClass('blank');
            }
        };

        var onFocus = function () {
            if (me.selection == null && me.textbox.val() == me.promptText) {
                me.textbox.val('');
            }
            else {
                me.textbox.select();
            }
            me.textbox.removeClass('blank');
        };



        var onSelection = function (item) {
            setSelection(item);
        };

        var setSelection = function (item) {
            me.selection = item;
            if (item != null) {
                me.textbox.val(item.City.Name + ', ' + item.State.Abbreviation);
                me.textbox.removeClass('blank');
            }
            else {
                me.textbox.val('');
                me.textbox.addClass('blank');
            }
        };

        var getSelection = function () {
            return me.selection;
        };


        me.textbox.focus(onFocus);
        me.textbox.blur(onBlur);


        me.textbox.autocomplete({
            appendTo: me.textbox.parent(),
            source: function (request, response) {

                var callback = function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: wrap(item.DisplayName, request.term),
                            value: item
                        };
                    }));
                };

                dataLayer.searchPlaces({ term: request.term, maxResults: me.maxResults }, callback);
            },
            minLength: me.minLength,
            select: function (event, ui) {
                onSelection(ui.item.value);
                me.onChange(ui.item.value);
                return false;
            },
            focus: function (event, ui) {
                return false;
            },
            open: function (event, ui) {
                $("ul.ui-autocomplete.ui-menu .ui-menu-item:even").addClass('odd');
            },
        }).data('autocomplete')._renderItem = function (ul, item) {
            return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append('<a>' + item.label + '</a>')
                    .appendTo(ul);
        };


        var publicObj = {
            getSelection: function () {
                return getSelection();
            },
            setSelection: function (item) {
                setSelection(item);
            }
        };
        init();
        return publicObj;
    };
})();













