(function () {
    sizeup.core.namespace('sizeup.views.business');
    sizeup.views.business.business = function (opts) {


        var defaults = {
            mapSettings: sizeup.maps.mapOptions.getDefaults(),
            styles: sizeup.maps.mapStyles.getDefaults()
        };
        var me = {};

        me.opts = $.extend(true, defaults, opts);

        me.data = {};
        me.data.heatMapOverlays = [];
        me.data.activeHeatmap = null;
        me.container = $('#business');
        var dataLayer = new sizeup.core.data();
        var templates = new sizeup.core.templates(me.container);
        var notifier = new sizeup.core.notifier(function () { init(); });


        me.content = {};

        dataLayer.getBoundingBox({ id: opts.location.CurrentPlace.City.Id, granularity: 'City' }, notifier.getNotifier(function (data) { me.data.BoundingBox = data; }));

        var init = function () {

            var businessPoint = new sizeup.maps.latLng(me.opts.businessLocation);
            var bounds = new sizeup.maps.latLngBounds();
            bounds.extend(new sizeup.maps.latLng({ lat: me.data.BoundingBox.SouthWest.Lat, lng: me.data.BoundingBox.SouthWest.Lng }));
            bounds.extend(new sizeup.maps.latLng({ lat: me.data.BoundingBox.NorthEast.Lat, lng: me.data.BoundingBox.NorthEast.Lng }));


            me.content.map = new sizeup.maps.map({
                container: me.container.find('.mapWrapper.container .map')
            });
          
            me.content.map.addEventListener('zoom_changed', mapZoomUpdated);


            var borderOverlay = new sizeup.maps.overlay({
                tileUrl: '/tiles/geographyBoundary/',
                tileParams: {
                    id: opts.location.CurrentPlace.City.Id,
                    granularity: 'City'
                }
            })



            me.content.map.setCenter(businessPoint);
            me.content.map.fitBounds(bounds);
            me.content.map.addOverlay(borderOverlay);

         
            me.content.businessPin = new sizeup.maps.imagePin({
                color: 'ff5522',
                position: businessPoint
            });

            me.content.map.addMarker(me.content.businessPin);

            var data = {
                averageRevenue: {},
                totalRevenue: {},
                averageEmployees: {},
                totalEmployees: {},
                averageSalary: {},
                costEffectiveness: {}
            };

            var notifiers = {
                averageRevenue: new sizeup.core.notifier(function () { initAverageRevenueChart(data.averageRevenue); }),
                totalRevenue: new sizeup.core.notifier(function () { initTotalRevenueChart(data.totalRevenue); }),
                averageEmployees: new sizeup.core.notifier(function () { initAverageEmployeesChart(data.averageEmployees); }),
                totalEmployees: new sizeup.core.notifier(function () { initTotalEmployeesChart(data.totalEmployees); }),
                averageSalary: new sizeup.core.notifier(function () { initAverageSalaryChart(data.averageSalary); }),
                costEffectiveness: new sizeup.core.notifier(function () { initCostEffectivenessChart(data.costEffectiveness); }),
            };

            dataLayer.getAverageRevenueChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id, granularity: 'City' }, notifiers.averageRevenue.getNotifier(function (d) { data.averageRevenue.City = d; }));
            dataLayer.getAverageRevenueChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id, granularity: 'County' }, notifiers.averageRevenue.getNotifier(function (d) { data.averageRevenue.County = d; }));
            dataLayer.getAverageRevenueChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id, granularity: 'Metro' }, notifiers.averageRevenue.getNotifier(function (d) { data.averageRevenue.Metro = d; }));
            dataLayer.getAverageRevenueChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id, granularity: 'State' }, notifiers.averageRevenue.getNotifier(function (d) { data.averageRevenue.State = d; }));
            dataLayer.getAverageRevenueChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id, granularity: 'Nation' }, notifiers.averageRevenue.getNotifier(function (d) { data.averageRevenue.Nation = d; }));


            dataLayer.getTotalRevenueChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id, granularity: 'City' }, notifiers.totalRevenue.getNotifier(function (d) { data.totalRevenue.City = d; }));
            dataLayer.getTotalRevenueChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id, granularity: 'County' }, notifiers.totalRevenue.getNotifier(function (d) { data.totalRevenue.County = d; }));
            dataLayer.getTotalRevenueChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id, granularity: 'Metro' }, notifiers.totalRevenue.getNotifier(function (d) { data.totalRevenue.Metro = d; }));
            dataLayer.getTotalRevenueChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id, granularity: 'State' }, notifiers.totalRevenue.getNotifier(function (d) { data.totalRevenue.State = d; }));
            dataLayer.getTotalRevenueChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id, granularity: 'Nation' }, notifiers.totalRevenue.getNotifier(function (d) { data.totalRevenue.Nation = d; }));


            dataLayer.getAverageEmployeesChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id, granularity: 'City' }, notifiers.averageEmployees.getNotifier(function (d) { data.averageEmployees.City = d; }));
            dataLayer.getAverageEmployeesChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id, granularity: 'County' }, notifiers.averageEmployees.getNotifier(function (d) { data.averageEmployees.County = d; }));
            dataLayer.getAverageEmployeesChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id, granularity: 'Metro' }, notifiers.averageEmployees.getNotifier(function (d) { data.averageEmployees.Metro = d; }));
            dataLayer.getAverageEmployeesChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id, granularity: 'State' }, notifiers.averageEmployees.getNotifier(function (d) { data.averageEmployees.State = d; }));
            dataLayer.getAverageEmployeesChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id, granularity: 'Nation' }, notifiers.averageEmployees.getNotifier(function (d) { data.averageEmployees.Nation = d; }));


            dataLayer.getTotalEmployeesChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id, granularity: 'City' }, notifiers.totalEmployees.getNotifier(function (d) { data.totalEmployees.City = d; }));
            dataLayer.getTotalEmployeesChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id, granularity: 'County' }, notifiers.totalEmployees.getNotifier(function (d) { data.totalEmployees.County = d; }));
            dataLayer.getTotalEmployeesChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id, granularity: 'Metro' }, notifiers.totalEmployees.getNotifier(function (d) { data.totalEmployees.Metro = d; }));
            dataLayer.getTotalEmployeesChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id, granularity: 'State' }, notifiers.totalEmployees.getNotifier(function (d) { data.totalEmployees.State = d; }));
            dataLayer.getTotalEmployeesChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id, granularity: 'Nation' }, notifiers.totalEmployees.getNotifier(function (d) { data.totalEmployees.Nation = d; }));



            dataLayer.getAverageSalaryChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id, granularity: 'County' }, notifiers.averageSalary.getNotifier(function (d) { data.averageSalary.County = d; }));
            dataLayer.getAverageSalaryChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id, granularity: 'Metro' }, notifiers.averageSalary.getNotifier(function (d) { data.averageSalary.Metro = d; }));
            dataLayer.getAverageSalaryChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id, granularity: 'State' }, notifiers.averageSalary.getNotifier(function (d) { data.averageSalary.State = d; }));
            dataLayer.getAverageSalaryChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id, granularity: 'Nation' }, notifiers.averageSalary.getNotifier(function (d) { data.averageSalary.Nation = d; }));



            dataLayer.getCostEffectivenessChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id, granularity: 'County' }, notifiers.costEffectiveness.getNotifier(function (d) { data.costEffectiveness.County = d; }));
            dataLayer.getCostEffectivenessChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id, granularity: 'Metro' }, notifiers.costEffectiveness.getNotifier(function (d) { data.costEffectiveness.Metro = d; }));
            dataLayer.getCostEffectivenessChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id, granularity: 'State' }, notifiers.costEffectiveness.getNotifier(function (d) { data.costEffectiveness.State = d; }));
            dataLayer.getCostEffectivenessChart({ industryId: me.opts.location.CurrentIndustry.Id, placeId: me.opts.location.CurrentPlace.Id, granularity: 'Nation' }, notifiers.costEffectiveness.getNotifier(function (d) { data.costEffectiveness.Nation = d; }));



            
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
                dataLayer.getAverageRevenueBands({
                    placeId: me.opts.location.CurrentPlace.Id,
                    industryId: me.opts.location.CurrentIndustry.Id,
                    granularity: 'ZipCode',
                    boundingGranularity: 'County',
                    bands: 7
                }, itemsNotify);
            }

            if (me.opts.location.CurrentPlace.Metro.Id != null) {
                if (z <= 10 && z >= 8) {
                    data.title = 'Average Business Annual Revenue by county in ' + me.opts.location.CurrentPlace.Metro.Name + ' (Metro)';
                    dataLayer.getAverageRevenueBands({
                        placeId: me.opts.location.CurrentPlace.Id,
                        industryId: me.opts.location.CurrentIndustry.Id,
                        granularity: 'County',
                        boundingGranularity: 'Metro',
                        bands: 7
                    }, itemsNotify);


                }

                if (z <= 7 && z >= 5) {
                    data.title = 'Average Business Annual Revenue by county in ' + me.opts.location.CurrentPlace.State.Name;
                    dataLayer.getAverageRevenueBands({
                        placeId: me.opts.location.CurrentPlace.Id,
                        industryId: me.opts.location.CurrentIndustry.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        bands: 7
                    }, itemsNotify);
                }
            }
            else {
                if (z <= 10 && z >= 5) {

                    data.title = 'Average Business Annual Revenue by county in ' + me.opts.location.CurrentPlace.State.Name;
                    dataLayer.getAverageRevenueBands({
                        placeId: me.opts.location.CurrentPlace.Id,
                        industryId: me.opts.location.CurrentIndustry.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        bands: 7
                    }, itemsNotify);

                }
            }

            if (z <= 4 && z >= 0) {
                data.title = 'Average Business Annual Revenue by state in the USA';
                dataLayer.getAverageRevenueBands({
                    placeId: me.opts.location.CurrentPlace.Id,
                    industryId: me.opts.location.CurrentIndustry.Id,
                    granularity: 'State',
                    bands: 7
                }, itemsNotify);
            }
        };

        var setAverageRevenueOverlays = function () {

            var overlays = [];

            overlays.push(new sizeup.maps.overlay({
                tileUrl: '/tiles/AverageRevenue/',
                tileParams: {
                    colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                    ],
                    placeId: me.opts.location.CurrentPlace.Id,
                    granularity: 'State',
                    industryId: me.opts.location.CurrentIndustry.Id
                },
                minZoom: 0,
                maxZoom: 4
            }));

            if (me.opts.location.CurrentPlace.Metro.Id != null) {
                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/AverageRevenue/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ],
                        placeId: me.opts.location.CurrentPlace.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 5,
                    maxZoom: 7
                }));

                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/AverageRevenue/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ],
                        placeId: me.opts.location.CurrentPlace.Id,
                        granularity: 'County',
                        boundingGranularity: 'Metro',
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 8,
                    maxZoom: 10
                }));
            }
            else {
                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/AverageRevenue/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ],
                        placeId: me.opts.location.CurrentPlace.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 5,
                    maxZoom: 10
                }));
            }

            overlays.push(new sizeup.maps.overlay({
                tileUrl: '/tiles/AverageRevenue/',
                tileParams: {
                    colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                    ],
                    placeId: me.opts.location.CurrentPlace.Id,
                    granularity: 'ZipCode',
                    boundingGranularity: 'County',
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
                dataLayer.getTotalRevenueBands({
                    placeId: me.opts.location.CurrentPlace.Id,
                    industryId: me.opts.location.CurrentIndustry.Id,
                    granularity: 'ZipCode',
                    boundingGranularity: 'County',
                    bands: 7
                }, itemsNotify);
            }

            if (me.opts.location.CurrentPlace.Metro.Id != null) {
                if (z <= 10 && z >= 8) {
                    data.title = 'Total Revenue by county in ' + me.opts.location.CurrentPlace.Metro.Name + ' (Metro)';
                    dataLayer.getTotalRevenueBands({
                        placeId: me.opts.location.CurrentPlace.Id,
                        industryId: me.opts.location.CurrentIndustry.Id,
                        granularity: 'County',
                        boundingGranularity: 'Metro',
                        bands: 7
                    }, itemsNotify);


                }

                if (z <= 7 && z >= 5) {
                    data.title = 'Total Revenue by county in ' + me.opts.location.CurrentPlace.State.Name;
                    dataLayer.getTotalRevenueBands({
                        placeId: me.opts.location.CurrentPlace.Id,
                        industryId: me.opts.location.CurrentIndustry.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        bands: 7
                    }, itemsNotify);
                }
            }
            else {
                if (z <= 10 && z >= 5) {

                    data.title = 'Total Revenue by county in ' + me.opts.location.CurrentPlace.State.Name;
                    dataLayer.getTotalRevenueBands({
                        placeId: me.opts.location.CurrentPlace.Id,
                        industryId: me.opts.location.CurrentIndustry.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        bands: 7
                    }, itemsNotify);

                }
            }

            if (z <= 4 && z >= 0) {
                data.title = 'Total Revenue by state in the USA';
                dataLayer.getTotalRevenueBands({
                    placeId: me.opts.location.CurrentPlace.Id,
                    industryId: me.opts.location.CurrentIndustry.Id,
                    granularity: 'State',
                    bands: 7
                }, itemsNotify);
            }
        };

        var setTotalRevenueOverlays = function () {

            var overlays = [];

            overlays.push(new sizeup.maps.overlay({
                tileUrl: '/tiles/TotalRevenue/',
                tileParams: {
                    colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                    ],
                    placeId: me.opts.location.CurrentPlace.Id,
                    granularity: 'State',
                    industryId: me.opts.location.CurrentIndustry.Id
                },
                minZoom: 0,
                maxZoom: 4
            }));

            if (me.opts.location.CurrentPlace.Metro.Id != null) {
                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/TotalRevenue/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ],
                        placeId: me.opts.location.CurrentPlace.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 5,
                    maxZoom: 7
                }));

                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/TotalRevenue/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ],
                        placeId: me.opts.location.CurrentPlace.Id,
                        granularity: 'County',
                        boundingGranularity: 'Metro',
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 8,
                    maxZoom: 10
                }));
            }
            else {
                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/TotalRevenue/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ],
                        placeId: me.opts.location.CurrentPlace.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 5,
                    maxZoom: 10
                }));
            }

            overlays.push(new sizeup.maps.overlay({
                tileUrl: '/tiles/TotalRevenue/',
                tileParams: {
                    colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                    ],
                    placeId: me.opts.location.CurrentPlace.Id,
                    granularity: 'ZipCode',
                    boundingGranularity: 'County',
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
                dataLayer.getAverageEmployeesBands({
                    placeId: me.opts.location.CurrentPlace.Id,
                    industryId: me.opts.location.CurrentIndustry.Id,
                    granularity: 'ZipCode',
                    boundingGranularity: 'County',
                    bands: 7
                }, itemsNotify);
            }

            if (me.opts.location.CurrentPlace.Metro.Id != null) {
                if (z <= 10 && z >= 8) {
                    data.title = 'Average Employees per business by county in ' + me.opts.location.CurrentPlace.Metro.Name + ' (Metro)';
                    dataLayer.getAverageEmployeesBands({
                        placeId: me.opts.location.CurrentPlace.Id,
                        industryId: me.opts.location.CurrentIndustry.Id,
                        granularity: 'County',
                        boundingGranularity: 'Metro',
                        bands: 7
                    }, itemsNotify);


                }

                if (z <= 7 && z >= 5) {
                    data.title = 'Average Employees per business by county in ' + me.opts.location.CurrentPlace.State.Name;
                    dataLayer.getAverageEmployeesBands({
                        placeId: me.opts.location.CurrentPlace.Id,
                        industryId: me.opts.location.CurrentIndustry.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        bands: 7
                    }, itemsNotify);
                }
            }
            else {
                if (z <= 10 && z >= 5) {

                    data.title = 'Average Employees per business by county in ' + me.opts.location.CurrentPlace.State.Name;
                    dataLayer.getAverageEmployeesBands({
                        placeId: me.opts.location.CurrentPlace.Id,
                        industryId: me.opts.location.CurrentIndustry.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        bands: 7
                    }, itemsNotify);

                }
            }

            if (z <= 4 && z >= 0) {
                data.title = 'Average Employees per business by state in the USA';
                dataLayer.getAverageEmployeesBands({
                    placeId: me.opts.location.CurrentPlace.Id,
                    industryId: me.opts.location.CurrentIndustry.Id,
                    granularity: 'State',
                    bands: 7
                }, itemsNotify);
            }
        };

        var setAverageEmployeesOverlays = function () {

            var overlays = [];

            overlays.push(new sizeup.maps.overlay({
                tileUrl: '/tiles/AverageEmployees/',
                tileParams: {
                    colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                    ],
                    placeId: me.opts.location.CurrentPlace.Id,
                    granularity: 'State',
                    industryId: me.opts.location.CurrentIndustry.Id
                },
                minZoom: 0,
                maxZoom: 4
            }));

            if (me.opts.location.CurrentPlace.Metro.Id != null) {
                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/AverageEmployees/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ],
                        placeId: me.opts.location.CurrentPlace.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 5,
                    maxZoom: 7
                }));

                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/AverageEmployees/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ],
                        placeId: me.opts.location.CurrentPlace.Id,
                        granularity: 'County',
                        boundingGranularity: 'Metro',
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 8,
                    maxZoom: 10
                }));
            }
            else {
                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/AverageEmployees/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ],
                        placeId: me.opts.location.CurrentPlace.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 5,
                    maxZoom: 10
                }));
            }

            overlays.push(new sizeup.maps.overlay({
                tileUrl: '/tiles/AverageEmployees/',
                tileParams: {
                    colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                    ],
                    placeId: me.opts.location.CurrentPlace.Id,
                    granularity: 'ZipCode',
                    boundingGranularity: 'County',
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
                dataLayer.getAverageEmployeesBands({
                    placeId: me.opts.location.CurrentPlace.Id,
                    industryId: me.opts.location.CurrentIndustry.Id,
                    granularity: 'ZipCode',
                    boundingGranularity: 'County',
                    bands: 7
                }, itemsNotify);
            }

            if (me.opts.location.CurrentPlace.Metro.Id != null) {
                if (z <= 10 && z >= 8) {
                    data.title = 'Total Employees by county in ' + me.opts.location.CurrentPlace.Metro.Name + ' (Metro)';
                    dataLayer.getAverageEmployeesBands({
                        placeId: me.opts.location.CurrentPlace.Id,
                        industryId: me.opts.location.CurrentIndustry.Id,
                        granularity: 'County',
                        boundingGranularity: 'Metro',
                        bands: 7
                    }, itemsNotify);


                }

                if (z <= 7 && z >= 5) {
                    data.title = 'Total Employees by county in ' + me.opts.location.CurrentPlace.State.Name;
                    dataLayer.getAverageEmployeesBands({
                        placeId: me.opts.location.CurrentPlace.Id,
                        industryId: me.opts.location.CurrentIndustry.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        bands: 7
                    }, itemsNotify);
                }
            }
            else {
                if (z <= 10 && z >= 5) {

                    data.title = 'Total Employees by county in ' + me.opts.location.CurrentPlace.State.Name;
                    dataLayer.getAverageEmployeesBands({
                        placeId: me.opts.location.CurrentPlace.Id,
                        industryId: me.opts.location.CurrentIndustry.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        bands: 7
                    }, itemsNotify);

                }
            }

            if (z <= 4 && z >= 0) {
                data.title = 'Total Employees by state in the USA';
                dataLayer.getAverageEmployeesBands({
                    placeId: me.opts.location.CurrentPlace.Id,
                    industryId: me.opts.location.CurrentIndustry.Id,
                    granularity: 'State',
                    bands: 7
                }, itemsNotify);
            }
        };

        var setTotalEmployeesOverlays = function () {

            var overlays = [];

            overlays.push(new sizeup.maps.overlay({
                tileUrl: '/tiles/TotalEmployees/',
                tileParams: {
                    colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                    ],
                    placeId: me.opts.location.CurrentPlace.Id,
                    granularity: 'State',
                    industryId: me.opts.location.CurrentIndustry.Id
                },
                minZoom: 0,
                maxZoom: 4
            }));

            if (me.opts.location.CurrentPlace.Metro.Id != null) {
                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/TotalEmployees/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ],
                        placeId: me.opts.location.CurrentPlace.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 5,
                    maxZoom: 7
                }));

                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/TotalEmployees/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ],
                        placeId: me.opts.location.CurrentPlace.Id,
                        granularity: 'County',
                        boundingGranularity: 'Metro',
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 8,
                    maxZoom: 10
                }));
            }
            else {
                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/TotalEmployees/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ],
                        placeId: me.opts.location.CurrentPlace.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 5,
                    maxZoom: 10
                }));
            }

            overlays.push(new sizeup.maps.overlay({
                tileUrl: '/tiles/TotalEmployees/',
                tileParams: {
                    colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                    ],
                    placeId: me.opts.location.CurrentPlace.Id,
                    granularity: 'ZipCode',
                    boundingGranularity: 'County',
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
                    dataLayer.getAverageSalaryBands({
                        placeId: me.opts.location.CurrentPlace.Id,
                        industryId: me.opts.location.CurrentIndustry.Id,
                        granularity: 'County',
                        boundingGranularity: 'Metro',
                        bands: 7
                    }, itemsNotify);


                }

                if (z <= 7 && z >= 5) {
                    data.title = 'Average Salary by county in ' + me.opts.location.CurrentPlace.State.Name;
                    dataLayer.getAverageSalaryBands({
                        placeId: me.opts.location.CurrentPlace.Id,
                        industryId: me.opts.location.CurrentIndustry.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        bands: 7
                    }, itemsNotify);
                }
            }
            else {
                if (z <= 32 && z >= 5) {

                    data.title = 'Average Salary by county in ' + me.opts.location.CurrentPlace.State.Name;
                    dataLayer.getAverageSalaryBands({
                        placeId: me.opts.location.CurrentPlace.Id,
                        industryId: me.opts.location.CurrentIndustry.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        bands: 7
                    }, itemsNotify);

                }
            }

            if (z <= 4 && z >= 0) {
                data.title = 'Average Salary by state in the USA';
                dataLayer.getAverageSalaryBands({
                    placeId: me.opts.location.CurrentPlace.Id,
                    industryId: me.opts.location.CurrentIndustry.Id,
                    granularity: 'State',
                    bands: 7
                }, itemsNotify);
            }
        };

        var setAverageSalaryOverlays = function () {

            var overlays = [];

            overlays.push(new sizeup.maps.overlay({
                tileUrl: '/tiles/AverageSalary/',
                tileParams: {
                    colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                    ],
                    placeId: me.opts.location.CurrentPlace.Id,
                    granularity: 'State',
                    industryId: me.opts.location.CurrentIndustry.Id
                },
                minZoom: 0,
                maxZoom: 4
            }));

            if (me.opts.location.CurrentPlace.Metro.Id != null) {
                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/AverageSalary/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ],
                        placeId: me.opts.location.CurrentPlace.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 5,
                    maxZoom: 7
                }));

                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/AverageSalary/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ],
                        placeId: me.opts.location.CurrentPlace.Id,
                        granularity: 'County',
                        boundingGranularity: 'Metro',
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 8,
                    maxZoom: 32
                }));
            }
            else {
                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/AverageSalary/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ],
                        placeId: me.opts.location.CurrentPlace.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
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
                    dataLayer.getCostEffectivenessBands({
                        placeId: me.opts.location.CurrentPlace.Id,
                        industryId: me.opts.location.CurrentIndustry.Id,
                        granularity: 'County',
                        boundingGranularity: 'Metro',
                        bands: 7
                    }, itemsNotify);


                }

                if (z <= 7 && z >= 5) {
                    data.title = 'Cost Effectiveness by county in ' + me.opts.location.CurrentPlace.State.Name;
                    dataLayer.getCostEffectivenessBands({
                        placeId: me.opts.location.CurrentPlace.Id,
                        industryId: me.opts.location.CurrentIndustry.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        bands: 7
                    }, itemsNotify);
                }
            }
            else {
                if (z <= 32 && z >= 5) {

                    data.title = 'Cost Effectiveness by county in ' + me.opts.location.CurrentPlace.State.Name;
                    dataLayer.getCostEffectivenessBands({
                        placeId: me.opts.location.CurrentPlace.Id,
                        industryId: me.opts.location.CurrentIndustry.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        bands: 7
                    }, itemsNotify);

                }
            }

            if (z <= 4 && z >= 0) {
                data.title = 'Cost Effectiveness by state in the USA';
                dataLayer.getCostEffectivenessBands({
                    placeId: me.opts.location.CurrentPlace.Id,
                    industryId: me.opts.location.CurrentIndustry.Id,
                    granularity: 'State',
                    bands: 7
                }, itemsNotify);
            }
        };

        var setCostEffectivenessOverlays = function () {

            var overlays = [];

            overlays.push(new sizeup.maps.overlay({
                tileUrl: '/tiles/CostEffectiveness/',
                tileParams: {
                    colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                    ],
                    placeId: me.opts.location.CurrentPlace.Id,
                    granularity: 'State',
                    industryId: me.opts.location.CurrentIndustry.Id
                },
                minZoom: 0,
                maxZoom: 4
            }));

            if (me.opts.location.CurrentPlace.Metro.Id != null) {
                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/CostEffectiveness/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ],
                        placeId: me.opts.location.CurrentPlace.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 5,
                    maxZoom: 7
                }));

                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/CostEffectiveness/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ],
                        placeId: me.opts.location.CurrentPlace.Id,
                        granularity: 'County',
                        boundingGranularity: 'Metro',
                        industryId: me.opts.location.CurrentIndustry.Id
                    },
                    minZoom: 8,
                    maxZoom: 32
                }));
            }
            else {
                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/CostEffectiveness/',
                    tileParams: {
                        colors: [
                                    '#F5F500',
                                    '#F5CC00',
                                    '#F5A300',
                                    '#F57A00',
                                    '#F55200',
                                    '#F52900',
                                    '#F50000'
                        ],
                        placeId: me.opts.location.CurrentPlace.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
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