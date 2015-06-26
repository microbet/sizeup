(function () {
    sizeup.core.namespace('sizeup.views.bestPlaces');
    sizeup.views.bestPlaces.bestPlaces = function (opts) {


        var defaults = {
            itemsPerPage: 25,
            bandCount: 5,
            startColor: 'ff0000',
            endColor: 'ffff00',
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
            activeIndustry: me.opts.CurrentInfo.CurrentIndustry,
            attributes: {
                totalRevenue: 'Total Annual Revenue',
                totalEmployees: 'Total Employees',
                averageRevenue: 'Average Revenue',
                averageEmployees: 'Average Employees',
                employeesPerCapita: 'Employees Per Capita',
                revenuePerCapita: 'Revenue Per Capita',
                underservedMarkets: 'Revenue Per Capita'
            }
        };


        me.container = $('#bestPlaces');
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
            me.content.variableHeader = me.content.container.find('#variableHeader');
            me.content.bands = me.content.container.find('.mapContent.container .bandContainer');

            me.content.map = new sizeup.maps.map({
                container: me.content.container.find('.mapContent.container .map')
            });
           

            me.content.filters = {};
            me.content.filters.sliders = {};
            me.content.filters.labels = {};

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
                revertToSelection: true,
                onChange: function (item) { onIndustryChange(item); },
                onBlur: function () { industryBoxBlur(); }
            });

            me.content.share = sizeup.controls.share({
                container: me.content.container.find('.shareWrapper'),
                options: {
                    embed: {
                        getCode: function () { return getEmbedCode(); },
                        menuItem: me.content.container.find('.share.container .menu .embed'),
                        contentItem: me.content.container.find('.share.container .content .embed')
                    },
                    link: {
                        getCode: function () { return window.location.href; },
                        menuItem: me.content.container.find('.share.container .menu .link'),
                        contentItem: me.content.container.find('.share.container .content .link')
                    }
                }
            });


            me.content.industrySelector.setSelection(me.data.activeIndustry);

            me.content.filters.container = me.content.container.find('.filters').hide().removeClass('hidden');

            me.content.filters.filtersToggle = new sizeup.controls.toggleButton(
               {
                   button: me.content.container.find('.filtersToggle'),
                   onClick: filtersToggleClicked
               });

            initFilterLabels();
            initSliders();



            me.content.changeIndustry.click(changeIndustryClicked);

            //init state
            me.content.placeTypeMenu.setValue(params.placeType);
            me.content.attributeMenu.setValue(params.attribute);
            me.content.variableHeader.html(me.data.attributes[me.content.attributeMenu.getValue()]);

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

        var initFilterLabels = function () {
            var params = getParameters();

            me.content.filters.labels['bachelorOrHigher'] = new sizeup.controls.rangeLabel({
                container: me.content.container.find('.filterLabels .bachelorOrHigher')
            });

            me.content.filters.labels['blueCollarWorkers'] = new sizeup.controls.rangeLabel({
                container: me.content.container.find('.filterLabels .blueCollarWorkers')
            });

            me.content.filters.labels['highSchoolOrHigher'] = new sizeup.controls.rangeLabel({
                container: me.content.container.find('.filterLabels .highSchoolOrHigher')
            });

            me.content.filters.labels['whiteCollarWorkers'] = new sizeup.controls.rangeLabel({
                container: me.content.container.find('.filterLabels .whiteCollarWorkers')
            });

            me.content.filters.labels['airportsNearby'] = new sizeup.controls.rangeLabel({
                container: me.content.container.find('.filterLabels .airportsNearby')
            });

            me.content.filters.labels['youngEducated'] = new sizeup.controls.rangeLabel({
                container: me.content.container.find('.filterLabels .youngEducated')
            });

            me.content.filters.labels['universitiesNearby'] = new sizeup.controls.rangeLabel({
                container: me.content.container.find('.filterLabels .universitiesNearby')
            });

            me.content.filters.labels['commuteTime'] = new sizeup.controls.rangeLabel({
                container: me.content.container.find('.filterLabels .commuteTime')
            });

            me.content.filters.labels['medianAge'] = new sizeup.controls.rangeLabel({
                container: me.content.container.find('.filterLabels .medianAge')
            });

            me.content.filters.labels['householdExpenditures'] = new sizeup.controls.rangeLabel({
                container: me.content.container.find('.filterLabels .householdExpenditures')
            });

            me.content.filters.labels['averageRevenue'] = new sizeup.controls.rangeLabel({
                container: me.content.container.find('.filterLabels .averageRevenue')
            });

            me.content.filters.labels['totalRevenue'] = new sizeup.controls.rangeLabel({
                container: me.content.container.find('.filterLabels .totalRevenue')
            });

            me.content.filters.labels['totalEmployees'] = new sizeup.controls.rangeLabel({
                container: me.content.container.find('.filterLabels .totalEmployees')
            });

            me.content.filters.labels['revenuePerCapita'] = new sizeup.controls.rangeLabel({
                container: me.content.container.find('.filterLabels .revenuePerCapita')
            });

            me.content.filters.labels['householdIncome'] = new sizeup.controls.rangeLabel({
                container: me.content.container.find('.filterLabels .householdIncome')
            });


            for (var x in me.content.filters.labels) {
                me.content.filters.labels[x].getContainer().delegate('a', 'click', x, function (e) {clearSlider(e.data);});
            }



        };

        var initSliders = function () {
            var params = getParameters();

            me.content.filters.sliders['bachelorOrHigher'] = new sizeup.controls.rangeSlider({
                container: me.content.container.find('.filters .bachelorOrHigher'),
                label: me.content.container.find('.filters .bachelorOrHigher .valueLabel'),
                range: {  min: 1,  max: 95 },
                mode: 'min',
                value: params['bachelorOrHigher'],
                onChange: function () { sliderChanged('bachelorOrHigher'); }
            });

            me.content.filters.sliders['blueCollarWorkers'] = new sizeup.controls.rangeSlider({
                container: me.content.container.find('.filters .blueCollarWorkers'),
                label: me.content.container.find('.filters .blueCollarWorkers .valueLabel'),
                range: { min: 1, max: 95 },
                mode: 'min',
                value: params['blueCollarWorkers'],
                onChange: function () { sliderChanged('blueCollarWorkers'); }
            });

            me.content.filters.sliders['highSchoolOrHigher'] = new sizeup.controls.rangeSlider({
                container: me.content.container.find('.filters .highSchoolOrHigher'),
                label: me.content.container.find('.filters .highSchoolOrHigher .valueLabel'),
                range: { min: 1, max: 95 },
                mode: 'min',
                value: params['highSchoolOrHigher'],
                onChange: function () { sliderChanged('highSchoolOrHigher'); }
            });

            me.content.filters.sliders['whiteCollarWorkers'] = new sizeup.controls.rangeSlider({
                container: me.content.container.find('.filters .whiteCollarWorkers'),
                label: me.content.container.find('.filters .whiteCollarWorkers .valueLabel'),
                range: { min: 1, max: 95 },
                mode: 'min',
                value: params['whiteCollarWorkers'],
                onChange: function () { sliderChanged('whiteCollarWorkers'); }
            });


            me.content.filters.sliders['airportsNearby'] = new sizeup.controls.rangeSlider({
                container: me.content.container.find('.filters .airportsNearby'),
                label: me.content.container.find('.filters .airportsNearby .valueLabel'),
                range: { min: 1, max: 9 },
                mode: 'min',
                value: params['airportsNearby'],
                onChange: function () { sliderChanged('airportsNearby'); }
            });

            me.content.filters.sliders['youngEducated'] = new sizeup.controls.rangeSlider({
                container: me.content.container.find('.filters .youngEducated'),
                label: me.content.container.find('.filters .youngEducated .valueLabel'),
                range: { min: 1, max: 15 },
                mode: 'min',
                value: params['youngEducated'],
                onChange: function () { sliderChanged('youngEducated'); }
            });

            me.content.filters.sliders['universitiesNearby'] = new sizeup.controls.rangeSlider({
                container: me.content.container.find('.filters .universitiesNearby'),
                label: me.content.container.find('.filters .universitiesNearby .valueLabel'),
                range: { min: 1, max: 20 },
                mode: 'min',
                value: params['universitiesNearby'],
                onChange: function () { sliderChanged('universitiesNearby'); }
            });

            me.content.filters.sliders['commuteTime'] = new sizeup.controls.rangeSlider({
                container: me.content.container.find('.filters .commuteTime'),
                label: me.content.container.find('.filters .commuteTime .valueLabel'),
                range: { min: 1, max: 60 },
                mode: 'max',
                value: params['commuteTime'],
                onChange: function () { sliderChanged('commuteTime'); }
            });

            me.content.filters.sliders['medianAge'] = new sizeup.controls.rangeSlider({
                container: me.content.container.find('.filters .medianAge'),
                label: me.content.container.find('.filters .medianAge .valueLabel'),
                range: { min: 1, max: 82 },
                mode: 'range',
                value: params['medianAge'],
                onChange: function () { sliderChanged('medianAge'); }
            });
          
            me.content.filters.sliders['householdExpenditures'] = new sizeup.controls.rangeSlider({
                container: me.content.container.find('.filters .householdExpenditures'),
                label: me.content.container.find('.filters .householdExpenditures .valueLabel'),
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
                ],
                onChange: function () { sliderChanged('householdExpenditures'); }
            });

            me.content.filters.sliders['averageRevenue'] = new sizeup.controls.rangeSlider({
                container: me.content.container.find('.filters .averageRevenue'),
                label: me.content.container.find('.filters .averageRevenue .valueLabel'),
                mode: 'range',
                value: params['averageRevenue'],
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
                ],
                onChange: function () { sliderChanged('averageRevenue'); }
            });

            me.content.filters.sliders['totalRevenue'] = new sizeup.controls.rangeSlider({
                container: me.content.container.find('.filters .totalRevenue'),
                label: me.content.container.find('.filters .totalRevenue .valueLabel'),
                mode: 'range',
                value: params['totalRevenue'],
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
                ],
                onChange: function () { sliderChanged('totalRevenue'); }
            });

            me.content.filters.sliders['totalEmployees'] = new sizeup.controls.rangeSlider({
                container: me.content.container.find('.filters .totalEmployees'),
                label: me.content.container.find('.filters .totalEmployees .valueLabel'),
                mode: 'range',
                value: params['totalEmployees'],
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
                ],
                onChange: function () { sliderChanged('totalEmployees'); }
            });

            me.content.filters.sliders['revenuePerCapita'] = new sizeup.controls.rangeSlider({
                container: me.content.container.find('.filters .revenuePerCapita'),
                label: me.content.container.find('.filters .revenuePerCapita .valueLabel'),
                mode: 'range',
                value: params['revenuePerCapita'],
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
                ],
                onChange: function () { sliderChanged('revenuePerCapita'); }
            });


            me.content.filters.sliders['householdIncome'] = new sizeup.controls.rangeSlider({
                container: me.content.container.find('.filters .householdIncome'),
                label: me.content.container.find('.filters .householdIncome .valueLabel'),
                mode: 'range',
                value: params['householdIncome'],
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
                ],
                onChange: function () { sliderChanged('householdIncome'); }
            });


            for (var x in me.content.filters.labels) {
                var v = me.content.filters.sliders[x].getValue();
                me.content.filters.labels[x].setValues(v);
                if (v == null) {
                    me.content.filters.labels[x].hide();
                }
                else {
                    me.content.filters.labels[x].show();
                }
            }


        };
         
        //////event actions//////////////////
     
        var toggleSource = function () {
            me.content.source.slideToggle();
        };

        var clearSlider = function (index) {
            me.content.filters.sliders[index].setParam(null);
        };

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
                me.content.changeIndustry.html(i.Name);
                var p = { industry: me.data.activeIndustry.Name };
                new sizeup.core.analytics().bestPlacesIndustryChanged(p);
                var params = getParameters();
                var url = document.location.pathname;
                url = url.replace(me.data.activeIndustry.SEOKey, i.SEOKey);
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

        var placeTypeMenuChanged = function (e) {
            var p = { placeType: me.content.placeTypeMenu.getValue() };
            new sizeup.core.analytics().bestPlacesPlaceTypeChanged(p);
            pushUrlState();
            loadReport();
        };

        var attributeMenuChanged = function (e) {
            me.content.variableHeader.html(me.data.attributes[me.content.attributeMenu.getValue()]);
            var p = { attribute: me.content.attributeMenu.getValue() };
            new sizeup.core.analytics().bestPlacesAttributeChanged(p);
            pushUrlState();
            loadReport();
        };

        var regionMenuChanged = function (e) {
            var p = { region: me.content.regionMenu.getName() };
            new sizeup.core.analytics().bestPlacesRegionChanged(p);
            pushUrlState();
            loadReport();
        };

        var sliderChanged = function (attribute) {
            var v = me.content.filters.sliders[attribute].getValue();
            me.content.filters.labels[attribute].setValues(v);
            if (v == null) {
                me.content.filters.labels[attribute].hide();
            }
            else {
                me.content.filters.labels[attribute].show();
            }
            var p = { attribute: attribute };
            new sizeup.core.analytics().bestPlacesAdvancedFilterChanged(p);
            pushUrlState();
            loadReport();
        };

        //////////end event actions/////////////////////////////
      
        var getEmbedCode = function () {
            var base = '/widget/get/bestPlaces/';
            var p = getParameters();
            p.industry = me.data.activeIndustry.SEOKey;
            var url = jQuery.param.fragment(base, p, 2);
            var code =
            '<div>' +
            '<span><a href="//' + window.location.host + '" target="_blank">SizeUp</a></span>' +
            '<script src="' + window.location.protocol + '//' + window.location.host + url + '"></script>' +
            '</div>';

            return code;
        };


        var formatSliderData = function (data) {
            var obj = null;
            if (data.length == 2) {
                obj = {
                    min: data[0],
                    max: data[1]
                };
            }
            else if (data.length == 1) {
                obj = {
                    value: data[0]
                };
            }
            return obj;
        };


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

            var p;
            for (var x in me.content.filters.sliders) {
                p = me.content.filters.sliders[x].getParam();
                if (p != null) {
                    params[x] = p;
                }
            }          
            jQuery.bbq.pushState(params, 2);
        };

      

        var getParameters = function () {
            var params = jQuery.bbq.getState();
            return params;
        };

        var loadReport = function () {


            me.content.loader.show();
            me.content.results.hide();

           
            var params = getParameters();

            params.industryId = me.data.activeIndustry.Id;
            params.itemCount = me.opts.itemsPerPage;
            params.bands = me.opts.bandCount;

            var p = { label: params.attribute };
            new sizeup.core.analytics().bestPlacesReportLoaded(p);




            var reportData = {
                list: null,
                bands: null
            };
            var notifier = new sizeup.core.notifier(function () {

                me.data.xhr['list'] = null;
                me.data.xhr['bands'] = null;


                var heatmapOpts = { startColor: me.opts.startColor, endColor: me.opts.endColor, bands: reportData.bands.length };
                var heatmapColors = new sizeup.maps.heatmapColors(heatmapOpts);
                me.opts.bandColors = heatmapColors.getColors();

                bindList(reportData.list);
                bindBands(formatBands(reportData.bands));
                bindMap(reportData);
               

                me.content.loader.hide();
                me.content.results.show();
            });

            if (me.data.xhr['list'] != null) {
                me.data.xhr['list'].abort();
            }
            if (me.data.xhr['bands'] != null) {
                me.data.xhr['bands'].abort();
            }

            params.granularity = params.placeType;


            if (params.placeType == 'city') {
                me.data.xhr['list'] = sizeup.api.data.getBestPlaces(params, notifier.getNotifier(function (data) {
                    reportData.list = formatCityList(data);
                }));
                
                me.data.xhr['bands'] = sizeup.api.data.getBestPlacesBands(params, notifier.getNotifier(function (data) {
                    reportData.bands = data;
                }));
            }
            else if (params.placeType == 'county') {
                me.data.xhr['list'] = sizeup.api.data.getBestPlaces(params, notifier.getNotifier(function (data) {
                    reportData.list = formatCountyList(data);
                }));
                
                me.data.xhr['bands'] = sizeup.api.data.getBestPlacesBands(params, notifier.getNotifier(function (data) {
                    reportData.bands = data;
                }));
            }
            else if (params.placeType == 'metro') {
                me.data.xhr['list'] = sizeup.api.data.getBestPlaces(params, notifier.getNotifier(function (data) {
                    reportData.list = formatMetroList(data);
                }));

                me.data.xhr['bands'] = sizeup.api.data.getBestPlacesBands(params, notifier.getNotifier(function (data) {
                    reportData.bands = data;
                }));
            }
            else if (params.placeType == 'state') {
                me.data.xhr['list'] = sizeup.api.data.getBestPlaces(params, notifier.getNotifier(function (data) {
                    reportData.list = formatStateList(data);
                }));

                me.data.xhr['bands'] = sizeup.api.data.getBestPlacesBands(params, notifier.getNotifier(function (data) {
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
                    d.label = x == data.length - 1 ? data[x].Max + ' and below' : data[x].Min + ' - ' + data[x].Max;
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
                    position: new sizeup.maps.latLng({ lat: data.list[x].centroid.Lat, lng: data.list[x].centroid.Lng }),
                    color: getColor(data.list[x], data.bands),
                    title: data.list[x].label
                });
                //create 2 new latlngs here
                var sw = new sizeup.maps.latLng({ lat: data.list[x].southWest.Lat, lng: data.list[x].southWest.Lng });
                var ne = new sizeup.maps.latLng({ lat: data.list[x].northEast.Lat, lng: data.list[x].northEast.Lng });
                latLngBounds.extend(sw);
                latLngBounds.extend(ne);

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
                if (value.minValue >= bandData[x].Min && value.maxValue <= bandData[x].Max) {
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
                    centroid: data[x].Centroid,
                    southWest: data[x].BoundingBox.SouthWest,
                    northEast: data[x].BoundingBox.NorthEast,
                    label: data[x].State.Name,
                    state: data[x].State,
                    industry: me.data.activeIndustry,
                    minValue: extractValue(data[x], attr).Min,
                    maxValue: extractValue(data[x], attr).Max,
                    formattedMin: formatValue(extractValue(data[x], attr).Min, attr),
                    formattedMax: formatValue(extractValue(data[x], attr).Max, attr)

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
                    centroid: data[x].Centroid,
                    southWest: data[x].BoundingBox.SouthWest,
                    northEast: data[x].BoundingBox.NorthEast,
                    label: data[x].Metro.Name,
                    metro: data[x].Metro,
                    industry: me.data.activeIndustry,
                    minValue: extractValue(data[x], attr).Min,
                    maxValue: extractValue(data[x], attr).Max,
                    formattedMin: formatValue(extractValue(data[x], attr).Min, attr),
                    formattedMax: formatValue(extractValue(data[x], attr).Max, attr)

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
                    centroid: data[x].Centroid,
                    southWest: data[x].BoundingBox.SouthWest,
                    northEast: data[x].BoundingBox.NorthEast,
                    label: data[x].County.Name + ' County , ' + data[x].State.Abbreviation,
                    county: data[x].County,
                    state: data[x].State,
                    industry: me.data.activeIndustry,
                    minValue: extractValue(data[x], attr).Min,
                    maxValue: extractValue(data[x], attr).Max,
                    formattedMin: formatValue(extractValue(data[x], attr).Min, attr),
                    formattedMax: formatValue(extractValue(data[x], attr).Max, attr)

                });
            }
            return newData;
        };

        var formatCityList = function (data) {
            var newData = [];
            var attr = getParameters().attribute;
            for (var x = 0; x < data.length; x++) {
                data[x].City.Counties[data[x].City.Counties.length - 1].last = true;
                newData.push({
                    rank: x + 1,
                    centroid: data[x].Centroid,
                    southWest: data[x].BoundingBox.SouthWest,
                    northEast: data[x].BoundingBox.NorthEast,
                    label: data[x].City.Name + ', ' + data[x].State.Abbreviation,
                    city: data[x].City,
                    county: data[x].County,
                    state: data[x].State,
                    counties: data[x].City.Counties,
                    industry: me.data.activeIndustry,
                    minValue: extractValue(data[x], attr).Min,
                    maxValue : extractValue(data[x], attr).Max,
                    formattedMin: formatValue(extractValue(data[x], attr).Min, attr),
                    formattedMax: formatValue(extractValue(data[x], attr).Max, attr)
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
                formattedVal = '$' + sizeup.util.numbers.format.abbreviate(val, 2);
            }
            else if (attr == 'averageRevenue') {
                formattedVal = '$' + sizeup.util.numbers.format.abbreviate(val, 2);
            }
            else if (attr == 'totalEmployees') {
                formattedVal = sizeup.util.numbers.format.abbreviate(val, 2);
            }
            else if (attr == 'averageEmployees') {
                formattedVal = sizeup.util.numbers.format.abbreviate(val, 2);
            }
            else if (attr == 'employeesPerCapita') {
                formattedVal = sizeup.util.numbers.format.sigFig(val, 3);
            }
            else if (attr == 'revenuePerCapita') {
                formattedVal = '$' + sizeup.util.numbers.format.abbreviate(val, 2);
            }
            else if (attr == 'underservedMarkets') {
                formattedVal = '$' + sizeup.util.numbers.format.abbreviate(val, 2);
            }
            return formattedVal;
        };

      


        var publicObj = {

        };
        init();
        return publicObj;
        
    };
})();



