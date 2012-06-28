(function () {
    sizeup.core.namespace('sizeup.views.advertising');
    sizeup.views.advertising.advertising = function (opts) {

        var defaults = {
            mapSettings: sizeup.maps.mapOptions.getDefaults(),
            styles: sizeup.maps.mapStyles.getDefaults(),
            colors: [],
            overlays: [],

   
            slideTime: 500

           
        };
        var dataLayer = new sizeup.core.data();
        var templates = new sizeup.core.templates();
        var notifier = new sizeup.core.notifier(function () { init(); });
        var me = {};
        me.opts = $.extend(true, defaults, opts);
        me.container = $('#advertising');


        

        //dataLayer.getCityCentroid({ id: opts.report.Locations.City.Id }, notifier.getNotifier(function (data) { me.opts.report.MapCenter = data; }));
        var init = function () {
            
            me.content = {};
            me.content.container = me.container.find('.content');

            me.content.map = new sizeup.maps.map({
                container: me.content.container.find('.mapContent .map'),
                mapSettings: me.opts.mapSettings,
                styles: me.opts.styles
            });

            me.content.filterSettingsButton = me.content.container.find('#filterSettingsButton');
            me.content.filterSettingsButton.click(function () { filterSettingsButtonClicked(); });

            me.content.optionMenu = me.content.container.find('#optionMenu');
            me.content.optionMenu.chosen();

            me.filterSettings = {};
            me.filterSettings.container = me.container.find('#filterSettings').hide().removeClass('hidden');
            me.filterSettings.submitButton = me.filterSettings.container.find('.submit');
            me.filterSettings.cancelButton = me.filterSettings.container.find('.cancel');


            me.filterSettings.submitButton.click(function () { submitClicked(); });
            me.filterSettings.cancelButton.click(function () { cancelClicked(); });


            me.filterSettings.container.find('.slider').slider({range: true,
                min: 0,
                max: 500,
                values: [ 75, 300 ]});


           
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
        init();
        return publicObj;
        
    };
})();