(function () {
    sizeup.core.namespace('sizeup.views.community');
    sizeup.views.community.cityCommunity = function (opts) {


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

        dataLayer.getCityBoundingBox({ id: opts.CurrentPlace.City.Id }, notifier.getNotifier(function (data) { me.data.BoundingBox = data; }));
        dataLayer.getCityCentroid({ id: opts.CurrentPlace.City.Id }, notifier.getNotifier(function (data) { me.data.CityCenter = new sizeup.maps.latLng({ lat: data.Lat, lng: data.Lng }); }));
        dataLayer.getDemographics({ id: opts.CurrentPlace.City.Id, granularity: 'City' }, notifier.getNotifier(function (data) { me.data.Demographics = formatDemographics(data); }));
        var init = function () {

            var bounds = new sizeup.maps.latLngBounds();
            bounds.extend(new sizeup.maps.latLng({ lat: me.data.BoundingBox[0].Lat, lng: me.data.BoundingBox[0].Lng }));
            bounds.extend(new sizeup.maps.latLng({ lat: me.data.BoundingBox[1].Lat, lng: me.data.BoundingBox[1].Lng }));



            me.content.map = new sizeup.maps.map({
                container: me.container.find('.map')
            });

            var borderOverlay = new sizeup.maps.overlay({
                tileUrl: '/tiles/geographyBoundary/',
                tileParams: {
                    id: opts.CurrentPlace.City.Id,
                    granularity: 'City'
                }
            });

            me.content.map.setCenter(me.data.CityCenter);
            me.content.map.fitBounds(bounds);
            me.content.map.addOverlay(borderOverlay);
 
            me.content.report = me.container.find('.report').hide().removeClass('hidden');

            me.content.report.html(templates.bind(templates.get('demographics'), me.data.Demographics));

            me.content.report.show();
            me.container.find('.loading').remove();

        };


        var formatDemographics = function (data) {
            if (data.Population != null) {
                data.Population = sizeup.util.numbers.format.addCommas(data.Population);
            }
            if (data.LaborForce != null) {
                data.LaborForce = sizeup.util.numbers.format.addCommas(data.LaborForce);
            }
            if (data.SmallBusinesses != null) {
                data.SmallBusinesses = sizeup.util.numbers.format.addCommas(data.SmallBusinesses);
            }
            if (data.JobGrowth != null) {
                data.JobGrowth = sizeup.util.numbers.format.percentage(data.JobGrowth, 1);
            }
            if (data.Unemployment != null) {
                data.Unemployment = sizeup.util.numbers.format.percentage(data.Unemployment, 1);
            }
            if (data.MedianAge != null) {
                data.MedianAge = sizeup.util.numbers.format.addCommas(data.MedianAge);
            }
            if (data.AverageHouseholdExpenditures != null) {
                data.AverageHouseholdExpenditures = '$' + sizeup.util.numbers.format.addCommas(sizeup.util.numbers.format.round(data.AverageHouseholdExpenditures,0));
            }
            if (data.HouseholdIncome != null) {
                data.HouseholdIncome = '$' + sizeup.util.numbers.format.addCommas(data.HouseholdIncome);
            }
            if (data.PersonalIncomeTax != null) {
                data.PersonalIncomeTax = sizeup.util.numbers.format.percentage(data.PersonalIncomeTax, 2);
            }
            if (data.PersonalCapitalGainsTax != null) {
                data.PersonalCapitalGainsTax = sizeup.util.numbers.format.percentage(data.PersonalCapitalGainsTax, 2);
            }
            if (data.CorporateIncomeTax != null) {
                data.CorporateIncomeTax = sizeup.util.numbers.format.percentage(data.CorporateIncomeTax, 2);
            }
            if (data.CorporateCapitalGainsTax != null) {
                data.CorporateCapitalGainsTax = sizeup.util.numbers.format.percentage(data.CorporateCapitalGainsTax, 2);
            }
            if (data.SalesTax != null) {
                data.SalesTax = sizeup.util.numbers.format.percentage(data.SalesTax, 2);
            }
            if (data.PropertyTax != null) {
                data.PropertyTax = sizeup.util.numbers.format.percentage(data.PropertyTax, 2);
            }
            if (data.HomeValue != null) {
                data.HomeValue = '$' + sizeup.util.numbers.format.addCommas(data.HomeValue);
            }
            if (data.BachelorsOrHigherPercentage != null) {
                data.BachelorsOrHigherPercentage = sizeup.util.numbers.format.percentage(data.BachelorsOrHigherPercentage, 1);
            }
            if (data.HighschoolOrHigherPercentage != null) {
                data.HighschoolOrHigherPercentage = sizeup.util.numbers.format.percentage(data.HighschoolOrHigherPercentage, 1);
            }
            if (data.WhiteCollarWorkersPercentage != null) {
                data.WhiteCollarWorkersPercentage = sizeup.util.numbers.format.percentage(data.WhiteCollarWorkersPercentage, 1);
            }
            if (data.BlueCollarWorkersPercentage != null) {
                data.BlueCollarWorkersPercentage = sizeup.util.numbers.format.percentage(data.BlueCollarWorkersPercentage, 1);
            }
            if (data.VeryCreativeProfessionalsPercentage != null) {
                data.VeryCreativeProfessionalsPercentage = sizeup.util.numbers.format.percentage(data.VeryCreativeProfessionalsPercentage, 1);
            }
            if (data.CreativeProfessionalsPercentage != null) {
                data.CreativeProfessionalsPercentage = sizeup.util.numbers.format.percentage(data.CreativeProfessionalsPercentage, 1);
            }
            if (data.YoungEducatedPercentage != null) {
                data.YoungEducatedPercentage = sizeup.util.numbers.format.percentage(data.YoungEducatedPercentage, 1);
            }
            if (data.InternationalTalentPercentage != null) {
                data.InternationalTalentPercentage = sizeup.util.numbers.format.percentage(data.InternationalTalentPercentage, 1);
            }
            if (data.Universities != null) {
                data.Universities = sizeup.util.numbers.format.addCommas(data.Universities);
            }
            if (data.Universities50Miles != null) {
                data.Universities50Miles = sizeup.util.numbers.format.addCommas(data.Universities50Miles);
            }
            if (data.CommuteTime != null) {
                data.CommuteTime = sizeup.util.numbers.format.round(data.CommuteTime, 0) + ' minutes';
            }

            
            
            

            

            return data;
        };



        var publicObj = {

        };
        return publicObj;

    };
})();