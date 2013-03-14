﻿(function () {
    sizeup.core.namespace('sizeup.views.dashboard.averageSalary');
    sizeup.views.dashboard.averageSalary = function (opts) {

        var me = {};
        var dataLayer = new sizeup.core.data();
        var templates = new sizeup.core.templates(opts.container);
        me.opts = opts;
        me.data = {};
        me.container = opts.container;
        me.data.enteredValue = jQuery.bbq.getState().salary;
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
                                jQuery.bbq.removeState('salary');
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
                answers: [
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
            me.question.showAnswer(index);
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
                    tileUrl: '/tiles/averageSalary/',
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
                    tileUrl: '/tiles/averageSalary/',
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
                    tileUrl: '/tiles/averageSalary/',
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
                tileUrl: '/tiles/averageSalary/',
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
                    items:[]
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
                        format: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); }
                    });
                    me.map.setLegend(legend);
            });



            var itemsNotify = notify.getNotifier(function (d) { data.items = d; });


            if (me.opts.report.CurrentPlace.Metro.Id != null) {
                if (z <= 32 && z >= 8) {
                    data.title = 'Average Salary by county in ' + me.opts.report.CurrentPlace.Metro.Name + ' (Metro)';
                    me.data.textAlternative = {
                        granularity: 'County',
                        boundingGranularity: 'Metro'
                    };
                    dataLayer.getAverageSalaryBands({
                        placeId: me.opts.report.CurrentPlace.Id,
                        industryId: me.opts.report.CurrentIndustry.Id,
                        bands: 7,
                        granularity: 'County',
                        boundingGranularity: 'Metro'
                    }, itemsNotify);
         
           
                }

                if (z <= 7 && z >= 5) {
                    data.title = 'Average Salary by county in ' + me.opts.report.CurrentPlace.State.Name;
                    me.data.textAlternative = {
                        granularity: 'County',
                        boundingGranularity: 'State'
                    };
                    dataLayer.getAverageSalaryBands({
                        placeId: me.opts.report.CurrentPlace.Id,
                        industryId: me.opts.report.CurrentIndustry.Id,
                        bands: 7,
                        granularity: 'County',
                        boundingGranularity: 'State'
                    }, itemsNotify);
                }
            }
            else {
                if (z <= 32 && z >= 5) {

                    data.title = 'Average Salary by county in ' + me.opts.report.CurrentPlace.State.Name;
                    me.data.textAlternative = {
                        granularity: 'County',
                        boundingGranularity: 'State'
                    };
                    dataLayer.getAverageSalaryBands({
                        placeId: me.opts.report.CurrentPlace.Id,
                        industryId: me.opts.report.CurrentIndustry.Id,
                        bands: 7,
                        granularity: 'County',
                        boundingGranularity: 'State'
                    }, itemsNotify);
                     
                }
            }

            if (z <= 4 && z >= 0) {

                data.title = 'Average Salary by state in the USA';
                me.data.textAlternative = {
                    granularity: 'State',
                    boundingGranularity: 'Nation'
                };
                dataLayer.getAverageSalaryBands({
                    placeId: me.opts.report.CurrentPlace.Id,
                    industryId: me.opts.report.CurrentIndustry.Id,
                    bands: 7,
                    granularity: 'State'
                }, itemsNotify);
            }
        };

        var mapZoomUpdated = function () {
            setLegend();
        };

        var textAlternativeClicked = function () {
            var url = '/accessibility/averageSalary/';
            var data = {
                bands: 7,
                industryId: me.opts.report.CurrentIndustry.Id,
                placeId: me.opts.report.CurrentPlace.Id,
                granularity: me.data.textAlternative.granularity,
                boundingGranularity: me.data.textAlternative.boundingGranularity
            };
            window.open(jQuery.param.querystring(url, data), '_blank');
        };



        var displayReport = function () {

            me.reportContainer.setGauge(me.data.gauge);
  
            me.reportData.show();

            setHeatmap();
                
            me.chart = new sizeup.charts.barChart({

                valueFormat: function(val){ return '$' + sizeup.util.numbers.format.addCommas(Math.floor(val));},
                container: me.container.find('.chart .container'),
                title: 'average annual salary',
                bars: me.data.chart
            });
            me.chart.draw();

            me.table = new sizeup.charts.tableChart({
                container: me.container.find('.table').hide(),
                rowTemplate: templates.get('tableRow'),
                rows:me.data.table
            });


            me.data.description = {
                Percentage: me.data.gauge.tooltip,
                NAICS6: me.opts.report.CurrentIndustry.NAICS6,
                Salary: me.data.table['Nation'].value
            };

            me.description.html(templates.bind(templates.get("description"), me.data.description));

           
        };

        var runReport = function (e) {
            new sizeup.core.analytics().dashboardReportLoaded({ report: 'averageSalary' });

            var notifier = new sizeup.core.notifier(function () {
                e.callback();
                displayReport();
            });

            var percentileData = {};
            var chartData = {};
            var percentileNotifier = new sizeup.core.notifier(notifier.getNotifier(function () { percentileDataReturned(percentileData); }));
            var chartNotifier = new sizeup.core.notifier(notifier.getNotifier(function () { chartDataReturned(chartData); }));


            me.data.enteredValue = me.reportContainer.getValue();
            jQuery.bbq.pushState({ salary: me.data.enteredValue });
            dataLayer.getAverageSalaryChart({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'County' }, chartNotifier.getNotifier(function (data) { chartData.County = data; }));
            dataLayer.getAverageSalaryChart({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'Metro' }, chartNotifier.getNotifier(function (data) { chartData.Metro = data; }));
            dataLayer.getAverageSalaryChart({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'State' }, chartNotifier.getNotifier(function (data) { chartData.State = data; }));
            dataLayer.getAverageSalaryChart({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, granularity: 'Nation' }, chartNotifier.getNotifier(function (data) { chartData.Nation = data; }));
            dataLayer.getAverageSalaryPercentage({ industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, value: me.data.enteredValue, granularity: 'Nation' }, percentileNotifier.getNotifier(function (data) { percentileData.Nation = data; }));
        };

        var percentileDataReturned = function (data) {
            if (data.Nation!=null) {
                var val = 50 + (data.Nation.Percentage / 2);
                var percentage = sizeup.util.numbers.format.percentage(Math.abs(data.Nation.Percentage));
                me.data.gauge = {
                    value: val,
                    tooltip: data.Nation.Percentage < 0 ? percentage + ' Below Average' : data.Nation.Percentage == 0 ? 'Average' : percentage + ' Above Average'
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
            
       
            var indexes = ['County', 'Metro', 'State', 'Nation'];
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