(function () {
    sizeup.core.namespace('sizeup.core');
    sizeup.core.templates = function (dom) {

        var templateList = {};
        if (dom) {
            dom.find('[data-template]')
                        .remove()
                        .each(function () {
                            var i = $(this);
                            var label = i.attr('data-template');
                            i.removeAttr('data-template');
                            var text = i.wrap('<div></div>').parent().html();
                            templateList[label] = text;
                        });
        }

        var bind = function (html, obj) {
            var output = $('<div></div>').html(Mustache.to_html(html, obj));
            var overrides = [
                'data-override-href',
                'data-override-src',
                'data-override-onclick',
                'data-override-ondblclick',
                'data-override-onmouseover',
                'data-override-onmouseout',
                'data-override-style'
            ];
            for (var x in overrides) {
                var index = '[' + overrides[x] + ']';
                var attr = overrides[x].replace('data-override-', '');
                output.find(index).each(function () {
                    var i = $(this);
                    i.attr(attr, i.attr(overrides[x]));
                    i.removeAttr(overrides[x]);
                });
            }
            return output.html();
        };


        var publicObj = {
            get: function (template) {
                return templateList[template];
            },
            bind: function (html, obj) {
                return bind(html, obj);
            }
        };
        return publicObj;

    };
})();