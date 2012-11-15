(function () {
    sizeup.core.namespace('sizeup.views.topPlaces');
    sizeup.views.topPlaces.topPlaces = function (opts) {


        var defaults = {
            itemsPerPage: 25,
            bandCount: 5,
            bandColors: ['ff0000', 'ff6400', 'ff9600', 'ffc800', 'ffff00'],
            params: {
                placeType: 'state',
                attribute:'totalRevenue'
            }
        };
        var me = {};
        
        me.opts = $.extend(true, defaults, opts);

        me.data = {
            xhr: {}
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
            me.content.filters.placeTypeOption = me.content.container.find('#filterSettings #placeTypeOption');

            me.content.filterSettingsButton = me.content.container.find('#filterSettingsButton');
            me.content.filterSettingsButton.click(function () { filterSettingsButtonClicked(); });

            me.content.optionMenu = me.content.container.find('#optionMenu').chosen();
            me.content.optionMenu.change(optionMenuChanged);

            /*
            me.content.filters = {};
            me.content.filters.container = me.content.container.find('#filterSettings').hide().removeClass('hidden');
            */

            //init state
            me.content.filters.placeTypeOption.find('input[data-index=' + params.placeType + ']').attr('checked', 'checked');


            //events
            me.content.filters.placeTypeOption.find('input[name=placeType]').click(placeTypeClicked);

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

        var optionMenuChanged = function (e) {

        };

        var filterSettingsButtonClicked = function () {
            me.content.filters.container.slideToggle();
        };

        
        //////////end event actions/////////////////////////////
      
        var pushUrlState = function () {
            var params = getParameters();
            params.placeType = me.content.filters.placeTypeOption.find('input:checked').attr('data-index');

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

        };

        var formatStateList = function (data) {
            var newData = [];
            for (var x = 0; x < data.length; x++) {
                newData.push({
                    rank: x + 1,
                    state: data[x].State,
                    value: extractValue(data[x])

                });
            }
            return newData;
        };

        var formatMetroList = function (data) {
            var newData = [];
            for (var x = 0; x < data.length; x++) {
                newData.push({
                    rank: x + 1,
                    metro: data[x].Metro,
                    value: extractValue(data[x])

                });
            }
            return newData;
        };

        var formatCountyList = function (data) {
            var newData = [];
            for (var x = 0; x < data.length; x++) {
                newData.push({
                    rank: x + 1,
                    county: data[x].County,
                    state: data[x].State,
                    value: extractValue(data[x])

                });
            }
            return newData;
        };

        var formatCityList = function (data) {
            var newData = [];
            for (var x = 0; x < data.length; x++) {
                newData.push({
                    rank: x + 1,
                    city: data[x].City,
                    county: data[x].County,
                    state: data[x].State,
                    value: extractValue(data[x])

                });
            }
            return newData;
        };

        var extractValue = function (data) {
            var formattedVal = '';
            var attr = getParameters().attribute;
            if (attr == 'totalRevenue') {
                formattedVal = '$' + sizeup.util.numbers.format.addCommas(data.TotalRevenue);
            }
            else if (attr == 'averageRevenue') {
                formattedVal = '$' + sizeup.util.numbers.format.addCommas(data.AverageRevenue);
            }
            else if (attr == 'totalEmployees') {
                formattedVal = sizeup.util.numbers.format.addCommas(data.TotalEmployees);
            }
            else if (attr == 'averageEmployees') {
                formattedVal = sizeup.util.numbers.format.addCommas(data.AverageEmployees);
            }
            else if (attr == 'employeesPerCapita') {
                formattedVal = sizeup.util.numbers.format.addCommas(data.EmployeesPerCapita);
            }
            return formattedVal;
        };

       


        var publicObj = {

        };
        init();
        return publicObj;
        
    };
})();



