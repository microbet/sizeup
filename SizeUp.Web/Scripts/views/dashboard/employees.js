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


        var setAverageEmployeesHeatmap = function () {

            var overlays = [];

            overlays.push(new sizeup.maps.overlay({
                tileUrl: '/tiles/averageEmployees/',
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
                    tileUrl: '/tiles/averageEmployees/',
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
                    tileUrl: '/tiles/averageEmployees/',
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
                    tileUrl: '/tiles/averageEmployees/',
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
                tileUrl: '/tiles/averageEmployees/',
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


            me.averageEmployees.map = new sizeup.maps.map({
                container: me.container.find('.averageEmployees .mapWrapper.container .map')
            });
            me.averageEmployees.map.setCenter(new sizeup.maps.latLng({ lat: me.opts.report.MapCenter.Lat, lng: me.opts.report.MapCenter.Lng }));
            me.averageEmployees.map.setZoom(12);
            me.averageEmployees.map.addEventListener('zoom_changed', mapZoomAverageEmployeesUpdated);

            for (var x in overlays) {
                me.averageEmployees.map.addOverlay(overlays[x], 0);
            }

            setAverageEmployeesLegend();

            me.averageEmployees.textAlternative = me.container.find('.averageEmployees .mapWrapper .textAlternative');
            me.averageEmployees.textAlternative.click(textAlternativeAverageEmployeesClicked);
        };

        var setAverageEmployeesLegend = function () {

            var data = {
                title: '',
                items: []
            };
            var z = me.averageEmployees.map.getZoom();

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
                me.averageEmployees.map.setLegend(legend);
            });



            var itemsNotify = notify.getNotifier(function (d) { data.items = d; });


            if (z <= 32 && z >= 11) {
                data.title = 'Average Employees per business by ZIP code in ' + me.opts.report.CurrentPlace.County.Name + ', ' + me.opts.report.CurrentPlace.State.Abbreviation;
                me.data.averageEmployees.currentBoundingEntityId = 'co' + me.opts.report.CurrentPlace.County.Id;
                me.data.averageEmployees.textAlternativeUrl = '/accessibility/averageEmployees/zip/';
                dataLayer.getAverageEmployeesBands({
                    placeId: me.opts.report.CurrentPlace.Id,
                    industryId: me.opts.report.IndustryDetails.Industry.Id,
                    granularity: 'ZipCode',
                    boundingGranularity: 'County',
                    bands: 7
                }, itemsNotify);
            }

            if (me.opts.report.CurrentPlace.Metro.Id != null) {
                if (z <= 10 && z >= 8) {
                    data.title = 'Average Employees per business by county in ' + me.opts.report.CurrentPlace.Metro.Name + ' (Metro)';
                    me.data.averageEmployees.currentBoundingEntityId = 'm' + me.opts.report.CurrentPlace.Metro.Id;
                    me.data.averageEmployees.textAlternativeUrl = '/accessibility/averageEmployees/county/';
                    dataLayer.getAverageEmployeesBands({
                        placeId: me.opts.report.CurrentPlace.Id,
                        industryId: me.opts.report.IndustryDetails.Industry.Id,
                        granularity: 'County',
                        boundingGranularity:'Metro',
                        bands: 7
                    }, itemsNotify);


                }

                if (z <= 7 && z >= 5) {
                    data.title = 'Average Employees per business by county in ' + me.opts.report.CurrentPlace.State.Name;
                    me.data.averageEmployees.currentBoundingEntityId = 's' + me.opts.report.CurrentPlace.State.Id;
                    me.data.averageEmployees.textAlternativeUrl = '/accessibility/averageEmployees/county/';
                    dataLayer.getAverageEmployeesBands({
                        placeId: me.opts.report.CurrentPlace.Id,
                        industryId: me.opts.report.IndustryDetails.Industry.Id,
                        granularity: 'County',
                        boundingGranularity:'State',
                        bands: 7
                    }, itemsNotify);
                }
            }
            else {
                if (z <= 10 && z >= 5) {

                    data.title = 'Average Employees per business by county in ' + me.opts.report.CurrentPlace.State.Name;
                    me.data.averageEmployees.textAlternativeUrl = '/accessibility/averageEmployees/county/';
                    me.data.averageEmployees.currentBoundingEntityId = 's' + me.opts.report.CurrentPlace.State.Id;
                    dataLayer.getAverageEmployeesBands({
                        placeId: me.opts.report.CurrentPlace.Id,
                        industryId: me.opts.report.IndustryDetails.Industry.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        bands: 7
                    }, itemsNotify);

                }
            }

            if (z <= 4 && z >= 0) {

                data.title = 'Average Employees per business by state in the USA';
                me.data.averageEmployees.textAlternativeUrl = '/accessibility/averageEmployees/state/';
                me.data.averageEmployees.currentBoundingEntityId = null;
                dataLayer.getAverageEmployeesBands({
                    placeId: me.opts.report.CurrentPlace.Id,
                    industryId: me.opts.report.IndustryDetails.Industry.Id,
                    granularity: 'State',
                    bands: 7
                }, itemsNotify);
            }
        };

        var mapZoomAverageEmployeesUpdated = function () {
            setAverageEmployeesLegend();
        };

        var textAlternativeAverageEmployeesClicked = function () {
            var url = me.data.averageEmployees.textAlternativeUrl;
            var bounds = me.averageEmployees.map.getBounds();
            var data = {
                bands: 7,
                industryId: me.opts.report.IndustryDetails.Industry.Id,
                boundingEntityId: me.data.averageEmployees.currentBoundingEntityId,
                southWest: bounds.getSouthWest().toString(),
                northEast: bounds.getNorthEast().toString()
            };

            window.open(jQuery.param.querystring(url, data), '_blank');

        };




        var setEmployeesPerCapitaHeatmap = function () {

            var overlays = [];

            overlays.push(new sizeup.maps.overlay({
                tileUrl: '/tiles/EmployeesPerCapita/',
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
                    tileUrl: '/tiles/EmployeesPerCapita/',
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
                    tileUrl: '/tiles/EmployeesPerCapita/',
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
                    tileUrl: '/tiles/EmployeesPerCapita/',
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
                tileUrl: '/tiles/EmployeesPerCapita/',
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


            me.employeesPerCapita.map = new sizeup.maps.map({
                container: me.container.find('.employeesPerCapita .mapWrapper.container .map')
            });
            me.employeesPerCapita.map.setCenter(new sizeup.maps.latLng({ lat: me.opts.report.MapCenter.Lat, lng: me.opts.report.MapCenter.Lng }));
            me.employeesPerCapita.map.setZoom(12);
            me.employeesPerCapita.map.addEventListener('zoom_changed', mapZoomEmployeesPerCapitaUpdated);

            for (var x in overlays) {
                me.employeesPerCapita.map.addOverlay(overlays[x], 0);
            }

            setEmployeesPerCapitaLegend();

            me.employeesPerCapita.textAlternative = me.container.find('.averageEmployees .mapWrapper .textAlternative');
            me.employeesPerCapita.textAlternative.click(textAlternativeEmployeesPerCapitaClicked);
        };

        var setEmployeesPerCapitaLegend = function () {

            var data = {
                title: '',
                items: []
            };
            var z = me.employeesPerCapita.map.getZoom();

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
                    format: function (val) { return sizeup.util.numbers.format.sigFig(val, 3); }
                });
                me.employeesPerCapita.map.setLegend(legend);
            });



            var itemsNotify = notify.getNotifier(function (d) { data.items = d; });


            if (z <= 32 && z >= 11) {
                data.title = 'Employees Per Capita by ZIP code in ' + me.opts.report.CurrentPlace.County.Name + ', ' + me.opts.report.CurrentPlace.State.Abbreviation;
                me.data.employeesPerCapita.currentBoundingEntityId = 'co' + me.opts.report.CurrentPlace.County.Id;
                me.data.employeesPerCapita.textAlternativeUrl = '/accessibility/employeesPerCapita/zip/';
                dataLayer.getEmployeesPerCapitaBands({
                    placeId: me.opts.report.CurrentPlace.Id,
                    industryId: me.opts.report.IndustryDetails.Industry.Id,
                    granularity: 'ZipCode',
                    boundingGranularity: 'County',
                    bands: 7
                }, itemsNotify);
            }

            if (me.opts.report.CurrentPlace.Metro.Id != null) {
                if (z <= 10 && z >= 8) {
                    data.title = 'Employees Per Capita by county in ' + me.opts.report.CurrentPlace.Metro.Name + ' (Metro)';
                    me.data.employeesPerCapita.currentBoundingEntityId = 'm' + me.opts.report.CurrentPlace.Metro.Id;
                    me.data.employeesPerCapita.textAlternativeUrl = '/accessibility/employeesPerCapita/county/';
                    dataLayer.getEmployeesPerCapitaBands({
                        placeId: me.opts.report.CurrentPlace.Id,
                        industryId: me.opts.report.IndustryDetails.Industry.Id,
                        granularity: 'County',
                        boundingGranularity: 'Metro',
                        bands: 7
                    }, itemsNotify);


                }

                if (z <= 7 && z >= 5) {
                    data.title = 'Employees Per Capita by county in ' + me.opts.report.CurrentPlace.State.Name;
                    me.data.employeesPerCapita.currentBoundingEntityId = 's' + me.opts.report.CurrentPlace.State.Id;
                    me.data.employeesPerCapita.textAlternativeUrl = '/accessibility/employeesPerCapita/county/';
                    dataLayer.getEmployeesPerCapitaBands({
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

                    data.title = 'Employees Per Capita by county in ' + me.opts.report.CurrentPlace.State.Name;
                    me.data.employeesPerCapita.textAlternativeUrl = '/accessibility/employeesPerCapita/county/';
                    me.data.employeesPerCapita.currentBoundingEntityId = 's' + me.opts.report.CurrentPlace.State.Id;
                    dataLayer.getEmployeesPerCapitaBands({
                        placeId: me.opts.report.CurrentPlace.Id,
                        industryId: me.opts.report.IndustryDetails.Industry.Id,
                        granularity: 'County',
                        boundingGranularity: 'State',
                        bands: 7
                    }, itemsNotify);

                }
            }

            if (z <= 4 && z >= 0) {

                data.title = 'Employees Per Capita by state in the USA';
                me.data.employeesPerCapita.textAlternativeUrl = '/accessibility/employeesPerCapita/state/';
                me.data.employeesPerCapita.currentBoundingEntityId = null;
                dataLayer.getEmployeesPerCapitaBands({
                    placeId: me.opts.report.CurrentPlace.Id,
                    industryId: me.opts.report.IndustryDetails.Industry.Id,
                    granularity: 'State',
                    bands: 7
                }, itemsNotify);
            }
        };

        var mapZoomEmployeesPerCapitaUpdated = function () {
            setEmployeesPerCapitaLegend();
        };

        var textAlternativeEmployeesPerCapitaClicked = function () {
            var url = me.data.employeesPerCapita.textAlternativeUrl;
            var bounds = me.employeesPerCapita.map.getBounds();
            var data = {
                bands: 7,
                industryId: me.opts.report.IndustryDetails.Industry.Id,
                boundingEntityId: me.data.employeesPerCapita.currentBoundingEntityId,
                southWest: bounds.getSouthWest().toString(),
                northEast: bounds.getNorthEast().toString()
            };

            window.open(jQuery.param.querystring(url, data), '_blank');

        };



        var displayReport = function () {

            me.reportContainer.setGauge(me.data.gauge);
            if (me.data.averageEmployees.hasData) {
                me.averageEmployees.noData.hide();
                me.averageEmployees.reportData.show();

                setAverageEmployeesHeatmap();

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
            }
            else {
                me.averageEmployees.noData.show();
                me.averageEmployees.reportData.hide();
                me.reportContainer.hideGauge();
            }

            if (me.data.employeesPerCapita.hasData) {
                me.employeesPerCapita.noData.hide();
                me.employeesPerCapita.reportData.show();

                setEmployeesPerCapitaHeatmap();

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


            dataLayer.getAverageEmployeesChart({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'City' }, averageEmployeesChartNotifier.getNotifier(function (data) { averageEmployeesChartData.City = data; }));
            dataLayer.getAverageEmployeesChart({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'County' }, averageEmployeesChartNotifier.getNotifier(function (data) { averageEmployeesChartData.County = data; }));
            dataLayer.getAverageEmployeesChart({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'Metro' }, averageEmployeesChartNotifier.getNotifier(function (data) { averageEmployeesChartData.Metro = data; }));
            dataLayer.getAverageEmployeesChart({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'State' }, averageEmployeesChartNotifier.getNotifier(function (data) { averageEmployeesChartData.State = data; }));
            dataLayer.getAverageEmployeesChart({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'Nation' }, averageEmployeesChartNotifier.getNotifier(function (data) { averageEmployeesChartData.Nation = data; }));

            dataLayer.getEmployeesPerCapitaChart({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'City' }, employeesPerCapitaChartNotifier.getNotifier(function (data) { employeesPerCapitaChartData.City = data; }));
            dataLayer.getEmployeesPerCapitaChart({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'County' }, employeesPerCapitaChartNotifier.getNotifier(function (data) { employeesPerCapitaChartData.County = data; }));
            dataLayer.getEmployeesPerCapitaChart({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'Metro' }, employeesPerCapitaChartNotifier.getNotifier(function (data) { employeesPerCapitaChartData.Metro = data; }));
            dataLayer.getEmployeesPerCapitaChart({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'State' }, employeesPerCapitaChartNotifier.getNotifier(function (data) { employeesPerCapitaChartData.State = data; }));
            dataLayer.getEmployeesPerCapitaChart({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'Nation' }, employeesPerCapitaChartNotifier.getNotifier(function (data) { employeesPerCapitaChartData.Nation = data; }));


            dataLayer.getAverageEmployeesPercentile({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id, value: me.data.enteredValue, granularity: 'City' }, averageEmployeesPercentileNotifier.getNotifier(function (data) { averageEmployeesPercentileData.City = data; }));
            dataLayer.getAverageEmployeesPercentile({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id, value: me.data.enteredValue, granularity: 'County' }, averageEmployeesPercentileNotifier.getNotifier(function (data) { averageEmployeesPercentileData.County = data; }));
            dataLayer.getAverageEmployeesPercentile({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id, value: me.data.enteredValue, granularity: 'Metro' }, averageEmployeesPercentileNotifier.getNotifier(function (data) { averageEmployeesPercentileData.Metro = data; }));
            dataLayer.getAverageEmployeesPercentile({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id, value: me.data.enteredValue, granularity: 'State' }, averageEmployeesPercentileNotifier.getNotifier(function (data) { averageEmployeesPercentileData.State = data; }));
            dataLayer.getAverageEmployeesPercentile({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id, value: me.data.enteredValue, granularity: 'Nation' }, averageEmployeesPercentileNotifier.getNotifier(function (data) { averageEmployeesPercentileData.Nation = data; }));


            dataLayer.getEmployeesPerCapitaPercentile({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'County' }, employeesPerCapitaPercentileNotifier.getNotifier(function (data) { employeesPerCapitaPercentileData.County = data; }));
            dataLayer.getEmployeesPerCapitaPercentile({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'Metro' }, employeesPerCapitaPercentileNotifier.getNotifier(function (data) { employeesPerCapitaPercentileData.Metro = data; }));
            dataLayer.getEmployeesPerCapitaPercentile({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'State' }, employeesPerCapitaPercentileNotifier.getNotifier(function (data) { employeesPerCapitaPercentileData.State = data; }));
            dataLayer.getEmployeesPerCapitaPercentile({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'Nation' }, employeesPerCapitaPercentileNotifier.getNotifier(function (data) { employeesPerCapitaPercentileData.Nation = data; }));

        };

        var averageEmployeesPercentileDataReturned = function (data) {
            if (data) {
                me.data.averageEmployees.hasData = true;
                
                me.data.averageEmployees.percentiles = {
                    City: data.City.Percentile < 1 ? 'less employees than 1%' : data.City.Percentile > 99 ? 'more employees than 99%' : 'more employees than ' + data.City.Percentile + '%',
                    County: data.County.Percentile < 1 ? 'less employees than 1%' : data.County.Percentile > 99 ? 'more employees than 99%' : 'more employees than ' + data.County.Percentile + '%',
                    State: data.State.Percentile < 1 ? 'less employees than 1%' : data.State.Percentile > 99 ? 'more employees than 99%' : 'more employees than ' + data.State.Percentile + '%',
                    Nation: data.Nation.Percentile < 1 ? 'less employees than 1%' : data.Nation.Percentile > 99 ? 'more employees than 99%' : 'more employees than ' + data.Nation.Percentile + '%'
                };

                if (data.Metro) {
                    me.data.averageEmployees.percentiles.Metro = data.Metro.Percentile < 1 ? 'less employees than 1%' : data.Metro.Percentile > 99 ? 'more employees than 99%' : 'more employees than ' + data.Metro.Percentile + '%';
                }

                me.data.gauge = {
                    value: data.Nation.Percentile,
                    tooltip: data.Nation.Percentile < 1 ? '<1st Percentile' : data.Nation.Percentile > 99 ? '>99th Percentile' : sizeup.util.numbers.format.ordinal(data.Nation.Percentile) + ' Percentile'
                };




                me.data.averageEmployees.description = {
                    Percentiles: me.data.averageEmployees.percentiles
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
               


                me.data.employeesPerCapita.percentiles = {
                    County: data.County.Percentile < 1 ? 'less than 1%' : data.County.Percentile > 99 ? 'more than 99%' : 'more than ' + data.County.Percentile + '%',
                    State: data.State.Percentile < 1 ? 'less than 1%' : data.State.Percentile > 99 ? 'more than 99%' : 'more than ' + data.State.Percentile + '%',
                    Nation: data.Nation.Percentile < 1 ? 'less than 1%' : data.Nation.Percentile > 99 ? 'more than 99%' : 'more than ' + data.Nation.Percentile + '%'
                };

                if (data.Metro) {
                    me.data.employeesPerCapita.percentiles.Metro = data.Metro.Percentile < 1 ? 'less than 1%' : data.Metro.Percentile > 99 ? 'more than 99%' : 'more than ' + data.Metro.Percentile + '%';
                }

                me.data.gauge = {
                    value: data.Nation.Percentile,
                    tooltip: data.Nation.Percentile < 1 ? '<1st Percentile' : data.Nation.Percentile > 99 ? '>99th Percentile' : sizeup.util.numbers.format.ordinal(data.Nation.Percentile) + ' Percentile'
                };




                me.data.employeesPerCapita.description = {
                    Percentiles: me.data.employeesPerCapita.percentiles
                };
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


