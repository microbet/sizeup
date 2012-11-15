﻿(function () {
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
            me.content.filters.placeType = me.content.container.find('#filterSettings input[name=placeType]');

            me.content.filterSettingsButton = me.content.container.find('#filterSettingsButton');
            me.content.filterSettingsButton.click(function () { filterSettingsButtonClicked(); });

            me.content.optionMenu = me.content.container.find('#optionMenu').chosen();
            me.content.optionMenu.change(optionMenuChanged);

            /*
            me.content.filters = {};
            me.content.filters.container = me.content.container.find('#filterSettings').hide().removeClass('hidden');
            */

            //events
            me.content.filters.placeType.click(placeTypeClicked);

            me.loader.hide();
            me.content.container.show();

            loadReport();

        };

         
        //////event actions//////////////////
     
        var placeTypeClicked = function (e) {
            //var target = $(e.target);
            
            //me.data.activeMapFilter = target.attr('value');
            pushUrlState();
        };

        var optionMenuChanged = function (e) {

        };

        var filterSettingsButtonClicked = function () {
            me.content.filters.container.slideToggle();
        };

        
        //////////end event actions/////////////////////////////
      
        var pushUrlState = function () {
            var params = getParameters();

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

                //var formattedData = formatData(reportData.list);
                //var formattedBands = formatBands(reportData.bands, params.attribute);


                bindList(reportData.list);
                //bindBands(formattedBands);
                //bindDescription();

                //bindMap(reportData, params.attribute);



                me.content.loader.hide();
                me.content.results.show();
            });

            if (params.placeType == 'city') {
                dataLayer.getTopPlacesByCity(params, notifier.getNotifier(function (data) {
                    reportData.list = data;
                }));
            }
            else if (params.placeType == 'county') {
                dataLayer.getTopPlacesByCounty(params, notifier.getNotifier(function (data) {
                    reportData.list = data;
                }));
            }
            else if (params.placeType == 'metro') {
                dataLayer.getTopPlacesByMetro(params, notifier.getNotifier(function (data) {
                    reportData.list = data;
                }));
            }
            else if (params.placeType == 'state') {
                dataLayer.getTopPlacesByState(params, notifier.getNotifier(function (data) {
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
                html = html + templates.bind(templates.get('stateItem'), data[x]);
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
            for (var x = 0; x < data.length;x++){
                newData.push({
                    rank: x + 1,
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



