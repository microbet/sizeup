(function () {
    sizeup.core.namespace('sizeup.views.dashboard.dashboard');
    sizeup.views.dashboard.dashboard = function (opts) {

        var me = {};
        var dataLayer = new sizeup.core.data();
        var notifier = new sizeup.core.notifier(function () { init();});

        var reportIndexes = [
            'revenue',
            'yearStarted',
            'salary',
            'employees',
            'costEffectiveness',
            'healthcareCost',
            'workersComp',
            'revenuePerCapita',
            'turnover'
        ];

        me.opts = opts;
        me.data = {};
        me.data.locations = {};
        me.reports = [];
        me.container = $('#dashboard');
        me.reportsCollapsed = false;

        dataLayer.getCityCentroid({ id: opts.report.CurrentPlace.Id }, notifier.getNotifier(function (data) { me.opts.report.MapCenter = data; }));
        var init = function () {
            
            me.resourceToggle = new sizeup.controls.toggleButton(
                {
                    button: me.container.find('#summaryView'),
                    onClick: function () { toggleAllReports(); }
                });


            me.reports['revenue'] = new sizeup.views.dashboard.revenue({ container: $('#revenue'), report: me.opts.report });
            me.reports['yearStarted'] = new sizeup.views.dashboard.yearStarted({ container: $('#yearStarted'), report: me.opts.report });
            me.reports['salary'] = new sizeup.views.dashboard.averageSalary({ container: $('#salary'), report: me.opts.report });
            if (me.opts.isAuthenticated) {
                me.reports['employees'] = new sizeup.views.dashboard.employees({ container: $('#employees'), report: me.opts.report });
                me.reports['costEffectiveness'] = new sizeup.views.dashboard.costEffectiveness({ container: $('#costEffectiveness'), report: me.opts.report });
                me.reports['healthcareCost'] = new sizeup.views.dashboard.healthcareCost({ container: $('#healthcareCost'), report: me.opts.report });
                me.reports['workersComp'] = new sizeup.views.dashboard.workersComp({ container: $('#workersComp'), report: me.opts.report });
                me.reports['revenuePerCapita'] = new sizeup.views.dashboard.revenuePerCapita({ container: $('#revenuePerCapita'), report: me.opts.report });
                me.reports['turnover'] = new sizeup.views.dashboard.turnover({ container: $('#turnover'), report: me.opts.report });
            }

            //revenuePerCapita.setupReport();
            

            $('#dashboard').removeClass('hidden');
            initAllReports();
        };


        var toggleAllReports = function () {
            for (var x in me.reports) {
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
            
            for (var x = 0; x < reportIndexes.length; x++) {
                var step = 250;
                var f = function(report, delay){
                    return function(){
                        report.setupReport();
                    }
                };
                if (me.reports[reportIndexes[x]]) {
                    setTimeout(f(me.reports[reportIndexes[x]]), step * x);
                }
            }
        };

      
        var publicObj = {

        };
        return publicObj;
        
    };
})();