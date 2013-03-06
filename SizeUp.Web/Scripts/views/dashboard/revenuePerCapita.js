﻿(function () {
    sizeup.core.namespace('sizeup.views.dashboard.revenuePerCapita');
    sizeup.views.dashboard.revenuePerCapita = function (opts) {

        var me = {};
        var dataLayer = new sizeup.core.data();
        var templates = new sizeup.core.templates(opts.container);
        me.opts = opts;
        me.data = {};
        me.container = opts.container;
        me.data.hasData = false;
        me.data.description = {};

        var init = function () {

            me.reportContainer = new sizeup.views.dashboard.reportContainer(
                {
                    container: me.container,
                    inputValidation: /^[0-9]+$/g,
                    inputCleaning: /[\$\,]/g,
                    events:
                    {
                        runReport: runReport,
                        valueChanged: function (val) { }
                    },
                    inputFormat: function (val) {
                        return '$' + sizeup.util.numbers.format.addCommas(val);
                    }
                });

            me.sourceButton = new sizeup.controls.toggleButton(
                {
                    button: me.container.find('.reportContainer .links .source'),
                    onClick: function () { toggleSource(); }
                });

            me.mapToggle = new sizeup.controls.toggleButton(
                {
                    button: me.container.find('.mapToggle'),
                    onClick: function () { toggleMap(); }
                });

            me.chartToggle = new sizeup.controls.toggleButton(
                {
                    button: me.container.find('.chartToggle'),
                    onClick: function () { toggleChart(); }
                });

            me.considerationToggle = new sizeup.controls.toggleButton(
                {
                    button: me.container.find('.reportSidebar .considerationToggle'),
                    onClick: function () { toggleConsiderations(); }
                });

            me.sourceContent = me.container.find('.reportContainer .sourceContent').hide();
            me.considerations = me.container.find('.reportContainer .considerations');
            me.resources = me.container.find('.reportContainer .resources');
            me.description = me.container.find('.reportContainer .description');



            me.noData = me.container.find('.noDataError').hide();
            me.reportData = me.container.find('.reportData');

            me.reportContainer.setValue('');
     

        };

        var toggleMap = function () {
            me.revenuePerCapitaMap.getContainer().toggle("slide", { direction: "up" }, 350);
            me.totalRevenueMap.getContainer().toggle("slide", { direction: "up" }, 350);
        };

        var toggleSource = function () {
            me.sourceContent.slideToggle();
        };

        var toggleConsiderations = function () {
            me.considerations.toggleClass('collapsed', 1000);
        };

        var toggleChart = function () {
            if (me.chart.getContainer().is(':visible')) {
                me.chart.getContainer().toggle("slide", { direction: "up" }, 350, function () {
                    me.table.getContainer().toggle("slide", { direction: "up" }, 350);
                });
            }
            else {
                me.table.getContainer().toggle("slide", { direction: "up" }, 350, function () {
                    me.chart.getContainer().toggle("slide", { direction: "up" }, 350);
                });
            }
        };


        var setRevenuePerCapitaHeatmap = function () {

            var overlays = [];

            overlays.push(new sizeup.maps.overlay({
                tileUrl: '/tiles/revenuePerCapita/',
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
                    placeId: me.opts.report.CurrentPlace.Id,
                    granularity: 'ZipCode',
                    boundingGranularity: 'County',
                    industryId: me.opts.report.IndustryDetails.Industry.Id
                },
                minZoom: 11,
                maxZoom: 32
            }));
            if (me.opts.report.CurrentPlace.Metro.Id != null) {

                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/revenuePerCapita/',
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
                        placeId: me.opts.report.CurrentPlace.Id,
                        granularity: 'County',
                        boundingGranularity: 'Metro',
                        industryId: me.opts.report.IndustryDetails.Industry.Id
                    },
                    minZoom: 8,
                    maxZoom: 10
                }));

                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/revenuePerCapita/',
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
                        placeId: me.opts.report.CurrentPlace.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        industryId: me.opts.report.IndustryDetails.Industry.Id
                    },
                    minZoom: 5,
                    maxZoom: 7
                }));
            }
            else {
                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/revenuePerCapita/',
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
                        placeId: me.opts.report.CurrentPlace.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        industryId: me.opts.report.IndustryDetails.Industry.Id
                    },
                    minZoom: 5,
                    maxZoom: 10
                }));
            }


            overlays.push(new sizeup.maps.overlay({
                tileUrl: '/tiles/revenuePerCapita/',
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
                    placeId: me.opts.report.CurrentPlace.Id,
                    granularity: 'State',
                    industryId: me.opts.report.IndustryDetails.Industry.Id
                },
                minZoom: 0,
                maxZoom: 4
            }));


            me.revenuePerCapitaMap = new sizeup.maps.map({
                container: me.container.find('.mapWrapper.container.revenuePerCapita .map')
            });
            me.revenuePerCapitaMap.setCenter(new sizeup.maps.latLng({ lat: me.opts.report.MapCenter.Lat, lng: me.opts.report.MapCenter.Lng }));
            me.revenuePerCapitaMap.setZoom(12);
            me.revenuePerCapitaMap.addEventListener('zoom_changed', mapZoomRevenuePerCapitaUpdated);

            for (var x in overlays) {
                me.revenuePerCapitaMap.addOverlay(overlays[x], 0);
            }

            setRevenuePerCapitaLegend();

            me.textAlternativeRevenuePerCapita = me.container.find('.mapWrapper.revenuePerCapita .textAlternative');
            me.textAlternativeRevenuePerCapita.click(textAlternativeRevenuePerCapitaClicked);
        };

        var setRevenuePerCapitaLegend = function () {

            var data = {
                title: '',
                items: []
            };
            var z = me.revenuePerCapitaMap.getZoom();

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
                me.revenuePerCapitaMap.setLegend(legend);
            });



            var itemsNotify = notify.getNotifier(function (d) { data.items = d; });


            if (z <= 32 && z >= 11) {
                data.title = 'Revenue Per Capita by ZIP code in ' + me.opts.report.CurrentPlace.County.Name + ', ' + me.opts.report.CurrentPlace.State.Abbreviation;
                me.data.currentRevenuePerCapitaBoundingEntityId = 'co' + me.opts.report.CurrentPlace.County.Id;
                me.data.textAlternativeRevenuePerCapitaUrl = '/accessibility/reveuePerCapita/zip/';
                dataLayer.getRevenuePerCapitaBands({
                    placeId: me.opts.report.CurrentPlace.Id,
                    industryId: me.opts.report.IndustryDetails.Industry.Id,
                    granularity: 'ZipCode',
                    boundingGranularity: 'County',
                    bands: 7
                }, itemsNotify);
            }

            if (me.opts.report.CurrentPlace.Metro.Id != null) {
                if (z <= 10 && z >= 8) {
                    data.title = 'Revenue Per Capita by county in ' + me.opts.report.CurrentPlace.Metro.Name + ' (Metro)';
                    me.data.currentRevenuePerCapitaBoundingEntityId = 'm' + me.opts.report.CurrentPlace.Metro.Id;
                    me.data.textAlternativeRevenuePerCapitaUrl = '/accessibility/reveuePerCapita/county/';
                    dataLayer.getRevenuePerCapitaBands({
                        placeId: me.opts.report.CurrentPlace.Id,
                        industryId: me.opts.report.IndustryDetails.Industry.Id,
                        granularity: 'County',
                        boundingGranularity: 'Metro',
                        bands: 7
                    }, itemsNotify);


                }

                if (z <= 7 && z >= 5) {
                    data.title = 'Revenue Per Capita by county in ' + me.opts.report.CurrentPlace.State.Name;
                    me.data.currentRevenuePerCapitaBoundingEntityId = 's' + me.opts.report.CurrentPlace.State.Id;
                    me.data.textAlternativeRevenuePerCapitaUrl = '/accessibility/reveuePerCapita/county/';
                    dataLayer.getRevenuePerCapitaBands({
                        placeId: me.opts.report.CurrentPlace.Id,
                        industryId: me.opts.report.IndustryDetails.Industry.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        bands: 7
                    }, itemsNotify);
                }
            }
            else {
                if (z <= 10 && z >= 5) {

                    data.title = 'Revenue Per Capita by county in ' + me.opts.report.CurrentPlace.State.Name;
                    me.data.textAlternativeRevenuePerCapitaUrl = '/accessibility/revenuePerCapita/county/';
                    me.data.currentRevenuePerCapitaBoundingEntityId = 's' + me.opts.report.CurrentPlace.State.Id;
                    dataLayer.getRevenuePerCapitaBands({
                        placeId: me.opts.report.CurrentPlace.Id,
                        industryId: me.opts.report.IndustryDetails.Industry.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        bands: 7
                    }, itemsNotify);

                }
            }

            if (z <= 4 && z >= 0) {

                data.title = 'Revenue Per Capita by state in the USA';
                me.data.textAlternativeRevenuePerCapitaUrl = '/accessibility/revenuePerCapita/state/';
                me.data.currentRevenuePerCapitaBoundingEntityId = null;
                dataLayer.getRevenuePerCapitaBands({
                    placeId: me.opts.report.CurrentPlace.Id,
                    industryId: me.opts.report.IndustryDetails.Industry.Id,
                    granularity: 'State',
                    bands: 7
                }, itemsNotify);
            }
        };

        var mapZoomRevenuePerCapitaUpdated = function () {
            setRevenuePerCapitaLegend();
        };

        var textAlternativeRevenuePerCapitaClicked = function () {
            var url = me.data.textAlternativeRevenuePerCapitaUrl;
            var bounds = me.map.getBounds();
            var data = {
                bands: 7,
                industryId: me.opts.report.IndustryDetails.Industry.Id,
                boundingEntityId: me.data.currentRevenuePerCapitaBoundingEntityId,
                southWest: bounds.getSouthWest().toString(),
                northEast: bounds.getNorthEast().toString()
            };

            window.open(jQuery.param.querystring(url, data), '_blank');

        };



        var setTotalRevenueHeatmap = function () {

            var overlays = [];

            overlays.push(new sizeup.maps.overlay({
                tileUrl: '/tiles/totalRevenue/',
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
                    placeId: me.opts.report.CurrentPlace.Id,
                    granularity: 'ZipCode',
                    boundingGranularity: 'County',
                    industryId: me.opts.report.IndustryDetails.Industry.Id
                },
                minZoom: 11,
                maxZoom: 32
            }));
            if (me.opts.report.CurrentPlace.Metro.Id != null) {

                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/totalRevenue/',
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
                        placeId: me.opts.report.CurrentPlace.Id,
                        granularity: 'County',
                        boundingGranularity: 'Metro',
                        industryId: me.opts.report.IndustryDetails.Industry.Id
                    },
                    minZoom: 8,
                    maxZoom: 10
                }));

                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/totalRevenue/',
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
                        placeId: me.opts.report.CurrentPlace.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        industryId: me.opts.report.IndustryDetails.Industry.Id
                    },
                    minZoom: 5,
                    maxZoom: 7
                }));
            }
            else {
                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/totalRevenue/',
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
                        placeId: me.opts.report.CurrentPlace.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        industryId: me.opts.report.IndustryDetails.Industry.Id
                    },
                    minZoom: 5,
                    maxZoom: 10
                }));
            }


            overlays.push(new sizeup.maps.overlay({
                tileUrl: '/tiles/totalRevenue/',
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
                    placeId: me.opts.report.CurrentPlace.Id,
                    granularity: 'State',
                    industryId: me.opts.report.IndustryDetails.Industry.Id
                },
                minZoom: 0,
                maxZoom: 4
            }));


            me.totalRevenueMap = new sizeup.maps.map({
                container: me.container.find('.mapWrapper.container.totalRevenue .map')
            });
            me.totalRevenueMap.setCenter(new sizeup.maps.latLng({ lat: me.opts.report.MapCenter.Lat, lng: me.opts.report.MapCenter.Lng }));
            me.totalRevenueMap.setZoom(12);
            me.totalRevenueMap.addEventListener('zoom_changed', mapZoomTotalRevenueUpdated);

            for (var x in overlays) {
                me.totalRevenueMap.addOverlay(overlays[x], 0);
            }

            setTotalRevenueLegend();

            me.textAlternativeTotalRevenue = me.container.find('.mapWrapper.totalRevenue .textAlternative');
            me.textAlternativeTotalRevenue.click(textAlternativeTotalRevenueClicked);
        };

        var setTotalRevenueLegend = function () {

            var data = {
                title: '',
                items: []
            };
            var z = me.totalRevenueMap.getZoom();

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
                me.totalRevenueMap.setLegend(legend);
            });



            var itemsNotify = notify.getNotifier(function (d) { data.items = d; });


            if (z <= 32 && z >= 11) {
                data.title = 'Total Revenue by ZIP code in ' + me.opts.report.CurrentPlace.County.Name + ', ' + me.opts.report.CurrentPlace.State.Abbreviation;
                me.data.currentTotalRevenueBoundingEntityId = 'co' + me.opts.report.CurrentPlace.County.Id;
                me.data.textAlternativeTotalRevenueUrl = '/accessibility/totalRevenue/zip/';
                dataLayer.getTotalRevenueBands({
                    placeId: me.opts.report.CurrentPlace.Id,
                    industryId: me.opts.report.IndustryDetails.Industry.Id,
                    granularity: 'ZipCode',
                    boundingGranularity: 'County',
                    bands: 7
                }, itemsNotify);
            }

            if (me.opts.report.CurrentPlace.Metro.Id != null) {
                if (z <= 10 && z >= 8) {
                    data.title = 'Total Revenue by county in ' + me.opts.report.CurrentPlace.Metro.Name + ' (Metro)';
                    me.data.currentTotalRevenueBoundingEntityId = 'm' + me.opts.report.CurrentPlace.Metro.Id;
                    me.data.textAlternativeTotalRevenueUrl = '/accessibility/totalRevenue/county/';
                    dataLayer.getTotalRevenueBands({
                        placeId: me.opts.report.CurrentPlace.Id,
                        industryId: me.opts.report.IndustryDetails.Industry.Id,
                        granularity: 'County',
                        boundingGranularity: 'Metro',
                        bands: 7
                    }, itemsNotify);


                }

                if (z <= 7 && z >= 5) {
                    data.title = 'Total Revenue by county in ' + me.opts.report.CurrentPlace.State.Name;
                    me.data.currentTotalRevenueBoundingEntityId = 's' + me.opts.report.CurrentPlace.State.Id;
                    me.data.textAlternativeTotalRevenueUrl = '/accessibility/totalRevenue/county/';
                    dataLayer.getTotalRevenueBands({
                        placeId: me.opts.report.CurrentPlace.Id,
                        industryId: me.opts.report.IndustryDetails.Industry.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        bands: 7
                    }, itemsNotify);
                }
            }
            else {
                if (z <= 10 && z >= 5) {

                    data.title = 'Total Revenue by county in ' + me.opts.report.CurrentPlace.State.Name;
                    me.data.textAlternativeTotalRevenueUrl = '/accessibility/totalRevenue/county/';
                    me.data.currentTotalRevenueBoundingEntityId = 's' + me.opts.report.CurrentPlace.State.Id;
                    dataLayer.getTotalRevenueBands({
                        placeId: me.opts.report.CurrentPlace.Id,
                        industryId: me.opts.report.IndustryDetails.Industry.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        bands: 7
                    }, itemsNotify);

                }
            }

            if (z <= 4 && z >= 0) {

                data.title = 'Total Revenue by state in the USA';
                me.data.textAlternativeTotalRevenueUrl = '/accessibility/totalRevenue/state/';
                me.data.currentTotalRevenueBoundingEntityId = null;
                dataLayer.getTotalRevenueBands({
                    placeId: me.opts.report.CurrentPlace.Id,
                    industryId: me.opts.report.IndustryDetails.Industry.Id,
                    granularity: 'State',
                    bands: 7
                }, itemsNotify);
            }
        };

        var mapZoomTotalRevenueUpdated = function () {
            setTotalRevenueLegend();
        };

        var textAlternativeTotalRevenueClicked = function () {
            var url = me.data.textAlternativeTotalRevenueUrl;
            var bounds = me.map.getBounds();
            var data = {
                bands: 7,
                industryId: me.opts.report.IndustryDetails.Industry.Id,
                boundingEntityId: me.data.currentTotalRevenueBoundingEntityId,
                southWest: bounds.getSouthWest().toString(),
                northEast: bounds.getNorthEast().toString()
            };

            window.open(jQuery.param.querystring(url, data), '_blank');

        };


        var displayReport = function () {

            me.reportContainer.setGauge(me.data.gauge);
            if (me.data.hasData) {
                me.noData.hide();
                me.reportData.show();

                me.reportContainer.setValue(me.data.displayValue);

                setRevenuePerCapitaHeatmap();

                setTotalRevenueHeatmap();


                me.chart = new sizeup.charts.barChart({

                    valueFormat: function (val) { return '$' + sizeup.util.numbers.format.addCommas(Math.floor(val)); },
                    container: me.container.find('.chart .container'),
                    title: 'revenue per capita',
                    bars: me.data.chart
                });
                me.chart.draw();

                me.table = new sizeup.charts.tableChart({
                    container: me.container.find('.table').hide(),
                    rowTemplate: templates.get('tableRow'),
                    rows: me.data.table
                });


                me.description.html(templates.bind(templates.get("description"), me.data.description));

            }
            else {
                me.noData.show();
                me.reportData.hide();
                me.reportContainer.hideGauge();
            }
        };



        var runReport = function (e) {
            new sizeup.core.analytics().dashboardReportLoaded({ report: 'revenuePerCapita' });


            var notifier = new sizeup.core.notifier(function () {
                e.callback();
                displayReport();
            });

            var percentileData = {};
            var chartData = {};
            var percentileNotifier = new sizeup.core.notifier(notifier.getNotifier(function () { percentileDataReturned(percentileData); }));
            var chartNotifier = new sizeup.core.notifier(notifier.getNotifier(function () { chartDataReturned(chartData); }));

            dataLayer.getRevenuePerCapitaChart({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'City' }, chartNotifier.getNotifier(function (data) { chartData.City = data; }));
            dataLayer.getRevenuePerCapitaChart({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'County' }, chartNotifier.getNotifier(function (data) { chartData.County = data; }));
            dataLayer.getRevenuePerCapitaChart({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'Metro' }, chartNotifier.getNotifier(function (data) { chartData.Metro = data; }));
            dataLayer.getRevenuePerCapitaChart({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'State' }, chartNotifier.getNotifier(function (data) { chartData.State = data; }));
            dataLayer.getRevenuePerCapitaChart({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'Nation' }, chartNotifier.getNotifier(function (data) { chartData.Nation = data; }));

            dataLayer.getRevenuePerCapitaPercentile({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'County' }, percentileNotifier.getNotifier(function (data) { percentileData.County = data; }));
            dataLayer.getRevenuePerCapitaPercentile({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'Metro' }, percentileNotifier.getNotifier(function (data) { percentileData.Metro = data; }));
            dataLayer.getRevenuePerCapitaPercentile({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'State' }, percentileNotifier.getNotifier(function (data) { percentileData.State = data; }));
            dataLayer.getRevenuePerCapitaPercentile({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'Nation' }, percentileNotifier.getNotifier(function (data) { percentileData.Nation = data; }));


        };

        var percentileDataReturned = function (data) {
            if (data) {
                me.data.hasData = true;
                

                me.data.percentiles = {
                    County: data.County.Percentile < 1 ? 'less than 1%' : data.County.Percentile > 99 ? 'more than 99%' : 'more than ' + data.County.Percentile + '%',
                    State: data.State.Percentile < 1 ? 'less than 1%' : data.State.Percentile > 99 ? 'more than 99%' : 'more than ' + data.State.Percentile + '%',
                    Nation: data.Nation.Percentile < 1 ? 'less than 1%' : data.Nation.Percentile > 99 ? 'more than 99%' : 'more than ' + data.Nation.Percentile + '%'
                };

                if (data.Metro.Percentile) {
                    me.data.percentiles.Metro = data.Metro.Percentile < 1 ? 'less than 1%' : data.Metro.Percentile > 99 ? 'more than 99%' : 'more than ' + data.Metro.Percentile + '%';
                }

                me.data.gauge = {
                    value: data.Nation.Percentile,
                    tooltip: data.Nation.Percentile < 1 ? '<1st Percentile' : data.Nation.Percentile > 99 ? '>99th Percentile' : sizeup.util.numbers.format.ordinal(data.Nation.Percentile) + ' Percentile'
                };

                me.data.description = {
                    Percentiles: me.data.percentiles
                };

            }
            else {
                me.data.gauge = {
                    value: 0,
                    tooltip: 'No data'
                };
            }
        };

        var chartDataReturned = function (data) {

            me.data.chart = {};
            me.data.table = {};

            var indexes = ['City', 'County', 'Metro', 'State', 'Nation'];
            for (var x = 0; x < indexes.length; x++) {
                if (data[indexes[x]] != null) {
                    me.data.chart[indexes[x]] =
                    {
                        value: data[indexes[x]].Value,
                        label: indexes[x],
                        name: data[indexes[x]].Name,
                        color: '#0af'
                    };

                    me.data.table[indexes[x]] = {
                        name: data[indexes[x]].Name,
                        value: '$' + sizeup.util.numbers.format.addCommas(parseInt(data[indexes[x]].Value))
                    };
                }
            }
            if (data.City != null) {
                me.data.displayValue = data.City.Value;
            }
            else {
                me.data.hasData = false;
            }
        };



        var setupReport = function () {
            me.reportContainer.doGetReport();
        };

        var fadeInPrompt = function (delay, callback) {
            me.reportContainer.fadeInPrompt(delay, callback);
        };

        var collapseReport = function () {
            me.reportContainer.collapseReport();
        };

        var expandReport = function () {
            me.reportContainer.expandReport();
        };


        var publicObj = {

            fadeInPrompt: function (delay, callback) {
                fadeInPrompt(delay, callback);
            },
            setupReport: function () {
                setupReport();
            },
            collapseReport: function () {
                collapseReport();
            },
            expandReport: function () {
                expandReport();
            }
        };
        init();
        return publicObj;

    };
})();