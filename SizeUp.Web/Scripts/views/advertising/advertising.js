(function () {
    sizeup.core.namespace('sizeup.views.advertising');
    sizeup.views.advertising.advertising = function (opts) {

        var defaults = {
            mapSettings: sizeup.maps.mapOptions.getDefaults(),
            styles: sizeup.maps.mapStyles.getDefaults(),
            colors: [],
            overlays: [],
            slideTime: 500,
            filterOptions: {
                distance: 20,
                attribute: 'TotalRevenue',
                sort: 'desc'
            }
        };
        var me = {};
        me.container = $('#advertising');
        var dataLayer = new sizeup.core.data();
        var templates = new sizeup.core.templates(me.container);
        var notifier = new sizeup.core.notifier(function () { init(); });

        me.opts = $.extend(true, defaults, opts);
       
        me.data = {};

        

        dataLayer.getCityCentroid({ id: opts.CurrentPlace.City.Id }, notifier.getNotifier(function (data) { me.data.CityCenter = new sizeup.maps.latLng({lat:data.Lat, lng: data.Lng}); }));
        var init = function () {
            
            var params = jQuery.bbq.getState();
            jQuery.bbq.pushState($.extend(true, me.opts.filterOptions, params));

            me.content = {};
            me.content.container = me.container.find('.content').hide().removeClass('hidden');

            me.content.map = new sizeup.maps.map({
                container: me.content.container.find('.mapContent .map'),
                mapSettings: me.opts.mapSettings,
                styles: me.opts.styles
            });

            me.content.map.setCenter(me.data.CityCenter);

            me.content.filterSettingsButton = me.content.container.find('#filterSettingsButton');
            me.content.filterSettingsButton.click(function () { filterSettingsButtonClicked(); });

            me.content.optionMenu = me.content.container.find('#optionMenu');
            me.content.optionMenu.chosen();

            me.content.list = {};
            me.content.list.container = me.content.container.find('.listWrapper');

            me.content.list.body = me.content.list.container.find('.results');


            me.filterSettings = {};
            me.filterSettings.container = me.container.find('#filterSettings').hide().removeClass('hidden');
            me.filterSettings.submitButton = me.filterSettings.container.find('.submit');
            me.filterSettings.cancelButton = me.filterSettings.container.find('.cancel');


            me.filterSettings.submitButton.click(function () { submitClicked(); });
            me.filterSettings.cancelButton.click(function () { cancelClicked(); });


            initFilterSliders();


            me.pageLoader = me.container.find('.page.loading');
            me.listLoader = me.container.find('.list.loading').hide().removeClass('hidden');
            me.pageLoader.hide();
            me.content.container.show();

            loadReport();
        };

        var initFilterSliders = function () {
            var params = getParameters();
            me.filterSettings.sliders = {};
            me.filterSettings.sliders['distance'] = new sizeup.controls.slider({
                container: me.filterSettings.container.find('#distance'),
                value: params['distance'],
                min: 1,
                max: 150,
                range: 'min'
            });

            me.filterSettings.sliders['bachelorOrHigher'] = new sizeup.controls.slider({
                container: me.filterSettings.container.find('#bachelorOrHigher'),
                value: params['bachelorOrHigher'],
                min: 0,
                max: 95,
                range: 'max'
            });

            me.filterSettings.sliders['highSchoolOrHigher'] = new sizeup.controls.slider({
                container: me.filterSettings.container.find('#highSchoolOrHigher'),
                value: params['highSchoolOrHigher'],
                min: 0,
                max: 98,
                range: 'max'
            });

            me.filterSettings.sliders['whiteCollar'] = new sizeup.controls.slider({
                container: me.filterSettings.container.find('#whiteCollar'),
                value: params['whiteCollar'],
                min: 0,
                max: 95,
                range: 'max'
            });


            me.filterSettings.sliders['averageRevenue'] = new sizeup.controls.rangeSlider({
                container: me.filterSettings.container.find('#averageRevenue'),
                values: params['averageRevenue'],
                min: 0,
                max: 10,
                mapping: [
                    { range: { min: 0, max: 1 }, mappedValue: 50000, mappedLabel: '$50,000' },
                    { range: { min: 1, max: 2 }, mappedValue: 100000, mappedLabel: '$100,000' },
                    { range: { min: 2, max: 3 }, mappedValue: 250000, mappedLabel: '$250,000' },
                    { range: { min: 3, max: 4 }, mappedValue: 500000, mappedLabel: '$500,000' },
                    { range: { min: 4, max: 5 }, mappedValue: 750000, mappedLabel: '$750,000' },
                    { range: { min: 5, max: 6 }, mappedValue: 1000000, mappedLabel: '$1 million' },
                    { range: { min: 6, max: 7 }, mappedValue: 2500000, mappedLabel: '$2.5 million' },
                    { range: { min: 7, max: 8 }, mappedValue: 5000000, mappedLabel: '$5 million' },
                    { range: { min: 8, max: 9 }, mappedValue: 7500000, mappedLabel: '$7.5 million' },
                    { range: { min: 9, max: 10 }, mappedValue: 10000000, mappedLabel: '$10 million' },
                    { range: { min: 10, max: 11 }, mappedValue: 50000000, mappedLabel: '$50 million+' }
                ]
            });


            me.filterSettings.sliders['totalRevenue'] = new sizeup.controls.rangeSlider({
                container: me.filterSettings.container.find('#totalRevenue'),
                values: params['totalRevenue'],
                min: 0,
                max: 10,
                mapping: [
                    { range: { min: 0, max: 1 }, mappedValue: 100000, mappedLabel: '$100,000' },
                    { range: { min: 1, max: 2 }, mappedValue: 500000, mappedLabel: '$500,000' },
                    { range: { min: 2, max: 3 }, mappedValue: 1000000, mappedLabel: '$1 million' },
                    { range: { min: 3, max: 4 }, mappedValue: 5000000, mappedLabel: '$5 million' },
                    { range: { min: 4, max: 5 }, mappedValue: 10000000, mappedLabel: '$10 million' },
                    { range: { min: 5, max: 6 }, mappedValue: 50000000, mappedLabel: '$50 million' },
                    { range: { min: 6, max: 7 }, mappedValue: 100000000, mappedLabel: '$100 million' },
                    { range: { min: 7, max: 8 }, mappedValue: 500000000, mappedLabel: '$500 million' },
                    { range: { min: 8, max: 9 }, mappedValue: 1000000000, mappedLabel: '$1 billion' },
                    { range: { min: 9, max: 10 }, mappedValue: 50000000000, mappedLabel: '$50 billion' },
                    { range: { min: 10, max: 11 }, mappedValue: 100000000000, mappedLabel: '$100 billion+' }
                ]
            });

            me.filterSettings.sliders['totalEmployees'] = new sizeup.controls.rangeSlider({
                container: me.filterSettings.container.find('#totalEmployees'),
                values: params['totalEmployees'],
                min: 0,
                max: 10,
                mapping: [
                    { range: { min: 0, max: 1 }, mappedValue: 10, mappedLabel: '10' },
                    { range: { min: 1, max: 2 }, mappedValue: 50, mappedLabel: '50' },
                    { range: { min: 2, max: 3 }, mappedValue: 100, mappedLabel: '100' },
                    { range: { min: 3, max: 4 }, mappedValue: 500, mappedLabel: '500' },
                    { range: { min: 4, max: 5 }, mappedValue: 1000, mappedLabel: '1,000' },
                    { range: { min: 5, max: 6 }, mappedValue: 5000, mappedLabel: '5,000' },
                    { range: { min: 6, max: 7 }, mappedValue: 10000, mappedLabel: '10,000' },
                    { range: { min: 7, max: 8 }, mappedValue: 50000, mappedLabel: '50,000' },
                    { range: { min: 8, max: 9 }, mappedValue: 100000, mappedLabel: '100,000' },
                    { range: { min: 9, max: 10 }, mappedValue: 500000, mappedLabel: '500,000' },
                    { range: { min: 10, max: 11 }, mappedValue: 1000000, mappedLabel: '1 million+' }
                ]
            });

            me.filterSettings.sliders['revenuePerCapita'] = new sizeup.controls.rangeSlider({
                container: me.filterSettings.container.find('#revenuePerCapita'),
                values: params['revenuePerCapita'],
                min: 0,
                max: 10,
                mapping: [
                    { range: { min: 0, max: 1 }, mappedValue: 5, mappedLabel: '$5' },
                    { range: { min: 1, max: 2 }, mappedValue: 10, mappedLabel: '$10' },
                    { range: { min: 2, max: 3 }, mappedValue: 50, mappedLabel: '$50' },
                    { range: { min: 3, max: 4 }, mappedValue: 100, mappedLabel: '$100' },
                    { range: { min: 4, max: 5 }, mappedValue: 500, mappedLabel: '$500' },
                    { range: { min: 5, max: 6 }, mappedValue: 1000, mappedLabel: '$1,000' },
                    { range: { min: 6, max: 7 }, mappedValue: 2500, mappedLabel: '$2,500' },
                    { range: { min: 7, max: 8 }, mappedValue: 5000, mappedLabel: '$5,000' },
                    { range: { min: 8, max: 9 }, mappedValue: 7500, mappedLabel: '$7,500' },
                    { range: { min: 9, max: 10 }, mappedValue: 10000, mappedLabel: '$10,000' },
                    { range: { min: 10, max: 11 }, mappedValue: 15000, mappedLabel: '$15,000+' }
                ]
            });


            me.filterSettings.sliders['householdIncome'] = new sizeup.controls.rangeSlider({
                container: me.filterSettings.container.find('#householdIncome'),
                values: params['householdIncome'],
                min: 0,
                max: 9,
                mapping: [
                    { range: { min: 0, max: 1 }, mappedValue: 10000, mappedLabel: '$10,000' },
                    { range: { min: 1, max: 2 }, mappedValue: 20000, mappedLabel: '$20,000' },
                    { range: { min: 2, max: 3 }, mappedValue: 30000, mappedLabel: '$30,000' },
                    { range: { min: 3, max: 4 }, mappedValue: 40000, mappedLabel: '$40,000' },
                    { range: { min: 4, max: 5 }, mappedValue: 50000, mappedLabel: '$50,000' },
                    { range: { min: 5, max: 6 }, mappedValue: 75000, mappedLabel: '$75,000' },
                    { range: { min: 6, max: 7 }, mappedValue: 100000, mappedLabel: '$100,000' },
                    { range: { min: 7, max: 8 }, mappedValue: 150000, mappedLabel: '$150,000' },
                    { range: { min: 8, max: 9 }, mappedValue: 200000, mappedLabel: '$200,000' },
                    { range: { min: 9, max: 10 }, mappedValue: 250000, mappedLabel: '$250,000+' }
                ]
            });

            me.filterSettings.sliders['householdExpenditures'] = new sizeup.controls.rangeSlider({
                container: me.filterSettings.container.find('#householdExpenditures'),
                values: params['householdExpenditures'],
                min: 0,
                max: 9,
                mapping: [
                    { range: { min: 0, max: 1 }, mappedValue: 10000, mappedLabel: '$10,000' },
                    { range: { min: 1, max: 2 }, mappedValue: 20000, mappedLabel: '$20,000' },
                    { range: { min: 2, max: 3 }, mappedValue: 30000, mappedLabel: '$30,000' },
                    { range: { min: 3, max: 4 }, mappedValue: 40000, mappedLabel: '$40,000' },
                    { range: { min: 4, max: 5 }, mappedValue: 50000, mappedLabel: '$50,000' },
                    { range: { min: 5, max: 6 }, mappedValue: 75000, mappedLabel: '$75,000' },
                    { range: { min: 6, max: 7 }, mappedValue: 100000, mappedLabel: '$100,000' },
                    { range: { min: 7, max: 8 }, mappedValue: 150000, mappedLabel: '$150,000' },
                    { range: { min: 8, max: 9 }, mappedValue: 200000, mappedLabel: '$200,000' },
                    { range: { min: 9, max: 10 }, mappedValue: 250000, mappedLabel: '$250,000+' }
                ]
            });

            me.filterSettings.sliders['medianAge'] = new sizeup.controls.rangeSlider({
                container: me.filterSettings.container.find('#medianAge'),
                values: params['medianAge'],
                min: 0,
                max: 82
            });


        };

  
        var setParameters = function () {
            var params = {};
            for (var x in me.filterSettings.sliders) {
                params[x] = me.filterSettings.sliders[x].getParam();
                if (params[x] == null) {
                    delete params[x];
                }
            }
            jQuery.bbq.pushState(params);
        };


        var getParameters = function () {
            var params = jQuery.bbq.getState();
            var p = {
                cityId: me.opts.CurrentPlace.City.Id,
                industryId: me.opts.CurrentIndustry.Id
            };

            return $.extend(true, p,params);
        };

        var loadReport = function () {
            me.listLoader.show();
            me.content.list.container.hide();
            var params = getParameters();
            dataLayer.getBestPlacesToAdvertise(params, function (data) {
                bindCityList(data);
                me.listLoader.hide();
                me.content.list.container.show();
            });
        };
      
        var bindCityList = function (data) {
            var html = '';
            for (var x in data.Items) {
                var template = templates.get('listItem');
                html = html + templates.bind(template, data.Items[x]);
            }
            me.content.list.body.html(html);
        };

      

        var slideInFilterSettings = function (callback) {
            me.filterSettings.container.show(
                 "slide",
                 { direction: "right" },
                 me.opts.slideTime,
                callback);
        };

        var slideOutFilterSettings = function (callback) {
            me.filterSettings.container.hide(
                "slide",
                { direction: "right" },
                me.opts.slideTime,
            callback);
        };

        var slideInContent = function (callback) {
            me.content.container.show(
               "slide",
               { direction: "left" },
               me.opts.slideTime,
               function () {
                   me.content.map.triggerEvent('resize');
                   if (callback) {
                       callback();
                   }
               }
           );
        };

        var slideOutContent = function (callback) {
            me.content.container.hide(
                "slide",
                { direction: "left" },
                me.opts.slideTime,
            callback);
        };




        var cancelClicked = function () {
            slideOutFilterSettings(function () {
                slideInContent();
            });
        };

        var submitClicked = function () {
            setParameters();
            loadReport();
            slideOutFilterSettings(function () {
                slideInContent();
            });
        };


        var filterSettingsButtonClicked = function () {
            slideOutContent(function () {
                slideInFilterSettings();
            });
        }

      
        var publicObj = {

        };
        return publicObj;
        
    };
})();