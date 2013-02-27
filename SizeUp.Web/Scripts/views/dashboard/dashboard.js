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

        dataLayer.getPlaceCentroid({ id: opts.report.CurrentPlace.Id }, notifier.getNotifier(function (data) { me.opts.report.MapCenter = data; }));
        dataLayer.getDashboardValues({placeId: opts.report.CurrentPlace.Id, industryId: opts.report.IndustryDetails.Industry.Id}, notifier.getNotifier(function (data) { me.data.dashboardValues = data; }));
        var init = function () {
            
           

            if (!jQuery.isEmptyObject(me.data.dashboardValues)) {
                jQuery.bbq.pushState(me.data.dashboardValues, 1);
                var p = $.extend(true, { placeId: me.opts.report.CurrentPlace.Id, industryId: me.opts.report.IndustryDetails.Industry.Id }, jQuery.bbq.getState());
                dataLayer.setDashboardValues(p);
            }

            me.signinPanels = {};

            
            me.resourceToggle = new sizeup.controls.toggleButton(
                {
                    button: me.container.find('#summaryView'),
                    onClick: function () { toggleAllReports(); }
                });

            me.sessionLoadedBox = new sizeup.controls.flashBox(
               {
                   container: me.container.find('#sessionLoadedBox')
               });

           

            $(window).bind('hashchange', function (e) { hashChanged(e); });


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
            else {
                me.signinPanels['employees'] = new sizeup.views.shared.signin({
                    container: me.container.find('#employees .signinPanel.form')//,
                    //toggle: me.container.find('#employees .header')
                });
                
                me.signinPanels['costEffectiveness'] = new sizeup.views.shared.signin({
                    container: me.container.find('#costEffectiveness .signinPanel.form')//,
                   // toggle: me.container.find('#costEffectiveness .header')
                });

                me.signinPanels['healthcareCost'] = new sizeup.views.shared.signin({
                    container: me.container.find('#healthcareCost .signinPanel.form')//,
                    //toggle: me.container.find('#healthcareCost .header')
                });

                me.signinPanels['workersComp'] = new sizeup.views.shared.signin({
                    container: me.container.find('#workersComp .signinPanel.form')//,
                    //toggle: me.container.find('#workersComp .header')
                });

                me.signinPanels['revenuePerCapita'] = new sizeup.views.shared.signin({
                    container: me.container.find('#revenuePerCapita .signinPanel.form')//,
                    //toggle: me.container.find('#revenuePerCapita .header')
                });

                me.signinPanels['turnover'] = new sizeup.views.shared.signin({
                    container: me.container.find('#turnover .signinPanel.form')//,
                    //toggle: me.container.find('#turnover .header')
                });


                me.container.find('#employees .header').bind('click', 'employees', function (e) {
                    signinFormClicked(e.data);
                });

                me.container.find('#costEffectiveness .header').bind('click', 'costEffectiveness', function (e) {
                    signinFormClicked(e.data);
                });

                me.container.find('#healthcareCost .header').bind('click', 'healthcareCost', function (e) {
                    signinFormClicked(e.data);
                });

                me.container.find('#workersComp .header').bind('click', 'workersComp', function (e) {
                    signinFormClicked(e.data);
                });

                me.container.find('#revenuePerCapita .header').bind('click', 'revenuePerCapita', function (e) {
                    signinFormClicked(e.data);
                });

                me.container.find('#turnover .header').bind('click', 'turnover', function (e) {
                    signinFormClicked(e.data);
                });
            }

            $('#dashboard').removeClass('hidden');
            initAllReports();

                       

            if (!jQuery.isEmptyObject(me.data.dashboardValues)) {
                me.sessionLoadedBox.flash();
            }
        };

        var signinFormClicked = function(data){
            for (var x in me.signinPanels) {
                if (x != data) {
                    me.signinPanels[x].closeForm();
                }
            }
            me.signinPanels[data].toggleSigninForm();
        };

        var hashChanged = function (e) {
            var p = $.extend(true, { placeId: me.opts.report.CurrentPlace.Id, industryId: me.opts.report.IndustryDetails.Industry.Id }, e.getState());
            dataLayer.setDashboardValues(p);
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