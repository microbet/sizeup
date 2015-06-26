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
            '<span><a href="//' + window.location.host + '" target="_blank">SizeUp</a></span>' +
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

       

        bindData = function () {
            var data = formatBestIndustries();

            me.content.bestIndustries = {};

            me.content.bestIndustries.wrapper = me.container.find('.wrapper.bestIndustries .report');
            me.content.bestIndustries.wrapper.html(templates.bind(templates.get('bestIndustries'), data));

            if (data.Industries.length > 0) {
                me.container.find('.wrapper.bestIndustries').removeClass('hidden');
            }

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