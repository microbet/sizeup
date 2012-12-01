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
            'commuteTime'
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
            me.content.filters.filtersToggle = me.content.container.find('.filtersToggle');

            for (var x in filterVars) {
                me.content.filters.sliders[filterVars[x]] = new sizeup.controls.slider({
                    container: me.content.container.find('#' + filterVars[x]),
                    value: params[filterVars[x]],
                    min: 0,
                    max: 99,
                    range: 'max',
                    onChange: function () { sliderChanged(); }
                });
            }

            me.content.industryBox.blur(industryBoxBlur);
            me.content.changeIndustry.click(changeIndustryClicked);
            me.content.filters.filtersToggle.click(filtersToggleClicked);

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
           


            me.loader.hide();
            me.content.container.show();

            loadReport();

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
            me.data.activeIndustry = i;
            me.content.changeIndustry.html(i.Name);
            me.content.changeIndustry.show();
            me.content.industryBox.hide();
            pushUrlState();
            loadReport();
        };

        var industryBoxBlur = function () {         
            me.content.changeIndustry.show();
            me.content.industryBox.hide();
            me.content.industrySelector.setSelection(me.data.activeIndustry);
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



