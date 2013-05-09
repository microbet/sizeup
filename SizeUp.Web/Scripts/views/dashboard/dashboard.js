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

        dataLayer.getCentroid({ id: opts.currentInfo.CurrentPlace.Id, granularity: 'Place' }, notifier.getNotifier(function (data) { me.opts.MapCenter = data; }));
        dataLayer.getBoundingBox({ id: opts.currentInfo.CurrentPlace.Id, granularity: 'Place' }, notifier.getNotifier(function (data) { me.opts.BoundingBox = data; }));
        dataLayer.getDashboardValues({ placeId: opts.currentInfo.CurrentPlace.Id, industryId: opts.currentInfo.CurrentIndustry.Id }, notifier.getNotifier(function (data) { me.data.dashboardValues = data; }));
        var init = function () {
            
            me.data.activeIndustry = me.opts.currentInfo.CurrentIndustry;
            me.data.activePlace = me.opts.currentInfo.CurrentPlace;

            if (!jQuery.isEmptyObject(me.data.dashboardValues)) {
                jQuery.bbq.pushState(me.data.dashboardValues, 1);
                var p = $.extend(true, { placeId: me.opts.currentInfo.CurrentPlace.Id, industryId: me.opts.currentInfo.CurrentIndustry.Id }, jQuery.bbq.getState());
                dataLayer.setDashboardValues(p);
            }

            me.signinPanels = {};
            me.content = {};
            
            me.resourceToggle = new sizeup.controls.toggleButton(
                {
                    button: me.container.find('#summaryView'),
                    onClick: function () { toggleAllReports(); }
                });

            me.sessionLoadedBox = new sizeup.controls.flashBox(
               {
                   container: me.container.find('#sessionLoadedBox')
               });


            me.content.industryBox = me.container.find('#industryBox').hide().removeClass('hidden');
            me.content.changeIndustry = me.container.find('#changeIndustry');

            me.content.industrySelector = sizeup.controls.industrySelector({
                textbox: me.content.industryBox,
                revertToSelection: true,
                onChange: function (item) { onIndustryChange(item); },
                onBlur: function () { industryBoxBlur(); }
            });

            me.content.placeBox = me.container.find('#placeBox').hide().removeClass('hidden');
            me.content.changePlace = me.container.find('#changePlace');

            me.content.placeSelector = sizeup.controls.placeSelector({
                textbox: me.content.placeBox,
                revertToSelection: true,
                onChange: function (item) { onPlaceChange(item); },
                onBlur: function () { placeBoxBlur(); }
            });




            me.content.industrySelector.setSelection(me.data.activeIndustry);
            me.content.changeIndustry.click(changeIndustryClicked);

            me.content.placeSelector.setSelection(me.data.activePlace);
            me.content.changePlace.click(changePlaceClicked);




            $(window).bind('hashchange', function (e) { hashChanged(e); });


            me.reports['revenue'] = new sizeup.views.dashboard.revenue({ container: $('#revenue'), report: me.opts.currentInfo, centroid: me.opts.MapCenter, boundingBox: me.opts.BoundingBox });
            me.reports['yearStarted'] = new sizeup.views.dashboard.yearStarted({ container: $('#yearStarted'), report: me.opts.currentInfo, centroid: me.opts.MapCenter, boundingBox: me.opts.BoundingBox });
            me.reports['salary'] = new sizeup.views.dashboard.averageSalary({ container: $('#salary'), report: me.opts.currentInfo, centroid: me.opts.MapCenter, boundingBox: me.opts.BoundingBox });
            if (me.opts.isAuthenticated) {
                me.reports['employees'] = new sizeup.views.dashboard.employees({ container: $('#employees'), report: me.opts.currentInfo, centroid: me.opts.MapCenter, boundingBox: me.opts.BoundingBox });
                me.reports['costEffectiveness'] = new sizeup.views.dashboard.costEffectiveness({ container: $('#costEffectiveness'), report: me.opts.currentInfo, centroid: me.opts.MapCenter, boundingBox: me.opts.BoundingBox });
                me.reports['healthcareCost'] = new sizeup.views.dashboard.healthcareCost({ container: $('#healthcareCost'), report: me.opts.currentInfo, centroid: me.opts.MapCenter, boundingBox: me.opts.BoundingBox });
                me.reports['workersComp'] = new sizeup.views.dashboard.workersComp({ container: $('#workersComp'), report: me.opts.currentInfo, centroid: me.opts.MapCenter, boundingBox: me.opts.BoundingBox });
                me.reports['revenuePerCapita'] = new sizeup.views.dashboard.revenuePerCapita({ container: $('#revenuePerCapita'), report: me.opts.currentInfo, centroid: me.opts.MapCenter, boundingBox: me.opts.BoundingBox });
                me.reports['turnover'] = new sizeup.views.dashboard.turnover({ container: $('#turnover'), report: me.opts.currentInfo, centroid: me.opts.MapCenter, boundingBox: me.opts.BoundingBox });
            }
            else {
                me.signinPanels['employees'] = new sizeup.views.shared.signin({
                    container: me.container.find('#employees .signinPanel.form')
                });
                
                me.signinPanels['costEffectiveness'] = new sizeup.views.shared.signin({
                    container: me.container.find('#costEffectiveness .signinPanel.form')
                });

                me.signinPanels['healthcareCost'] = new sizeup.views.shared.signin({
                    container: me.container.find('#healthcareCost .signinPanel.form')
                });

                me.signinPanels['workersComp'] = new sizeup.views.shared.signin({
                    container: me.container.find('#workersComp .signinPanel.form')
                });

                me.signinPanels['revenuePerCapita'] = new sizeup.views.shared.signin({
                    container: me.container.find('#revenuePerCapita .signinPanel.form')
                });

                me.signinPanels['turnover'] = new sizeup.views.shared.signin({
                    container: me.container.find('#turnover .signinPanel.form')
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

        var getParameters = function () {
            var params = jQuery.bbq.getState();
            return params;
        };


        var changeIndustryClicked = function () {
            me.content.changeIndustry.hide();
            me.content.industryBox.show();
            me.content.industryBox.focus();
        };

        var onIndustryChange = function (i) {
            if (i != null && i.Id != me.data.activeIndustry.Id) {
                me.content.changeIndustry.html(i.Name);
                var p = { industry: me.data.activeIndustry.Name };
                new sizeup.core.analytics().dashboardIndustryChanged(p);
                var params = getParameters();
                var url = document.location.pathname;
                url = url.replace(me.data.activeIndustry.SEOKey, i.SEOKey);
                url = jQuery.param.fragment(url, params, 2);
                document.location = url;
            }
            else {
                me.content.changeIndustry.show();
                me.content.industryBox.hide();
                me.content.industrySelector.setSelection(me.data.activeIndustry);
            }
        };

        var industryBoxBlur = function () {
            me.content.changeIndustry.show();
            me.content.industryBox.hide();           
        };


        var changePlaceClicked = function () {
            me.content.changePlace.hide();
            me.content.placeBox.show();
            me.content.placeBox.focus();
        };

        var onPlaceChange = function (i) {
            if (i != null && i.Id != me.data.activePlace.Id) {
                me.content.changePlace.html(i.City.Name + ', ' + i.State.Abbreviation);
                var p = { place: me.data.activePlace.City.Name + ', ' + me.data.activePlace.State.Abbreviation };
                new sizeup.core.analytics().dashboardPlaceChanged(p);
                var params = getParameters();
                var url = document.location.href;
                url = url.substring(0, url.indexOf('dashboard'));
                url = url + 'dashboard/' + i.State.SEOKey + '/' + i.County.SEOKey + '/' + i.City.SEOKey + '/' + me.data.activeIndustry.SEOKey + '/';
                url = jQuery.param.fragment(url, params, 2);
                document.location = url;
            }
            else {
                me.content.placeSelector.setSelection(me.data.activePlace);
            }
        };

        var placeBoxBlur = function () {
            me.content.changePlace.show();
            me.content.placeBox.hide();
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
            var p = $.extend(true, { placeId: me.opts.currentInfo.CurrentPlace.Id, industryId: me.opts.currentInfo.CurrentIndustry.Id }, e.getState());
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