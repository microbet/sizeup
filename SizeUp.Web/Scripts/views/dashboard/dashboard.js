(function () {
    sizeup.core.namespace('sizeup.views.dashboard.dashboard');
    sizeup.views.dashboard.dashboard = function (opts) {

        var me = {};
        var dataLayer = new sizeup.core.data();
        var notifier = new sizeup.core.notifier(function () { init(); });
        
        me.opts = opts;
        me.data = {};
        me.data.locations = {};
        me.reports = [];
       
        dataLayer.getCityCentroid({ id: opts.Locations.City.Id }, notifier.getNotifier(function (i) { me.data.cityCentroid = new sizeup.maps.latLng({ lat: i.Lat, lng: i.Lng }); }));

        var init = function () {
            
            me.reports.push(new sizeup.views.dashboard.revenue({ container: $('#revenue'), report: me.opts, center: me.data.cityCentroid }));
            me.reports.push(new sizeup.views.dashboard.yearStarted({ container: $('#yearStarted'), report: me.opts, center: me.data.cityCentroid }));
            me.reports.push(new sizeup.views.dashboard.averageSalary({ container: $('#salary'), report: me.opts, center: me.data.cityCentroid }));
            me.reports.push(new sizeup.views.dashboard.employees({ container: $('#employees'), report: me.opts, }));
            me.reports.push(new sizeup.views.dashboard.costEffectiveness({ container: $('#costEffectiveness'), report: me.opts, }));
            me.reports.push(new sizeup.views.dashboard.revenuePerCapita({ container: $('#revenuePerCapita'), report: me.opts, }));
            me.reports.push(new sizeup.views.dashboard.turnover({ container: $('#turnover'), report: me.opts, }));
            me.reports.push(new sizeup.views.dashboard.healthcareCost({ container: $('#healthcareCost'), report: me.opts, }));
            me.reports.push(new sizeup.views.dashboard.workersComp({ container: $('#workersComp'), report: me.opts, }));

            $('#dashboard').removeClass('hidden');
            initAllReports();
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
        return publicObj;
        
    };
})();