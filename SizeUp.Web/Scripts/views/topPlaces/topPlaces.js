(function () {
    sizeup.core.namespace('sizeup.views.topPlaces');
    sizeup.views.topPlaces.topPlaces = function (opts) {


        var defaults = {
            itemsPerPage: 25
        };
        var me = {};
        
        me.opts = $.extend(true, defaults, opts);

        me.data = {
            
        };
        me.opts.filterOptions = {};


        me.container = $('#topPlaces');
        var dataLayer = new sizeup.core.data();
        var templates = new sizeup.core.templates(me.container);

       


        var notifier = new sizeup.core.notifier(function () { init(); });
        
        dataLayer.getPlaceBoundingBox({id: opts.CurrentInfo.CurrentPlace.Id}, notifier.getNotifier(function (data) { 
            me.data.cityBoundingBox = new sizeup.maps.latLngBounds();
            me.data.cityBoundingBox.extend(new sizeup.maps.latLng({lat: data[0].Lat, lng: data[0].Lng}));
            me.data.cityBoundingBox.extend(new sizeup.maps.latLng({lat: data[1].Lat, lng: data[1].Lng}));
        }));

        var params = jQuery.bbq.getState();

       

 

        var init = function () {
            me.content = {};

            me.loader = me.container.find('.loading.page');

            me.content.container = me.container.find('.content.container').hide().removeClass('hidden');
            me.content.loader = me.content.container.find('.list.container .loading').removeClass('hidden').hide();
            me.content.noResults = me.content.container.find('.list.container .noResults').removeClass('hidden').hide();
           
            me.content.results = me.content.container.find('.list.container .results');
  

            me.content.map = new sizeup.maps.map({
                container: me.content.container.find('.mapContent.container .map')
            });
           // me.content.map.fitBounds(me.data.cityBoundingBox);
            //set bounds to be USA
 


            me.content.filterSettingsButton = me.content.container.find('#filterSettingsButton');
            me.content.filterSettingsButton.click(function () { filterSettingsButtonClicked(); });

            me.content.optionMenu = {};
            me.content.optionMenu.option = me.content.container.find('#optionMenu');
            me.content.optionMenu.custom = me.content.optionMenu.option.find('.custom');
            me.content.optionMenu.custom.remove();
            me.content.optionMenu.menu = me.content.optionMenu.option.chosen();
            me.content.optionMenu.menu.change(optionMenuChanged);

            me.content.filters = {};
            me.content.filters.container = me.content.container.find('#filterSettings').hide().removeClass('hidden');




            me.loader.hide();
            me.content.container.show();

            loadReport();

        };

         
        //////event actions//////////////////
     
        var optionMenuChanged = function (e) {

        };

        var filterSettingsButtonClicked = function () {
            me.content.filters.container.slideToggle();
        };

        
        //////////end event actions/////////////////////////////
      
        var setParameters = function (params) {
            me.opts.filterOptions = $.extend(true, {}, params);
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

            //setValueMenu();
            params.industryId = me.opts.CurrentInfo.CurrentIndustry.Id;
            params.itemCount = me.opts.itemsPerPage;
           

            var reportData = {
                list: null,
                bands: null
            };
            var notifier = new sizeup.core.notifier(function () {

               // var formattedData = formatData(reportData.zips);
                //var formattedBands = formatBands(reportData.bands, params.attribute);


                //bindList(formattedData);
                //bindBands(formattedBands);
                //bindDescription();

                //bindMap(reportData, params.attribute);



                me.content.loader.hide();
                me.content.results.show();
            });

            dataLayer.getTopPlacesByCity(params, notifier.getNotifier(function (data) {
                reportData.list = data;
            }));

           /* params.bands = me.opts.bandCount;
            dataLayer.getBestPlacesToAdvertiseBands(params, notifier.getNotifier(function (data) {
                reportData.bands = data;
            }));*/
        };

        var publicObj = {

        };
        return publicObj;
        
    };
})();



