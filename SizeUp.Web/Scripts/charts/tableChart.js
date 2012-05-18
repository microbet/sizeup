(function () {
    sizeup.core.namespace('sizeup.charts');
    sizeup.charts.tableChart = function (opts) {

        
        var defaults =
        {
        };
        var me = {};
        me.opts = $.extend(true, defaults, opts);
        me.container = opts.container;
        me.rowContainer = me.container.find('.container');

        var init = function () {
            var tableString = '';
            for (var x in me.opts.rows) {
                tableString = tableString + me.opts.templates.bind(me.opts.templates.get('tableRow'), me.opts.rows[x]);
            }
            me.rowContainer.html(tableString);
        };

   
        var publicObj = {
            getContainer: function () {
                return me.container;
            }
        };
        init();
        return publicObj;

    };
})();