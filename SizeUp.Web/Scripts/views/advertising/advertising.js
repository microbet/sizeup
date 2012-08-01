(function () {
    sizeup.core.namespace('sizeup.views.advertising');
    sizeup.views.advertising.advertising = function (opts) {

        var defaults = {
            mapSettings: sizeup.maps.mapOptions.getDefaults(),
            styles: sizeup.maps.mapStyles.getDefaults(),
            colors: [],
            overlays: [],
            slideTime: 500,
            filterTemplates: {
                averageRevenue: {
                    distance: 20,
                    attribute: 'AverageRevenue',
                    sortAttribute: 'AverageRevenue',
                    sort: 'desc',
                    template: 'averageRevenue'
                },
                totalRevenue: {
                    distance: 20,
                    attribute: 'TotalRevenue',
                    sortAttribute: 'TotalRevenue',
                    sort: 'desc',
                    template: 'totalRevenue'
                },
                underservedMarkets: {
                    distance: 20,
                    attribute: 'RevenuePerCapita',
                    sortAttribute: 'RevenuePerCapita',
                    sort: 'asc',
                    template: 'underservedMarkets'
                },
                revenuePerCapita: {
                    distance: 20,
                    attribute: 'RevenuePerCapita',
                    sortAttribute: 'RevenuePerCapita',
                    sort: 'desc',
                    template: 'revenuePerCapita'
                },
                householdIncome: {
                    distance: 20,
                    attribute: 'HouseholdIncome',
                    sortAttribute: 'HouseholdIncome',
                    sort: 'desc',
                    template: 'householdIncome'
                }
            }
        };
        var me = {};
        me.container = $('#advertising');
        var dataLayer = new sizeup.core.data();
        var templates = new sizeup.core.templates(me.container);
        var notifier = new sizeup.core.notifier(function () { init(); });

        me.opts = $.extend(true, defaults, opts);
       
        me.data = {};
        me.opts.filterOptions = {};
        

        dataLayer.getCityCentroid({ id: opts.CurrentPlace.City.Id }, notifier.getNotifier(function (data) { me.data.CityCenter = new sizeup.maps.latLng({lat:data.Lat, lng: data.Lng}); }));
        var init = function () {
            
            var params = jQuery.bbq.getState();
            if (params.template == null) {
                setParameters(me.opts.filterTemplates.totalRevenue);
            }
            params = jQuery.bbq.getState();
            me.opts.filterOptions = params;


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

            me.content.optionMenu = {};
            me.content.optionMenu.option = me.content.container.find('#optionMenu');
            me.content.optionMenu.custom = me.content.optionMenu.option.find('.custom');
            me.content.optionMenu.custom.remove();
            me.content.optionMenu.menu = me.content.optionMenu.option.chosen();
            me.content.optionMenu.menu.change(optionMenuChanged);
            
           

            me.content.list = {};
            me.content.list.container = me.content.container.find('.listWrapper');

            me.content.list.body = me.content.list.container.find('.results');
            me.content.list.sort = {
                name: me.content.list.container.find('.sort .name'),
                value: me.content.list.container.find('.sort .value')
            };

            if (params.sortAttribute == 'Name') {
                me.content.list.sort.name.addClass(params.sort);
            }

            me.content.list.sort.name.click(function () { nameSortClicked(); });
            me.content.list.sort.value.click(function () { valueSortClicked(); });

            me.filterSettings = {};
            me.filterSettings.container = me.container.find('#filterSettings').hide().removeClass('hidden');
            me.filterSettings.submitButton = me.filterSettings.container.find('.submit');
            me.filterSettings.cancelButton = me.filterSettings.container.find('.cancel');


            me.filterSettings.submitButton.click(function () { submitClicked(); });
            me.filterSettings.cancelButton.click(function () { cancelClicked(); });


            initFilterSliders();
            setOptionMenu(params.template);
            setSliderValues(params);

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

        var optionMenuChanged = function () {
            var x = me.content.optionMenu.option.val();
            setOptionMenu(x);
            if (x != 'custom') {
                setParameters(me.opts.filterTemplates[x]);
            }
            loadReport();
        };

        var setOptionMenu = function (index) {
            if (index == 'custom') {
                me.content.optionMenu.option.append(me.content.optionMenu.custom);
                me.opts.filterOptions.template = 'custom';
            }
            else {
                me.content.optionMenu.custom.remove();
            }
            me.content.optionMenu.option.find('option[value=' + index + ']').attr('selected','selected');
            me.content.optionMenu.menu.trigger('liszt:updated');
            me.content.optionMenu.option.val(index);
        };


        var setSliderValues = function (params) {
            for (var x in me.filterSettings.sliders) {
                me.filterSettings.sliders[x].setParam(params[x]);
            }
        };

        var getSliderValues = function () {
            var params = {};
            for (var x in me.filterSettings.sliders) {
                params[x] = me.filterSettings.sliders[x].getParam();
                if (params[x] == null) {
                    delete params[x];
                }
            }
            return params;
        };

        var setParameters = function (params) {
            me.opts.filterOptions = params;
            jQuery.bbq.pushState(params, 2);
        };


        var getParameters = function () {
            var params = jQuery.bbq.getState();
            return params;
        };

        var loadReport = function () {
            me.listLoader.show();
            me.content.list.body.hide();
            var params = getParameters();
            params.placeId = me.opts.CurrentPlace.Id;
            params.industryId = me.opts.CurrentIndustry.Id;
            dataLayer.getBestPlacesToAdvertise(params, function (data) {
                var formattedData = formatData(data);
                bindZipList(formattedData);
                me.listLoader.hide();
                me.content.list.body.show();
            });
        };
      
        var formatData = function (data) {
            var newData = {
                Items: [],
                Total: data.Total
            };

            for (var x in data.Items) {
                newData.Items.push(formatDataItem(data.Items[x]));
            }
            return newData;
        };

        var formatDataItem = function (item) {
            var newItem = {};
            newItem['Name'] = item.Name;
            newItem['TotalPopulation'] = sizeup.util.numbers.format.addCommas(item.TotalPopulation == null ? 0 : item.TotalPopulation);
            newItem['TotalRevenue'] = '$' + sizeup.util.numbers.format.addCommas(item.TotalRevenue == null ? 0 : item.TotalRevenue);

            if (me.opts.filterOptions['averageRevenue'] != null || me.opts.filterOptions.attribute == 'AverageRevenue') {
                newItem['AverageRevenue'] = '$' + sizeup.util.numbers.format.addCommas(item.AverageRevenue == null ? 0 : item.AverageRevenue);
            }
            if (me.opts.filterOptions['totalEmployees'] != null || me.opts.filterOptions.attribute == 'TotalEmployees') {
                newItem['TotalEmployees'] = sizeup.util.numbers.format.addCommas(item.TotalEmployees == null ? 0 : item.TotalEmployees);
            }
            if (me.opts.filterOptions['revenuePerCapita'] != null || me.opts.filterOptions.attribute == 'RevenuePerCapita') {
                newItem['RevenuePerCapita'] = '$' + sizeup.util.numbers.format.addCommas(item.RevenuePerCapita == null ? 0 : item.RevenuePerCapita);
            }
            if (me.opts.filterOptions['householdIncome'] != null || me.opts.filterOptions.attribute == 'HouseholdIncome') {
                newItem['HouseholdIncome'] = '$' + sizeup.util.numbers.format.addCommas(item.HouseholdIncome == null ? 0 : item.HouseholdIncome);
            }
            if (me.opts.filterOptions['householdExpenditures'] != null || me.opts.filterOptions.attribute == 'HouseholdExpenditures') {
                newItem['HouseholdExpenditures'] = '$' + sizeup.util.numbers.format.addCommas(item.HouseholdExpenditures == null ? 0 : item.HouseholdExpenditures);
            }
            if (me.opts.filterOptions['medianAge'] != null || me.opts.filterOptions.attribute == 'MedianAge') {
                newItem['MedianAge'] = item.MedianAge == null ? 0 : item.MedianAge;
            }
            if (me.opts.filterOptions['highSchoolOrHigher'] != null || me.opts.filterOptions.attribute == 'HighSchoolOrHigher') {
                newItem['HighSchoolOrHigher'] = (item.HighSchoolOrHigher == null ? 0 : item.HighSchoolOrHigher * 100).toFixed(1) + '%';
            }
            if (me.opts.filterOptions['whiteCollar'] != null || me.opts.filterOptions.attribute == 'WhiteCollarWorkers') {
                newItem['WhiteCollarWorkers'] = (item.WhiteCollarWorkers == null ? 0 : item.WhiteCollarWorkers * 100).toFixed(1) + '%';
            }
            if (me.opts.filterOptions['bachelorOrHigher'] != null || me.opts.filterOptions.attribute == 'BachelorsDegreeOrHigher') {
                newItem['BachelorsDegreeOrHigher'] = (item.BachelorsDegreeOrHigher == null ? 0 : item.BachelorsDegreeOrHigher * 100).toFixed(1) + '%';
            }

  
            newItem['Value'] = newItem[me.opts.filterOptions.attribute];
            delete newItem[me.opts.filterOptions.attribute];


            return newItem;
        };


        var bindZipList = function (data) {
            var html = '';
            for (var x in data.Items) {
                var template = templates.get('listItem');
                html = html + templates.bind(template, data.Items[x]);
            }
            me.content.list.body.html(html);
        };

      

        var nameSortClicked = function () {
            if (me.content.list.sort.name.hasClass('asc')) {
                me.content.list.sort.name.removeClass('asc');
                me.content.list.sort.name.addClass('desc');
                me.opts.filterOptions.sort = 'desc';
                me.opts.filterOptions.sortAttribute = 'Name';
            }
            else {
                me.content.list.sort.name.removeClass('desc');
                me.content.list.sort.name.addClass('asc');
                me.opts.filterOptions.sort = 'asc';
                me.opts.filterOptions.sortAttribute = 'Name';
            }
            me.content.list.sort.value.removeClass('asc');
            me.content.list.sort.value.removeClass('desc');
            me.opts.filterOptions.template = 'custom';
            setOptionMenu('custom');
            setParameters(me.opts.filterOptions);
            loadReport();
        };

        var valueSortClicked = function () {

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
            var params = getSliderValues();
            params.sort = me.opts.filterOptions.sort;
            params.sortAttribute = me.opts.filterOptions.sortAttribute;
            params.attribute = me.opts.filterOptions.attribute;
            params.template = 'custom';
            setOptionMenu('custom');
            setParameters(params);
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