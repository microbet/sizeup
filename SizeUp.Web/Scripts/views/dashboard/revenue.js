﻿(function () {
    sizeup.core.namespace('sizeup.views.dashboard.revenue');
    sizeup.views.dashboard.revenue = function (opts) {

        var me = {};
        var dataLayer = new sizeup.core.data();
        var templates = new sizeup.core.templates(opts.container);
        me.opts = opts;
        me.data = {};
        me.container = opts.container;
        me.data.enteredValue = jQuery.bbq.getState().revenue;
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

            me.resourceToggle = new sizeup.controls.toggleButton(
                {
                    button: me.container.find('.reportSidebar .resourcesToggle'),
                    onClick: function () { toggleResources(); }
                });



            me.sourceContent = me.container.find('.reportContainer .sourceContent').hide();
            me.considerations = me.container.find('.reportContainer .considerations');
            me.resources = me.container.find('.reportContainer .resources');
            me.description = me.container.find('.reportContainer .description');



            me.noData = me.container.find('.noDataError').hide();
            me.reportData = me.container.find('.reportData');
            if (me.data.enteredValue) {
                me.reportContainer.setValue(me.data.enteredValue);
            }
        };


        var toggleMap = function () {
            me.map.getContainer().toggle("slide", { direction: "up" }, 350);
        };

        var toggleSource = function () {
            me.sourceContent.slideToggle();
        };

        var toggleConsiderations = function () {
            me.considerations.toggleClass('collapsed', 1000);
        };

        var toggleResources = function () {
            me.resources.toggleClass('collapsed', 1000);
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

                me.map = new sizeup.maps.heatMap({
                    legendItemTemplate: templates.get('legendItem'),
                    container: me.container.find('.reportContainer .map'),
                    overlays: [
                        {
                            tileUrl: "/tiles/revenue/state/",
                            legendSource: function (callback) {
                                dataLayer.geRevenueBandsByState({
                                    industryId: me.opts.report.IndustryDetails.Industry.Id,
                                    bands: 7
                                }, callback);
                            },
                            legendTitle: 'Average Business Annual Revenue by state in the USA',
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
                            tileUrl: "/tiles/revenue/county/",
                            legendSource: function (callback) {
                                dataLayer.getRevenueBandsByCounty({
                                    industryId: me.opts.report.IndustryDetails.Industry.Id,
                                    bands: 7,
                                    boundingEntityId: 's' + me.opts.report.Locations.State.Id
                                }, callback);
                            },
                            legendTitle: 'Average Business Annual Revenue by county in ' + me.opts.report.Locations.State.Name,
                            legendFormat: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); },
                            industryId: me.opts.report.IndustryDetails.Industry.Id,
                            minZoom: 5,
                            maxZoom: 8,
                            boundingEntityId: 's' + me.opts.report.Locations.State.Id,
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
                            tileUrl: "/tiles/revenue/county/",
                            legendSource: function (callback) {
                                dataLayer.getRevenueBandsByCounty({
                                    industryId: me.opts.report.IndustryDetails.Industry.Id,
                                    bands: 7,
                                    boundingEntityId: me.opts.report.Locations.Metro ? 'm' + me.opts.report.Locations.Metro.Id : 's' + me.opts.report.Locations.State.Id
                                }, callback);
                            },
                            legendTitle: 'Average Business Annual Revenue by county in ' + (me.opts.report.Locations.Metro ? me.opts.report.Locations.Metro.Name + ' (Metro)' : me.opts.report.Locations.State.Name),
                            legendFormat: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); },
                            industryId: me.opts.report.IndustryDetails.Industry.Id,
                            minZoom: 9,
                            maxZoom: 32,
                            boundingEntityId: me.opts.report.Locations.Metro ? 'm' + me.opts.report.Locations.Metro.Id : 's' + me.opts.report.Locations.State.Id,
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
                            tileUrl: "/tiles/revenue/zip/",
                            legendSource: function (callback) {
                                dataLayer.getRevenueBandsByZip({
                                    industryId: me.opts.report.IndustryDetails.Industry.Id,
                                    bands: 7,
                                    boundingEntityId: 'co' + me.opts.report.Locations.County.Id
                                }, callback);
                            },
                            legendTitle: 'Average Business Annual Revenue by ZIP code in ' +  me.opts.report.Locations.County.Name + ', ' + me.opts.report.Locations.State.Abbreviation,
                            legendFormat: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); },
                            industryId: me.opts.report.IndustryDetails.Industry.Id,
                            minZoom: 9,
                            maxZoom: 32,
                            boundingEntityId: 'co' + me.opts.report.Locations.County.Id,
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
                me.map.setCenter(new sizeup.maps.latLng({ lat: me.opts.report.MapCenter.Lat, lng: me.opts.report.MapCenter.Lng }));
                me.chart = new sizeup.charts.barChart({

                    valueFormat: function (val) { return '$' + sizeup.util.numbers.format.addCommas(Math.floor(val)); },
                    container: me.container.find('.chart .container'),
                    title: 'average annual revenue per business',
                    bars: me.data.chart
                });
                me.chart.draw();

                me.table = new sizeup.charts.tableChart({
                    container: me.container.find('.table').hide(),
                    rowTemplate: templates.get('tableRow'),
                    rows: me.data.table
                });


                me.data.description = {
                    Percentage: me.data.gauge.tooltip,
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

            me.data.enteredValue = me.reportContainer.getValue();
            jQuery.bbq.pushState({ revenue: me.data.enteredValue });
            dataLayer.getRevenueChart({ industryId: me.opts.report.IndustryDetails.Industry.Id, cityId: me.opts.report.Locations.City.Id, countyId: me.opts.report.Locations.County.Id }, notifier.getNotifier(chartDataReturned));
            dataLayer.getRevenuePercentile({ industryId: me.opts.report.IndustryDetails.Industry.Id, value: me.data.enteredValue }, notifier.getNotifier(percentileDataReturned));
        };

        var percentileDataReturned = function (data) {
            if (data) {
                me.data.hasData = true;
                var percentile = sizeup.util.numbers.format.ordinal(data.Percentile);
                me.data.gauge = {
                    value: data.Percentile,
                    tooltip: data.Percentile < 1 ? +'<1st Percentile' : percentile + ' Percentile'
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
            me.data.chart['me'] =
                {
                    value: me.data.enteredValue,
                    label: '',
                    name: 'My Business',
                    color: '#5b0'
                };


            me.data.table['me'] =
                {
                    name: 'My Business',
                    value: '$' + sizeup.util.numbers.format.addCommas(me.data.enteredValue)
                };


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
            if (me.data.enteredValue) {
                me.reportContainer.doSubmit();
            }
            else {
                fadeInPrompt(0);
            }
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