(function () {
    sizeup.core.namespace('sizeup.charts');
    sizeup.charts.tableChart = function (opts) {

        
        var defaults =
        {
            rowTemplate: '',
            rowContainer: $()
        };
        var templates = new sizeup.core.templates();
        var me = {};
        me.opts = $.extend(true, defaults, opts);
        me.container = opts.container;


        var init = function () {
            var tableString = '';
            var t = function () { return me.opts.rowTemplate };
            for (var x in me.opts.rows) {
                tableString = tableString + templates.bind(t(), me.opts.rows[x]);
            }
            me.opts.rowContainer.html(tableString);
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