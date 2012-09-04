(function () {
    sizeup.core.namespace('sizeup.views.community');
    sizeup.views.community.community = function (opts) {


        var defaults = {
            mapSettings: sizeup.maps.mapOptions.getDefaults(),
            styles: sizeup.maps.mapStyles.getDefaults()
        };
        var me = {};

        me.opts = $.extend(true, defaults, opts);

        me.data = {};
        me.container = $('#community');
        var dataLayer = new sizeup.core.data();
        var templates = new sizeup.core.templates(me.container);
        var notifier = new sizeup.core.notifier(function () { init(); });


        me.content = {};


        dataLayer.getCityCentroid({ id: opts.CurrentPlace.Id }, notifier.getNotifier(function (data) { me.data.CityCenter = new sizeup.maps.latLng({ lat: data.Lat, lng: data.Lng }); }));
        dataLayer.getDemographics({ id: opts.CurrentPlace.Id }, notifier.getNotifier(function (data) { me.data.Demographics = formatDemographics(data); }));
        var init = function () {


            me.content.map = new sizeup.maps.map({
                container: me.container.find('.map'),
                mapSettings: me.opts.mapSettings,
                styles: me.opts.styles
            });
            me.content.map.setCenter(me.data.CityCenter);

            me.content.report = me.container.find('.report').hide().removeClass('hidden');

            me.content.report.html(templates.bind(templates.get('demographics'), me.data.demographics));

            me.content.report.show();
            me.container.find('.loading').remove();

        };


        var formatDemographics = function (data) {
            data.Population = sizeup.util.numbers.format.addCommas(data.Population);
            data.LaborForce = sizeup.util.numbers.format.addCommas(data.LaborForce);

            return data;
        };



        var publicObj = {

        };
        return publicObj;

    };
})();