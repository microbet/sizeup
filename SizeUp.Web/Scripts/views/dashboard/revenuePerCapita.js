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
                        if (isNaN(val)) {
                            return 'N/A';
                        } else {
                            return '$' + sizeup.util.numbers.format.addCommas(val);
                        }
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


            me.revenuePerCapita = {};
            me.revenuePerCapita.map = new sizeup.maps.map({
                container: me.container.find('.mapWrapper.container.revenuePerCapita  .map')
            });

            me.revenuePerCapita.textAlternative = me.container.find('.mapWrapper.revenuePerCapita  .textAlternative');
            me.revenuePerCapita.textAlternative.click(textAlternativeRevenuePerCapitaClicked);

            me.totalRevenue = {};
            me.totalRevenue.map = new sizeup.maps.map({
                container: me.container.find('.mapWrapper.container.totalRevenue .map')
            });

            me.totalRevenue.textAlternative = me.container.find('.mapWrapper.totalRevenue .textAlternative');
            me.totalRevenue.textAlternative.click(textAlternativeTotalRevenueClicked);



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
            var overlays = me.revenuePerCapitaOverlay.getOverlays();
            me.revenuePerCapita.map.triggerEvent('resize');
            me.revenuePerCapita.map.setCenter(new sizeup.maps.latLng({ lat: me.opts.centroid.Lat, lng: me.opts.centroid.Lng }));
            me.revenuePerCapita.map.setZoom(me.revenuePerCapitaOverlay.getZoomExtent().County + 2);
            me.revenuePerCapita.map.addEventListener('zoom_changed', mapZoomRevenuePerCapitaUpdated);
            for (var x in overlays) {
                me.revenuePerCapita.map.addOverlay(overlays[x], 0);
            }

            setRevenuePerCapitaLegend();
        };

        var setRevenuePerCapitaLegend = function () {
            var z = me.revenuePerCapita.map.getZoom();
            var callback = function (legend) {
                me.revenuePerCapita.map.setLegend(legend);
            };

            me.revenuePerCapitaOverlay.getLegend(z, callback);
            me.data.revenuePerCapitaTextAlternative = me.revenuePerCapitaOverlay.getParams(z);
        };

        var mapZoomRevenuePerCapitaUpdated = function () {
            setRevenuePerCapitaLegend();
        };

        var textAlternativeRevenuePerCapitaClicked = function () {
            var url = '/accessibility/revenuePerCapita/';
            window.open(jQuery.param.querystring(url, me.data.revenuePerCapitaTextAlternative), '_blank');
        };


        var setTotalRevenueHeatmap = function () {
            var overlays = me.totalRevenueOverlay.getOverlays();
            me.totalRevenue.map.triggerEvent('resize');
            me.totalRevenue.map.setCenter(new sizeup.maps.latLng({ lat: me.opts.centroid.Lat, lng: me.opts.centroid.Lng }));
            me.totalRevenue.map.setZoom(me.totalRevenueOverlay.getZoomExtent().County + 2);
            me.totalRevenue.map.addEventListener('zoom_changed', mapZoomTotalRevenueUpdated);
            for (var x in overlays) {
                me.totalRevenue.map.addOverlay(overlays[x], 0);
            }

            setTotalRevenueLegend();
        };

        var setTotalRevenueLegend = function () {
            var z = me.totalRevenue.map.getZoom();
            var callback = function (legend) {
                me.totalRevenue.map.setLegend(legend);
            };

            me.totalRevenueOverlay.getLegend(z, callback);
            me.data.totalRevenueTextAlternative = me.totalRevenueOverlay.getParams(z);
        };

        var mapZoomTotalRevenueUpdated = function () {
            setTotalRevenueLegend();
        };

        var textAlternativeTotalRevenueClicked = function () {
            var url = '/accessibility/totalRevenue/';
            window.open(jQuery.param.querystring(url, me.data.totalRevenueTextAlternative), '_blank');

        };


        var displayReport = function () {

            me.reportContainer.setGauge(me.data.gauge);
    
            me.reportData.show();

            me.reportContainer.setValue(me.data.displayValue);
            if (isNaN(me.data.displayValue)) {
                me.reportContainer.hideGauge();
            }

            dataLayer.getZoomExtent({ id: me.opts.report.CurrentPlace.Id, width: me.revenuePerCapita.map.getWidth() }, function (data) {
                me.revenuePerCapitaOverlay = new sizeup.maps.heatMapOverlays({
                    tileUrl: '/tiles/revenuePerCapita/',
                    place: me.opts.report.CurrentPlace,
                    params: { industryId: me.opts.report.CurrentIndustry.Id },
                    zoomExtent: data,
                    attributeLabel: 'Revenue Per Capita',
                    format: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); },
                    legendData: dataLayer.getRevenuePerCapitaBands,
                    templates: templates
                });
                setRevenuePerCapitaHeatmap();
            });

            dataLayer.getZoomExtent({ id: me.opts.report.CurrentPlace.Id, width: me.totalRevenue.map.getWidth() }, function (data) {
                me.totalRevenueOverlay = new sizeup.maps.heatMapOverlays({
                    tileUrl: '/tiles/totalRevenue/',
                    place: me.opts.report.CurrentPlace,
                    params: { industryId: me.opts.report.CurrentIndustry.Id },
                    zoomExtent: data,
                    attributeLabel: 'Total Revenue',
                    format: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); },
                    legendData: dataLayer.getTotalRevenueBands,
                    templates: templates
                });
                setTotalRevenueHeatmap();
            });

           


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


            me.data.description.HasData = me.data.chart.City != null;           
            me.description.html(templates.bind(templates.get("description"), me.data.description));

          
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

            dataLayer.getRevenuePerCapitaChart({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'City' }, chartNotifier.getNotifier(function (data) { chartData.City = data; }));
            dataLayer.getRevenuePerCapitaChart({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'County' }, chartNotifier.getNotifier(function (data) { chartData.County = data; }));
            dataLayer.getRevenuePerCapitaChart({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'Metro' }, chartNotifier.getNotifier(function (data) { chartData.Metro = data; }));
            dataLayer.getRevenuePerCapitaChart({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'State' }, chartNotifier.getNotifier(function (data) { chartData.State = data; }));
            dataLayer.getRevenuePerCapitaChart({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'Nation' }, chartNotifier.getNotifier(function (data) { chartData.Nation = data; }));

            dataLayer.getRevenuePerCapitaPercentile({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'County' }, percentileNotifier.getNotifier(function (data) { percentileData.County = data; }));
            dataLayer.getRevenuePerCapitaPercentile({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'Metro' }, percentileNotifier.getNotifier(function (data) { percentileData.Metro = data; }));
            dataLayer.getRevenuePerCapitaPercentile({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'State' }, percentileNotifier.getNotifier(function (data) { percentileData.State = data; }));
            dataLayer.getRevenuePerCapitaPercentile({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'Nation' }, percentileNotifier.getNotifier(function (data) { percentileData.Nation = data; }));


        };

        var percentileDataReturned = function (data) {
           
            me.data.percentiles = {};

            if (data.County) {
                me.data.percentiles.County = data.County.Percentile < 1 ? 'less than 1%' : data.County.Percentile > 99 ? 'more than 99%' : 'more than ' + data.County.Percentile + '%';
            }
            if (data.Metro) {
                me.data.percentiles.Metro = data.Metro.Percentile < 1 ? 'less than 1%' : data.Metro.Percentile > 99 ? 'more than 99%' : 'more than ' + data.Metro.Percentile + '%';
            }
            if (data.State) {
                me.data.percentiles.State = data.State.Percentile < 1 ? 'less than 1%' : data.State.Percentile > 99 ? 'more than 99%' : 'more than ' + data.State.Percentile + '%';
            }
            if (data.Nation) {
                me.data.percentiles.Nation = data.Nation.Percentile < 1 ? 'less than 1%' : data.Nation.Percentile > 99 ? 'more than 99%' : 'more than ' + data.Nation.Percentile + '%';
                me.data.gauge = {
                    value: data.Nation.Percentile,
                    tooltip: data.Nation.Percentile < 1 ? '<1st Percentile' : data.Nation.Percentile > 99 ? '>99th Percentile' : sizeup.util.numbers.format.ordinal(data.Nation.Percentile) + ' Percentile'
                };
            }
            else{
                me.data.gauge = {
                    value: 0,
                    tooltip: 'No data'
                };
            }

            me.data.description = {
                Percentiles: me.data.percentiles
            };
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
                me.data.displayValue = 'No Data';
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