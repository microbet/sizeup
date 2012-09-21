(function () {
    sizeup.core.namespace('sizeup.views.dashboard.employees');
    sizeup.views.dashboard.employees = function (opts) {

        var me = {};
        var dataLayer = new sizeup.core.data();
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

            me.averageEmployees.noData = me.container.find('.averageEmployees.noDataError').hide();
            me.averageEmployees.reportData = me.container.find('.averageEmployees.reportData');





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

            me.employeesPerCapita.noData = me.container.find('.employeesPerCapita.noDataError').hide();
            me.employeesPerCapita.reportData = me.container.find('.employeesPerCapita.reportData');









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


        var displayReport = function () {

            me.reportContainer.setGauge(me.data.gauge);
            if (me.data.averageEmployees.hasData) {
                me.averageEmployees.noData.hide();
                me.averageEmployees.reportData.show();

                me.averageEmployees.map = new sizeup.maps.heatMap({
                    legendItemTemplate: templates.get('legendItem'),
                    container: me.container.find('.reportContainer .averageEmployees .map'),
                    overlays: [
                       {
                           textAlternativeUrl: '/accessibility/averageEmployees/state/?bands=7',
                           tileUrl: "/tiles/AverageEmployees/state/",
                           legendSource: function (callback) { 
                               return dataLayer.getAverageEmployeesBandsByState({
                                   industryId: me.opts.report.IndustryDetails.Industry.Id,
                                   bands: 7
                               }, callback);
                           },
                           legendTitle: 'Average Employees per business by state in the USA',
                           legendFormat: function (val) { return sizeup.util.numbers.format.abbreviate(val, 0); },
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
                            textAlternativeUrl: '/accessibility/averageEmployees/county/?bands=7',
                            tileUrl: "/tiles/AverageEmployees/county/",
                            legendSource: function (callback) { 
                                return dataLayer.getAverageEmployeesBandsByCounty({
                                    industryId: me.opts.report.IndustryDetails.Industry.Id,
                                    bands: 7,
                                    boundingEntityId: 's' + me.opts.report.CurrentPlace.State.Id
                                }, callback);
                            },
                            legendTitle: 'Average Employees per business by county in ' + me.opts.report.CurrentPlace.State.Name,
                            legendFormat: function (val) { return sizeup.util.numbers.format.abbreviate(val, 0); },
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
                            textAlternativeUrl: '/accessibility/averageEmployees/county/?bands=7',
                            tileUrl: "/tiles/AverageEmployees/county/",
                            legendSource: function (callback) { 
                                return dataLayer.getAverageEmployeesBandsByCounty({
                                    industryId: me.opts.report.IndustryDetails.Industry.Id,
                                    bands: 7,
                                    boundingEntityId: me.opts.report.CurrentPlace.Metro.Id ? 'm' + me.opts.report.CurrentPlace.Metro.Id : 's' + me.opts.report.CurrentPlace.State.Id
                                }, callback);
                            },
                            legendTitle: 'Average Employees per business by county in ' + (me.opts.report.CurrentPlace.Metro.Id ? me.opts.report.CurrentPlace.Metro.Name + ' (Metro)' : me.opts.report.CurrentPlace.State.Name),
                            legendFormat: function (val) { return sizeup.util.numbers.format.abbreviate(val, 0); },
                            industryId: me.opts.report.IndustryDetails.Industry.Id,
                            minZoom: 9,
                            maxZoom: 11,
                            boundingEntityId: me.opts.report.CurrentPlace.Metro.Id ? 'm' + me.opts.report.CurrentPlace.Metro.Id : 's' + me.opts.report.CurrentPlace.State.Id,
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
                            textAlternativeUrl: '/accessibility/averageEmployees/zip/?bands=7',
                            tileUrl: "/tiles/AverageEmployees/zip/",
                            legendSource: function (callback) { 
                                return dataLayer.getAverageEmployeesBandsByZip({
                                    industryId: me.opts.report.IndustryDetails.Industry.Id,
                                    bands: 7,
                                    boundingEntityId: 'co' + me.opts.report.CurrentPlace.County.Id
                                }, callback);
                            },
                            legendTitle: 'Average Employees per business by ZIP code in ' + me.opts.report.CurrentPlace.County.Name + ', ' + me.opts.report.CurrentPlace.State.Abbreviation,
                            legendFormat: function (val) { return sizeup.util.numbers.format.abbreviate(val, 0); },
                            industryId: me.opts.report.IndustryDetails.Industry.Id,
                            minZoom: 12,
                            maxZoom: 32,
                            boundingEntityId: 'co' + me.opts.report.CurrentPlace.County.Id,
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
                me.averageEmployees.map.setCenter(new sizeup.maps.latLng({ lat: me.opts.report.MapCenter.Lat, lng: me.opts.report.MapCenter.Lng }));
                me.averageEmployees.map.setZoom(12);


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


                me.data.averageEmployees.description = {
                    Percentiles: me.data.averageEmployees.percentiles
                };

                me.averageEmployees.description.html(templates.bind(templates.get("averageEmployeesDescription"), me.data.averageEmployees.description));
            }
            else {
                me.averageEmployees.noData.show();
                me.averageEmployees.reportData.hide();
            }

            if (me.data.employeesPerCapita.hasData) {
                me.employeesPerCapita.noData.hide();
                me.employeesPerCapita.reportData.show();

                me.employeesPerCapita.map = new sizeup.maps.heatMap({
                    legendItemTemplate: templates.get('legendItem'),
                    container: me.container.find('.reportContainer .employeesPerCapita .map'),
                    overlays: [
                       {
                           textAlternativeUrl: '/accessibility/EmployeesPerCapita/state/?bands=7',
                           tileUrl: "/tiles/EmployeesPerCapita/state/",
                           legendSource: function (callback) { 
                               return dataLayer.getEmployeesPerCapitaBandsByState({
                                   industryId: me.opts.report.IndustryDetails.Industry.Id,
                                   bands: 7
                               }, callback);
                           },
                           legendTitle: 'Employees Per Capita per business by state in the USA',
                           legendFormat: function (val) { return sizeup.util.numbers.format.sigFig(val, 3); },
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
                            textAlternativeUrl: '/accessibility/EmployeesPerCapita/county/?bands=7',
                            tileUrl: "/tiles/EmployeesPerCapita/county/",
                            legendSource: function (callback) { 
                                return dataLayer.getEmployeesPerCapitaBandsByCounty({
                                    industryId: me.opts.report.IndustryDetails.Industry.Id,
                                    bands: 7,
                                    boundingEntityId: 's' + me.opts.report.CurrentPlace.State.Id
                                }, callback);
                            },
                            legendTitle: 'Employees Per Capita per business by county in ' + me.opts.report.CurrentPlace.State.Name,
                            legendFormat: function (val) { return sizeup.util.numbers.format.sigFig(val, 3); },
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
                            textAlternativeUrl: '/accessibility/EmployeesPerCapita/county/?bands=7',
                            tileUrl: "/tiles/EmployeesPerCapita/county/",
                            legendSource: function (callback) { 
                                return dataLayer.getEmployeesPerCapitaBandsByCounty({
                                    industryId: me.opts.report.IndustryDetails.Industry.Id,
                                    bands: 7,
                                    boundingEntityId: me.opts.report.CurrentPlace.Metro.Id ? 'm' + me.opts.report.CurrentPlace.Metro.Id : 's' + me.opts.report.CurrentPlace.State.Id
                                }, callback);
                            },
                            legendTitle: 'Employees Per Capita per business by county in ' + (me.opts.report.CurrentPlace.Metro.Id ? me.opts.report.CurrentPlace.Metro.Name + ' (Metro)' : me.opts.report.CurrentPlace.State.Name),
                            legendFormat: function (val) { return sizeup.util.numbers.format.sigFig(val, 3); },
                            industryId: me.opts.report.IndustryDetails.Industry.Id,
                            minZoom: 9,
                            maxZoom: 11,
                            boundingEntityId: me.opts.report.CurrentPlace.Metro.Id ? 'm' + me.opts.report.CurrentPlace.Metro.Id : 's' + me.opts.report.CurrentPlace.State.Id,
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
                            textAlternativeUrl: '/accessibility/EmployeesPerCapita/zip/?bands=7',
                            tileUrl: "/tiles/EmployeesPerCapita/zip/",
                            legendSource: function (callback) { 
                                return dataLayer.getEmployeesPerCapitaBandsByZip({
                                    industryId: me.opts.report.IndustryDetails.Industry.Id,
                                    bands: 7,
                                    boundingEntityId: 'co' + me.opts.report.CurrentPlace.County.Id
                                }, callback);
                            },
                            legendTitle: 'Employees Per Capita by ZIP code in ' + me.opts.report.CurrentPlace.County.Name + ', ' + me.opts.report.CurrentPlace.State.Abbreviation,
                            legendFormat: function (val) { return sizeup.util.numbers.format.sigFig(val, 3); },
                            industryId: me.opts.report.IndustryDetails.Industry.Id,
                            minZoom: 12,
                            maxZoom: 32,
                            boundingEntityId: 'co' + me.opts.report.CurrentPlace.County.Id,
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
                me.employeesPerCapita.map.setCenter(new sizeup.maps.latLng({ lat: me.opts.report.MapCenter.Lat, lng: me.opts.report.MapCenter.Lng }));
                me.employeesPerCapita.map.setZoom(12);


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


                me.data.employeesPerCapita.description = {
                    Percentiles: me.data.employeesPerCapita.percentiles
                };

                me.employeesPerCapita.description.html(templates.bind(templates.get("employeesPerCapitaDescription"), me.data.employeesPerCapita.description));
            }
            else {
                me.employeesPerCapita.noData.show();
                me.employeesPerCapita.reportData.hide();
            }
        };


        var runReport = function (e) {
            new sizeup.core.analytics().dashboardReportLoaded({ report: 'employees' });

            var notifier = new sizeup.core.notifier(function () {
                e.callback();
                displayReport();
            });

            me.data.enteredValue = me.reportContainer.getValue();
            jQuery.bbq.pushState({ employees: me.data.enteredValue });

            dataLayer.getAverageEmployeesChart({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id }, notifier.getNotifier(averageEmployeesChartDataReturned));
            dataLayer.getAverageEmployeesPercentile({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id, value: me.data.enteredValue }, notifier.getNotifier(averageEmployeesPercentileDataReturned));

            dataLayer.getEmployeesPerCapitaChart({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id }, notifier.getNotifier(employeesPerCapitaChartDataReturned));
            dataLayer.getEmployeesPerCapitaPercentile({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id }, notifier.getNotifier(employeesPerCapitaPercentileDataReturned));
        };

        var averageEmployeesPercentileDataReturned = function (data) {
            if (data) {
                me.data.averageEmployees.hasData = true;
                me.data.averageEmployees.percentiles = data;

                me.data.gauge = {
                    value: me.data.averageEmployees.percentiles.Nation,
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
            if (data) {
                me.data.employeesPerCapita.hasData = true;
                me.data.employeesPerCapita.percentiles = data;
            }
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


