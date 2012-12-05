(function () {
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
            mapPins: [],
            activeIndustry: me.opts.CurrentInfo.CurrentIndustry
        };


        me.container = $('#topPlaces');
        var dataLayer = new sizeup.core.data();
        var templates = new sizeup.core.templates(me.container);

       
        var filterVars = [
            'bachelorOrHigher',
            'blueCollarWorkers',
            'highSchoolOrHigher',
            'whiteCollarWorkers',
            'airportsNearby',
            'youngEducated',
            'universitiesNearby',
            'commuteTime',
            'medianAge',
            'householdExpenditures',
            'householdIncome'
        ];


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
            me.content.variableHeader = me.content.container.find('#variableHeader');
            me.content.bands = me.content.container.find('.mapContent.container .bandContainer');

            me.content.map = new sizeup.maps.map({
                container: me.content.container.find('.mapContent.container .map')
            });
           

            me.content.filters = {};
            me.content.filters.sliders = {};

            me.content.industryBox = me.content.container.find('#industryBox').hide().removeClass('hidden');
            me.content.changeIndustry = me.content.container.find('#changeIndustry');

            me.content.attributeMenu = new sizeup.controls.selectList({
                select: me.content.container.find('#attributeMenu'),
                onChange: function () { attributeMenuChanged(); }
            });
            me.content.regionMenu = new sizeup.controls.selectList({
                select: me.content.container.find('#regionMenu'),
                onChange: function () { regionMenuChanged(); }
            });
            me.content.placeTypeMenu = new sizeup.controls.selectList({
                select: me.content.container.find('#placeTypeMenu'),
                onChange: function () { placeTypeMenuChanged(); }
            });
           
            me.content.industrySelector = sizeup.controls.industrySelector({
                textbox: me.content.industryBox,
                onChange: function (item) { onIndustryChange(item); }
            });

            me.content.industrySelector.setSelection(me.data.activeIndustry);

            me.content.filters.container = me.content.container.find('.filters').hide().removeClass('hidden');

            me.content.filters.filtersToggle = new sizeup.controls.toggleButton(
               {
                   button: me.content.container.find('.filtersToggle'),
                   onClick: filtersToggleClicked
               });


            initSliders();



            me.content.populationMin = new sizeup.controls.promptBox({
                textbox: me.content.container.find('#populationMin'),
                onChange: populationChanged
            });

            me.content.populationMax = new sizeup.controls.promptBox({
                textbox: me.content.container.find('#populationMax'),
                onChange: populationChanged
            });


            me.content.industryBox.blur(industryBoxBlur);
            me.content.changeIndustry.click(changeIndustryClicked);

            //init state
            me.content.placeTypeMenu.setValue(params.placeType);
            me.content.attributeMenu.setValue(params.attribute);
            me.content.variableHeader.html(me.content.attributeMenu.getName());

            if (params.regionId) {
                me.content.regionMenu.setValue('r' + params.regionId);
            }
            if (params.stateId) {
                me.content.regionMenu.setValue('s' + params.stateId);
            }
            if (params.population) {
                me.content.populationMin.setValue(params.population[0]);
                me.content.populationMax.setValue(params.population[1]);
            }
           
           


            me.loader.hide();
            me.content.container.show();

            loadReport();

        };

        var initSliders = function () {
            var params = getParameters();

            /*
            'bachelorOrHigher',
'blueCollarWorkers',
'highSchoolOrHigher',
'whiteCollarWorkers',
'airportsNearby',
'youngEducated',
'universitiesNearby',
'commuteTime',
'medianAge',
'householdExpenditures',
'householdIncome'

            revenuePerCapita
            totalEmployees
            totalRevenue
            averageRevenue

            */




     
            me.content.filters.sliders['bachelorOrHigher'] = new sizeup.controls.slider({
                container: me.content.container.find('#bachelorOrHigher'),
                value: params['bachelorOrHigher'],
                min: 0,
                max: 95,
                range: 'max',
                onChange: function () { sliderChanged(); }
            });

            me.content.filters.sliders['blueCollarWorkers'] = new sizeup.controls.slider({
                container: me.content.container.find('#blueCollarWorkers'),
                value: params['blueCollarWorkers'],
                min: 0,
                max: 95,
                range: 'max',
                onChange: function () { sliderChanged(); }
            });

            me.content.filters.sliders['highSchoolOrHigher'] = new sizeup.controls.slider({
                container: me.content.container.find('#highSchoolOrHigher'),
                value: params['highSchoolOrHigher'],
                min: 0,
                max: 95,
                range: 'max',
                onChange: function () { sliderChanged(); }
            });

            me.content.filters.sliders['whiteCollarWorkers'] = new sizeup.controls.slider({
                container: me.content.container.find('#whiteCollarWorkers'),
                value: params['whiteCollarWorkers'],
                min: 0,
                max: 95,
                range: 'max',
                onChange: function () { sliderChanged(); }
            });


            me.content.filters.sliders['airportsNearby'] = new sizeup.controls.slider({
                container: me.content.container.find('#airportsNearby'),
                value: params['airportsNearby'],
                min: 0,
                max: 9,
                range: 'max',
                onChange: function () { sliderChanged(); }
            });

            me.content.filters.sliders['youngEducated'] = new sizeup.controls.slider({
                container: me.content.container.find('#youngEducated'),
                value: params['youngEducated'],
                min: 0,
                max: 95,
                range: 'max',
                onChange: function () { sliderChanged(); }
            });

            me.content.filters.sliders['universitiesNearby'] = new sizeup.controls.slider({
                container: me.content.container.find('#universitiesNearby'),
                value: params['universitiesNearby'],
                min: 0,
                max: 20,
                range: 'max',
                onChange: function () { sliderChanged(); }
            });

            me.content.filters.sliders['commuteTime'] = new sizeup.controls.slider({
                container: me.content.container.find('#commuteTime'),
                value: params['commuteTime'],
                min: 0,
                max: 60,
                range: 'min',
                onChange: function () { sliderChanged(); }
            });

            me.content.filters.sliders['medianAge'] = new sizeup.controls.rangeSlider({
                container: me.content.container.find('#medianAge'),
                values: params['medianAge'],
                min: 0,
                max: 82,
                onChange: function () { sliderChanged(); }
            });
          
            me.content.filters.sliders['householdExpenditures'] = new sizeup.controls.rangeSlider({
                container: me.content.container.find('#householdExpenditures'),
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

            me.content.filters.sliders['averageRevenue'] = new sizeup.controls.rangeSlider({
                container: me.content.container.find('#averageRevenue'),
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


            me.content.filters.sliders['totalRevenue'] = new sizeup.controls.rangeSlider({
                container: me.content.container.find('#totalRevenue'),
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

            me.content.filters.sliders['totalEmployees'] = new sizeup.controls.rangeSlider({
                container: me.content.container.find('#totalEmployees'),
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

            me.content.filters.sliders['revenuePerCapita'] = new sizeup.controls.rangeSlider({
                container: me.content.container.find('#revenuePerCapita'),
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


            me.content.filters.sliders['householdIncome'] = new sizeup.controls.rangeSlider({
                container: me.content.container.find('#householdIncome'),
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





        };
         
        //////event actions//////////////////
     
       

        var filtersToggleClicked = function () {
            me.content.filters.container.slideToggle(500);
        };

        var changeIndustryClicked = function () {
            me.content.changeIndustry.hide();
            me.content.industryBox.show();
            me.content.industryBox.focus();
        };

        var onIndustryChange = function (i) {
            if (i.Id != me.data.activeIndustry.Id) {
                var params = getParameters();
                var url = '/topplaces/' + i.SEOKey + '/';
                url = jQuery.param.fragment(url, params, 2);
                document.location = url;
            }
            else {
                me.content.changeIndustry.show();
                me.content.industryBox.hide();
            }
        };

        var industryBoxBlur = function () {         
            me.content.changeIndustry.show();
            me.content.industryBox.hide();
            me.content.industrySelector.setSelection(me.data.activeIndustry);
        };

        var populationChanged = function () {
            pushUrlState();
            loadReport();
        }; 
        var placeTypeMenuChanged = function (e) {
            pushUrlState();
            loadReport();
        };

        var attributeMenuChanged = function (e) {
            me.content.variableHeader.html(me.content.attributeMenu.getName());
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
            var region = me.content.regionMenu.getValue();
            params.placeType = me.content.placeTypeMenu.getValue();
            params.attribute = me.content.attributeMenu.getValue();
            if (region != '') {
                if (region.charAt(0) == 's') {
                    params.stateId = region.substring(1, region.length);
                }
                else if (region.charAt(0) == 'r') {
                    params.regionId = region.substring(1, region.length);
                }
            }

            var maxPop = me.content.populationMax.getValue();
            var minPop = me.content.populationMin.getValue();

            if (maxPop != '' || minPop != '') {
                params.population = [minPop, maxPop];
            }

            for (var x in filterVars) {
                var p;
                p = me.content.filters.sliders[filterVars[x]].getParam();
                if (p != null) {
                    params[filterVars[x]] = p;
                }
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

            params.industryId = me.data.activeIndustry.Id;
            params.itemCount = me.opts.itemsPerPage;
            params.bands = me.opts.bandCount;

            var reportData = {
                list: null,
                bands: null
            };
            var notifier = new sizeup.core.notifier(function () {

                me.data.xhr['list'] = null;
                me.data.xhr['bands'] = null;

                bindList(reportData.list);
                bindBands(formatBands(reportData.bands));
                bindMap(reportData);
               
                //bindDescription();



                me.content.loader.hide();
                me.content.results.show();
            });

            if (me.data.xhr['list'] != null) {
                me.data.xhr['list'].abort();
            }
            if (me.data.xhr['bands'] != null) {
                me.data.xhr['bands'].abort();
            }

            if (params.placeType == 'city') {
                me.data.xhr['list'] = dataLayer.getTopPlacesByCity(params, notifier.getNotifier(function (data) {
                    reportData.list = formatCityList(data);
                }));

                me.data.xhr['bands'] = dataLayer.getTopPlacesBandsByCity(params, notifier.getNotifier(function (data) {
                    reportData.bands = data;
                }));
            }
            else if (params.placeType == 'county') {
                me.data.xhr['list'] = dataLayer.getTopPlacesByCounty(params, notifier.getNotifier(function (data) {
                    reportData.list = formatCountyList(data);
                }));

                me.data.xhr['bands'] = dataLayer.getTopPlacesBandsByCounty(params, notifier.getNotifier(function (data) {
                    reportData.bands = data;
                }));
            }
            else if (params.placeType == 'metro') {
                me.data.xhr['list'] = dataLayer.getTopPlacesByMetro(params, notifier.getNotifier(function (data) {
                    reportData.list = formatMetroList(data);
                }));

                me.data.xhr['bands'] = dataLayer.getTopPlacesBandsByMetro(params, notifier.getNotifier(function (data) {
                    reportData.bands = data;
                }));
            }
            else if (params.placeType == 'state') {
                me.data.xhr['list'] = dataLayer.getTopPlacesByState(params, notifier.getNotifier(function (data) {
                    reportData.list = formatStateList(data);
                }));

                me.data.xhr['bands'] = dataLayer.getTopPlacesBandsByState(params, notifier.getNotifier(function (data) {
                    reportData.bands = data;
                }));
            }
            else {
                notifier.getNotifier(function () {
                    reportData.list = [];
                    reportData.bands = [];
                })();
            }
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

        var bindBands = function (data) {
            var params = getParameters();
            var html = '';
            for (var x = 0 ; x < data.length; x++) {
                var d = {
                    color: me.opts.bandColors[x]
                };

                if (params.attribute != 'underservedMarkets') {
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

        var bindMap = function (data) {
            for (var x in me.data.mapPins) {
                me.content.map.removeMarker(me.data.mapPins[x]);
            }
            me.data.mapPins = [];
            var latLngBounds = new sizeup.maps.latLngBounds();

            for (var x in data.list) {
                var pin = new sizeup.maps.heatPin({
                    position: new sizeup.maps.latLng({ lat: data.list[x].latLng.Lat, lng: data.list[x].latLng.Lng }),
                    color: getColor(data.list[x].value, data.bands),
                    title: data.list[x].label
                });
                latLngBounds.extend(pin.getPosition());
                me.data.mapPins.push(pin);
                me.content.map.addMarker(pin);
            };
            if (me.data.mapPins.length > 0) {
                me.content.map.fitBounds(latLngBounds);
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

        var formatBands = function (data) {
            var newData = [];
            var attr = getParameters().attribute;
            for (var x = 0; x < data.length; x++) {
                newData.push({
                    
                    Min: formatValue(data[x].Min, attr),
                    Max: formatValue(data[x].Max, attr)

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
            else if (attr == 'underservedMarkets') {
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
            else if (attr == 'underservedMarkets') {
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



