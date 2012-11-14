(function () {
    sizeup.core.namespace('sizeup.views.community');
    sizeup.views.community.communityWithIndustry = function (opts) {


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


        dataLayer.getCityCentroid({ id: opts.location.CurrentPlace.City.Id }, notifier.getNotifier(function (data) { me.data.CityCenter = new sizeup.maps.latLng({ lat: data.Lat, lng: data.Lng }); }));
        dataLayer.getCityBoundingBox({ id: opts.location.CurrentPlace.City.Id }, notifier.getNotifier(function (data) { me.data.BoundingBox = data; }));

        var init = function () {

            var bounds = new sizeup.maps.latLngBounds();
            bounds.extend(new sizeup.maps.latLng({ lat: me.data.BoundingBox[0].Lat, lng: me.data.BoundingBox[0].Lng }));
            bounds.extend(new sizeup.maps.latLng({ lat: me.data.BoundingBox[1].Lat, lng: me.data.BoundingBox[1].Lng }));


            me.content.map = new sizeup.maps.map({
                container: me.container.find('.mapWrapper.container .map')
            });

            me.content.map.addEventListener('zoom_changed', mapZoomUpdated);


            var borderOverlay = new sizeup.maps.overlay({
                tileUrl: '/tiles/geographyBoundary/',
                tileParams: {
                    entityId: 'c' + opts.location.CurrentPlace.City.Id
                }
            });



            me.content.map.setCenter(me.data.CityCenter);
            me.content.map.fitBounds(bounds);
            me.content.map.addOverlay(borderOverlay);


            dataLayer.getAverageRevenueChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id }, initAverageRevenueChart);
            dataLayer.getTotalRevenueChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id }, initTotalRevenueChart);

            dataLayer.getAverageEmployeesChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id }, initAverageEmployeesChart);
            dataLayer.getTotalEmployeesChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id }, initTotalEmployeesChart);

            dataLayer.getAverageSalaryChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id }, initAverageSalaryChart);
            dataLayer.getCostEffectivenessChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id }, initCostEffectivenessChart);


        };

        var mapZoomUpdated = function () {
            setLegend();
        };

        var setHeatmap = function () {
            clearHeatmap();
            if (me.data.activeHeatmap == 'averageRevenue') {
                setAverageRevenueOverlays();
            }
            if (me.data.activeHeatmap == 'totalRevenue') {
                setTotalRevenueOverlays();
            }
            if (me.data.activeHeatmap == 'averageEmployees') {
                setAverageEmployeesOverlays();
            }
            if (me.data.activeHeatmap == 'totalEmployees') {
                setTotalEmployeesOverlays();
            }
            if (me.data.activeHeatmap == 'averageSalary') {
                setAverageSalaryOverlays();
            }
            if (me.data.activeHeatmap == 'costEffectiveness') {
                setCostEffectivenessOverlays();
            }
            setLegend();
        };

        var setLegend = function () {
            if (me.data.activeHeatmap == 'averageRevenue') {
                setAverageRevenueLegend();
            }
            if (me.data.activeHeatmap == 'totalRevenue') {
                setTotalRevenueLegend();
            }
            if (me.data.activeHeatmap == 'averageEmployees') {
                setAverageEmployeesLegend();
            }
            if (me.data.activeHeatmap == 'totalEmployees') {
                setTotalEmployeesLegend();
            }
            if (me.data.activeHeatmap == 'averageSalary') {
                setAverageSalaryLegend();
            }
            if (me.data.activeHeatmap == 'costEffectiveness') {
                setCostEffectivenessLegend();
            }
        };

        var clearHeatmap = function () {
            for (var x in me.data.heatMapOverlays) {
                me.content.map.removeOverlay(me.data.heatMapOverlays[x]);
            }
        };

        var clearLegend = function () {
            me.content.map.clearLegend();
        };

        var setAverageRevenueLegend = function () {
            var data = {
                title: '',
                items: []
            };
            var z = me.content.map.getZoom();

            var notify = new sizeup.core.notifier(function () {

                var legend = new sizeup.maps.legend({
                    templates: templates,
                    title: data.title,
                    items: data.items,
                    colors: [
                    '#F5F500',
                    '#F5CC00',
                    '#F5A300',
                    '#F57A00',
                    '#F55200',
                    '#F52900',
                    '#F50000'
                    ],
                    format: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); }
                });
                me.content.map.setLegend(legend);
            });



            var itemsNotify = notify.getNotifier(function (d) { data.items = d; });


            if (z <= 32 && z >= 11) {
                data.title = 'Average Business Annual Revenue by ZIP code in ' + me.opts.location.CurrentPlace.County.Name + ', ' + me.opts.location.CurrentPlace.State.Abbreviation;
                dataLayer.getAverageRevenueBandsByZip({
                    industryId: me.opts.location.CurrentIndustry.Id,
                    boundingEntityId: 'co' + me.opts.location.CurrentPlace.County.Id,
                    bands: 7
                }, itemsNotify);
            }

            if (me.opts.location.CurrentPlace.Metro.Id != null) {
                if (z <= 10 && z >= 8) {
                    data.title = 'Average Business Annual Revenue by county in ' + me.opts.location.CurrentPlace.Metro.Name + ' (Metro)';
                    dataLayer.getAverageRevenueBandsByCounty({
                        industryId: me.opts.location.CurrentIndustry.Id,
                        boundingEntityId: 'm' + me.opts.location.CurrentPlace.Metro.Id,
                        bands: 7
                    }, itemsNotify);


                }

                if (z <= 7 && z >= 5) {
                    data.title = 'Average Business Annual Revenue by county in ' + me.opts.location.CurrentPlace.State.Name;
                    dataLayer.getAverageRevenueBandsByCounty({
                        industryId: me.opts.location.CurrentIndustry.Id,
                        boundingEntityId: 's' + me.opts.location.CurrentPlace.State.Id,
                        bands: 7
                    }, itemsNotify);
                }
            }
            else {
                if (z <= 10 && z >= 5) {

                    data.title = 'Average Business Annual Revenue by county in ' + me.opts.location.CurrentPlace.State.Name;
                    dataLayer.getAverageRevenueBandsByState({
                        industryId: me.opts.location.CurrentIndustry.Id,
                        boundingEntityId: 's' + me.opts.location.CurrentPlace.State.Id,
                        bands: 7
                    }, itemsNotify);

                }
            }

            if (z <= 4 && z >= 0) {
                data.title = 'Average Business Annual Revenue by state in the USA';
                dataLayer.getAverageRevenueBandsByState({
                    industryId: me.opts.location.CurrentIndustry.Id,
                    bands: 7
                }, itemsNotify);
            }
        };

        var setAverageRevenueOverlays = function () {

            var overlays = [];

            overlays.push(new sizeup.maps.overlay({
                tileUrl: '/tiles/AverageRevenue/state/',
                tileParams: {
                    colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                    ].join(','),
                    industryId: me.opts.location.CurrentIndustry.Id
                },
                minZoom: 0,
                maxZoom: 4
            }));

            if (me.opts.location.CurrentPlace.Metro.Id != null) {
                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/AverageRevenue/county/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ].join(','),
                        boundingEntityId: 's' + me.opts.location.CurrentPlace.State.Id,
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 5,
                    maxZoom: 7
                }));

                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/AverageRevenue/county/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ].join(','),
                        boundingEntityId: 'm' + me.opts.location.CurrentPlace.Metro.Id,
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 8,
                    maxZoom: 10
                }));
            }
            else {
                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/AverageRevenue/county/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ].join(','),
                        boundingEntityId: 'm' + me.opts.location.CurrentPlace.Metro.Id,
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 5,
                    maxZoom: 10
                }));
            }

            overlays.push(new sizeup.maps.overlay({
                tileUrl: '/tiles/AverageRevenue/zip/',
                tileParams: {
                    colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                    ].join(','),
                    boundingEntityId: 'co' + me.opts.location.CurrentPlace.County.Id,
                    industryId: me.opts.location.CurrentIndustry.Id
                },
                minZoom: 11,
                maxZoom: 32
            }));

            me.data.heatMapOverlays = overlays;

            for (var x in overlays) {
                me.content.map.addOverlay(overlays[x]);
            }
        };

        //////////////////////////////////////////////////////////////////////////////////////
        var setTotalRevenueLegend = function () {
            var data = {
                title: '',
                items: []
            };
            var z = me.content.map.getZoom();

            var notify = new sizeup.core.notifier(function () {

                var legend = new sizeup.maps.legend({
                    templates: templates,
                    title: data.title,
                    items: data.items,
                    colors: [
                    '#F5F500',
                    '#F5CC00',
                    '#F5A300',
                    '#F57A00',
                    '#F55200',
                    '#F52900',
                    '#F50000'
                    ],
                    format: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); }
                });
                me.content.map.setLegend(legend);
            });



            var itemsNotify = notify.getNotifier(function (d) { data.items = d; });


            if (z <= 32 && z >= 11) {
                data.title = 'Total Revenue by ZIP code in ' + me.opts.location.CurrentPlace.County.Name + ', ' + me.opts.location.CurrentPlace.State.Abbreviation;
                dataLayer.getTotalRevenueBandsByZip({
                    industryId: me.opts.location.CurrentIndustry.Id,
                    boundingEntityId: 'co' + me.opts.location.CurrentPlace.County.Id,
                    bands: 7
                }, itemsNotify);
            }

            if (me.opts.location.CurrentPlace.Metro.Id != null) {
                if (z <= 10 && z >= 8) {
                    data.title = 'Total Revenue by county in ' + me.opts.location.CurrentPlace.Metro.Name + ' (Metro)';
                    dataLayer.getTotalRevenueBandsByCounty({
                        industryId: me.opts.location.CurrentIndustry.Id,
                        boundingEntityId: 'm' + me.opts.location.CurrentPlace.Metro.Id,
                        bands: 7
                    }, itemsNotify);


                }

                if (z <= 7 && z >= 5) {
                    data.title = 'Total Revenue by county in ' + me.opts.location.CurrentPlace.State.Name;
                    dataLayer.getTotalRevenueBandsByCounty({
                        industryId: me.opts.location.CurrentIndustry.Id,
                        boundingEntityId: 's' + me.opts.location.CurrentPlace.State.Id,
                        bands: 7
                    }, itemsNotify);
                }
            }
            else {
                if (z <= 10 && z >= 5) {

                    data.title = 'Total Revenue by county in ' + me.opts.location.CurrentPlace.State.Name;
                    dataLayer.getTotalRevenueBandsByState({
                        industryId: me.opts.location.CurrentIndustry.Id,
                        boundingEntityId: 's' + me.opts.location.CurrentPlace.State.Id,
                        bands: 7
                    }, itemsNotify);

                }
            }

            if (z <= 4 && z >= 0) {
                data.title = 'Total Revenue by state in the USA';
                dataLayer.getTotalRevenueBandsByState({
                    industryId: me.opts.location.CurrentIndustry.Id,
                    bands: 7
                }, itemsNotify);
            }
        };

        var setTotalRevenueOverlays = function () {

            var overlays = [];

            overlays.push(new sizeup.maps.overlay({
                tileUrl: '/tiles/TotalRevenue/state/',
                tileParams: {
                    colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                    ].join(','),
                    industryId: me.opts.location.CurrentIndustry.Id
                },
                minZoom: 0,
                maxZoom: 4
            }));

            if (me.opts.location.CurrentPlace.Metro.Id != null) {
                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/TotalRevenue/county/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ].join(','),
                        boundingEntityId: 's' + me.opts.location.CurrentPlace.State.Id,
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 5,
                    maxZoom: 7
                }));

                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/TotalRevenue/county/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ].join(','),
                        boundingEntityId: 'm' + me.opts.location.CurrentPlace.Metro.Id,
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 8,
                    maxZoom: 10
                }));
            }
            else {
                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/TotalRevenue/county/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ].join(','),
                        boundingEntityId: 'm' + me.opts.location.CurrentPlace.Metro.Id,
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 5,
                    maxZoom: 10
                }));
            }

            overlays.push(new sizeup.maps.overlay({
                tileUrl: '/tiles/TotalRevenue/zip/',
                tileParams: {
                    colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                    ].join(','),
                    boundingEntityId: 'co' + me.opts.location.CurrentPlace.County.Id,
                    industryId: me.opts.location.CurrentIndustry.Id
                },
                minZoom: 11,
                maxZoom: 32
            }));

            me.data.heatMapOverlays = overlays;

            for (var x in overlays) {
                me.content.map.addOverlay(overlays[x]);
            }
        };


        //////////////////////////////////////////////////////////////////////////////////////
        var setAverageEmployeesLegend = function () {
            var data = {
                title: '',
                items: []
            };
            var z = me.content.map.getZoom();

            var notify = new sizeup.core.notifier(function () {

                var legend = new sizeup.maps.legend({
                    templates: templates,
                    title: data.title,
                    items: data.items,
                    colors: [
                    '#F5F500',
                    '#F5CC00',
                    '#F5A300',
                    '#F57A00',
                    '#F55200',
                    '#F52900',
                    '#F50000'
                    ],
                    format: function (val) { return sizeup.util.numbers.format.abbreviate(val); }
                });
                me.content.map.setLegend(legend);
            });



            var itemsNotify = notify.getNotifier(function (d) { data.items = d; });


            if (z <= 32 && z >= 11) {
                data.title = 'Average Employees per business by ZIP code in ' + me.opts.location.CurrentPlace.County.Name + ', ' + me.opts.location.CurrentPlace.State.Abbreviation;
                dataLayer.getAverageEmployeesBandsByZip({
                    industryId: me.opts.location.CurrentIndustry.Id,
                    boundingEntityId: 'co' + me.opts.location.CurrentPlace.County.Id,
                    bands: 7
                }, itemsNotify);
            }

            if (me.opts.location.CurrentPlace.Metro.Id != null) {
                if (z <= 10 && z >= 8) {
                    data.title = 'Average Employees per business by county in ' + me.opts.location.CurrentPlace.Metro.Name + ' (Metro)';
                    dataLayer.getAverageEmployeesBandsByCounty({
                        industryId: me.opts.location.CurrentIndustry.Id,
                        boundingEntityId: 'm' + me.opts.location.CurrentPlace.Metro.Id,
                        bands: 7
                    }, itemsNotify);


                }

                if (z <= 7 && z >= 5) {
                    data.title = 'Average Employees per business by county in ' + me.opts.location.CurrentPlace.State.Name;
                    dataLayer.getAverageEmployeesBandsByCounty({
                        industryId: me.opts.location.CurrentIndustry.Id,
                        boundingEntityId: 's' + me.opts.location.CurrentPlace.State.Id,
                        bands: 7
                    }, itemsNotify);
                }
            }
            else {
                if (z <= 10 && z >= 5) {

                    data.title = 'Average Employees per business by county in ' + me.opts.location.CurrentPlace.State.Name;
                    dataLayer.getAverageEmployeesBandsByState({
                        industryId: me.opts.location.CurrentIndustry.Id,
                        boundingEntityId: 's' + me.opts.location.CurrentPlace.State.Id,
                        bands: 7
                    }, itemsNotify);

                }
            }

            if (z <= 4 && z >= 0) {
                data.title = 'Average Employees per business by state in the USA';
                dataLayer.getAverageEmployeesBandsByState({
                    industryId: me.opts.location.CurrentIndustry.Id,
                    bands: 7
                }, itemsNotify);
            }
        };

        var setAverageEmployeesOverlays = function () {

            var overlays = [];

            overlays.push(new sizeup.maps.overlay({
                tileUrl: '/tiles/AverageEmployees/state/',
                tileParams: {
                    colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                    ].join(','),
                    industryId: me.opts.location.CurrentIndustry.Id
                },
                minZoom: 0,
                maxZoom: 4
            }));

            if (me.opts.location.CurrentPlace.Metro.Id != null) {
                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/AverageEmployees/county/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ].join(','),
                        boundingEntityId: 's' + me.opts.location.CurrentPlace.State.Id,
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 5,
                    maxZoom: 7
                }));

                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/AverageEmployees/county/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ].join(','),
                        boundingEntityId: 'm' + me.opts.location.CurrentPlace.Metro.Id,
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 8,
                    maxZoom: 10
                }));
            }
            else {
                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/AverageEmployees/county/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ].join(','),
                        boundingEntityId: 'm' + me.opts.location.CurrentPlace.Metro.Id,
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 5,
                    maxZoom: 10
                }));
            }

            overlays.push(new sizeup.maps.overlay({
                tileUrl: '/tiles/AverageEmployees/zip/',
                tileParams: {
                    colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                    ].join(','),
                    boundingEntityId: 'co' + me.opts.location.CurrentPlace.County.Id,
                    industryId: me.opts.location.CurrentIndustry.Id
                },
                minZoom: 11,
                maxZoom: 32
            }));

            me.data.heatMapOverlays = overlays;

            for (var x in overlays) {
                me.content.map.addOverlay(overlays[x]);
            }
        };




        //////////////////////////////////////////////////////////////////////////////////////
        var setTotalEmployeesLegend = function () {
            var data = {
                title: '',
                items: []
            };
            var z = me.content.map.getZoom();

            var notify = new sizeup.core.notifier(function () {

                var legend = new sizeup.maps.legend({
                    templates: templates,
                    title: data.title,
                    items: data.items,
                    colors: [
                    '#F5F500',
                    '#F5CC00',
                    '#F5A300',
                    '#F57A00',
                    '#F55200',
                    '#F52900',
                    '#F50000'
                    ],
                    format: function (val) { return sizeup.util.numbers.format.abbreviate(val); }
                });
                me.content.map.setLegend(legend);
            });



            var itemsNotify = notify.getNotifier(function (d) { data.items = d; });


            if (z <= 32 && z >= 11) {
                data.title = 'Total Employees by ZIP code in ' + me.opts.location.CurrentPlace.County.Name + ', ' + me.opts.location.CurrentPlace.State.Abbreviation;
                dataLayer.getAverageEmployeesBandsByZip({
                    industryId: me.opts.location.CurrentIndustry.Id,
                    boundingEntityId: 'co' + me.opts.location.CurrentPlace.County.Id,
                    bands: 7
                }, itemsNotify);
            }

            if (me.opts.location.CurrentPlace.Metro.Id != null) {
                if (z <= 10 && z >= 8) {
                    data.title = 'Total Employees by county in ' + me.opts.location.CurrentPlace.Metro.Name + ' (Metro)';
                    dataLayer.getAverageEmployeesBandsByCounty({
                        industryId: me.opts.location.CurrentIndustry.Id,
                        boundingEntityId: 'm' + me.opts.location.CurrentPlace.Metro.Id,
                        bands: 7
                    }, itemsNotify);


                }

                if (z <= 7 && z >= 5) {
                    data.title = 'Total Employees by county in ' + me.opts.location.CurrentPlace.State.Name;
                    dataLayer.getAverageEmployeesBandsByCounty({
                        industryId: me.opts.location.CurrentIndustry.Id,
                        boundingEntityId: 's' + me.opts.location.CurrentPlace.State.Id,
                        bands: 7
                    }, itemsNotify);
                }
            }
            else {
                if (z <= 10 && z >= 5) {

                    data.title = 'Total Employees by county in ' + me.opts.location.CurrentPlace.State.Name;
                    dataLayer.getAverageEmployeesBandsByState({
                        industryId: me.opts.location.CurrentIndustry.Id,
                        boundingEntityId: 's' + me.opts.location.CurrentPlace.State.Id,
                        bands: 7
                    }, itemsNotify);

                }
            }

            if (z <= 4 && z >= 0) {
                data.title = 'Total Employees by state in the USA';
                dataLayer.getAverageEmployeesBandsByState({
                    industryId: me.opts.location.CurrentIndustry.Id,
                    bands: 7
                }, itemsNotify);
            }
        };

        var setTotalEmployeesOverlays = function () {

            var overlays = [];

            overlays.push(new sizeup.maps.overlay({
                tileUrl: '/tiles/TotalEmployees/state/',
                tileParams: {
                    colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                    ].join(','),
                    industryId: me.opts.location.CurrentIndustry.Id
                },
                minZoom: 0,
                maxZoom: 4
            }));

            if (me.opts.location.CurrentPlace.Metro.Id != null) {
                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/TotalEmployees/county/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ].join(','),
                        boundingEntityId: 's' + me.opts.location.CurrentPlace.State.Id,
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 5,
                    maxZoom: 7
                }));

                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/TotalEmployees/county/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ].join(','),
                        boundingEntityId: 'm' + me.opts.location.CurrentPlace.Metro.Id,
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 8,
                    maxZoom: 10
                }));
            }
            else {
                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/TotalEmployees/county/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ].join(','),
                        boundingEntityId: 'm' + me.opts.location.CurrentPlace.Metro.Id,
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 5,
                    maxZoom: 10
                }));
            }

            overlays.push(new sizeup.maps.overlay({
                tileUrl: '/tiles/TotalEmployees/zip/',
                tileParams: {
                    colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                    ].join(','),
                    boundingEntityId: 'co' + me.opts.location.CurrentPlace.County.Id,
                    industryId: me.opts.location.CurrentIndustry.Id
                },
                minZoom: 11,
                maxZoom: 32
            }));


            me.data.heatMapOverlays = overlays;

            for (var x in overlays) {
                me.content.map.addOverlay(overlays[x]);
            }
        };


        //////////////////////////////////////////////////////////////////////////////////////
        var setAverageSalaryLegend = function () {
            var data = {
                title: '',
                items: []
            };
            var z = me.content.map.getZoom();

            var notify = new sizeup.core.notifier(function () {

                var legend = new sizeup.maps.legend({
                    templates: templates,
                    title: data.title,
                    items: data.items,
                    colors: [
                    '#F5F500',
                    '#F5CC00',
                    '#F5A300',
                    '#F57A00',
                    '#F55200',
                    '#F52900',
                    '#F50000'
                    ],
                    format: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); }
                });
                me.content.map.setLegend(legend);
            });



            var itemsNotify = notify.getNotifier(function (d) { data.items = d; });


            if (me.opts.location.CurrentPlace.Metro.Id != null) {
                if (z <= 32 && z >= 8) {
                    data.title = 'Average Salary by county in ' + me.opts.location.CurrentPlace.Metro.Name + ' (Metro)';
                    dataLayer.getAverageSalaryBandsByCounty({
                        industryId: me.opts.location.CurrentIndustry.Id,
                        boundingEntityId: 'm' + me.opts.location.CurrentPlace.Metro.Id,
                        bands: 7
                    }, itemsNotify);


                }

                if (z <= 7 && z >= 5) {
                    data.title = 'Average Salary by county in ' + me.opts.location.CurrentPlace.State.Name;
                    dataLayer.getAverageSalaryBandsByCounty({
                        industryId: me.opts.location.CurrentIndustry.Id,
                        boundingEntityId: 's' + me.opts.location.CurrentPlace.State.Id,
                        bands: 7
                    }, itemsNotify);
                }
            }
            else {
                if (z <= 32 && z >= 5) {

                    data.title = 'Average Salary by county in ' + me.opts.location.CurrentPlace.State.Name;
                    dataLayer.getAverageSalaryBandsByState({
                        industryId: me.opts.location.CurrentIndustry.Id,
                        boundingEntityId: 's' + me.opts.location.CurrentPlace.State.Id,
                        bands: 7
                    }, itemsNotify);

                }
            }

            if (z <= 4 && z >= 0) {
                data.title = 'Average Salary by state in the USA';
                dataLayer.getAverageSalaryBandsByState({
                    industryId: me.opts.location.CurrentIndustry.Id,
                    bands: 7
                }, itemsNotify);
            }
        };

        var setAverageSalaryOverlays = function () {

            var overlays = [];

            overlays.push(new sizeup.maps.overlay({
                tileUrl: '/tiles/AverageSalary/state/',
                tileParams: {
                    colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                    ].join(','),
                    industryId: me.opts.location.CurrentIndustry.Id
                },
                minZoom: 0,
                maxZoom: 4
            }));

            if (me.opts.location.CurrentPlace.Metro.Id != null) {
                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/AverageSalary/county/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ].join(','),
                        boundingEntityId: 's' + me.opts.location.CurrentPlace.State.Id,
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 5,
                    maxZoom: 7
                }));

                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/AverageSalary/county/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ].join(','),
                        boundingEntityId: 'm' + me.opts.location.CurrentPlace.Metro.Id,
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 8,
                    maxZoom: 32
                }));
            }
            else {
                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/AverageSalary/county/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ].join(','),
                        boundingEntityId: 'm' + me.opts.location.CurrentPlace.Metro.Id,
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 5,
                    maxZoom: 32
                }));
            }

            me.data.heatMapOverlays = overlays;

            for (var x in overlays) {
                me.content.map.addOverlay(overlays[x]);
            }
        };



        //////////////////////////////////////////////////////////////////////////////////////
        var setCostEffectivenessLegend = function () {
            var data = {
                title: '',
                items: []
            };
            var z = me.content.map.getZoom();

            var notify = new sizeup.core.notifier(function () {

                var legend = new sizeup.maps.legend({
                    templates: templates,
                    title: data.title,
                    items: data.items,
                    colors: [
                    '#F5F500',
                    '#F5CC00',
                    '#F5A300',
                    '#F57A00',
                    '#F55200',
                    '#F52900',
                    '#F50000'
                    ],
                    format: function (val) { return sizeup.util.numbers.format.round(val, 2); }
                });
                me.content.map.setLegend(legend);
            });



            var itemsNotify = notify.getNotifier(function (d) { data.items = d; });


            if (me.opts.location.CurrentPlace.Metro.Id != null) {
                if (z <= 32 && z >= 8) {
                    data.title = 'Cost Effectiveness by county in ' + me.opts.location.CurrentPlace.Metro.Name + ' (Metro)';
                    dataLayer.getCostEffectivenessBandsByCounty({
                        industryId: me.opts.location.CurrentIndustry.Id,
                        boundingEntityId: 'm' + me.opts.location.CurrentPlace.Metro.Id,
                        bands: 7
                    }, itemsNotify);


                }

                if (z <= 7 && z >= 5) {
                    data.title = 'Cost Effectiveness by county in ' + me.opts.location.CurrentPlace.State.Name;
                    dataLayer.getCostEffectivenessBandsByCounty({
                        industryId: me.opts.location.CurrentIndustry.Id,
                        boundingEntityId: 's' + me.opts.location.CurrentPlace.State.Id,
                        bands: 7
                    }, itemsNotify);
                }
            }
            else {
                if (z <= 32 && z >= 5) {

                    data.title = 'Cost Effectiveness by county in ' + me.opts.location.CurrentPlace.State.Name;
                    dataLayer.getCostEffectivenessBandsByState({
                        industryId: me.opts.location.CurrentIndustry.Id,
                        boundingEntityId: 's' + me.opts.location.CurrentPlace.State.Id,
                        bands: 7
                    }, itemsNotify);

                }
            }

            if (z <= 4 && z >= 0) {
                data.title = 'Cost Effectiveness by state in the USA';
                dataLayer.getCostEffectivenessBandsByState({
                    industryId: me.opts.location.CurrentIndustry.Id,
                    bands: 7
                }, itemsNotify);
            }
        };

        var setCostEffectivenessOverlays = function () {

            var overlays = [];

            overlays.push(new sizeup.maps.overlay({
                tileUrl: '/tiles/CostEffectiveness/state/',
                tileParams: {
                    colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                    ].join(','),
                    industryId: me.opts.location.CurrentIndustry.Id
                },
                minZoom: 0,
                maxZoom: 4
            }));

            if (me.opts.location.CurrentPlace.Metro.Id != null) {
                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/CostEffectiveness/county/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ].join(','),
                        boundingEntityId: 's' + me.opts.location.CurrentPlace.State.Id,
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 5,
                    maxZoom: 7
                }));

                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/CostEffectiveness/county/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ].join(','),
                        boundingEntityId: 'm' + me.opts.location.CurrentPlace.Metro.Id,
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 8,
                    maxZoom: 32
                }));
            }
            else {
                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/CostEffectiveness/county/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ].join(','),
                        boundingEntityId: 'm' + me.opts.location.CurrentPlace.Metro.Id,
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 5,
                    maxZoom: 32
                }));
            }

            me.data.heatMapOverlays = overlays;

            for (var x in overlays) {
                me.content.map.addOverlay(overlays[x]);
            }
        };





        var formatChartData = function (data, indexes) {
            var formattedData = {};
            var hasData = false;
            for (var x = 0; x < indexes.length; x++) {
                if (data[indexes[x]] != null) {
                    hasData = true;
                    formattedData[indexes[x]] =
                    {
                        value: data[indexes[x]].Value,
                        label: indexes[x],
                        name: data[indexes[x]].Name,
                        color: '#0af'
                    };
                }
            }
            return hasData ? formattedData : null;
        };



        var initAverageRevenueChart = function (data) {
            var container = me.container.find('#averageRevenue');
            container.find('.loading').remove();
            var indexes = ['City', 'County', 'Metro', 'State', 'Nation'];
            var formattedData = formatChartData(data, indexes);
            if (formattedData == null) {
                container.find('.noData').removeClass('hidden');
                container.find('.chartWrapper').remove();
            }
            else {
                var chart = new sizeup.charts.barChart({
                    valueFormat: function (val) { return '$' + sizeup.util.numbers.format.addCommas(Math.floor(val)); },
                    bar: { height: 13, padding: 4 },
                    gutters: { left: 50, top: 1 },
                    fillRight: false,
                    container: container.find('.chart'),
                    title: null,
                    bars: formattedData
                });
                chart.draw();


                container.find('.buttons .mapActivate').click(function () {
                    me.data.activeHeatmap = 'averageRevenue';
                    setHeatmap();

                });

                container.find('.buttons .mapClear').click(function () {
                    me.data.activeHeatmap = null;
                    clearHeatmap();
                    clearLegend();
                });
            }
        };

      

        var initTotalRevenueChart = function (data) {
            var container = me.container.find('#totalRevenue');
            container.find('.loading').remove();
            var indexes = ['City', 'County', 'Metro', 'State'];
            var formattedData = formatChartData(data, indexes);

            if (formattedData == null) {
                container.find('.noData').removeClass('hidden');
                container.find('.chartWrapper').remove();
            }
            else {
                var chart = new sizeup.charts.barChart({
                    valueFormat: function (val) { return '$' + sizeup.util.numbers.format.addCommas(Math.floor(val)); },
                    bar: { height: 13, padding: 4 },
                    gutters: { left: 50, top: 1 },
                    fillRight: false,
                    container: container.find('.chart'),
                    title: null,
                    bars: formattedData
                });
                chart.draw();


                container.find('.buttons .mapActivate').click(function () {
                    me.data.activeHeatmap = 'totalRevenue';
                    setHeatmap();
                });

                container.find('.buttons .mapClear').click(function () {
                    me.data.activeHeatmap = null;
                    clearHeatmap();
                    clearLegend();
                });
            }

        };

        var initAverageEmployeesChart = function (data) {
            var container = me.container.find('#averageEmployees');
            container.find('.loading').remove();
            var indexes = ['City', 'County', 'Metro', 'State', 'Nation'];
            var formattedData = formatChartData(data, indexes);

            if (formattedData == null) {
                container.find('.noData').removeClass('hidden');
                container.find('.chartWrapper').remove();
            }
            else {
                var chart = new sizeup.charts.barChart({
                    valueFormat: function (val) { return sizeup.util.numbers.format.addCommas(Math.floor(val)); },
                    bar: { height: 13, padding: 4 },
                    gutters: { left: 50, top: 1 },
                    fillRight: false,
                    container: container.find('.chart'),
                    title: null,
                    bars: formattedData
                });
                chart.draw();

                container.find('.buttons .mapActivate').click(function () {
                    me.data.activeHeatmap = 'averageEmployees';
                    setHeatmap();

                });

                container.find('.buttons .mapClear').click(function () {
                    me.data.activeHeatmap = null;
                    clearHeatmap();
                    clearLegend();
                });
            }
        };

        var initTotalEmployeesChart = function (data) {
            var container = me.container.find('#totalEmployees');
            container.find('.loading').remove();
            var indexes = ['City', 'County', 'Metro', 'State'];
            var formattedData = formatChartData(data, indexes);

            if (formattedData == null) {
                container.find('.noData').removeClass('hidden');
                container.find('.chartWrapper').remove();
            }
            else {
                var chart = new sizeup.charts.barChart({
                    valueFormat: function (val) { return sizeup.util.numbers.format.addCommas(Math.floor(val)); },
                    bar: { height: 13, padding: 4 },
                    gutters: { left: 50, top: 1 },
                    fillRight: false,
                    container: container.find('.chart'),
                    title: null,
                    bars: formattedData
                });
                chart.draw();

                container.find('.buttons .mapActivate').click(function () {
                    me.data.activeHeatmap = 'totalEmployees';
                    setHeatmap();

                });

                container.find('.buttons .mapClear').click(function () {
                    me.data.activeHeatmap = null;
                    clearHeatmap();
                    clearLegend();
                });
            }
        };


        var initAverageSalaryChart = function (data) {
            var container = me.container.find('#averageSalary');
            container.find('.loading').remove();
            var indexes = ['City', 'County', 'Metro', 'State', 'Nation'];
            var formattedData = formatChartData(data, indexes);

            if (formattedData == null) {
                container.find('.noData').removeClass('hidden');
                container.find('.chartWrapper').remove();
            }
            else {
                var chart = new sizeup.charts.barChart({
                    valueFormat: function (val) { return '$' + sizeup.util.numbers.format.addCommas(Math.floor(val)); },
                    bar: { height: 13, padding: 4 },
                    gutters: { left: 50, top: 1 },
                    fillRight: false,
                    container: container.find('.chart'),
                    title: null,
                    bars: formattedData
                });
                chart.draw();

                container.find('.buttons .mapActivate').click(function () {
                    me.data.activeHeatmap = 'averageSalary';
                    setHeatmap();

                });

                container.find('.buttons .mapClear').click(function () {
                    me.data.activeHeatmap = null;
                    clearHeatmap();
                    clearLegend();
                });
            }
        };


        var initCostEffectivenessChart = function (data) {
            var container = me.container.find('#costEffectiveness');
            container.find('.loading').remove();
            var indexes = ['City', 'County', 'Metro', 'State', 'Nation'];
            var formattedData = formatChartData(data, indexes);

            if (formattedData == null) {
                container.find('.noData').removeClass('hidden');
                container.find('.chartWrapper').remove();
            }
            else {
                var chart = new sizeup.charts.barChart({
                    valueFormat: function (val) { return sizeup.util.numbers.format.round(val, 1); },
                    bar: { height: 13, padding: 4 },
                    gutters: { left: 50, top: 1 },
                    fillRight: false,
                    container: container.find('.chart'),
                    title: null,
                    bars: formattedData
                });
                chart.draw();

                container.find('.buttons .mapActivate').click(function () {
                    me.data.activeHeatmap = 'costEffectiveness';
                    setHeatmap();

                });

                container.find('.buttons .mapClear').click(function () {
                    me.data.activeHeatmap = null;
                    clearHeatmap();
                    clearLegend();
                });
            }
        };



        var publicObj = {

        };
        return publicObj;

    };
})();