﻿(function () {
    sizeup.core.namespace('sizeup.views.topPlaces');
    sizeup.views.topPlaces.topPlaces = function (opts) {


        var defaults = {
            itemsPerPage: 25,
            bandCount: 5,
            bandColors: ['ff0000', 'ff6400', 'ff9600', 'ffc800', 'ffff00'],
            params: {
                placeType: 'city',
                attribute: 'totalRevenue'
            }
        };
        var me = {};
        
        me.opts = $.extend(true, defaults, opts);

        me.data = {
            xhr: {},
            mapPins: []
        };


        me.container = $('#topPlaces');
        var dataLayer = new sizeup.core.data();
        var templates = new sizeup.core.templates(me.container);

       


    

 

        var init = function () {

            var params = getParameters();
            params = $.extend(true, me.opts.params, params);
            jQuery.bbq.pushState(params, 2);




            me.content = {};

            me.loader = me.container.find('.loading.page');

            me.content.container = me.container.find('.content.container').hide().removeClass('hidden');
            me.content.loader = me.content.container.find('.list.container .loading').removeClass('hidden').hide();
            me.content.noResults = me.content.container.find('.list.container .noResults').removeClass('hidden').hide();
           
            me.content.results = me.content.container.find('.list.container .results');
  

            me.content.map = new sizeup.maps.map({
                container: me.content.container.find('.mapContent.container .map')
            });
           

            me.content.filters = {};
            me.content.filters.sliders = {};
            me.content.filters.placeTypeOption = me.content.container.find('#placeTypeOption');



            me.content.attributeMenu = me.content.container.find('#attributeMenu').chosen();
            me.content.regionMenu = me.content.container.find('#regionMenu').chosen();
           






            me.content.filters.sliders['averageRevenue'] = new sizeup.controls.rangeSlider({
                container: me.content.container.find('.filters .options #averageRevenue'),
                values: params['averageRevenue'],
                min: 0,
                max: 11,
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
                    { range: { min: 10, max: 11 }, mappedValue: 50000000, mappedLabel: '$50 million' }
                ],
                onChange: function () { sliderChanged(); }
            });
            

            me.content.filters.sliders['totalRevenue'] = new sizeup.controls.rangeSlider({
                container: me.content.container.find('.filters .options #totalRevenue'),
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
                ],
                onChange: function () { sliderChanged(); }
            });

            me.content.filters.sliders['totalEmployees'] = new sizeup.controls.rangeSlider({
                container: me.content.container.find('.filters .options #totalEmployees'),
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
                ],
                onChange: function () { sliderChanged(); }
            });


            me.content.filters.sliders['averageEmployees'] = new sizeup.controls.rangeSlider({
                container: me.content.container.find('.filters .options #averageEmployees'),
                values: params['averageEmployees'],
                min: 0,
                max: 150,
                onChange: function () { sliderChanged(); }
            });









            //init state
            me.content.filters.placeTypeOption.find('input[data-index=' + params.placeType + ']').attr('checked', 'checked');
            me.content.attributeMenu.val(params.attribute);
            me.content.attributeMenu.trigger('liszt:updated');
            if (params.regionId) {
                me.content.regionMenu.val('r' + params.regionId);
            }
            if (params.stateId) {
                me.content.regionMenu.val('s' + params.stateId);
            }
            me.content.regionMenu.trigger('liszt:updated');

            //events
            me.content.filters.placeTypeOption.find('input[name=placeType]').click(placeTypeClicked);
            me.content.attributeMenu.change(attributeMenuChanged);
            me.content.regionMenu.change(regionMenuChanged);





            me.loader.hide();
            me.content.container.show();

            loadReport();

        };

         
        //////event actions//////////////////
     
        var placeTypeClicked = function (e) {
            e.stopPropagation();
            //var target = $(e.target);
            
            //me.data.activeMapFilter = target.attr('value');
            pushUrlState();
            loadReport();

        };

        var attributeMenuChanged = function (e) {
            pushUrlState();
            loadReport();
        };

        var regionMenuChanged = function (e) {
            pushUrlState();
            loadReport();
        };

        var sliderChanged = function () {
            pushUrlState();
            loadReport();
        }
     

        
        //////////end event actions/////////////////////////////
      
        var pushUrlState = function () {
            var params = {};
            var region = me.content.regionMenu.val();
            params.placeType = me.content.filters.placeTypeOption.find('input:checked').attr('data-index');
            params.attribute = me.content.attributeMenu.val();
            if (region != '') {
                if (region.charAt(0) == 's') {
                    params.stateId = region.substring(1, region.length);
                }
                else if (region.charAt(0) == 'r') {
                    params.regionId = region.substring(1, region.length);
                }
            }

            var p;

            p = me.content.filters.sliders['averageRevenue'].getParam();
            if (p != null) {
                params.averageRevenue = p;
            }
            p = me.content.filters.sliders['totalRevenue'].getParam();
            if (p != null) {
                params.totalRevenue = p;
            }
            p = me.content.filters.sliders['averageEmployees'].getParam();
            if (p != null) {
                params.averageEmployees = p;
            }
            p = me.content.filters.sliders['totalEmployees'].getParam();
            if (p != null) {
                params.totalEmployees = p;
            }


            jQuery.bbq.pushState(params, 2);
        };

      

        var getParameters = function () {
            var params = jQuery.bbq.getState();
            return params;
        };

        var loadReport = function () {

            new sizeup.core.analytics().topPlacesReportLoaded();

            me.content.loader.show();
            me.content.results.hide();

           
            var params = getParameters();

            params.industryId = me.opts.CurrentInfo.CurrentIndustry.Id;
            params.itemCount = me.opts.itemsPerPage;
           

            var reportData = {
                list: null,
                bands: null
            };
            var notifier = new sizeup.core.notifier(function () {

                me.data.xhr['list'] = null;
                //var formattedData = formatData(reportData.list);
                //var formattedBands = formatBands(reportData.bands, params.attribute);


                bindList(reportData.list);
                bindMap(reportData.list);
                //bindBands(formattedBands);
                //bindDescription();

                //bindMap(reportData, params.attribute);



                me.content.loader.hide();
                me.content.results.show();
            });

            if (me.data.xhr['list'] != null) {
                me.data.xhr['list'].abort();
            }

            if (params.placeType == 'city') {
                me.data.xhr['list'] = dataLayer.getTopPlacesByCity(params, notifier.getNotifier(function (data) {
                    reportData.list = formatCityList(data);
                }));
            }
            else if (params.placeType == 'county') {
                me.data.xhr['list'] = dataLayer.getTopPlacesByCounty(params, notifier.getNotifier(function (data) {
                    reportData.list = formatCountyList(data);
                }));
            }
            else if (params.placeType == 'metro') {
                me.data.xhr['list'] = dataLayer.getTopPlacesByMetro(params, notifier.getNotifier(function (data) {
                    reportData.list = formatMetroList(data);
                }));
            }
            else if (params.placeType == 'state') {
                me.data.xhr['list'] = dataLayer.getTopPlacesByState(params, notifier.getNotifier(function (data) {
                    reportData.list = formatStateList(data);
                }));
            }
            else {
                notifier.getNotifier(function () {
                    reportData.list = [];
                })();
            }

           /* params.bands = me.opts.bandCount;
            dataLayer.getBestPlacesToAdvertiseBands(params, notifier.getNotifier(function (data) {
                reportData.bands = data;
            }));*/
        };

        var bindList = function (data) {
            var params = getParameters();

            me.content.results.empty();
            me.content.noResults.hide();
            var html = '';
            for (var x = 0; x < data.length; x++) {
                html = html + templates.bind(templates.get(params.placeType + 'Item'), data[x]);
            }

            me.content.results.html(html);
            if (data.length == 0) {
                me.content.noResults.show();
            }
        };

        var bindMap = function (data) {
            for (var x in me.data.mapPins) {
                me.content.map.removeMarker(me.data.mapPins[x]);
            }
            me.data.mapPins = [];
            var latLngBounds = new sizeup.maps.latLngBounds();

            for (var x in data) {
                var pin = new sizeup.maps.heatPin({
                    position: new sizeup.maps.latLng({ lat: data[x].latLng.Lat, lng: data[x].latLng.Lng }),
                    color: 'ff0000',//getColor(getValue(data.zips.Items[x], attribute), data.bands),
                    title: data[x].label
                });
                latLngBounds.extend(pin.getPosition());
                me.data.mapPins.push(pin);
                me.content.map.addMarker(pin);
            };
            if (me.data.mapPins.length > 0) {
                me.content.map.fitBounds(latLngBounds);
            }
        };

        var formatStateList = function (data) {
            var newData = [];
            var attr = getParameters().attribute;
            for (var x = 0; x < data.length; x++) {
                newData.push({
                    rank: x + 1,
                    latLng: data[x].State.Centroid,
                    label: data[x].State.Name,
                    state: data[x].State,
                    value: extractValue(data[x], attr),
                    formattedValue: formatValue(extractValue(data[x], attr), attr)

                });
            }
            return newData;
        };

        var formatMetroList = function (data) {
            var newData = [];
            var attr = getParameters().attribute;
            for (var x = 0; x < data.length; x++) {
                newData.push({
                    rank: x + 1,
                    latLng: data[x].Metro.Centroid,
                    label: data[x].Metro.Name,
                    metro: data[x].Metro,
                    value: extractValue(data[x], attr),
                    formattedValue: formatValue(extractValue(data[x], attr), attr)

                });
            }
            return newData;
        };

        var formatCountyList = function (data) {
            var newData = [];
            var attr = getParameters().attribute;
            for (var x = 0; x < data.length; x++) {
                newData.push({
                    rank: x + 1,
                    latLng: data[x].County.Centroid,
                    label: data[x].County.Name + ' County , ' +data[x].State.Abbreviation,
                    county: data[x].County,
                    state: data[x].State,
                    value: extractValue(data[x], attr),
                    formattedValue: formatValue(extractValue(data[x], attr), attr)

                });
            }
            return newData;
        };

        var formatCityList = function (data) {
            var newData = [];
            var attr = getParameters().attribute;
            for (var x = 0; x < data.length; x++) {
                newData.push({
                    rank: x + 1,
                    latLng: data[x].City.Centroid,
                    label: data[x].City.Name + ', ' + data[x].State.Abbreviation,
                    city: data[x].City,
                    county: data[x].County,
                    state: data[x].State,
                    value: extractValue(data[x], attr),
                    formattedValue: formatValue(extractValue(data[x], attr), attr)

                });
            }
            return newData;
        };

        var extractValue = function (data, attr) {
            var val = '';
            if (attr == 'totalRevenue') {
                val = data.TotalRevenue;
            }
            else if (attr == 'averageRevenue') {
                val = data.AverageRevenue;
            }
            else if (attr == 'totalEmployees') {
                val = data.TotalEmployees;
            }
            else if (attr == 'averageEmployees') {
                val = data.AverageEmployees;
            }
            else if (attr == 'employeesPerCapita') {
                val = data.EmployeesPerCapita;
            }
            else if (attr == 'revenuePerCapita') {
                val = data.RevenuePerCapita;
            }
            return val;
        };

        var formatValue = function (val, attr) {
            var formattedVal = '';
            if (attr == 'totalRevenue') {
                formattedVal = '$' + sizeup.util.numbers.format.addCommas(val);
            }
            else if (attr == 'averageRevenue') {
                formattedVal = '$' + sizeup.util.numbers.format.addCommas(val);
            }
            else if (attr == 'totalEmployees') {
                formattedVal = sizeup.util.numbers.format.addCommas(val);
            }
            else if (attr == 'averageEmployees') {
                formattedVal = sizeup.util.numbers.format.addCommas(val);
            }
            else if (attr == 'employeesPerCapita') {
                formattedVal = sizeup.util.numbers.format.sigFig(val, 3);
            }
            else if (attr == 'revenuePerCapita') {
                formattedVal = '$' + sizeup.util.numbers.format.addCommas(val);
            }
            return formattedVal;
        };

      


        var publicObj = {

        };
        init();
        return publicObj;
        
    };
})();



