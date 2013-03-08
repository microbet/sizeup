﻿(function () {
    sizeup.core.namespace('sizeup.views.dashboard.costEffectiveness');
    sizeup.views.dashboard.costEffectiveness = function (opts) {

        var me = {};
        var dataLayer = new sizeup.core.data();
        var templates = new sizeup.core.templates(opts.container);
        me.opts = opts;
        me.data = {};
        me.container = opts.container;
        me.data.enteredEmployees = jQuery.bbq.getState().employees;
        me.data.enteredRevenue = jQuery.bbq.getState().revenue;
        me.data.enteredSalary = jQuery.bbq.getState().salary;
        me.data.description = {};
        me.data.hasData = false;

        var init = function () {

            me.reportContainer = new sizeup.views.dashboard.reportContainer(
                {
                    container: me.container,
                    inputValidation: /^[0-9]+$/g,
                    inputCleaning: /[\$\,]/g,
                    events:
                    {
                        runReport: runReport,
                        valueChanged: function () { }
                    },
                    inputFormat: function (val) {
                        var out = "";
                        if (val < 10) {
                            out = "Poor";
                        }
                        else if (val < 40) {
                            out = "Below Average";
                        }
                        else if (val <= 60) {
                            out = "Average";
                        }
                        else if (val <= 90) {
                            out = "Above Average";
                        }
                        else if (val > 90) {
                            out = "Superior";
                        }

                        return out;
                    }
                });

          
            me.sourceButton = new sizeup.controls.toggleButton(
            {
                button: me.container.find('.reportContainer .links .source'),
                onClick: function () { toggleSource(); }
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


            me.sourceContent = me.container.find('.reportContainer .sourceContent').hide();
            me.considerations = me.container.find('.reportContainer .considerations');
            me.resources = me.container.find('.reportContainer .resources');
            me.description = me.container.find('.reportContainer .description');


            $(window).bind('hashchange', function (e) { hashChanged(e); });

            me.noData = me.container.find('.noDataError').hide();
            me.reportData = me.container.find('.reportData');

        };

        var hashChanged = function (e) {
            var employees = e.getState('employees');
            var salary = e.getState('salary');
            var revenue = e.getState('revenue');

            if ((employees != me.data.enteredEmployees ||
                salary != me.data.enteredSalary ||
                revenue != me.data.enteredRevenue) &&
                employees != null &&
                salary != null &&
                revenue != null
                ) {
                me.data.enteredEmployees = employees;
                me.data.enteredSalary = salary;
                me.data.enteredRevenue = revenue;
                setupReport();
            }
            else if (!(employees != null &&
                salary != null &&
                revenue != null)) {
                me.reportContainer.clearReport();
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


        var setHeatmap = function () {

            var overlays = [];

            if (me.opts.report.CurrentPlace.Metro.Id != null) {

                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/costEffectiveness/',
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
                        industryId: me.opts.report.CurrentIndustry.Id
                    },
                    minZoom: 8,
                    maxZoom: 32
                }));

                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/costEffectiveness/',
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
                        industryId: me.opts.report.CurrentIndustry.Id
                    },
                    minZoom: 5,
                    maxZoom: 7
                }));
            }
            else {
                overlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/costEffectiveness/',
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
                        industryId: me.opts.report.CurrentIndustry.Id
                    },
                    minZoom: 5,
                    maxZoom: 32
                }));
            }


            overlays.push(new sizeup.maps.overlay({
                tileUrl: '/tiles/costEffectiveness/',
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
                    industryId: me.opts.report.CurrentIndustry.Id
                },
                minZoom: 0,
                maxZoom: 4
            }));


            me.map = new sizeup.maps.map({
                container: me.container.find('.mapWrapper.container .map')
            });
            me.map.setCenter(new sizeup.maps.latLng({ lat: me.opts.centroid.Lat, lng: me.opts.centroid.Lng }));
            me.map.setZoom(12);
            me.map.addEventListener('zoom_changed', mapZoomUpdated);

            for (var x in overlays) {
                me.map.addOverlay(overlays[x], 0);
            }

            setLegend();

            me.textAlternative = me.container.find('.mapWrapper .textAlternative');
            me.textAlternative.click(textAlternativeClicked);
        };

        var setLegend = function () {

            var data = {
                title: '',
                items: []
            };
            var z = me.map.getZoom();

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
                me.map.setLegend(legend);
            });



            var itemsNotify = notify.getNotifier(function (d) { data.items = d; });

            if (me.opts.report.CurrentPlace.Metro.Id != null) {
                if (z <= 32 && z >= 8) {
                    data.title = 'Cost Effectiveness by county in ' + me.opts.report.CurrentPlace.Metro.Name + ' (Metro)';
                    me.data.currentBoundingEntityId = 'm' + me.opts.report.CurrentPlace.Metro.Id;
                    me.data.textAlternativeUrl = '/accessibility/costEffectiveness/county/';
                    dataLayer.getCostEffectivenessBands({
                        placeId: me.opts.report.CurrentPlace.Id,
                        industryId: me.opts.report.CurrentIndustry.Id,
                        granularity: 'County',
                        boundingGranularity: 'Metro',
                        bands: 7
                    }, itemsNotify);


                }

                if (z <= 7 && z >= 5) {
                    data.title = 'Cost Effectiveness by county in ' + me.opts.report.CurrentPlace.State.Name;
                    me.data.currentBoundingEntityId = 's' + me.opts.report.CurrentPlace.State.Id;
                    me.data.textAlternativeUrl = '/accessibility/costEffectiveness/county/';
                    dataLayer.getCostEffectivenessBands({
                        placeId: me.opts.report.CurrentPlace.Id,
                        industryId: me.opts.report.CurrentIndustry.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        bands: 7
                    }, itemsNotify);
                }
            }
            else {
                if (z <= 32 && z >= 5) {

                    data.title = 'Cost Effectiveness by county in ' + me.opts.report.CurrentPlace.State.Name;
                    me.data.textAlternativeUrl = '/accessibility/costEffectiveness/county/';
                    me.data.currentBoundingEntityId = 's' + me.opts.report.CurrentPlace.State.Id;
                    dataLayer.getCostEffectivenessBands({
                        placeId: me.opts.report.CurrentPlace.Id,
                        industryId: me.opts.report.CurrentIndustry.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        bands: 7
                    }, itemsNotify);

                }
            }

            if (z <= 4 && z >= 0) {

                data.title = 'Cost Effectiveness by state in the USA';
                me.data.textAlternativeUrl = '/accessibility/costEffectiveness/state/';
                me.data.currentBoundingEntityId = null;
                dataLayer.getCostEffectivenessBands({
                    placeId: me.opts.report.CurrentPlace.Id,
                    industryId: me.opts.report.CurrentIndustry.Id,
                    granularity: 'State',
                    bands: 7
                }, itemsNotify);
            }
        };

        var mapZoomUpdated = function () {
            setLegend();
        };

        var textAlternativeClicked = function () {
            var url = me.data.textAlternativeUrl;
            var bounds = me.map.getBounds();
            var data = {
                bands: 7,
                industryId: me.opts.report.CurrentIndustry.Id,
                boundingEntityId: me.data.currentBoundingEntityId,
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

                me.reportContainer.setValue(me.data.gauge.value);

                setHeatmap();

                me.chart = new sizeup.charts.barChart({

                    valueFormat: function (val) { return sizeup.util.numbers.format.sigFig(val, 3); },
                    container: me.container.find('.chart .container'),
                    title: 'cost effectiveness ratio',
                    bars: me.data.chart.bars
                });
                me.chart.draw();
                
                me.table = new sizeup.charts.tableChart({
                    container: me.container.find('.table').hide(),
                    rowTemplate: templates.get('tableRow'),
                    rows: me.data.table
                });

                
                me.data.description = {
                    Percentage: me.data.gauge.tooltip,
                    NAICS6: me.opts.report.CurrentIndustry.NAICS6,
                    Industry: me.opts.report.CurrentIndustry
                };
                
                me.description.html(templates.bind(templates.get("description"), me.data.description));

            }
            else {
                me.noData.show();
                me.reportData.hide();
                me.reportContainer.hideGauge();
            }
        };




        var runReport = function (e) {
            new sizeup.core.analytics().dashboardReportLoaded({ report: 'costEffectiveness' });

            var notifier = new sizeup.core.notifier(function () {
                e.callback();
                displayReport();
            });

            var percentileData = {};
            var chartData = {};
            var percentileNotifier = new sizeup.core.notifier(notifier.getNotifier(function () { percentageDataReturned(percentileData); }));
            var chartNotifier = new sizeup.core.notifier(notifier.getNotifier(function () { chartDataReturned(chartData); }));


            if (me.data.enteredEmployees != null &&
                me.data.enteredSalary != null &&
                me.data.enteredRevenue != null) {
                
                dataLayer.getCostEffectivenessChart({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'County' }, chartNotifier.getNotifier(function (data) { chartData.County = data; }));
                dataLayer.getCostEffectivenessChart({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'Metro' }, chartNotifier.getNotifier(function (data) { chartData.Metro = data; }));
                dataLayer.getCostEffectivenessChart({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'State' }, chartNotifier.getNotifier(function (data) { chartData.State = data; }));
                dataLayer.getCostEffectivenessChart({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'Nation' }, chartNotifier.getNotifier(function (data) { chartData.Nation = data; }));
                dataLayer.getCostEffectivenessPercentage({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, employees: me.data.enteredEmployees, salary: me.data.enteredSalary, revenue: me.data.enteredRevenue, granularity: 'County' }, percentileNotifier.getNotifier(function (data) { percentileData.County = data; }));
            }
        };

        var chartDataReturned = function (data) {

            var ce = me.data.enteredRevenue / (me.data.enteredEmployees * me.data.enteredSalary);

            me.data.chart = {
                bars: {}
            };
            me.data.table = {};
            me.data.chart.bars['me'] =
                {
                    value: ce,
                    label: '',
                    name: 'My Business',
                    color: '#5b0'
                };


            me.data.table['me'] =
                {
                    name: 'My Business',
                    value: sizeup.util.numbers.format.sigFig(ce, 3)
                };


            var indexes = ['County', 'Metro', 'State', 'Nation'];
            for (var x = 0; x < indexes.length; x++) {
                if (data[indexes[x]] != null) {
                    me.data.chart.bars[indexes[x]] =
                    {
                        value: data[indexes[x]].Value,
                        label: indexes[x],
                        name: data[indexes[x]].Name,
                        color: '#0af'
                    };

                    me.data.table[indexes[x]] = {
                        name: data[indexes[x]].Name,
                        value: sizeup.util.numbers.format.sigFig(data[indexes[x]].Value, 3)
                    };
                }
            }
            



        };

        var percentageDataReturned = function (data) {
            if (data) {
                me.data.hasData = true;
                var val = 50 + (data.County.Percentage / 2);
                var percentage = sizeup.util.numbers.format.percentage(Math.abs(data.County.Percentage));

                me.data.gauge = {
                    value: val,
                    tooltip: data.County.Percentage < 0 ? percentage + ' Below Average' : data.County.Percentage == 0 ? 'Average' : percentage + ' Above Average'
                };
            }
            else {
                me.data.gauge = {
                    value: 0,
                    tooltip: 'No data'
                };
            }
        };


        var setupReport = function () {
            if (me.data.enteredEmployees != null &&
                me.data.enteredSalary != null &&
                me.data.enteredRevenue != null) {
                    me.reportContainer.doGetReport();
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



