(function () {
    sizeup.core.namespace('sizeup.views.dashboard.employees');
    sizeup.views.dashboard.employees = function (opts) {

        var me = {};
        var templates = new sizeup.core.templates(opts.container);
        me.opts = opts;
        me.data = {};
        me.container = opts.container;
        me.data.enteredValue = jQuery.bbq.getState().employees;
 
        me.data.averageEmployees = {};
        me.data.employeesPerCapita = {};
        me.averageEmployees = {};
        me.employeesPerCapita = {};


        var init = function () {

            me.reportContainer = new sizeup.views.dashboard.reportContainer(
                {
                    container: me.container,
                    inputValidation: /^[0-9]+$/g,
                    events:
                    {
                        runReport: runReport,
                        valueChanged: function (val) {
                            if (val.value == '') {
                                jQuery.bbq.removeState('employees');
                            }
                        }
                    },
                    inputFormat: function (val) {
                        return val;
                    }
                });

            me.averageEmployees.sourceButton = new sizeup.controls.toggleButton(
                  {
                      button: me.container.find('.reportContainer .averageEmployees .links .source'),
                      onClick: function () { toggleSource('averageEmployees'); }
                  });

            me.averageEmployees.mapToggle = new sizeup.controls.toggleButton(
                {
                    button: me.container.find('.reportContainer .averageEmployees .mapToggle'),
                    onClick: function () { toggleMap('averageEmployees'); }
                });

            me.averageEmployees.chartToggle = new sizeup.controls.toggleButton(
                {
                    button: me.container.find('.reportContainer .averageEmployees .chartToggle'),
                    onClick: function () { toggleChart('averageEmployees'); }
                });

            me.averageEmployees.considerationToggle = new sizeup.controls.toggleButton(
                {
                    button: me.container.find('.reportSidebar.averageEmployees .considerationToggle'),
                    onClick: function () { toggleConsiderations('averageEmployees'); }
                });

            me.averageEmployees.resourcesToggle = new sizeup.controls.toggleButton(
               {
                   button: me.container.find('.reportSidebar.averageEmployees .resourcesToggle'),
                   onClick: function () { toggleResources('averageEmployees'); }
               });

            me.averageEmployees.sourceContent = me.container.find('.reportContainer .averageEmployees .sourceContent').hide();
            me.averageEmployees.considerations = me.container.find('.reportContainer .averageEmployees .considerations');
            me.averageEmployees.resources = me.container.find('.reportContainer .averageEmployees .resources');
            me.averageEmployees.description = me.container.find('.reportContainer .averageEmployees .description');

            me.averageEmployees.question = new sizeup.controls.question({
                answerClicked: function (index) { answerClicked(index); },
                answerCleared: function (index) { answerCleared(index); },
                questionContainer: me.container.find('.reportSidebar.averageEmployees .question'),
                clearingButtons: [me.container.find('.reportSidebar.averageEmployees .clearer')],
                answers: [
                        {
                            question: me.container.find('.reportSidebar.averageEmployees .question .large'),
                            answer: me.container.find('.reportSidebar.averageEmployees .answer.large'),
                            index: 'large'
                        },
                        {
                            question: me.container.find('.reportSidebar.averageEmployees .question .small'),
                            answer: me.container.find('.reportSidebar.averageEmployees .answer.small'),
                            index: 'small'
                        }
                ]
            });

         
            me.averageEmployees.reportData = me.container.find('.averageEmployees.reportData');

            me.averageEmployees.map = new sizeup.maps.map({
                container: me.container.find('.averageEmployees .mapWrapper.container .map')
            });

            me.averageEmployees.textAlternative = me.container.find('.averageEmployees .mapWrapper .textAlternative');
            me.averageEmployees.textAlternative.click(textAlternativeAverageEmployeesClicked);




            me.employeesPerCapita.sourceButton = new sizeup.controls.toggleButton(
                  {
                      button: me.container.find('.reportContainer .employeesPerCapita .links .source'),
                      onClick: function () { toggleSource('employeesPerCapita'); }
                  });

            me.employeesPerCapita.mapToggle = new sizeup.controls.toggleButton(
                {
                    button: me.container.find('.reportContainer .employeesPerCapita .mapToggle'),
                    onClick: function () { toggleMap('employeesPerCapita'); }
                });

            me.employeesPerCapita.chartToggle = new sizeup.controls.toggleButton(
                {
                    button: me.container.find('.reportContainer .employeesPerCapita .chartToggle'),
                    onClick: function () { toggleChart('employeesPerCapita'); }
                });

            me.employeesPerCapita.considerationToggle = new sizeup.controls.toggleButton(
                {
                    button: me.container.find('.reportSidebar.employeesPerCapita .considerationToggle'),
                    onClick: function () { toggleConsiderations('employeesPerCapita'); }
                });


            me.employeesPerCapita.sourceContent = me.container.find('.reportContainer .employeesPerCapita .sourceContent').hide();
            me.employeesPerCapita.considerations = me.container.find('.reportContainer .employeesPerCapita .considerations');
            me.employeesPerCapita.description = me.container.find('.reportContainer .employeesPerCapita .description');

            me.employeesPerCapita.question = new sizeup.controls.question({
                answerClicked: function (index) { answerClicked(index); },
                answerCleared: function (index) { answerCleared(index); },
                questionContainer: me.container.find('.reportSidebar.employeesPerCapita .question'),
                clearingButtons: [me.container.find('.reportSidebar.employeesPerCapita .clearer')],
                answers: [
                        {
                            question: me.container.find('.reportSidebar.employeesPerCapita .question .large'),
                            answer: me.container.find('.reportSidebar.employeesPerCapita .answer.large'),
                            index: 'large'
                        },
                        {
                            question: me.container.find('.reportSidebar.employeesPerCapita .question .small'),
                            answer: me.container.find('.reportSidebar.employeesPerCapita .answer.small'),
                            index: 'small'
                        }
                ]
            });

          
            me.employeesPerCapita.reportData = me.container.find('.employeesPerCapita.reportData');

            me.employeesPerCapita.map = new sizeup.maps.map({
                container: me.container.find('.employeesPerCapita .mapWrapper.container .map')
            });

            me.employeesPerCapita.textAlternative = me.container.find('.employeesPerCapita .mapWrapper .textAlternative');
            me.employeesPerCapita.textAlternative.click(textAlternativeEmployeesPerCapitaClicked);








            var index = jQuery.bbq.getState('businessSize');
            if (index) {
                me.employeesPerCapita.question.showAnswer(index);
            }
            $(window).bind('hashchange', function (e) { hashChanged(e); });

            if (me.data.enteredValue) {
                me.reportContainer.setValue(me.data.enteredValue);
            }

        };

        var toggleMap = function (index) {
            me[index].map.getContainer().toggle("slide", { direction: "up" }, 350);
        };

        var toggleSource = function (index) {
            me[index].sourceContent.slideToggle();
        };

        var toggleConsiderations = function (index) {
            me[index].considerations.toggleClass('collapsed', 1000);
        };

        var toggleResources = function (index) {
            me[index].resources.toggleClass('collapsed', 1000);
        };

        var toggleChart = function (index) {
            if (me[index].chart.getContainer().is(':visible')) {
                me[index].chart.getContainer().toggle("slide", { direction: "up" }, 350, function () {
                    me[index].table.getContainer().toggle("slide", { direction: "up" }, 350);
                });
            }
            else {
                me[index].table.getContainer().toggle("slide", { direction: "up" }, 350, function () {
                    me[index].chart.getContainer().toggle("slide", { direction: "up" }, 350);
                });
            }
        };

        var answerClicked = function (index) {
            jQuery.bbq.pushState({ businessSize: index });
        };

        var answerCleared = function () {
            jQuery.bbq.removeState('businessSize');
        };

        var hashChanged = function (e) {
            var index = e.getState('businessSize');
            me.employeesPerCapita.question.showAnswer(index);
        };


        var setAverageEmployeesHeatmap = function () {
            var overlays = me.averageEmployeesOverlay.getOverlays();
            me.averageEmployees.map.triggerEvent('resize');
            me.averageEmployees.map.setCenter(new sizeup.maps.latLng({ lat: me.opts.centroid.Lat, lng: me.opts.centroid.Lng }));
            me.averageEmployees.map.setZoom(me.averageEmployeesOverlay.getZoomExtent().County + 2);
            me.averageEmployees.map.addEventListener('zoom_changed', mapZoomAverageEmployeesUpdated);
            for (var x in overlays) {
                me.averageEmployees.map.addOverlay(overlays[x], 0);
            }

            setAverageEmployeesLegend();
        };

        var setAverageEmployeesLegend = function () {
            var z = me.averageEmployees.map.getZoom();
            var callback = function (legend) {
                me.averageEmployees.map.setLegend(legend);
            };

            me.averageEmployeesOverlay.getLegend(z, callback);
            me.data.averageEmployeesTextAlternative = me.averageEmployeesOverlay.getParams(z);
        };

        var mapZoomAverageEmployeesUpdated = function () {
            setAverageEmployeesLegend();
        };

        var textAlternativeAverageEmployeesClicked = function () {
            var url = '/accessibility/averageEmployees/';
            window.open(jQuery.param.querystring(url, me.data.averageEmployeesTextAlternative), '_blank');
        };


        var setEmployeesPerCapitaHeatmap = function () {
            var overlays = me.employeesPerCapitaOverlay.getOverlays();
            me.employeesPerCapita.map.triggerEvent('resize');
            me.employeesPerCapita.map.setCenter(new sizeup.maps.latLng({ lat: me.opts.centroid.Lat, lng: me.opts.centroid.Lng }));
            me.employeesPerCapita.map.setZoom(me.employeesPerCapitaOverlay.getZoomExtent().County + 2);
            me.employeesPerCapita.map.addEventListener('zoom_changed', mapZoomEmployeesPerCapitaUpdated);
            for (var x in overlays) {
                me.employeesPerCapita.map.addOverlay(overlays[x], 0);
            }

            setEmployeesPerCapitaLegend();
        };

        var setEmployeesPerCapitaLegend = function () {
            var z = me.employeesPerCapita.map.getZoom();
            var callback = function (legend) {
                me.employeesPerCapita.map.setLegend(legend);
            };

            me.employeesPerCapitaOverlay.getLegend(z, callback);
            me.data.employeesPerCapitaTextAlternative = me.employeesPerCapitaOverlay.getParams(z);
        };

        var mapZoomEmployeesPerCapitaUpdated = function () {
            setEmployeesPerCapitaLegend();
        };

        var textAlternativeEmployeesPerCapitaClicked = function () {
            var url = '/accessibility/employeesPerCapita/';
            window.open(jQuery.param.querystring(url, me.data.employeesPerCapitaTextAlternative), '_blank');
        };






        var displayReport = function () {

            me.reportContainer.setGauge(me.data.gauge);
            me.averageEmployees.reportData.show();


            me.averageEmployees.chart = new sizeup.charts.barChart({

                valueFormat: function (val) { return Math.floor(val); },
                container: me.container.find('.averageEmployees .chart .container'),
                title: 'average employees',
                bars: me.data.averageEmployees.chart.bars,
                marker: me.data.averageEmployees.chart.marker
            });
            me.averageEmployees.chart.draw();

            me.averageEmployees.table = new sizeup.charts.tableChart({
                container: me.container.find('.averageEmployees .table').hide(),
                rowTemplate: templates.get('AverageEmployeesTableRow'),
                rows: me.data.averageEmployees.table
            });


            me.averageEmployees.description.html(templates.bind(templates.get("averageEmployeesDescription"), me.data.averageEmployees.description));
           



            me.employeesPerCapita.reportData.show();

            me.employeesPerCapita.chart = new sizeup.charts.barChart({

                valueFormat: function (val) { return sizeup.util.numbers.format.sigFig(val, 3); },
                container: me.container.find('.employeesPerCapita .chart .container'),
                title: 'employees per capita',
                bars: me.data.employeesPerCapita.chart.bars,
                marker: me.data.employeesPerCapita.chart.marker
            });
            me.employeesPerCapita.chart.draw();

            me.employeesPerCapita.table = new sizeup.charts.tableChart({
                container: me.container.find('.employeesPerCapita .table').hide(),
                rowTemplate: templates.get('employeesPerCapitaTableRow'),
                rows: me.data.employeesPerCapita.table
            });



            me.employeesPerCapita.description.html(templates.bind(templates.get("employeesPerCapitaDescription"), me.data.employeesPerCapita.description));



            sizeup.api.data.getZoomExtent({ id: me.opts.report.CurrentPlace.Id, width: me.employeesPerCapita.map.getWidth() }, function (data) {
                me.employeesPerCapitaOverlay = new sizeup.maps.heatMapOverlays({
                    tileUrl: '/tiles/employeesPerCapita/',
                    place: me.opts.report.CurrentPlace,
                    params: { industryId: me.opts.report.CurrentIndustry.Id },
                    zoomExtent: data,
                    attributeLabel: 'Average Employees',
                    format: function (val) { return sizeup.util.numbers.format.sigFig(val, 3); },
                    legendData: sizeup.api.data.getEmployeesPerCapitaBands,
                    templates: templates
                });
                setEmployeesPerCapitaHeatmap();
            });


            sizeup.api.data.getZoomExtent({ id: me.opts.report.CurrentPlace.Id, width: me.averageEmployees.map.getWidth() }, function (data) {
                me.averageEmployeesOverlay = new sizeup.maps.heatMapOverlays({
                    tileUrl: '/tiles/averageEmployees/',
                    place: me.opts.report.CurrentPlace,
                    params: { industryId: me.opts.report.CurrentIndustry.Id },
                    zoomExtent: data,
                    attributeLabel: 'Average Employees',
                    format: function (val) { return sizeup.util.numbers.format.abbreviate(val); },
                    legendData: sizeup.api.data.getAverageEmployeesBands,
                    templates: templates
                });
                setAverageEmployeesHeatmap();
            });

        };


        var runReport = function (e) {
            new sizeup.core.analytics().dashboardReportLoaded({ report: 'employees' });

            var notifier = new sizeup.core.notifier(function () {
                e.callback();
                displayReport();
            });

            var averageEmployeesPercentileData = {};
            var averageEmployeesChartData = {};
            var employeesPerCapitaPercentileData = {};
            var employeesPerCapitaChartData = {};

            var averageEmployeesPercentileNotifier = new sizeup.core.notifier(notifier.getNotifier(function () { averageEmployeesPercentileDataReturned(averageEmployeesPercentileData); }));
            var averageEmployeesChartNotifier = new sizeup.core.notifier(notifier.getNotifier(function () { averageEmployeesChartDataReturned(averageEmployeesChartData); }));

            var employeesPerCapitaPercentileNotifier = new sizeup.core.notifier(notifier.getNotifier(function () { employeesPerCapitaPercentileDataReturned(employeesPerCapitaPercentileData); }));
            var employeesPerCapitaChartNotifier = new sizeup.core.notifier(notifier.getNotifier(function () { employeesPerCapitaChartDataReturned(employeesPerCapitaChartData); }));


            me.data.enteredValue = me.reportContainer.getValue();
            jQuery.bbq.pushState({ employees: me.data.enteredValue });


            sizeup.api.data.getAverageEmployees({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: sizeup.api.granularity.CITY }, averageEmployeesChartNotifier.getNotifier(function (data) { averageEmployeesChartData.City = data; }));
            sizeup.api.data.getAverageEmployees({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: sizeup.api.granularity.COUNTY }, averageEmployeesChartNotifier.getNotifier(function (data) { averageEmployeesChartData.County = data; }));
            sizeup.api.data.getAverageEmployees({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: sizeup.api.granularity.METRO }, averageEmployeesChartNotifier.getNotifier(function (data) { averageEmployeesChartData.Metro = data; }));
            sizeup.api.data.getAverageEmployees({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: sizeup.api.granularity.STATE }, averageEmployeesChartNotifier.getNotifier(function (data) { averageEmployeesChartData.State = data; }));
            sizeup.api.data.getAverageEmployees({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: sizeup.api.granularity.NATION }, averageEmployeesChartNotifier.getNotifier(function (data) { averageEmployeesChartData.Nation = data; }));

            sizeup.api.data.getEmployeesPerCapita({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: sizeup.api.granularity.CITY }, employeesPerCapitaChartNotifier.getNotifier(function (data) { employeesPerCapitaChartData.City = data; }));
            sizeup.api.data.getEmployeesPerCapita({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: sizeup.api.granularity.COUNTY }, employeesPerCapitaChartNotifier.getNotifier(function (data) { employeesPerCapitaChartData.County = data; }));
            sizeup.api.data.getEmployeesPerCapita({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: sizeup.api.granularity.METRO }, employeesPerCapitaChartNotifier.getNotifier(function (data) { employeesPerCapitaChartData.Metro = data; }));
            sizeup.api.data.getEmployeesPerCapita({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: sizeup.api.granularity.STATE }, employeesPerCapitaChartNotifier.getNotifier(function (data) { employeesPerCapitaChartData.State = data; }));
            sizeup.api.data.getEmployeesPerCapita({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: sizeup.api.granularity.NATION }, employeesPerCapitaChartNotifier.getNotifier(function (data) { employeesPerCapitaChartData.Nation = data; }));


            sizeup.api.data.getAverageEmployeesPercentile({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, value: me.data.enteredValue, granularity: sizeup.api.granularity.CITY }, averageEmployeesPercentileNotifier.getNotifier(function (data) { averageEmployeesPercentileData.City = data; }));
            sizeup.api.data.getAverageEmployeesPercentile({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, value: me.data.enteredValue, granularity: sizeup.api.granularity.COUNTY }, averageEmployeesPercentileNotifier.getNotifier(function (data) { averageEmployeesPercentileData.County = data; }));
            sizeup.api.data.getAverageEmployeesPercentile({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, value: me.data.enteredValue, granularity: sizeup.api.granularity.METRO }, averageEmployeesPercentileNotifier.getNotifier(function (data) { averageEmployeesPercentileData.Metro = data; }));
            sizeup.api.data.getAverageEmployeesPercentile({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, value: me.data.enteredValue, granularity: sizeup.api.granularity.STATE }, averageEmployeesPercentileNotifier.getNotifier(function (data) { averageEmployeesPercentileData.State = data; }));
            sizeup.api.data.getAverageEmployeesPercentile({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, value: me.data.enteredValue, granularity: sizeup.api.granularity.NATION }, averageEmployeesPercentileNotifier.getNotifier(function (data) { averageEmployeesPercentileData.Nation = data; }));


            sizeup.api.data.getEmployeesPerCapitaPercentile({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: sizeup.api.granularity.COUNTY }, employeesPerCapitaPercentileNotifier.getNotifier(function (data) { employeesPerCapitaPercentileData.County = data; }));
            sizeup.api.data.getEmployeesPerCapitaPercentile({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: sizeup.api.granularity.METRO }, employeesPerCapitaPercentileNotifier.getNotifier(function (data) { employeesPerCapitaPercentileData.Metro = data; }));
            sizeup.api.data.getEmployeesPerCapitaPercentile({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: sizeup.api.granularity.STATE }, employeesPerCapitaPercentileNotifier.getNotifier(function (data) { employeesPerCapitaPercentileData.State = data; }));
            sizeup.api.data.getEmployeesPerCapitaPercentile({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: sizeup.api.granularity.NATION }, employeesPerCapitaPercentileNotifier.getNotifier(function (data) { employeesPerCapitaPercentileData.Nation = data; }));

        };

        var averageEmployeesPercentileDataReturned = function (data) {
  
            me.data.averageEmployees.percentiles = {};

            if (data.City) {
                me.data.averageEmployees.percentiles.City = data.City.Percentile < 1 ? 'less than 1%' : data.City.Percentile > 99 ? 'more than 99%' : 'more than ' + data.City.Percentile + '%';
            }
            if (data.County) {
                me.data.averageEmployees.percentiles.County = data.County.Percentile < 1 ? 'less than 1%' : data.County.Percentile > 99 ? 'more than 99%' : 'more than ' + data.County.Percentile + '%';
            }
            if (data.Metro) {
                me.data.averageEmployees.percentiles.Metro = data.Metro.Percentile < 1 ? 'less than 1%' : data.Metro.Percentile > 99 ? 'more than 99%' : 'more than ' + data.Metro.Percentile + '%';
            }
            if (data.State) {
                me.data.averageEmployees.percentiles.State = data.State.Percentile < 1 ? 'less than 1%' : data.State.Percentile > 99 ? 'more than 99%' : 'more than ' + data.State.Percentile + '%';
            }
            if (data.Nation) {
                me.data.averageEmployees.percentiles.Nation = data.Nation.Percentile < 1 ? 'less than 1%' : data.Nation.Percentile > 99 ? 'more than 99%' : 'more than ' + data.Nation.Percentile + '%';
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
            me.data.averageEmployees.description = {
                Percentiles: me.data.averageEmployees.percentiles
            };


        };

        var averageEmployeesChartDataReturned = function (data) {
            me.data.averageEmployees.table = {};
            me.data.averageEmployees.chart = {
                bars: {},
                marker: null
            };
            me.data.averageEmployees.table = {};
            me.data.averageEmployees.chart.bars['me'] =
                {
                    value: me.data.enteredValue,
                    label: '',
                    name: 'My Business',
                    color: '#5b0'
                };

            me.data.averageEmployees.chart.marker =
                {
                    label: sizeup.util.numbers.format.addCommas(data['Nation'].Median),
                    value: data['Nation'].Median,
                    name: 'National Median',
                    color: '#f60'
                };


            me.data.averageEmployees.table['me'] =
                {
                    name: 'My Business',
                    value: sizeup.util.numbers.format.addCommas(me.data.enteredValue)
                };


            var indexes = ['City', 'County', 'Metro', 'State', 'Nation'];
            for (var x = 0; x < indexes.length; x++) {
                if (data[indexes[x]] != null) {
                    me.data.averageEmployees.chart.bars[indexes[x]] =
                    {
                        value: data[indexes[x]].Value,
                        label: indexes[x],
                        name: data[indexes[x]].Name,
                        color: '#0af'
                    };

                    me.data.averageEmployees.table[indexes[x]] = {
                        name: data[indexes[x]].Name,
                        value: sizeup.util.numbers.format.addCommas(parseInt(data[indexes[x]].Value))
                    };
                }
            }
            me.data.averageEmployees.table['median'] =
              {
                  name: 'National Median',
                  value: sizeup.util.numbers.format.addCommas(data['Nation'].Median)
              };
        };




        var employeesPerCapitaPercentileDataReturned = function (data) {

            me.data.employeesPerCapita.percentiles = {};

            if (data.County) {
                me.data.employeesPerCapita.percentiles.County = data.County.Percentile < 1 ? 'less than 1%' : data.County.Percentile > 99 ? 'more than 99%' : 'more than ' + data.County.Percentile + '%';
            }
            if (data.Metro) {
                me.data.employeesPerCapita.percentiles.Metro = data.Metro.Percentile < 1 ? 'less than 1%' : data.Metro.Percentile > 99 ? 'more than 99%' : 'more than ' + data.Metro.Percentile + '%';
            }
            if (data.State) {
                me.data.employeesPerCapita.percentiles.State = data.State.Percentile < 1 ? 'less than 1%' : data.State.Percentile > 99 ? 'more than 99%' : 'more than ' + data.State.Percentile + '%';
            }
            if (data.Nation) {
                me.data.employeesPerCapita.percentiles.Nation = data.Nation.Percentile < 1 ? 'less than 1%' : data.Nation.Percentile > 99 ? 'more than 99%' : 'more than ' + data.Nation.Percentile + '%';
            }

            me.data.employeesPerCapita.description = {
                Percentiles: me.data.employeesPerCapita.percentiles,
                HasData: me.data.employeesPerCapita.percentiles.County ||me.data.employeesPerCapita.percentiles.Metro ||me.data.employeesPerCapita.percentiles.State ||me.data.employeesPerCapita.percentiles.Nation
            };

        };

        var employeesPerCapitaChartDataReturned = function (data) {
            me.data.employeesPerCapita.table = {};
            me.data.employeesPerCapita.chart = {
                bars: {},
                marker: null
            };
            me.data.employeesPerCapita.table = {};

            var indexes = ['City', 'County', 'Metro', 'State', 'Nation'];
            for (var x = 0; x < indexes.length; x++) {
                if (data[indexes[x]] != null) {
                    me.data.employeesPerCapita.chart.bars[indexes[x]] =
                    {
                        value: sizeup.util.numbers.format.sigFig((data[indexes[x]].Value),3),
                        label: indexes[x],
                        name: data[indexes[x]].Name,
                        color: '#0af'
                    };

                    me.data.employeesPerCapita.table[indexes[x]] = {
                        name: data[indexes[x]].Name,
                        value: sizeup.util.numbers.format.sigFig((data[indexes[x]].Value), 3)
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


