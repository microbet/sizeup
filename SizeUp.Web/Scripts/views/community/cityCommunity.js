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
        var templates = new sizeup.core.templates(me.container);
        
        me.content = {};


        var init = function () {
            loadMap();
            loadDemographics();
            loadBestIndustries();
        };


        var loadMap = function () {
            var notifier = new sizeup.core.notifier(function () { bindMap(); });

            sizeup.api.data.getBoundingBox({ id: opts.CurrentPlace.City.Id, granularity: sizeup.api.granularity.CITY }, notifier.getNotifier(function (data) { me.data.BoundingBox = data; }));
            sizeup.api.data.getCentroid({ id: opts.CurrentPlace.City.Id, granularity: sizeup.api.granularity.CITY }, notifier.getNotifier(function (data) { me.data.CityCenter = new sizeup.maps.latLng({ lat: data.Lat, lng: data.Lng }); }));
        };

        var loadDemographics = function () {
            var notifier = new sizeup.core.notifier(function () { bindDemographics(); });
            sizeup.api.data.getDemographics({ id: opts.CurrentPlace.City.Id, granularity: sizeup.api.granularity.CITY }, notifier.getNotifier(function (data) { me.data.demographics = formatDemographics(data); }));
        };

        var loadBestIndustries = function () {
            me.data.bestIndustries = {};
            var notifier = new sizeup.core.notifier(function () { bindBestIndustries(); });
            sizeup.api.data.getBestIndustries({ geographicLocationId: opts.CurrentPlace.City.Id, attribute: sizeup.api.attributes.TOTAL_REVENUE }, notifier.getNotifier(function (data) { me.data.bestIndustries.totalRevenue = data; }));
            sizeup.api.data.getBestIndustries({ geographicLocationId: opts.CurrentPlace.City.Id, attribute: sizeup.api.attributes.REVENUE_PER_CAPITA }, notifier.getNotifier(function (data) { me.data.bestIndustries.revenuePerCapita = data; }));
        };

        var bindMap = function () {
            me.content.map = new sizeup.maps.map({
                container: me.container.find('.map')
            });

            var borderOverlay = new sizeup.maps.overlay({
                attribute: sizeup.api.tiles.overlayAttributes.geographyBoundary,
                tileParams: {
                    id: opts.CurrentPlace.City.Id,
                    granularity: sizeup.api.granularity.CITY
                }
            });

            var bounds = new sizeup.maps.latLngBounds();
            bounds.extend(new sizeup.maps.latLng({ lat: me.data.BoundingBox.SouthWest.Lat, lng: me.data.BoundingBox.SouthWest.Lng }));
            bounds.extend(new sizeup.maps.latLng({ lat: me.data.BoundingBox.NorthEast.Lat, lng: me.data.BoundingBox.NorthEast.Lng }));

            me.content.map.setCenter(me.data.CityCenter);
            me.content.map.fitBounds(bounds);
            me.content.map.addOverlay(borderOverlay);
        };

        var bindDemographics = function () {
            me.content.demographics = {};
            me.content.demographics.wrapper = me.container.find('.wrapper.demographics');
            me.content.demographics.report = me.content.demographics.wrapper.find('.report').hide().removeClass('hidden');

            me.content.demographics.report.html(templates.bind(templates.get('demographics'), me.data.demographics));
            me.content.demographics.report.show();
            me.content.demographics.wrapper.find('.loading').remove();
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
                data.AverageHouseholdExpenditures = '$' + sizeup.util.numbers.format.addCommas(sizeup.util.numbers.format.round(data.AverageHouseholdExpenditures, 0));
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


        var bindBestIndustries = function () {
            var data = formatBestIndustries();

            me.content.bestIndustries = {};
            me.content.bestIndustries.wrapper = me.container.find('.wrapper.bestIndustries');
            me.content.bestIndustries.report = me.content.bestIndustries.wrapper.find('.report').hide().removeClass('hidden');

            me.content.bestIndustries.report.html(templates.bind(templates.get('bestIndustries'), data));
            me.content.bestIndustries.report.show();
            me.content.bestIndustries.wrapper.find('.loading').remove();
        };

        var formatBestIndustries = function () {
            var temp = {};
            for (var x in me.data.bestIndustries) {
                for (var y in me.data.bestIndustries[x]) {

                    if (!temp[me.data.bestIndustries[x][y].Industry.Id]) {
                        temp[me.data.bestIndustries[x][y].Industry.Id] = {
                            Industry: me.data.bestIndustries[x][y].Industry
                        };
                    }
                    temp[me.data.bestIndustries[x][y].Industry.Id][x] = { rank: sizeup.util.numbers.format.ordinal(me.data.bestIndustries[x][y].Rank), isTop: me.data.bestIndustries[x][y].Rank >= 1 && me.data.bestIndustries[x][y].Rank <= 3 };
                }
            }
            var data = [];
            for (var x in temp) {
                data.push(temp[x]);
            }
            data.sort(sort);
            return { Industries: data };
        };

        var sort = function (a, b) {
            if (a.Industry.Name < b.Industry.Name)
                return -1;
            if (a.Industry.Name > b.Industry.Name)
                return 1;
            return 0;
        };

        init();
        var publicObj = {

        };
        return publicObj;

    };
})();