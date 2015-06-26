(function () {

    sizeup.core.namespace('sizeup.controls');
    sizeup.controls.pager = function (opts) {


        var defaults = {
            itemsPerPage: 10,
            totalItems: 0,
            currentPage: 1,
            pagesToShow: 5,
            templates: new sizeup.core.templates(),
            templateId: '',
            onUpdate: function () { }
        };
        var me = {};
        me.opts = $.extend(true, defaults, opts);
        me.container = opts.container;

        var init = function () {
            me.container.removeClass('hidden').hide();
            me.container.find('a').live('click', function (e) {
                var i = $(this);
                var index = i.attr('data-index');
                var data = gotoPage(index);
                me.opts.onUpdate(data);
            });
        };

        

        var getStartEnd = function (page) {
            var totalPages = getTotalPages();
            var data = {
                start: 1 + (page - 1) * me.opts.itemsPerPage,
                end: Math.min(me.opts.totalItems, page * me.opts.itemsPerPage),
                page: page,
                itemsPerPage: me.opts.itemsPerPage,
                totalItems: me.opts.totalItems,
                totalPages: totalPages,
                first: true,
                last: true,
                next: true,
                prev: true,
                pages: []
            };
            if (page === 1) {
                data.first = false;
                data.prev = false;
            }
            if (page === totalPages) {
                data.last = false;
                data.next = false;
            }
            var midPoint = Math.ceil(me.opts.pagesToShow / 2);
            var endPadding = (me.opts.currentPage - midPoint) > 0 ? 0 : -(me.opts.currentPage - midPoint);
            var startPadding = me.opts.currentPage + midPoint <= totalPages ? 0 : midPoint - (totalPages - me.opts.currentPage);
            var startPage = (me.opts.currentPage - midPoint - startPadding) > 0 ? me.opts.currentPage - midPoint - startPadding : 0;
            var endPage = me.opts.currentPage + midPoint + endPadding <= totalPages ? me.opts.currentPage + midPoint + endPadding : totalPages;

            var pagesToShow = endPage - startPage;
            for (var i = 0; i < pagesToShow; i++) {
                var index = i + startPage + 1;
                data.pages[i] = {
                    index: index,
                    seperator: i != pagesToShow - 1,
                    current: index === me.opts.currentPage
                }
            }
            return data;
        };

        var getPageData = function (index) {
            if (index === 'first') {
                data = getStartEnd(1);
            }
            else if (index === 'last') {
                data = getStartEnd(getTotalPages());
            }
            else if (index === 'next') {
                data = getStartEnd(me.opts.currentPage + 1);
            }
            else if (index === 'prev') {
                data = getStartEnd(me.opts.currentPage - 1);
            }
            else if (!isNaN(index)) {
                data = getStartEnd(index);
            }
            else {
                data = getStartEnd(me.opts.currentPage);
            }
            return data;
        };





        var getTotalPages = function () {
            return Math.ceil(me.opts.totalItems / me.opts.itemsPerPage);
        };

        var setState = function (obj) {
            me.opts.totalItems = parseInt(obj.Count);
            me.opts.currentPage = parseInt(obj.Page);
            bindTemplate();
        };

        var setItemsPerPage = function (count) {
            me.opts.itemsPerPage = count;
            bindTemplate();
        };

        var bindTemplate = function () {
            var template = me.opts.templates.get(me.opts.templateId);
            var data = getPageData();
            template = me.opts.templates.bind(template, data);
            me.container.html(template);
        };

        var gotoPage = function (index) {
            var data = getPageData(index);
            me.opts.currentPage = data.page;
            return data;
        }


        var publicObj = {
            gotoPage: function (index) {
                return gotoPage(index);
            },
            setState: function (obj) {
                setState(obj);
            },
            setItemsPerPage: function(count){
                setItemsPerPage(count);
            },
            getPageData: function (index) {
                return getPageData(index);
            },
            getContainer: function () {
                return me.container;
            }
        };
        init();
        return publicObj;
    };

})();

