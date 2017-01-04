(function () {

    sizeup.core.namespace('sizeup.controls');
    window.sizeup.controls.industryMultiSelector = function (opts) {

        var me = {};
        me.textbox = opts.textbox;
        me.promptText = me.textbox.attr('data-prompt');
        me.maxResults = opts.maxResults || 35;
        me.minLength = opts.minLength || 1;
        me.revertToSelection = opts.revertToSelection || false;
        me.onChange = opts.onChange || function () { };
        me.onBlur = opts.onBlur || function () { };
        me.onFocus = opts.onFocus || function () { };
        me.selection = [];
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
            me.textbox
                      .on("keydown", function (event) {
                          if (event.keyCode === $.ui.keyCode.TAB &&
                              $(this).autocomplete("instance").menu.active) {
                              event.preventDefault();
                          }
                      })
                          .autocomplete({
                              minLength: me.minLength,
                              appendTo: me.container,
                              source: function (request, response) {
                                  var term = request.term.split(',').pop();
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
                                  sizeup.api.data.findIndustry({ term: term, maxResults: me.maxResults }, callback);
                              },
                              focus: function () {
                                  // prevent value inserted on focus
                                  return false;
                              },
                              select: function (event, ui) {
                                  setSelection(ui.item.value);
                                  me.onChange(ui.item.value);
                                  me.changed = false;
                                  me.hasFocus = false;
                                  me.textbox.blur();
                                  return false;

                                  //var terms = split(this.value);
                                  //// remove the current input
                                  //terms.pop();
                                  //// add the selected item
                                  //terms.push(ui.item.value);
                                  //// add placeholder to get the comma-and-space at the end
                                  //terms.push("");
                                  //this.value = terms.join(", ");
                                  //return false;
                              },
                              change: function (event, ui) {
                                  updateSelectionArray();
                                  if (ui.item == null && me.changed) {
                                      if (!me.revertToSelection) {
                                          
                                      }
                                      setSelection(me.selection);
                                      me.onChange(me.selection);
                                      me.changed = false;
                                      me.hasFocus = false;
                                      me.onBlur(me.selection);
                                  }
                                  me.changed = false;
                              }
                          })
                      .data('autocomplete')._renderItem = function (ul, item) {
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
            updateSelectionArray();
            if ($.trim(me.textbox.val()) == '') {
                if (!me.revertToSelection) {
                    //me.selection = null;
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
            if (me.selection.length ==0) {
                me.textbox.val('');
            }
            else if (!me.changed) {
                setTimeout(function () { me.textbox.select(); }, 0);
            }
            me.textbox.removeClass('blank');
            me.onFocus();
        };

        var clearTextbox = function () {
            me.textbox.addClass('blank');
            me.selection = [];
            me.textbox.val(me.promptText);
        };


        var setSelection = function (item) {
            updateSelectionArray();
            if ($.isArray(item) || item == null || $.inArray(item.Id, $.map(me.selection, function (e, i) { return e.Id })) == 0) return; 
            me.changed = false;
            me.selection.push(item);
            if (item != null) {
                me.textbox.val($.map(me.selection, function (e, i) { return e.Name; }).join(',') + ', ');
                me.textbox.removeClass('blank');
            }
            else {
                clearTextbox();
            }
        };

        var setMultipleSelections = function (items)
        {
            if (!$.isArray(items)) return;
            $.each(items, function (i, e) {
                setSelection(e);
            });
        }

        var getSelection = function () {
            return me.selection;
        };

        var updateSelectionArray = function () {
            var selectedNames = me.textbox.val().trim().split(',');
            if (selectedNames.length == 0) me.selection = [];

            $.each(me.selection, function (i, e, a) {
                if ($.inArray(e.Name, selectedNames) == -1)
                    me.selection.splice(i, 1);
            });
        }

        var publicObj = {
            getSelection: function () {
                return getSelection();
            },
            setSelection: function (item) {
                setSelection(item);
            },
            setMultipleSelections: function (item) {
                setMultipleSelections(item);
            },
            hasFocus: function () {
                return me.hasFocus;
            }
        };
        var split = function (val) {
            return val.split(/,\s*/);
        }
        var extractLast = function (term) {
            return split(term).pop();
        }
        init();
        return publicObj;
    };
})();




