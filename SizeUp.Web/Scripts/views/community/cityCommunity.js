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

            me.content.share = sizeup.controls.share({
                container: me.container.find('.shareWrapper'),
                options: {
                    embed: {
                        getCode: function () { return getEmbedCode(); },
                        menuItem: me.container.find('.share.container .menu .embed'),
                        contentItem: me.container.find('.share.container .content .embed')
                    },
                    link: {
                        getCode: function () { return window.location.href; },
                        menuItem: me.container.find('.share.container .menu .link'),
                        contentItem: me.container.find('.share.container .content .link')
                    }
                }
            });

            loadMap();
            loadData();
        };


        var getEmbedCode = function () {
            var url = '/widget/get/bestIndustries/' + opts.CurrentPlace.State.SEOKey + '/' + opts.CurrentPlace.County.SEOKey + '/' + opts.CurrentPlace.City.SEOKey;
            var code =
            '<div>' +
            '<span><a href="http://' + window.location.host + '" target="_blank">SizeUp</a></span>' +
            '<script src="' + window.location.protocol + '//' + window.location.host + url + '"></script>' +
            '</div>';

            return code;
        };


        var loadMap = function () {
            var notifier = new sizeup.core.notifier(function () { bindMap(); });

            sizeup.api.data.getBoundingBox({ geographicLocationId: opts.CurrentPlace.City.Id }, notifier.getNotifier(function (data) { me.data.BoundingBox = data; }));
            sizeup.api.data.getCentroid({ geographicLocationId: opts.CurrentPlace.City.Id }, notifier.getNotifier(function (data) { me.data.CityCenter = new sizeup.maps.latLng({ lat: data.Lat, lng: data.Lng }); }));
        };

        var loadData = function () {
            var notifier = new sizeup.core.notifier(function () { bindData(); });
            me.data.bestIndustries = {};
            sizeup.api.data.getDemographics({ geographicLocationId: opts.CurrentPlace.City.Id }, notifier.getNotifier(function (data) { me.data.demographics = formatDemographics(data); }));
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
                    geographicLocationId: opts.CurrentPlace.City.Id
                }
            });

            var bounds = new sizeup.maps.latLngBounds();
            bounds.extend(new sizeup.maps.latLng({ lat: me.data.BoundingBox.SouthWest.Lat, lng: me.data.BoundingBox.SouthWest.Lng }));
            bounds.extend(new sizeup.maps.latLng({ lat: me.data.BoundingBox.NorthEast.Lat, lng: me.data.BoundingBox.NorthEast.Lng }));

            me.content.map.setCenter(me.data.CityCenter);
            me.content.map.fitBounds(bounds);
            me.content.map.addOverlay(borderOverlay);
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

        bindData = function () {
            var data = formatBestIndustries();
            me.content.demographics = {};
            me.content.bestIndustries = {};
            me.content.demographics.wrapper = me.container.find('.wrapper.demographics .report');
            me.content.demographics.wrapper.html(templates.bind(templates.get('demographics'), me.data.demographics));

            me.content.bestIndustries.wrapper = me.container.find('.wrapper.bestIndustries .report');
            me.content.bestIndustries.wrapper.html(templates.bind(templates.get('bestIndustries'), data));

            if (data.Industries.length > 0) {
                me.container.find('.wrapper.bestIndustries').removeClass('hidden');
            }
            me.container.find('.wrapper.demographics').removeClass('hidden');

            me.container.find('.wrapper .loading').remove();
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
                    temp[me.data.bestIndustries[x][y].Industry.Id][x] = { rank: sizeup.util.numbers.format.ordinal(me.data.bestIndustries[x][y].Rank), badgeType: ''};
                    if (me.data.bestIndustries[x][y].Rank >= 1 && me.data.bestIndustries[x][y].Rank <= 10) {
                        temp[me.data.bestIndustries[x][y].Industry.Id][x].badgeType = 'top10';
                        temp[me.data.bestIndustries[x][y].Industry.Id][x].badgeName = 'Top 10';
                    }
                    else if (me.data.bestIndustries[x][y].Rank >= 11 && me.data.bestIndustries[x][y].Rank <= 50) {
                        temp[me.data.bestIndustries[x][y].Industry.Id][x].badgeType = 'top50';
                        temp[me.data.bestIndustries[x][y].Industry.Id][x].badgeName = 'Top 50';
                    }
                    else if (me.data.bestIndustries[x][y].Rank >= 51 && me.data.bestIndustries[x][y].Rank <= 100) {
                        temp[me.data.bestIndustries[x][y].Industry.Id][x].badgeType = 'top100';
                        temp[me.data.bestIndustries[x][y].Industry.Id][x].badgeName = 'Top 100';
                    }
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