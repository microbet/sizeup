(function () {
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
                    inputValidation: /^[0-9\.]+$/g,
                    inputCleaning: /[\$\,]|\.[0-9]*/g,
                    events:
                    {
                        runReport: runReport,
                        valueChanged: function (val) {
                            if (val.value == '') {
                                jQuery.bbq.removeState('revenue');
                            }
                        }
                    },
                    inputFormat: function (val) {                        
                        return '$' + sizeup.util.numbers.format.addCommas(sizeup.util.numbers.format.round(val, 0));
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

            me.question = new sizeup.controls.question({
                answerClicked: function (index) { answerClicked(index); },
                answerCleared: function (index) { answerCleared(index); },
                questionContainer: me.container.find('.reportSidebar .question'),
                clearingButtons: [me.container.find('.reportSidebar .clearer')],
                answers:[
                        {
                            question: me.container.find('.reportSidebar .question .startup'),
                            answer: me.container.find('.reportSidebar .answer.startup'),
                            index: 'startup'
                        },
                        {
                            question: me.container.find('.reportSidebar .question .established'),
                            answer: me.container.find('.reportSidebar .answer.established'),
                            index: 'established'
                        }
                    ]
            });


            me.map = new sizeup.maps.map({
                container: me.container.find('.mapWrapper.container .map')
            });

            me.textAlternative = me.container.find('.mapWrapper .textAlternative');
            me.textAlternative.click(textAlternativeClicked);




            var index = jQuery.bbq.getState('businessType');
            if (index) {
                me.question.showAnswer(index);
            }
            $(window).bind('hashchange', function (e) { hashChanged(e); });

            me.sourceContent = me.container.find('.reportContainer .sourceContent').hide();
            me.considerations = me.container.find('.reportContainer .considerations');
            me.resources = me.container.find('.reportContainer .resources');
            me.description = me.container.find('.reportContainer .description');


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

        var answerClicked = function (index) {
            jQuery.bbq.pushState({ businessType: index });
        };

        var answerCleared = function () {
            jQuery.bbq.removeState('businessType');
        };

        var hashChanged = function (e) {
            var index = e.getState('businessType');
            if (index) {
                me.question.showAnswer(index);
            }
            else {
                me.question.clearAnswer();
            }
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
            var overlays = me.overlay.getOverlays();
            me.map.triggerEvent('resize');
            me.map.setCenter(new sizeup.maps.latLng({ lat: me.opts.centroid.Lat, lng: me.opts.centroid.Lng }));
            me.map.setZoom(me.overlay.getZoomExtent().County + 2);
            me.map.addEventListener('zoom_changed', mapZoomUpdated);
            for (var x in overlays) {
                me.map.addOverlay(overlays[x], 0);
            }

            setLegend();
        };

        var setLegend = function () {
            var z = me.map.getZoom();
            var callback = function (legend) {
                me.map.setLegend(legend);
            };

            me.overlay.getLegend(z, callback);
            me.data.textAlternative = me.overlay.getParams(z);
        };

        var mapZoomUpdated = function () {
            setLegend();
        };

        var textAlternativeClicked = function () {
            var url = '/accessibility/revenue/';
            window.open(jQuery.param.querystring(url, me.data.textAlternative), '_blank');
        };


        var displayReport = function () {

            me.reportContainer.setGauge(me.data.gauge);
            me.reportData.show();

            dataLayer.getZoomExtent({ id: me.opts.report.CurrentPlace.Id, width: me.map.getWidth() }, function (data) {
                me.overlay = new sizeup.maps.heatMapOverlays({
                    tileUrl: '/tiles/averageRevenue/',
                    place: me.opts.report.CurrentPlace,
                    params: { industryId: me.opts.report.CurrentIndustry.Id },
                    zoomExtent: data,
                    attributeLabel: 'Average Business Annual Revenue',
                    format: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); },
                    legendData: dataLayer.getAverageRevenueBands,
                    templates: templates
                });
                setHeatmap();
            });

            me.chart = new sizeup.charts.barChart({

                valueFormat: function (val) { return '$' + sizeup.util.numbers.format.addCommas(Math.floor(val)); },
                container: me.container.find('.chart .container'),
                title: 'average annual revenue per business',
                bars: me.data.chart.bars,
                marker: me.data.chart.marker
            });
            me.chart.draw();

            me.table = new sizeup.charts.tableChart({
                container: me.container.find('.table').hide(),
                rowTemplate: templates.get('tableRow'),
                rows: me.data.table
            });


            me.description.html(templates.bind(templates.get("description"), me.data.description));
        };

        var runReport = function (e) {
            new sizeup.core.analytics().dashboardReportLoaded({ report: 'revenue' });

            var notifier = new sizeup.core.notifier(function () {
                e.callback();
                displayReport();
            });

            var percentileData = {};
            var chartData = {};
            var percentileNotifier = new sizeup.core.notifier(notifier.getNotifier(function(){ percentileDataReturned(percentileData); }));
            var chartNotifier = new sizeup.core.notifier(notifier.getNotifier(function () { chartDataReturned(chartData); }));

            me.data.enteredValue = me.reportContainer.getValue();
            jQuery.bbq.pushState({ revenue: me.data.enteredValue });

            dataLayer.getAverageRevenueChart({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'City' }, chartNotifier.getNotifier(function (data) { chartData.City = data; }));
            dataLayer.getAverageRevenueChart({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'County' }, chartNotifier.getNotifier(function (data) { chartData.County = data; }));
            dataLayer.getAverageRevenueChart({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'Metro' }, chartNotifier.getNotifier(function (data) { chartData.Metro = data; }));
            dataLayer.getAverageRevenueChart({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'State' }, chartNotifier.getNotifier(function (data) { chartData.State = data; }));
            dataLayer.getAverageRevenueChart({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'Nation' }, chartNotifier.getNotifier(function (data) { chartData.Nation = data; }));
            dataLayer.getAverageRevenuePercentile({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, value: me.data.enteredValue, granularity: 'Nation' }, percentileNotifier.getNotifier(function (data) { percentileData.Nation = data; }));
        };

        var percentileDataReturned = function (data) {
            if (data.Nation.Percentile != null) {
                var percentile = sizeup.util.numbers.format.ordinal(data.Nation.Percentile);
                me.data.gauge = {
                    value: data.Nation.Percentile,
                    tooltip: data.Nation.Percentile < 1 ? '<1st Percentile' : data.Nation.Percentile > 99 ? '>99th percentile' : percentile + ' Percentile'
                };

                me.data.description = {
                    Percentile: data.Nation.Percentile < 1 ? 'less revenue than 1%' : data.Nation.Percentile > 99 ? 'more revenue than 99%' : 'more revenue than ' + data.Nation.Percentile + '%'
                };
            }
            else {
                me.data.description = {};
                me.data.gauge = {
                    value: 0,
                    tooltip: 'No data'
                };
            }
        };

        var chartDataReturned = function (data) {
            me.data.chart = {
                bars: {},
                marker: null
            };
            me.data.table = {};
            me.data.chart.bars['me'] =
                {
                    value: me.data.enteredValue,
                    label: '',
                    name: 'My Business',
                    color: '#5b0'
                };

            me.data.chart.marker =
                {
                    label: '$' + sizeup.util.numbers.format.addCommas(data['Nation'].Median),
                    value: data['Nation'].Median,
                    name: 'National Median',
                    color: '#f60'
                };

                
            me.data.table['me'] =
                {
                    name: 'My Business',
                    value: '$' + sizeup.util.numbers.format.addCommas(me.data.enteredValue)
                };

           
            var indexes = ['City', 'County', 'Metro', 'State', 'Nation'];
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
                        value: '$' + sizeup.util.numbers.format.addCommas(parseInt(data[indexes[x]].Value))
                    };
                }
            }
            me.data.table['median'] =
              {
                  name: 'National Median',
                  value: '$' + sizeup.util.numbers.format.addCommas(data['Nation'].Median)
              };
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