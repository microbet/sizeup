﻿(function () {
    sizeup.core.namespace('sizeup.views.advertising');
    sizeup.views.advertising.advertising = function (opts) {

        var defaults = {
            mapSettings: sizeup.maps.mapOptions.getDefaults(),
            styles: sizeup.maps.mapStyles.getDefaults(),
            colors: [],
            overlays: [],
            slideTime: 500,
            itemsPerPage: 20,
            pagesToShow: 5,
            bandCount: 5,
            bandColors: ['ff0000', 'ff6400', 'ff9600', 'ffc800', 'ffff00'],
            filterTemplates: {
                averageRevenue: {
                    attribute: 'averageRevenue',
                    sortAttribute: 'averageRevenue',
                    sort: 'desc',
                    order: 'highToLow',
                    template: 'averageRevenue'
                },
                totalRevenue: {
                    attribute: 'totalRevenue',
                    sortAttribute: 'totalRevenue',
                    sort: 'desc',
                    order: 'highToLow',
                    template: 'totalRevenue'
                },
                underservedMarkets: {
                    attribute: 'revenuePerCapita',
                    sortAttribute: 'revenuePerCapita',
                    sort: 'asc',
                    order: 'lowToHigh',
                    template: 'underservedMarkets'
                },
                revenuePerCapita: {
                    attribute: 'revenuePerCapita',
                    sortAttribute: 'revenuePerCapita',
                    sort: 'desc',
                    order: 'highToLow',
                    template: 'revenuePerCapita'
                },
                householdIncome: {
                    attribute: 'householdIncome',
                    sortAttribute: 'householdIncome',
                    sort: 'desc',
                    order: 'highToLow',
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
        
        
        var params = jQuery.bbq.getState();
        var minDistanceParams = { placeId: me.opts.CurrentPlace.Id, industryId: me.opts.CurrentIndustry.Id, itemCount: me.opts.itemsPerPage };
        if (params.template == null) {
            minDistanceParams = $.extend(true, minDistanceParams, me.opts.filterTemplates.totalRevenue);
        }


        dataLayer.getBestPlacesToAdvertiseMinimumDistance(minDistanceParams, notifier.getNotifier(function (data) { me.data.defaultDistance = data; }));
        dataLayer.getPlaceCentroid({ id: opts.CurrentPlace.Id }, notifier.getNotifier(function (data) {me.data.CityCenter = new sizeup.maps.latLng({ lat: data.Lat, lng: data.Lng });}));
        dataLayer.isAuthenticated(notifier.getNotifier(function (data) { me.isAuthenticated = data; }));
        var init = function () {
            
         
            var params = jQuery.bbq.getState();
            if (params.template == null) {
                var d = $.extend(me.opts.filterTemplates.totalRevenue, { distance: me.data.defaultDistance });
                setParameters(d);
            }
            params = jQuery.bbq.getState();
            me.opts.filterOptions = params;
           
  
            me.content = {};
            me.content.container = me.container.find('.content').hide().removeClass('hidden');

            me.content.mapPins = [];
            me.content.map = new sizeup.maps.map({
                container: me.content.container.find('.mapContent .map'),
                mapSettings: $.extend(me.opts.mapSettings, {maxZoom: 16}),
                styles: me.opts.styles
            });

            me.content.map.setCenter(me.data.CityCenter);
            me.content.map.setZoom(10);

            me.content.pager = new sizeup.controls.pager({
                container: me.content.container.find('.pager'),
                itemsPerPage: me.opts.itemsPerPage,
                pagesToShow: me.opts.pagesToShow,
                templates: templates,
                templateId: 'pager',
                onUpdate: function (data) { pagerOnUpdate( data); }
            });
            me.content.pager.getContainer().hide();


            me.content.bands = me.content.container.find('.mapContent .footer .bandContainer');


            me.content.filterSettingsButton = me.content.container.find('#filterSettingsButton');
            me.content.filterSettingsButton.click(function () { filterSettingsButtonClicked(); });

            me.content.optionMenu = {};
            me.content.optionMenu.option = new sizeup.controls.selectList({
                select: me.content.container.find('#optionMenu'),
                onChange: function () { optionMenuChanged(); }
            });
            me.content.optionMenu.custom = me.content.optionMenu.option.getSelectList().find('.custom');
            me.content.optionMenu.custom.remove();
            me.content.optionMenu.option.updateList();

           
            me.content.description = me.container.find('.description');
            me.content.list = {};
            me.content.list.container = me.content.container.find('.listWrapper');

            me.content.list.body = me.content.list.container.find('.results');
            me.content.list.sort = {
                name: me.content.list.container.find('.sort .name')
            };

            me.content.list.sort.value = {};
            me.content.list.sort.value.option = new sizeup.controls.selectList({
                select: me.content.list.container.find('.sort #valueMenu'),
                onChange: function () { valueMenuChanged(); }
            });


            me.content.list.sort.value.menuItems = {
                totalPopulation: me.content.list.sort.value.option.getSelectList().find('option[value=totalPopulation]').remove(),
                totalRevenue: me.content.list.sort.value.option.getSelectList().find('option[value=totalRevenue]').remove(),
                averageRevenue: me.content.list.sort.value.option.getSelectList().find('option[value=averageRevenue]').remove(),
                totalEmployees: me.content.list.sort.value.option.getSelectList().find('option[value=totalEmployees]').remove(),
                revenuePerCapita: me.content.list.sort.value.option.getSelectList().find('option[value=revenuePerCapita]').remove(),
                householdIncome: me.content.list.sort.value.option.getSelectList().find('option[value=householdIncome]').remove(),
                householdExpenditures: me.content.list.sort.value.option.getSelectList().find('option[value=householdExpenditures]').remove(),
                medianAge: me.content.list.sort.value.option.getSelectList().find('option[value=medianAge]').remove(),
                bachelorsDegreeOrHigher: me.content.list.sort.value.option.getSelectList().find('option[value=bachelorsDegreeOrHigher]').remove(),
                highSchoolOrHigher: me.content.list.sort.value.option.getSelectList().find('option[value=highSchoolOrHigher]').remove(),
                whiteCollarWorkers: me.content.list.sort.value.option.getSelectList().find('option[value=whiteCollarWorkers]').remove()
            };

            me.content.list.sort.value.direction = me.content.list.container.find('.sort .value .sorter');
            me.content.list.sort.value.option.updateList();

            if (params.sortAttribute == 'name') {
                me.content.list.sort.name.addClass(params.sort);
            }
            else {
                me.content.list.sort.value.direction.addClass(params.sort);
            }

            me.content.list.sort.name.click(function () { nameSortClicked(); });
            me.content.list.sort.value.direction.click(function () { valueSortClicked(); });

            me.content.list.noResults = me.container.find('.noResults').hide().removeClass('hidden');

            me.filterSettings = {};
            me.filterSettings.container = me.container.find('#filterSettings').hide().removeClass('hidden');
            me.filterSettings.submitButton = me.filterSettings.container.find('.submit');
            me.filterSettings.cancelButton = me.filterSettings.container.find('.cancel');


            me.filterSettings.submitButton.click(function () { submitClicked(); });
            me.filterSettings.cancelButton.click(function () { cancelClicked(); });


            me.signinPanel = new sizeup.views.shared.signin({
                container: me.container.find('.signinWrapper .signinPanel'),
                toggle: me.container.find('.signinWrapper .signinToggle')
            });
      


            initFilterSliders();
            setOptionMenu(params.template);
            setSliderValues(params);
            setValueMenu();

            me.pageLoader = me.container.find('.page.loading');
            me.listLoader = me.container.find('.list.loading').hide().removeClass('hidden');
            me.pageLoader.hide();
            me.content.container.show();

            loadReport();
        };

        var initFilterSliders = function () {
            var params = getParameters();
            me.filterSettings.sliders = {};
            me.filterSettings.sliders['distance'] = new sizeup.controls.rangeSlider({
                container: me.filterSettings.container.find('.distance'),
                label: me.filterSettings.container.find('.distance .valueLabel'),
                value: params['distance'],
                mode: 'min',
                range: { min: 1, max: 150 },
                invert: true
            });

            me.filterSettings.sliders['bachelorsDegreeOrHigher'] = new sizeup.controls.rangeSlider({
                container: me.filterSettings.container.find('.bachelorsDegreeOrHigher'),
                label: me.filterSettings.container.find('.bachelorsDegreeOrHigher .valueLabel'),
                value: params['bachelorsDegreeOrHigher'],
                mode: 'min',
                range:{ min: 0, max: 95}
            });
            
            me.filterSettings.sliders['highSchoolOrHigher'] = new sizeup.controls.rangeSlider({
                container: me.filterSettings.container.find('.highSchoolOrHigher'),
                label: me.filterSettings.container.find('.highSchoolOrHigher .valueLabel'),
                value: params['highSchoolOrHigher'],
                mode: 'min',
                range: { min: 0, max: 95 }
            });

            me.filterSettings.sliders['whiteCollarWorkers'] = new sizeup.controls.rangeSlider({
                container: me.filterSettings.container.find('.whiteCollarWorkers'),
                label: me.filterSettings.container.find('.whiteCollarWorkers .valueLabel'),
                value: params['whiteCollarWorkers'],
                mode: 'min',
                range: { min: 0, max: 95 }
            });

            me.filterSettings.sliders['averageRevenue'] = new sizeup.controls.rangeSlider({
                container: me.filterSettings.container.find('.averageRevenue'),
                label: me.filterSettings.container.find('.averageRevenue .valueLabel'),
                value: params['averageRevenue'],
                mode: 'range',
                range: [
                    { value: 50000, label: '$50,000' },
                    { value: 100000, label: '$100,000' },
                    { value: 250000, label: '$250,000' },
                    { value: 500000, label: '$500,000' },
                    { value: 750000, label: '$750,000' },
                    { value: 1000000, label: '$1 million' },
                    { value: 2500000, label: '$2.5 million' },
                    { value: 5000000, label: '$5 million' },
                    { value: 7500000, label: '$7.5 million' },
                    { value: 10000000, label: '$10 million' },
                    { value: 50000000, label: '$50 million' }
                ]
            });


            me.filterSettings.sliders['totalRevenue'] = new sizeup.controls.rangeSlider({
                container: me.filterSettings.container.find('.totalRevenue'),
                label: me.filterSettings.container.find('.totalRevenue .valueLabel'),
                value: params['totalRevenue'],
                mode: 'range',
                range: [
                    { value: 100000, label: '$100,000' },
                    { value: 500000, label: '$500,000' },
                    { value: 1000000, label: '$1 million' },
                    { value: 5000000, label: '$5 million' },
                    { value: 10000000, label: '$10 million' },
                    { value: 50000000, label: '$50 million' },
                    { value: 100000000, label: '$100 million' },
                    { value: 500000000, label: '$500 million' },
                    { value: 1000000000, label: '$1 billion' },
                    { value: 50000000000, label: '$50 billion' },
                    { value: 100000000000, label: '$100 billion' }
                ]
            });

            me.filterSettings.sliders['totalEmployees'] = new sizeup.controls.rangeSlider({
                container: me.filterSettings.container.find('.totalEmployees'),
                label: me.filterSettings.container.find('.totalEmployees .valueLabel'),
                value: params['totalEmployees'],
                mode: 'range',
                range: [
                    { value: 10, label: '10' },
                    { value: 50, label: '50' },
                    { value: 100, label: '100' },
                    { value: 500, label: '500' },
                    { value: 1000, label: '1,000' },
                    { value: 5000, label: '5,000' },
                    { value: 10000, label: '10,000' },
                    { value: 50000, label: '50,000' },
                    { value: 100000, label: '100,000' },
                    { value: 500000, label: '500,000' },
                    { value: 1000000, label: '1 million' }
                ]
            });

            me.filterSettings.sliders['revenuePerCapita'] = new sizeup.controls.rangeSlider({
                container: me.filterSettings.container.find('.revenuePerCapita'),
                label: me.filterSettings.container.find('.revenuePerCapita .valueLabel'),
                value: params['revenuePerCapita'],
                mode: 'range',
                range: [
                    { value: 5, label: '$5' },
                    { value: 10, label: '$10' },
                    { value: 50, label: '$50' },
                    { value: 100, label: '$100' },
                    { value: 500, label: '$500' },
                    { value: 1000, label: '$1,000' },
                    { value: 2500, label: '$2,500' },
                    { value: 5000, label: '$5,000' },
                    { value: 7500, label: '$7,500' },
                    { value: 10000, label: '$10,000' },
                    { value: 15000, label: '$15,000' }
                ]
            });


            me.filterSettings.sliders['householdIncome'] = new sizeup.controls.rangeSlider({
                container: me.filterSettings.container.find('.householdIncome'),
                label: me.filterSettings.container.find('.householdIncome .valueLabel'),
                value: params['householdIncome'],
                mode: 'range',
                range: [
                    { value: 10000, label: '$10,000' },
                    { value: 20000, label: '$20,000' },
                    { value: 30000, label: '$30,000' },
                    { value: 40000, label: '$40,000' },
                    { value: 50000, label: '$50,000' },
                    { value: 75000, label: '$75,000' },
                    { value: 100000, label: '$100,000' },
                    { value: 150000, label: '$150,000' },
                    { value: 200000, label: '$200,000' },
                    { value: 250000, label: '$250,000' }
                ]
            });

            me.filterSettings.sliders['householdExpenditures'] = new sizeup.controls.rangeSlider({
                container: me.filterSettings.container.find('.householdExpenditures'),
                label: me.filterSettings.container.find('.householdExpenditures .valueLabel'),
                mode: 'range',
                value: params['householdExpenditures'],
                range: [
                    { value: 10000, label: '$10,000' },
                    { value: 20000, label: '$20,000' },
                    { value: 30000, label: '$30,000' },
                    { value: 40000, label: '$40,000' },
                    { value: 50000, label: '$50,000' },
                    { value: 75000, label: '$75,000' },
                    { value: 100000, label: '$100,000' },
                    { value: 150000, label: '$150,000' },
                    { value: 200000, label: '$200,000' },
                    { value: 250000, label: '$250,000' }
                ]
            });

            me.filterSettings.sliders['medianAge'] = new sizeup.controls.rangeSlider({
                container: me.filterSettings.container.find('.medianAge'),
                label: me.filterSettings.container.find('.medianAge .valueLabel'),
                value: params['medianAge'],
                mode: 'range',
                range:{ min: 0, max: 82}
            });


        };

        var pagerOnUpdate = function (data) {
            var params = getParameters();
            params.page = data.page;
            setParameters(params);
            loadReport();
        };

        var optionMenuChanged = function () {
            var x = me.content.optionMenu.option.getValue();
            setOptionMenu(x);
            if (x != 'custom') {
                var d = $.extend(me.opts.filterTemplates[x], { distance: me.data.defaultDistance });
                setParameters(d);
                setSliderValues(d);
            }
            loadReport();
        };

        var valueMenuChanged = function () {
            var x = me.content.list.sort.value.option.getValue();
            var params = getParameters();
            params.attribute = x;
            params.sortAttribute = x;
            params.order = 'highToLow';
            params.template = 'custom';
            setOptionMenu('custom');
            setParameters(params);
            loadReport();
        };


        var setOptionMenu = function (index) {
            if (index == 'custom') {
                me.content.optionMenu.option.getSelectList().append(me.content.optionMenu.custom);
                me.opts.filterOptions.template = 'custom';
            }
            else {
                me.content.optionMenu.custom.remove();
            }       
            me.content.optionMenu.option.updateList();
            me.content.optionMenu.option.setValue(index);
        };

        var setValueMenu = function () {
            me.content.list.sort.value.option.getSelectList().empty();
            me.content.list.sort.value.option.getSelectList().append(me.content.list.sort.value.menuItems.totalPopulation);
            me.content.list.sort.value.option.getSelectList().append(me.content.list.sort.value.menuItems.totalRevenue);
            var params = me.opts.filterOptions;
            var p = getParameters();

            if(params.averageRevenue || p.attribute == 'averageRevenue'){
                me.content.list.sort.value.option.getSelectList().append(me.content.list.sort.value.menuItems.averageRevenue);
            }
            if (params.totalEmployees || p.attribute == 'totalEmployees') {
                me.content.list.sort.value.option.getSelectList().append(me.content.list.sort.value.menuItems.totalEmployees);
            }
            if (params.revenuePerCapita || p.attribute == 'revenuePerCapita') {
                me.content.list.sort.value.option.getSelectList().append(me.content.list.sort.value.menuItems.revenuePerCapita);
            }
            if (params.householdIncome || p.attribute == 'householdIncome') {
                me.content.list.sort.value.option.getSelectList().append(me.content.list.sort.value.menuItems.householdIncome);
            }
            if (params.householdExpenditures || p.attribute == 'householdExpenditures') {
                me.content.list.sort.value.option.getSelectList().append(me.content.list.sort.value.menuItems.householdExpenditures);
            }
            if (params.medianAge || p.attribute == 'medianAge') {
                me.content.list.sort.value.option.getSelectList().append(me.content.list.sort.value.menuItems.medianAge);
            }
            if (params.bachelorsDegreeOrHigher || p.attribute == 'bachelorsDegreeOrHigher') {
                me.content.list.sort.value.option.getSelectList().append(me.content.list.sort.value.menuItems.bachelorsDegreeOrHigher);
            }
            if (params.highSchoolOrHigher || p.attribute == 'highSchoolOrHigher') {
                me.content.list.sort.value.option.getSelectList().append(me.content.list.sort.value.menuItems.highSchoolOrHigher);
            }
            if (params.whiteCollarWorkers || p.attribute == 'whiteCollarWorkers') {
                me.content.list.sort.value.option.getSelectList().append(me.content.list.sort.value.menuItems.whiteCollarWorkers);
            }

            me.content.list.sort.value.option.setValue(p.attribute);
            me.content.list.sort.value.option.updateList();
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
            me.opts.filterOptions = $.extend(true,{},params);
            jQuery.bbq.pushState(params, 2);
        };


        var getParameters = function () {
            var params = jQuery.bbq.getState();
            return params;
        };

        var loadReport = function () {

            var params = getParameters();
            new sizeup.core.analytics().advertisingReportLoaded({ attribute: params.template });

            me.listLoader.show();
            me.content.list.body.hide();
            var pagerData = me.content.pager.getPageData();
            setValueMenu();
            params.placeId = me.opts.CurrentPlace.Id;
            params.industryId = me.opts.CurrentIndustry.Id;
            params.itemCount = me.opts.itemsPerPage,
            params.page = pagerData.page;

            var reportData = {
                zips: null,
                bands: null
            };
            var notifier = new sizeup.core.notifier(function () {
                setPager({ Count: reportData.zips.Total, Page: pagerData.page });
                var formattedData = formatData(reportData.zips);
                var formattedBands = formatBands(reportData.bands, params.attribute);

                bindCircle(params.distance);
                bindZipList(formattedData);
                bindBands(formattedBands);
                bindDescription();

                bindMap(reportData, params.attribute);



                me.listLoader.hide();
                me.content.list.body.show();
            });

            dataLayer.getBestPlacesToAdvertise(params, notifier.getNotifier(function (data) {
                reportData.zips = data;
            }));
            params.bands = me.opts.bandCount;
            dataLayer.getBestPlacesToAdvertiseBands(params, notifier.getNotifier(function (data) {
                reportData.bands = data;
            }));
        };

        var bindBands = function (data) {
            var params = getParameters();
            var html = '';
            for (var x = 0 ;x < data.length; x++) {
                var d = {
                    color: me.opts.bandColors[x]
                };

                if (params.order == 'highToLow') {
                    d.label = x == data.length - 1 ? data[x].Max + ' and below' : data[x].Max + ' - ' + data[x].Min;
                }
                else {
                    d.label = x == data.length - 1 ? data[x].Min + ' and above' : data[x].Min + ' - ' + data[x].Max;
                }

                var template = templates.get('bandItem');
                html = html + templates.bind(template, d);
            }
            me.content.bands.html(html);
        };

       
        var bindMap = function (data, attribute) {
            for (var x in me.content.mapPins) {
                me.content.map.removeMarker(me.content.mapPins[x]);
            }
            me.content.mapPins = [];
            var latLngBounds = new sizeup.maps.latLngBounds();
            var hasResults = false;
            for (var x in data.zips.Items) {
                var pin = new sizeup.maps.heatPin({
                    position: new sizeup.maps.latLng({ lat: data.zips.Items[x].Lat, lng: data.zips.Items[x].Long }),
                    color: getColor(getValue(data.zips.Items[x], attribute), data.bands),
                    title: data.zips.Items[x].Name
                });
                latLngBounds.extend(pin.getPosition());
                hasResults = true;
                me.content.mapPins.push(pin);
                me.content.map.addMarker(pin);
            };
            if (hasResults) {
                me.content.map.fitBounds(latLngBounds);
                me.content.list.noResults.hide();
            }
            else {
                me.content.list.noResults.show();
            }
        };

        var getColor = function (value, bandData) {
            var color = null;
            for (var x = 0; x < bandData.length; x++) {
                if (value >= bandData[x].Min && value <= bandData[x].Max) {
                    color = me.opts.bandColors[x];
                }
            }
            return color;
        };

        var getValue = function(item, attribute){
            var val = null;
            if(attribute == 'totalPopulation'){
                val = item.TotalPopulation;
            }
            else if (attribute == 'totalRevenue') {
                val = item.TotalRevenue;
            }
            else if (attribute == 'averageRevenue') {
                val = item.AverageRevenue;
            }
            else if (attribute == 'totalEmployees') {
                val = item.TotalEmployees;
            }
            else if (attribute == 'revenuePerCapita') {
                val = item.RevenuePerCapita;
            }
            else if (attribute == 'householdIncome') {
                val = item.HouseholdIncome;
            }
            else if (attribute == 'householdExpenditures') {
                val = item.HouseholdExpenditures;
            }
            else if (attribute == 'medianAge') {
                val = item.MedianAge;
            }
            else if (attribute == 'highSchoolOrHigher') {
                val = item.HighSchoolOrHigher;
            }
            else if (attribute == 'whiteCollarWorkers') {
                val = item.WhiteCollarWorkers;
            }
            else if (attribute == 'bachelorsDegreeOrHigher') {
                val = item.BachelorsDegreeOrHigher;
            }
            return val;
        }


        var bindCircle = function (radius) {
            if (me.content.mapCircle) {
                me.content.map.removeCircle(me.content.mapCircle);
            }
            if (radius) {
                me.content.mapCircle = new sizeup.maps.circle({ center: me.data.CityCenter, radius: radius * 1609.344, strokeColor: "#6495ED", strokeWeight: 2, strokeOpacity: 0.35, fillOpacity: 0 });
                me.content.map.addCircle(me.content.mapCircle);
            }
        };


        var setPager = function (data) {
            me.content.pager.setState(data);
            if (data.Count > me.opts.itemsPerPage && me.isAuthenticated) {
                me.content.pager.getContainer().show();
            }
            else {
                me.content.pager.getContainer().hide();
            }
        };
      
        var formatBands = function (data, attribute) {
            var newData = [];

            for (var x in data) {
                newData.push(formatBandItem(data[x], attribute));
            }
            return newData;
        };

        var formatBandItem = function (item, attribute) {
            var newItem = {};
            if(attribute == 'totalPopulation'){
                newItem.Min = sizeup.util.numbers.format.addCommas(item.Min);
                newItem.Max = sizeup.util.numbers.format.addCommas(item.Max);
            }
            else if (attribute == 'totalRevenue') {
                newItem.Min = '$' + sizeup.util.numbers.format.addCommas(item.Min);
                newItem.Max = '$' + sizeup.util.numbers.format.addCommas(item.Max);
            }
            else if (attribute == 'averageRevenue') {
                newItem.Min = '$' + sizeup.util.numbers.format.addCommas(item.Min);
                newItem.Max = '$' + sizeup.util.numbers.format.addCommas(item.Max);
            }
            else if (attribute == 'totalEmployees') {
                newItem.Min = sizeup.util.numbers.format.addCommas(item.Min);
                newItem.Max = sizeup.util.numbers.format.addCommas(item.Max);
            }
            else if (attribute == 'revenuePerCapita') {
                newItem.Min = '$' + sizeup.util.numbers.format.addCommas(item.Min);
                newItem.Max = '$' + sizeup.util.numbers.format.addCommas(item.Max);
            }
            else if (attribute == 'householdIncome') {
                newItem.Min = '$' + sizeup.util.numbers.format.addCommas(item.Min);
                newItem.Max = '$' + sizeup.util.numbers.format.addCommas(item.Max);
            }
            else if (attribute == 'householdExpenditures') {
                newItem.Min = '$' + sizeup.util.numbers.format.addCommas(item.Min);
                newItem.Max = '$' + sizeup.util.numbers.format.addCommas(item.Max);
            }
            else if (attribute == 'medianAge') {
                newItem.Min = sizeup.util.numbers.format.addCommas(item.Min);
                newItem.Max = sizeup.util.numbers.format.addCommas(item.Max);
            }
            else if (attribute == 'highSchoolOrHigher') {
                newItem.Min = (item.Min * 100).toFixed(1) + '%';
                newItem.Max = (item.Max * 100).toFixed(1) + '%';
            }
            else if (attribute == 'whiteCollarWorkers') {
                newItem.Min = (item.Min * 100).toFixed(1) + '%';
                newItem.Max = (item.Max * 100).toFixed(1) + '%';
            }
            else if (attribute == 'bachelorsDegreeOrHigher') {
                newItem.Min = (item.Min * 100).toFixed(1) + '%';
                newItem.Max = (item.Max * 100).toFixed(1) + '%';
            }


            return newItem;
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
            newItem['city'] = item.City;
            newItem['state'] = item.State;
            newItem['county'] = item.County;
            newItem['lat'] = item.Lat;
            newItem['long'] = item.Long;
            newItem['name'] = item.Name;
            newItem['totalPopulation'] = item.TotalPopulation == null ? { value: null } : { value: sizeup.util.numbers.format.addCommas(item.TotalPopulation) };
            newItem['totalRevenue'] = item.TotalRevenue == null ? { value: null } : { value: '$' + sizeup.util.numbers.format.addCommas(item.TotalRevenue)};

            if (me.opts.filterOptions['averageRevenue'] != null || me.opts.filterOptions.attribute == 'averageRevenue') {
                newItem['averageRevenue'] = item.AverageRevenue == null ? { value: null } : { value: '$' + sizeup.util.numbers.format.addCommas(item.AverageRevenue)};
            }
            if (me.opts.filterOptions['totalEmployees'] != null || me.opts.filterOptions.attribute == 'totalEmployees') {
                newItem['totalEmployees'] = item.TotalEmployees == null ? { value: null } : { value: sizeup.util.numbers.format.addCommas(item.TotalEmployees)};
            }
            if (me.opts.filterOptions['revenuePerCapita'] != null || me.opts.filterOptions.attribute == 'revenuePerCapita') {
                newItem['revenuePerCapita'] = item.RevenuePerCapita == null ? { value: null } : { value: '$' + sizeup.util.numbers.format.addCommas(item.RevenuePerCapita)};
            }
            if (me.opts.filterOptions['householdIncome'] != null || me.opts.filterOptions.attribute == 'householdIncome') {
                newItem['householdIncome'] = item.HouseholdIncome == null ? { value: null } : { value: '$' + sizeup.util.numbers.format.addCommas(item.HouseholdIncome)};
            }
            if (me.opts.filterOptions['householdExpenditures'] != null || me.opts.filterOptions.attribute == 'householdExpenditures') {
                newItem['householdExpenditures'] = item.HouseholdExpenditures == null ? { value: null } : { value: '$' + sizeup.util.numbers.format.addCommas(sizeup.util.numbers.format.round(item.HouseholdExpenditures,0)) };
            }
            if (me.opts.filterOptions['medianAge'] != null || me.opts.filterOptions.attribute == 'medianAge') {
                newItem['medianAge'] = item.MedianAge == null ? { value: null } : { value: item.MedianAge};
            }
            if (me.opts.filterOptions['highSchoolOrHigher'] != null || me.opts.filterOptions.attribute == 'highSchoolOrHigher') {
                newItem['highSchoolOrHigher'] = item.HighSchoolOrHigher == null ? { value: null } :{ value:  (item.HighSchoolOrHigher * 100).toFixed(1) + '%'};
            }
            if (me.opts.filterOptions['whiteCollarWorkers'] != null || me.opts.filterOptions.attribute == 'whiteCollarWorkers') {
                newItem['whiteCollarWorkers'] = item.WhiteCollarWorkers == null ? { value: null } : { value: (item.WhiteCollarWorkers * 100).toFixed(1) + '%'};
            }
            if (me.opts.filterOptions['bachelorsDegreeOrHigher'] != null || me.opts.filterOptions.attribute == 'bachelorsDegreeOrHigher') {
                newItem['bachelorsDegreeOrHigher'] = item.BachelorsDegreeOrHigher == null ? { value: null } : { value: (item.BachelorsDegreeOrHigher * 100).toFixed(1) + '%' };
            }

  
            newItem['value'] = newItem[me.opts.filterOptions.attribute];
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

        var bindDescription = function () {
            var x = me.content.optionMenu.option.getValue();
            var template = templates.get(x + 'Description');
            var data = {
                industry: me.opts.CurrentIndustry.Name
            };
            me.content.description.html(templates.bind(template, data));
        };


        var nameSortClicked = function () {
            if (me.content.list.sort.name.hasClass('asc')) {
                me.content.list.sort.name.removeClass('asc');
                me.content.list.sort.name.addClass('desc');
                me.opts.filterOptions.sort = 'desc';
                me.opts.filterOptions.sortAttribute = 'name';
            }
            else {
                me.content.list.sort.name.removeClass('desc');
                me.content.list.sort.name.addClass('asc');
                me.opts.filterOptions.sort = 'asc';
                me.opts.filterOptions.sortAttribute = 'name';
            }
            me.content.list.sort.value.direction.removeClass('asc');
            me.content.list.sort.value.direction.removeClass('desc');
            me.opts.filterOptions.template = 'custom';
            setOptionMenu('custom');
            setParameters(me.opts.filterOptions);
            loadReport();
        };

        var valueSortClicked = function () {
            if (me.content.list.sort.value.direction.hasClass('asc')) {
                me.content.list.sort.value.direction.removeClass('asc');
                me.content.list.sort.value.direction.addClass('desc');
                me.opts.filterOptions.sort = 'desc';
            }
            else {
                me.content.list.sort.value.direction.removeClass('desc');
                me.content.list.sort.value.direction.addClass('asc');
                me.opts.filterOptions.sort = 'asc';
            }
            me.content.list.sort.name.removeClass('asc');
            me.content.list.sort.name.removeClass('desc');
            me.opts.filterOptions.template = 'custom';
            me.opts.filterOptions.sortAttribute = me.content.list.sort.value.option.val();
            setOptionMenu('custom');
            setParameters(me.opts.filterOptions);
            loadReport();
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
            new sizeup.core.analytics().advertisingFiltersClicked();

            var p = getParameters();
            var params = getSliderValues();
            if (params[p.attribute]) {
                params.sort = p.sort;
                params.attribute = p.attribute;
                params.sortAttribute = p.sortAttribute;
            }
            else {
                params.sort = 'desc';
                params.attribute = 'totalRevenue';
                params.sortAttribute = 'totalRevenue';
            }
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