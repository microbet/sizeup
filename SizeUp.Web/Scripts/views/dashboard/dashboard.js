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
       
        dataLayer.getCity({ id: opts.cityId }, notifier.getNotifier(function (i) { me.data.locations.city = i; }));
        dataLayer.getCityCentroid({ id: opts.cityId }, notifier.getNotifier(function (i) { me.data.cityCentroid = new sizeup.maps.latLng({ lat: i.Lat, lng: i.Lng }); }));
        dataLayer.getCounty({ id: opts.countyId }, notifier.getNotifier(function (i) { me.data.locations.county = i; }));
        if (opts.metroId) {
            dataLayer.getMetro({ id: opts.metroId }, notifier.getNotifier(function (i) { me.data.locations.metro = i; }));
        }
        dataLayer.getState({ id: opts.stateId }, notifier.getNotifier(function (i) { me.data.locations.state = i; }));
        

        var init = function () {
            
            me.reports.push(new sizeup.views.dashboard.revenue({ container: $('#revenue'), locations: me.data.locations, industryId: me.opts.industryId, center: me.data.cityCentroid }));
            me.reports.push(new sizeup.views.dashboard.yearStarted({ container: $('#yearStarted'), locations: me.data.locations, industryId: me.opts.industryId, center: me.data.cityCentroid }));
            me.reports.push(new sizeup.views.dashboard.averageSalary({ container: $('#salary'), locations: me.data.locations, industryId: me.opts.industryId, center: me.data.cityCentroid }));
            me.reports.push(new sizeup.views.dashboard.employees({ container: $('#employees'), industryId: opts.industryId }));
            me.reports.push(new sizeup.views.dashboard.costEffectiveness({ container: $('#costEffectiveness'), industryId: opts.industryId }));
            me.reports.push(new sizeup.views.dashboard.revenuePerCapita({ container: $('#revenuePerCapita'), industryId: opts.industryId }));
            me.reports.push(new sizeup.views.dashboard.turnover({ container: $('#turnover'), industryId: opts.industryId }));
            me.reports.push(new sizeup.views.dashboard.healthcareCost({ container: $('#healthcareCost'), industryId: opts.industryId }));
            me.reports.push(new sizeup.views.dashboard.workersComp({ container: $('#workersComp'), industryId: opts.industryId }));

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