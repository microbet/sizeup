(function () {
    sizeup.core.namespace('sizeup.views.advertising');
    sizeup.views.advertising.advertising = function (opts) {


        var defaults = {
            itemsPerPage: 20,
            bandCount: 5,
            startColor: 'ff0000',
            endColor: 'ffff00',
            filterTemplates: {
                averageRevenue: {
                    label:'Average Revenue',
                    attribute: 'averageRevenue',
                    sort: 'desc',
                    order: 'highToLow',
                    template: 'averageRevenue',
                    page:1
                },
                totalRevenue: {
                    label: 'Total Revenue',
                    attribute: 'totalRevenue',
                    order: 'highToLow',
                    template: 'totalRevenue',
                    sort: 'desc'
                },
                underservedMarkets: {
                    label: 'Revenue Per Capita',
                    attribute: 'underservedMarkets',
                    order: 'lowToHigh',
                    template: 'underservedMarkets',
                    sort: 'asc'
                },
                revenuePerCapita: {
                    label: 'Revenue Per Capita',
                    attribute: 'revenuePerCapita',
                    order: 'highToLow',
                    template: 'revenuePerCapita',
                    sort: 'desc'
                },
                householdIncome: {
                    label: 'Household Income',
                    attribute: 'householdIncome',
                    order: 'highToLow',
                    template: 'householdIncome',
                    sort: 'desc'
                },
                totalEmployees: {
                    label: 'Total Employees',
                    attribute: 'totalEmployees',
                    order: 'highToLow',
                    template: 'totalEmployees',
                    sort: 'desc'
                }
            },
            defaultParams: {
                attribute: 'totalRevenue',
                order: 'highToLow',
                sort: 'desc',
                template: 'totalRevenue',
                page:1
            }
        };
        var me = {};

        me.opts = $.extend(true, defaults, opts);

        me.data = {
            xhr: {},
            mapPins: [],
            activeIndustry: me.opts.CurrentInfo.CurrentIndustry,
            activePlace: me.opts.CurrentInfo.CurrentPlace
        };


        me.container = $('#advertising');
        var templates = new sizeup.core.templates(me.container);
        var notifier = new sizeup.core.notifier(function () { init(); });



      

        
        var params = jQuery.bbq.getState();
        var minDistanceParams = { geographicLocationId: me.opts.CurrentInfo.CurrentPlace.Id, industryId: me.opts.CurrentInfo.CurrentIndustry.Id, itemCount: me.opts.itemsPerPage };
        if (params.template == null) {
            minDistanceParams = $.extend(true, minDistanceParams, me.opts.defaultParams);
        }
       
        sizeup.api.data.getBestPlacesToAdvertiseMinimumDistance(minDistanceParams, notifier.getNotifier(function (data) { me.opts.defaultDistance = data; }));
        sizeup.api.data.getCentroid({ geographicLocationId: me.opts.CurrentInfo.CurrentPlace.Id }, notifier.getNotifier(function (data) { me.data.CityCenter = new sizeup.maps.latLng({ lat: data.Lat, lng: data.Lng }); }));
      


        var init = function () {

            var params = getParameters();
            if (params.template == null) {
                var defaults = $.extend(true, { distance: me.opts.defaultDistance }, me.opts.defaultParams);
                params = $.extend(true, defaults, params);
                jQuery.bbq.pushState(params);
            }

            me.content = {};

            me.loader = me.container.find('.loading.page');

            me.content.container = me.container.find('.content.container').hide().removeClass('hidden');
            me.content.loader = me.container.find('.listWrapper .loading').hide();
            me.content.noResults = me.container.find('.listWrapper .noResults').hide();
            me.content.results = me.container.find('.listWrapper .results');
            me.content.bands = me.container.find('.mapContent.wrapper .bandContainer');
            me.content.description = me.container.find('.description');

            me.content.map = new sizeup.maps.map({
                container: me.container.find('.mapContent.wrapper .map'),
                mapSettings: { panControl: false, zoomControl: true, zoomControlOptions: { style: google.maps.ZoomControlStyle.SMALL } }
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
            me.content.placeSelector.setSelection(me.data.activePlace);


            me.content.attributeMenu = new sizeup.controls.selectList({
                select: me.container.find('#attributeMenu'),
                onChange: function () { attributeMenuChanged(); }
            });
            me.content.attributeMenu.setValue(params.attribute);

            me.content.filters = {};
            me.content.filters.sliders = {};
            me.content.filters.labels = {};

            me.content.filters.container = me.container.find('.filters').hide().removeClass('hidden');

            me.content.filters.filtersToggle = new sizeup.controls.toggleButton(
                {
                    button: me.container.find('#advancedFilters'),
                    onClick: filtersToggleClicked
                });

            me.content.pager = new sizeup.controls.pager({
                container: me.container.find('.pager'),
                currentPage: me.opts.IsAuthenticated ? params.page : 1,
                itemsPerPage: me.opts.IsAuthenticated ? me.opts.itemsPerPage : 3,
                pagesToShow: me.opts.pagesToShow,
                templates: templates,
                templateId: 'pager',
                onUpdate: function (data) { pagerOnUpdate(data); }
            });

            if (me.opts.IsCustomTools)
                me.content.pager.setItemsPerPage(me.opts.itemsPerPage)

            me.content.pager.getContainer().hide();


            me.content.nameSort = me.container.find('.sort .name');
            me.content.valueSort = me.container.find('.sort .value');
      

            me.signinPanel = new sizeup.views.shared.signin({
                container: me.container.find('.signinWrapper .signinPanel'),
                toggle: me.container.find('.signinWrapper .signinToggle')
            });

            updateSort();


            initFilterLabels();
            initSliders();



            ////////////event wirings/////////////

            me.content.changeIndustry.click(changeIndustryClicked);
            me.content.changePlace.click(changePlaceClicked);

            me.content.nameSort.click(nameSortClicked);
            me.content.valueSort.click(valueSortClicked);



          
            me.loader.hide();
            me.content.container.show();

            pushUrlState();
            loadReport();
            
        };

        var initFilterLabels = function () {
            var params = getParameters();

            me.content.filters.labels['distance'] = new sizeup.controls.rangeLabel({
                container: me.container.find('.filterLabels .distance')
            });

            me.content.filters.labels['bachelorsDegreeOrHigher'] = new sizeup.controls.rangeLabel({
                container: me.container.find('.filterLabels .bachelorOrHigher')
            });

            me.content.filters.labels['highSchoolOrHigher'] = new sizeup.controls.rangeLabel({
                container: me.container.find('.filterLabels .highSchoolOrHigher')
            });

            me.content.filters.labels['whiteCollarWorkers'] = new sizeup.controls.rangeLabel({
                container: me.container.find('.filterLabels .whiteCollarWorkers')
            });
         
            me.content.filters.labels['medianAge'] = new sizeup.controls.rangeLabel({
                container: me.container.find('.filterLabels .medianAge')
            });

            me.content.filters.labels['householdExpenditures'] = new sizeup.controls.rangeLabel({
                container: me.container.find('.filterLabels .householdExpenditures')
            });

            me.content.filters.labels['averageRevenue'] = new sizeup.controls.rangeLabel({
                container: me.container.find('.filterLabels .averageRevenue')
            });

            me.content.filters.labels['totalRevenue'] = new sizeup.controls.rangeLabel({
                container: me.container.find('.filterLabels .totalRevenue')
            });

            me.content.filters.labels['totalEmployees'] = new sizeup.controls.rangeLabel({
                container: me.container.find('.filterLabels .totalEmployees')
            });

            me.content.filters.labels['revenuePerCapita'] = new sizeup.controls.rangeLabel({
                container: me.container.find('.filterLabels .revenuePerCapita')
            });

            me.content.filters.labels['householdIncome'] = new sizeup.controls.rangeLabel({
                container: me.container.find('.filterLabels .householdIncome')
            });


            for (var x in me.content.filters.labels) {
                me.content.filters.labels[x].getContainer().delegate('a', 'click', x, function (e) { clearSlider(e.data); });
            }



        };

        var initSliders = function () {
            var params = getParameters();

            me.content.filters.sliders['bachelorsDegreeOrHigher'] = new sizeup.controls.rangeSlider({
                container: me.container.find('.filters .bachelorOrHigher'),
                label: me.container.find('.filters .bachelorOrHigher .valueLabel'),
                range: { min: 1, max: 95 },
                mode: 'min',
                value: params['bachelorsDegreeOrHigher'],
                onChange: function () { sliderChanged('bachelorsDegreeOrHigher'); }
            });

            me.content.filters.sliders['distance'] = new sizeup.controls.rangeSlider({
                container: me.container.find('.filters .distance'),
                label: me.container.find('.filters .distance .valueLabel'),
                range: { min: 1, max: 150 },
                mode: 'min',
                invert: true,
                value: params['distance'],
                onChange: function () { sliderChanged('distance'); }
            });

            me.content.filters.sliders['highSchoolOrHigher'] = new sizeup.controls.rangeSlider({
                container: me.container.find('.filters .highSchoolOrHigher'),
                label: me.container.find('.filters .highSchoolOrHigher .valueLabel'),
                range: { min: 1, max: 95 },
                mode: 'min',
                value: params['highSchoolOrHigher'],
                onChange: function () { sliderChanged('highSchoolOrHigher'); }
            });

            me.content.filters.sliders['whiteCollarWorkers'] = new sizeup.controls.rangeSlider({
                container: me.container.find('.filters .whiteCollarWorkers'),
                label: me.container.find('.filters .whiteCollarWorkers .valueLabel'),
                range: { min: 1, max: 95 },
                mode: 'min',
                value: params['whiteCollarWorkers'],
                onChange: function () { sliderChanged('whiteCollarWorkers'); }
            });

            me.content.filters.sliders['medianAge'] = new sizeup.controls.rangeSlider({
                container: me.container.find('.filters .medianAge'),
                label: me.container.find('.filters .medianAge .valueLabel'),
                range: { min: 1, max: 82 },
                mode: 'range',
                value: params['medianAge'],
                onChange: function () { sliderChanged('medianAge'); }
            });

            me.content.filters.sliders['householdExpenditures'] = new sizeup.controls.rangeSlider({
                container: me.container.find('.filters .householdExpenditures'),
                label: me.container.find('.filters .householdExpenditures .valueLabel'),
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
                container: me.container.find('.filters .averageRevenue'),
                label: me.container.find('.filters .averageRevenue .valueLabel'),
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
                container: me.container.find('.filters .totalRevenue'),
                label: me.container.find('.filters .totalRevenue .valueLabel'),
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
                container: me.container.find('.filters .totalEmployees'),
                label: me.container.find('.filters .totalEmployees .valueLabel'),
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
                container: me.container.find('.filters .revenuePerCapita'),
                label: me.container.find('.filters .revenuePerCapita .valueLabel'),
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
                container: me.container.find('.filters .householdIncome'),
                label: me.container.find('.filters .householdIncome .valueLabel'),
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
                updateFilterLabel(x);
            }


        };













        ////////events/////////////

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
                new sizeup.core.analytics().advertisingIndustryChanged(p);
                var params = getParameters();
                delete params.distance;
                delete params.template;
                params.page = 1;
                var url = document.location.pathname;
                url = url.replace(me.data.activeIndustry.SEOKey, i.SEOKey);
                url = jQuery.param.querystring(url, jQuery.param.querystring(), 2);
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
        };


        var changePlaceClicked = function () {
            me.content.changePlace.hide();
            me.content.placeBox.show();
            me.content.placeBox.focus();
        };

        var onPlaceChange = function (i) {
            if (i.Id != me.data.activePlace.Id) {
                me.content.changePlace.html(i.City.Name + ', ' + i.State.Abbreviation);
                var p = { place: me.data.activePlace.City.Name + ', ' + me.data.activePlace.State.Abbreviation };
                new sizeup.core.analytics().advertisingPlaceChanged(p);
                var params = getParameters();
                delete params.distance;
                delete params.template;
                params.page = 1;
                var url = document.location.href;
                url = url.substring(0, url.indexOf('advertising'));
                url = url + 'advertising/' + i.State.SEOKey + '/' + i.County.SEOKey + '/' + i.City.SEOKey + '/' + me.data.activeIndustry.SEOKey;
                url = jQuery.param.querystring(url, jQuery.param.querystring(), 2);
                url = jQuery.param.fragment(url, params, 2);
                document.location = url;
            }
            else {
                me.content.changePlace.show();
                me.content.placeBox.hide();
            }
        };

        var placeBoxBlur = function () {
            me.content.changePlace.show();
            me.content.placeBox.hide();
        };

        var attributeMenuChanged = function () {
            var attributeItem = me.content.attributeMenu.getValue();
            var params = {
                sort: me.opts.filterTemplates[attributeItem].sort,
                attribute: me.opts.filterTemplates[attributeItem].attribute,
                order: me.opts.filterTemplates[attributeItem].order,
                sortAttribute: me.opts.filterTemplates[attributeItem].attribute,
                template: me.opts.filterTemplates[attributeItem].template,
                page:1
            };

            jQuery.bbq.pushState(params);
            var p = { attribute: attributeItem };
            new sizeup.core.analytics().advertisingAttributeChanged(p);

            updateSort();
            pushUrlState();
            loadReport();
        };

        var sliderChanged = function (attribute) {
            updateFilterLabel(attribute);
            var p = { attribute: attribute };
            new sizeup.core.analytics().advertisingAdvancedFilterChanged(p);
            pushUrlState();
            loadReport();
        };

        var pagerOnUpdate = function (data) {
            var params = getParameters();
            params.page = data.page;
            jQuery.bbq.pushState(params);
            loadReport();
        };

        var nameSortClicked = function () {
            var params = getParameters();
            if (me.content.nameSort.hasClass('desc')) {
                me.content.nameSort.removeClass('desc');
                me.content.nameSort.addClass('asc');
            }
            else {
                me.content.nameSort.removeClass('asc');
                me.content.nameSort.addClass('desc');
            }
            me.content.valueSort.removeClass('asc').removeClass('desc');
            me.content.pager.gotoPage(1);
            if (params.attribute == 'underservedMarkets' || params.attribute == 'revenuePerCapita') {
                me.content.attributeMenu.setValue('revenuePerCapita');
            }
            pushUrlState();
            updateSort();
            loadReport();
        };


        var valueSortClicked = function () {
            var params = getParameters();
            if (me.content.valueSort.hasClass('desc')) {
                me.content.valueSort.removeClass('desc');
                me.content.valueSort.addClass('asc');
            }
            else {
                me.content.valueSort.removeClass('asc');
                me.content.valueSort.addClass('desc');
            }
            me.content.nameSort.removeClass('asc').removeClass('desc');
            me.content.pager.gotoPage(1);
            if (params.attribute == 'underservedMarkets') {
                me.content.attributeMenu.setValue('revenuePerCapita');
            }
            if (params.attribute == 'revenuePerCapita') {
                me.content.attributeMenu.setValue('underservedMarkets');
            }
            pushUrlState();
            updateSort();
            loadReport();
        };
        

        /////////end events//////////
        var getParameters = function () {
            var params = jQuery.bbq.getState();
            return params;
        };

        var pushUrlState = function () {
            var params = $.extend(true, {}, me.opts.defaultParams);
            var pagerData = me.content.pager.getPageData();
            var menuIndex = me.content.attributeMenu.getValue();
            params.attribute = me.opts.filterTemplates[menuIndex].attribute;
            params.order = me.opts.filterTemplates[menuIndex].order;
            params.page = pagerData.page;
            if (me.content.nameSort.hasClass('desc') || me.content.valueSort.hasClass('desc')) {
                params.sort = 'desc';
            }
            if (me.content.nameSort.hasClass('asc') || me.content.valueSort.hasClass('asc')) {
                params.sort = 'asc';
            }
            if (me.content.nameSort.hasClass('asc') || me.content.nameSort.hasClass('desc')) {
                params.sortAttribute = 'name';
            }
            else {
                params.sortAttribute = params.attribute;
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

        var updateSort = function () {
            var params = getParameters();
            me.content.valueSort.html(me.opts.filterTemplates[params.attribute].label);
            me.content.valueSort.removeClass('asc').removeClass('desc');
            me.content.nameSort.removeClass('asc').removeClass('desc');

            if (params.sortAttribute == 'name') {
                me.content.nameSort.addClass(params.sort);
                me.content.nameSort.attr('aria-sort', params.sort + 'ending');
                me.content.valueSort.attr('aria-sort', 'none');
            }
            else {
                me.content.valueSort.addClass(params.sort);
                me.content.valueSort.attr('aria-sort', params.sort + 'ending');
                me.content.nameSort.attr('aria-sort', 'none');
            }
        };



        var updateFilterLabel = function (attribute) {
            var v = me.content.filters.sliders[attribute].getValue();
            me.content.filters.labels[attribute].setValues(v);
            if (v == null && !me.content.filters.labels[attribute].getContainer().hasClass('fixed')) {
                me.content.filters.labels[attribute].hide();
            }
            else {
                me.content.filters.labels[attribute].show();
            }
        };

        var loadReport = function () {

            me.content.loader.show();
            me.content.results.hide();


            var params = getParameters();
            var pagerData = me.content.pager.getPageData();

            params.industryId = me.data.activeIndustry.Id;
            params.geographicLocationId = me.data.activePlace.Id;
            params.itemCount = pagerData.itemsPerPage;
            params.bands = me.opts.bandCount;
            params.page = pagerData.page;

            
            new sizeup.core.analytics().advertisingReportLoaded({ attribute: params.attribute });

            var reportData = {
                zips: null,
                bands: null
            };
            var notifier = new sizeup.core.notifier(function () {


                var heatmapOpts = { startColor: me.opts.startColor, endColor: me.opts.endColor, bands: reportData.bands.length };
                var heatmapColors = new sizeup.maps.heatmapColors(heatmapOpts);
                me.opts.bandColors = heatmapColors.getColors();

                setPager({ Count: reportData.zips.Total, Page: pagerData.page });
                var formattedData = formatData(reportData.zips);
                var formattedBands = formatBands(reportData.bands, params.attribute);

                bindCircle(params.distance);
                bindZipList(formattedData);
                bindBands(formattedBands);
                bindDescription();

                bindMap(reportData, params.attribute);

                me.content.loader.hide();
                me.content.results.show();
            });

            if (me.data.xhr.list != null) {
                me.data.xhr.list.abort();
            }
            if (me.data.xhr.bands != null) {
                me.data.xhr.bands.abort();
            }

            me.data.xhr.list = sizeup.api.data.getBestPlacesToAdvertise(params, notifier.getNotifier(function (data) {
                reportData.zips = data;
                me.data.xhr.list = null;
            }));
            params.bands = me.opts.bandCount;
            me.data.xhr.bands = sizeup.api.data.getBestPlacesToAdvertiseBands(params, notifier.getNotifier(function (data) {
                reportData.bands = data;
                me.data.xhr.bands = null;
            }));

        };


        var setPager = function (data) {
            me.content.pager.setState(data);
            if (data.Count > me.opts.itemsPerPage && (me.opts.IsAuthenticated || me.opts.IsCustomTools)) {
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
            if (attribute == 'totalPopulation') {
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
            else if (attribute == 'underservedMarkets') {
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
                newItem.Min = (item.Min).toFixed(1) + '%';
                newItem.Max = (item.Max).toFixed(1) + '%';
            }
            else if (attribute == 'whiteCollarWorkers') {
                newItem.Min = (item.Min).toFixed(1) + '%';
                newItem.Max = (item.Max).toFixed(1) + '%';
            }
            else if (attribute == 'bachelorsDegreeOrHigher') {
                newItem.Min = (item.Min).toFixed(1) + '%';
                newItem.Max = (item.Max).toFixed(1) + '%';
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
            var params = getParameters();
            var newItem = {};
            if (item.PlaceId) {
                newItem['city'] = { SEOKey: item.CitySEOKey };
                newItem['state'] = { SEOKey: item.StateSEOKey };
                newItem['county'] = { SEOKey: item.CountySEOKey };
            }
            newItem['lat'] = item.Centroid.Lat;
            newItem['long'] = item.Centroid.Long;
            newItem['name'] = item.ZipCode.Name;
            newItem['totalPopulation'] = item.Population == null ? { value: null } : { value: sizeup.util.numbers.format.addCommas(item.Population) };
            newItem['totalRevenue'] = item.TotalRevenue == null ? { value: null } : { value: { min: '$' + sizeup.util.numbers.format.addCommas(item.TotalRevenue.Min), max: '$' + sizeup.util.numbers.format.addCommas(item.TotalRevenue.Max) } };

            if (params['averageRevenue'] != null || params.attribute == 'averageRevenue') {
                newItem['averageRevenue'] = item.AverageRevenue == null ? { value: null } : { value: { min: '$' + sizeup.util.numbers.format.addCommas(item.AverageRevenue.Min), max: '$' + sizeup.util.numbers.format.addCommas(item.AverageRevenue.Max) } };
            }
            if (params['totalEmployees'] != null || params.attribute == 'totalEmployees') {
                newItem['totalEmployees'] = item.TotalEmployees == null ? { value: null } : { value: { min: sizeup.util.numbers.format.addCommas(item.TotalEmployees.Min), max: sizeup.util.numbers.format.addCommas(item.TotalEmployees.Max) } };
            }
            if (params['revenuePerCapita'] != null || params.attribute == 'revenuePerCapita') {
                newItem['revenuePerCapita'] = item.RevenuePerCapita == null ? { value: null } : { value: { min: '$' + sizeup.util.numbers.format.addCommas(item.RevenuePerCapita.Min), max: '$' + sizeup.util.numbers.format.addCommas(item.RevenuePerCapita.Max) } };
            }
            if (params['revenuePerCapita'] != null || params.attribute == 'underservedMarkets') {
                newItem['underservedMarkets'] = item.RevenuePerCapita == null ? { value: null } : { value: { min: '$' + sizeup.util.numbers.format.addCommas(item.RevenuePerCapita.Min), max: '$' + sizeup.util.numbers.format.addCommas(item.RevenuePerCapita.Max) } };
            }
            if (params['householdIncome'] != null || params.attribute == 'householdIncome') {
                newItem['householdIncome'] = item.HouseholdIncome == null ? { value: null } : { value: '$' + sizeup.util.numbers.format.addCommas(item.HouseholdIncome) };
            }
            if (params['householdExpenditures'] != null || params.attribute == 'householdExpenditures') {
                newItem['householdExpenditures'] = item.HouseholdExpenditures == null ? { value: null } : { value: '$' + sizeup.util.numbers.format.addCommas(sizeup.util.numbers.format.round(item.HouseholdExpenditures, 0)) };
            }
            if (params['medianAge'] != null || params.attribute == 'medianAge') {
                newItem['medianAge'] = item.MedianAge == null ? { value: null } : { value: item.MedianAge };
            }
            if (params['highSchoolOrHigher'] != null || params.attribute == 'highSchoolOrHigher') {
                newItem['highSchoolOrHigher'] = item.HighSchoolOrHigher == null ? { value: null } : { value: (item.HighSchoolOrHigher*100).toFixed(1) + '%' };
            }
            if (params['whiteCollarWorkers'] != null || params.attribute == 'whiteCollarWorkers') {
                newItem['whiteCollarWorkers'] = item.WhiteCollarWorkers == null ? { value: null } : { value: (item.WhiteCollarWorkers*100).toFixed(1) + '%' };
            }
            if (params['bachelorsDegreeOrHigher'] != null || params.attribute == 'bachelorsDegreeOrHigher') {
                newItem['bachelorsDegreeOrHigher'] = item.BachelorsDegreeOrHigher == null ? { value: null } : { value: (item.BachelorsDegreeOrHigher*100).toFixed(1) + '%' };
            }


            newItem['value'] = newItem[params.attribute];
            delete newItem[params.attribute];


            return newItem;
        };


        var bindBands = function (data) {
            var params = getParameters();
            var html = '';
            for (var x = 0 ; x < data.length; x++) {
                var d = {
                    color: me.opts.bandColors[x]
                };

                if (params.order == 'highToLow') {
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


        var bindMap = function (data, attribute) {
            for (var x in me.content.mapPins) {
                me.content.map.removeMarker(me.content.mapPins[x]);
            }
            me.content.mapPins = [];
            var latLngBounds = new sizeup.maps.latLngBounds();
            var hasResults = false;
            for (var x in data.zips.Items) {
                var pin = new sizeup.maps.heatPin({
                    position: new sizeup.maps.latLng({ lat: data.zips.Items[x].Centroid.Lat, lng: data.zips.Items[x].Centroid.Lng }),
                    color: getColor(getValue(data.zips.Items[x], attribute), data.bands),
                    title: data.zips.Items[x].ZipCode.Name
                });
                var sw = new sizeup.maps.latLng({ lat: data.zips.Items[x].BoundingBox.SouthWest.Lat, lng: data.zips.Items[x].BoundingBox.SouthWest.Lng });
                var ne = new sizeup.maps.latLng({ lat: data.zips.Items[x].BoundingBox.NorthEast.Lat, lng: data.zips.Items[x].BoundingBox.NorthEast.Lng });
                latLngBounds.extend(sw);
                latLngBounds.extend(ne);
                hasResults = true;
                me.content.mapPins.push(pin);
                me.content.map.addMarker(pin);
            };
            if (hasResults) {
                me.content.map.fitBounds(latLngBounds);
                me.content.noResults.hide();
            }
            else {
                me.content.noResults.show();
            }
        };

        var getColor = function (value, bandData) {
            var color = null;
            if (typeof (value) == 'number') {
                for (var x = 0; x < bandData.length; x++) {
                    if (value >= bandData[x].Min && value <= bandData[x].Max) {
                        color = me.opts.bandColors[x];
                    }
                }
            }
            else {
                for (var x = 0; x < bandData.length; x++) {
                    if (value.Min >= bandData[x].Min && value.Max <= bandData[x].Max) {
                        color = me.opts.bandColors[x];
                    }
                }
            }
            return color;
        };

        var getValue = function (item, attribute) {
            var val = null;
            if (attribute == 'totalPopulation') {
                val = item.Population;
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
            else if (attribute == 'underservedMarkets') {
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

        var bindZipList = function (data) {
            var html = '';
            for (var x in data.Items) {
                var template = templates.get('listItem');
                html = html + templates.bind(template, data.Items[x]);
            }
            me.content.results.html(html);
        };

        var bindDescription = function () {
            var x = me.content.attributeMenu.getValue();
            var template = templates.get(x + 'Description');
            var data = {
                industry: me.data.activeIndustry.Name
            };
            me.content.description.html(templates.bind(template, data));
        };

        var publicObj = {

        };
        return publicObj;
        
    };
})();