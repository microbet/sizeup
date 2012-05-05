(function () {
    sizeup.core.namespace('sizeup.views.dashboard.dashboard');
    sizeup.views.dashboard.dashboard = function (opts) {

        var me = {};
       /* var dataLayer = new sizeup.core.data();
        var notifier = new sizeup.core.notifier(function () { init(); });
        */
        me.opts = opts;
        me.data = {};
        me.reports = [];
       /* me.data.name = opts.name;

        dataLayer.getIndustry({id: opts.industryId}, notifier.getNotifier(function (i) { me.data.industry = i; }));
        dataLayer.getCity({id: opts.cityId}, notifier.getNotifier(function (i) { me.data.city = i; }));
        */

        var init = function () {
            
            me.reports.push(new sizeup.views.dashboard.revenue({ container: $('#revenue') }));
            me.reports.push(new sizeup.views.dashboard.yearStarted({ container: $('#yearStarted') }));
            me.reports.push(new sizeup.views.dashboard.averageSalary({ container: $('#salary') }));
            me.reports.push(new sizeup.views.dashboard.employees({ container: $('#employees') }));
            me.reports.push(new sizeup.views.dashboard.costEffectiveness({ container: $('#costEffectiveness') }));
            me.reports.push(new sizeup.views.dashboard.revenuePerCapita({ container: $('#revenuePerCapita') }));
            me.reports.push(new sizeup.views.dashboard.turnover({ container: $('#turnover') }));
            me.reports.push(new sizeup.views.dashboard.healthcareCost({ container: $('#healthcareCost') }));
            me.reports.push(new sizeup.views.dashboard.workersComp({ container: $('#workersComp') }));

            $('#dashboard').removeClass('hidden');
            initAllReports();
        };


        var initAllReports = function () {
            for (var x = 0; x < me.reports.length; x++) {
                var step = 250;
                me.reports[x].fadeInPrompt(step * x);
            }
        };

      
        var publicObj = {

        };
        init();
        return publicObj;
        
    };
})();