(function () {
    sizeup.core.namespace('sizeup.views.dashboard.dashboard');
    sizeup.views.dashboard.dashboard = function (opts) {

        var me = {};
        var dataLayer = new sizeup.core.data();
        
        me.opts = opts;
        me.data = {};
        me.data.locations = {};
        me.reports = [];
        me.container = $('#dashboard');
        me.reportsCollapsed = false;
        var init = function () {
            
            me.resourceToggle = new sizeup.controls.toggleButton(
                {
                    button: me.container.find('#summaryView'),
                    onClick: function () { toggleAllReports(); }
                });


            me.reports.push(new sizeup.views.dashboard.revenue({ container: $('#revenue'), report: me.opts.report }));
            me.reports.push(new sizeup.views.dashboard.yearStarted({ container: $('#yearStarted'), report: me.opts.report }));
            me.reports.push(new sizeup.views.dashboard.averageSalary({ container: $('#salary'), report: me.opts.report }));
            if (me.opts.isAuthenticated) {
               /* me.reports.push(new sizeup.views.dashboard.employees({ container: $('#employees'), report: me.opts.report }));
                me.reports.push(new sizeup.views.dashboard.costEffectiveness({ container: $('#costEffectiveness'), report: me.opts.report }));
                me.reports.push(new sizeup.views.dashboard.revenuePerCapita({ container: $('#revenuePerCapita'), report: me.opts.report }));
                me.reports.push(new sizeup.views.dashboard.turnover({ container: $('#turnover'), report: me.opts.report }));
                me.reports.push(new sizeup.views.dashboard.healthcareCost({ container: $('#healthcareCost'), report: me.opts.report }));
                me.reports.push(new sizeup.views.dashboard.workersComp({ container: $('#workersComp'), report: me.opts.report }));*/
            }

            $('#dashboard').removeClass('hidden');
            initAllReports();
        };


        var toggleAllReports = function () {
            for (var x = 0; x < me.reports.length; x++) {
                if (me.reportsCollapsed) {
                    me.reports[x].expandReport();
                }
                else {
                    me.reports[x].collapseReport();
                }
            }
            me.reportsCollapsed = !me.reportsCollapsed;
        };

        var initAllReports = function () {
            for (var x = 0; x < me.reports.length; x++) {
                var step = 250;
                var f = function(report, delay){
                    return function(){
                        report.setupReport();
                    }
                };
                setTimeout(f(me.reports[x]), step*x);
            }
        };

      
        var publicObj = {

        };
        init();
        return publicObj;
        
    };
})();