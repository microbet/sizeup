(function () {
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


        var displayReport = function () {

            me.reportContainer.setGauge(me.data.gauge);
            if (me.data.hasData) {
                me.noData.hide();
                me.reportData.show();

                me.revenuePerCapitaMap = new sizeup.maps.heatMap({
                    legendItemTemplate: templates.get('legendItem'),
                    container: me.container.find('.reportContainer .map.revenuePerCapita'),
                    overlays: [
                        {
                            tileUrl: "/tiles/revenuePerCapita/state/",
                            legendSource: function (callback) {
                                dataLayer.getRevenuePerCapitaBandsByState({
                                    industryId: me.opts.report.IndustryDetails.Industry.Id,
                                    bands: 7
                                }, callback);
                            },
                            legendTitle: 'Revenue Per Capita by state in the USA',
                            legendFormat: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); },
                            industryId: me.opts.report.IndustryDetails.Industry.Id,
                            minZoom: 0,
                            maxZoom: 4,
                            colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                            ]
                        },
                        {
                            tileUrl: "/tiles/revenuePerCapita/county/",
                            legendSource: function (callback) {
                                dataLayer.getRevenuePerCapitaBandsByCounty({
                                    industryId: me.opts.report.IndustryDetails.Industry.Id,
                                    bands: 7,
                                    boundingEntityId: 's' + me.opts.report.CurrentPlace.State.Id
                                }, callback);
                            },
                            legendTitle: 'Revenue Per Capita by county in ' + me.opts.report.CurrentPlace.State.Name,
                            legendFormat: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); },
                            industryId: me.opts.report.IndustryDetails.Industry.Id,
                            minZoom: 5,
                            maxZoom: 8,
                            boundingEntityId: 's' + me.opts.report.CurrentPlace.State.Id,
                            colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                            ]
                        },
                        {
                            tileUrl: "/tiles/revenuePerCapita/county/",
                            legendSource: function (callback) {
                                dataLayer.getRevenuePerCapitaBandsByCounty({
                                    industryId: me.opts.report.IndustryDetails.Industry.Id,
                                    bands: 7,
                                    boundingEntityId: me.opts.report.CurrentPlace.Metro ? 'm' + me.opts.report.CurrentPlace.Metro.Id : 's' + me.opts.report.CurrentPlace.State.Id
                                }, callback);
                            },
                            legendTitle: 'Revenue Per Capita by county in ' + (me.opts.report.CurrentPlace.Metro ? me.opts.report.CurrentPlace.Metro.Name + ' (Metro)' : me.opts.report.CurrentPlace.State.Name),
                            legendFormat: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); },
                            industryId: me.opts.report.IndustryDetails.Industry.Id,
                            minZoom: 9,
                            maxZoom: 32,
                            boundingEntityId: me.opts.report.CurrentPlace.Metro ? 'm' + me.opts.report.CurrentPlace.Metro.Id : 's' + me.opts.report.CurrentPlace.State.Id,
                            colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                            ]
                        }
                    ]
                });
                me.revenuePerCapitaMap.setCenter(new sizeup.maps.latLng({ lat: me.opts.report.MapCenter.Lat, lng: me.opts.report.MapCenter.Lng }));


                me.totalRevenueMap = new sizeup.maps.heatMap({
                    legendItemTemplate: templates.get('legendItem'),
                    container: me.container.find('.reportContainer .map.totalRevenue'),
                    overlays: [
                        {
                            tileUrl: "/tiles/totalRevenue/state/",
                            legendSource: function (callback) {
                                dataLayer.getTotalRevenueBandsByState({
                                    industryId: me.opts.report.IndustryDetails.Industry.Id,
                                    bands: 7
                                }, callback);
                            },
                            legendTitle: 'Total Revenue by state in the USA',
                            legendFormat: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); },
                            industryId: me.opts.report.IndustryDetails.Industry.Id,
                            minZoom: 0,
                            maxZoom: 4,
                            colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                            ]
                        },
                        {
                            tileUrl: "/tiles/totalRevenue/county/",
                            legendSource: function (callback) {
                                dataLayer.getTotalRevenueBandsByCounty({
                                    industryId: me.opts.report.IndustryDetails.Industry.Id,
                                    bands: 7,
                                    boundingEntityId: 's' + me.opts.report.CurrentPlace.State.Id
                                }, callback);
                            },
                            legendTitle: 'Total Revenue by county in ' + me.opts.report.CurrentPlace.State.Name,
                            legendFormat: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); },
                            industryId: me.opts.report.IndustryDetails.Industry.Id,
                            minZoom: 5,
                            maxZoom: 8,
                            boundingEntityId: 's' + me.opts.report.CurrentPlace.State.Id,
                            colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                            ]
                        },
                        {
                            tileUrl: "/tiles/totalRevenue/county/",
                            legendSource: function (callback) {
                                dataLayer.getTotalRevenueBandsByCounty({
                                    industryId: me.opts.report.IndustryDetails.Industry.Id,
                                    bands: 7,
                                    boundingEntityId: me.opts.report.CurrentPlace.Metro ? 'm' + me.opts.report.CurrentPlace.Metro.Id : 's' + me.opts.report.CurrentPlace.State.Id
                                }, callback);
                            },
                            legendTitle: 'Total Revenue by county in ' + (me.opts.report.CurrentPlace.Metro ? me.opts.report.CurrentPlace.Metro.Name + ' (Metro)' : me.opts.report.CurrentPlace.State.Name),
                            legendFormat: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); },
                            industryId: me.opts.report.IndustryDetails.Industry.Id,
                            minZoom: 9,
                            maxZoom: 32,
                            boundingEntityId: me.opts.report.CurrentPlace.Metro ? 'm' + me.opts.report.CurrentPlace.Metro.Id : 's' + me.opts.report.CurrentPlace.State.Id,
                            colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                            ]
                        }
                    ]
                });
                me.totalRevenueMap.setCenter(new sizeup.maps.latLng({ lat: me.opts.report.MapCenter.Lat, lng: me.opts.report.MapCenter.Lng }));


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


                me.data.description = {
                    Percentiles: me.data.percentiles,
                    NAICS6: me.opts.report.IndustryDetails.NAICS6,
                    Salary: me.data.table['County'].value
                };

                me.description.html(templates.bind(templates.get("description"), me.data.description));

            }
            else {
                me.noData.show();
                me.reportData.hide();
            }
        };



        var runReport = function (e) {
            var notifier = new sizeup.core.notifier(function () {
                e.callback();
                displayReport();
            });

            dataLayer.getRevenuePerCapitaChart({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id }, notifier.getNotifier(chartDataReturned));
            dataLayer.getRevenuePerCapitaPercentile({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id }, notifier.getNotifier(percentileDataReturned));
        };

        var percentileDataReturned = function (data) {
            if (data) {
                me.data.hasData = true;
                me.data.percentiles = data;

                me.data.gauge = {
                    value: me.data.percentiles.Nation,
                    tooltip: sizeup.util.numbers.format.ordinal(data.Nation) + ' Percentile'
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