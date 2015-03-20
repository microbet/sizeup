(function () {
    sizeup.core.namespace('sizeup.views.dashboard.yearStarted');
    sizeup.views.dashboard.yearStarted = function (opts) {

        var me = {};
        var templates = new sizeup.core.templates(opts.container);
        me.opts = opts;
        me.opts.startYear = 1986;
        me.opts.endYear = 2014;
        me.opts.maxYear = 2014;
        me.data = {};
        me.container = opts.container;
        me.data.enteredValue = jQuery.bbq.getState().yearStarted;
        me.data.hasData = false;
        me.data.description = {};


        var init = function () {

            me.reportContainer = new sizeup.views.dashboard.reportContainer(
                {
                    container: me.container,
                    inputValidation: /^[0-9]+$/g,
                    inputCleaning: /[%]/g,
                    events:
                    {
                        runReport: runReport,
                        valueChanged: function (val) {
                            if (val.value == '') {
                                jQuery.bbq.removeState('yearStarted');
                            }
                        }
                    },
                    inputFormat: function (val) {
                        return val;
                    }
                });

            me.sourceButton = new sizeup.controls.toggleButton(
                {
                    button: me.container.find('.reportContainer  .links .source'),
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

            
            me.container.find('.links .textAlternative').click(function () {
                var url = '/accessibility/yearStarted/';
                var data = { industryId: me.opts.report.CurrentIndustry.Id, placeId: me.opts.report.CurrentPlace.Id, startYear: me.opts.startYear, endYear: me.opts.endYear };
                url = jQuery.param.querystring(url, data)
                window.open(url,'_blank');                
            });



            var index = jQuery.bbq.getState('businessType');
            if (index) {
                me.question.showAnswer(index);
            }
            $(window).bind('hashchange', function (e) { hashChanged(e); });


            me.sourceContent = me.container.find('.reportContainer .sourceContent').hide();
            me.considerations = me.container.find('.reportContainer  .considerations');
            me.resources = me.container.find('.reportContainer  .resources');
            me.description = me.container.find('.reportContainer  .description');




            me.reportData = me.container.find('.reportData');
            me.noData = me.container.find('.reportContent.noDataError').removeClass('hidden').hide();
            if (me.data.enteredValue) {
                me.reportContainer.setValue(me.data.enteredValue);
            }

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

        var buildChart = function () {
            me.chart = new sizeup.charts.lineChart({
                container: me.container.find('.chart .container'),
                series: me.data.chart.series,
                marker: me.data.chart.marker,
                xBounds: { min: me.opts.startYear, max: me.opts.maxYear }
            });
            me.chart.draw();

            me.container.find('.chart .legend input[type=checkbox]')
                .attr('checked', 'checked')
                .click(function () {
                    var cb = $(this);
                    if (cb.is(':checked')) {
                        me.chart.showSeries(cb.val());
                    }
                    else {
                        me.chart.hideSeries(cb.val());
                    }
                    me.chart.draw();
                });
            
            if (me.data.chart.series['Metro']==null) {
                me.container.find('.chart .legend .metro').remove();
            }

        };

        var displayReport = function () {

            if (me.data.noData) {
                me.noData.show();
                me.reportData.hide();
                me.reportContainer.hideGauge();
            }
            else {
                me.reportContainer.setGauge(me.data.gauge);
                me.reportData.show();
                me.noData.hide();

                buildChart();

                me.data.description = {
                    Year: me.data.enteredValue,
                    Counts: me.data.counts,
                    Percentiles: me.data.percentiles
                };
                var dTemplate = me.data.enteredValue < me.opts.startYear ? templates.get('outOfBoundsDescription') : templates.get("description");
                me.description.html(templates.bind(dTemplate, me.data.description));

            }


        };

        var runReport = function (e) {
            new sizeup.core.analytics().dashboardReportLoaded({ report: 'yearStarted' });

            var notifier = new sizeup.core.notifier(function () {
                e.callback();
                displayReport();
            });

            var percentileData = {};
            var chartData = {};
            var percentileNotifier = new sizeup.core.notifier(notifier.getNotifier(function () { percentileDataReturned(percentileData); }));
            var chartNotifier = new sizeup.core.notifier(notifier.getNotifier(function () { chartDataReturned(chartData); }));


            me.data.enteredValue = me.reportContainer.getValue();
            jQuery.bbq.pushState({ yearStarted: me.data.enteredValue });

            sizeup.api.data.getYearStarted({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.City.Id, startYear: me.opts.startYear, endYear: me.opts.endYear}, chartNotifier.getNotifier(function (data) { chartData.City = data; }));
            sizeup.api.data.getYearStarted({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.County.Id, startYear: me.opts.startYear, endYear: me.opts.endYear }, chartNotifier.getNotifier(function (data) { chartData.County = data; }));
            if (me.opts.report.CurrentPlace.Metro.Id) {
                sizeup.api.data.getYearStarted({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.Metro.Id, startYear: me.opts.startYear, endYear: me.opts.endYear }, chartNotifier.getNotifier(function (data) { chartData.Metro = data; }));
            }
            sizeup.api.data.getYearStarted({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.State.Id, startYear: me.opts.startYear, endYear: me.opts.endYear }, chartNotifier.getNotifier(function (data) { chartData.State = data; }));
            sizeup.api.data.getYearStarted({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.Nation.Id, startYear: me.opts.startYear, endYear: me.opts.endYear }, chartNotifier.getNotifier(function (data) { chartData.Nation = data; }));

            sizeup.api.data.getYearStartedPercentile({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.City.Id, value: me.data.enteredValue }, percentileNotifier.getNotifier(function (data) { percentileData.City = data; }));
            sizeup.api.data.getYearStartedPercentile({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.County.Id, value: me.data.enteredValue }, percentileNotifier.getNotifier(function (data) { percentileData.County = data; }));
            if (me.opts.report.CurrentPlace.Metro.Id) {
                sizeup.api.data.getYearStartedPercentile({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.Metro.Id, value: me.data.enteredValue }, percentileNotifier.getNotifier(function (data) { percentileData.Metro = data; }));
            }
            sizeup.api.data.getYearStartedPercentile({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.State.Id, value: me.data.enteredValue }, percentileNotifier.getNotifier(function (data) { percentileData.State = data; }));
            sizeup.api.data.getYearStartedPercentile({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.Nation.Id, value: me.data.enteredValue }, percentileNotifier.getNotifier(function (data) { percentileData.Nation = data; }));
        };


        var percentileDataReturned = function (data) {

            me.data.percentiles = {};

            if (data.City) {
                me.data.percentiles.City = data.City.Percentile < 1 ? 'less than 99%' : data.City.Percentile > 99 ? 'longer than 99%' : 'as long as or longer than ' + sizeup.util.numbers.format.percentage(data.City.Percentile);
            }
            if (data.County) {
                me.data.percentiles.County = data.County.Percentile < 1 ? 'less than 99%' : data.County.Percentile > 99 ? 'longer than 99%' : 'as long as or longer than ' + sizeup.util.numbers.format.percentage(data.County.Percentile);
            }
            if (data.Metro) {
                me.data.percentiles.Metro = data.Metro.Percentile < 1 ? 'less than 99%' : data.Metro.Percentile > 99 ? 'longer than 99%' : 'as long as or longer than ' + sizeup.util.numbers.format.percentage(data.Metro.Percentile);
            }
            if (data.State) {
                me.data.percentiles.State = data.State.Percentile < 1 ? 'less than 99%' : data.State.Percentile > 99 ? 'longer than 99%' : 'as long as or longer than ' + sizeup.util.numbers.format.percentage(data.State.Percentile);
            }
            if (data.Nation) {
                me.data.percentiles.Nation = data.Nation.Percentile < 1 ? 'less than 99%' : data.Nation.Percentile > 99 ? 'longer than 99%' : 'as long as or longer than ' + sizeup.util.numbers.format.percentage(data.Nation.Percentile);
                me.data.gauge = {
                    value: data.Nation.Percentile,
                    tooltip: data.Nation.Percentile < 1 ? '<1st Percentile' : data.Nation.Percentile > 99 ? '>99th Percentile' : sizeup.util.numbers.format.ordinal(sizeup.util.numbers.format.round(data.Nation.Percentile,0)) + ' Percentile'
                };
            }
            else {
                me.data.gauge = {
                    value: 0,
                    tooltip: 'No data'
                };
            }
        };

        var getCounts = function (data, year) {
            for (var x in data) {
                if (data[x].Key == year) {
                    return data[x].Value;
                }
            }
            return null;
        };

        var chartDataReturned = function (data) {
            me.data.noData = true;
            me.data.chart = {
                marker:null,
                series: []
            };
            me.data.chart.marker = {
                color: '#5b0',
                value: me.data.enteredValue,
                label: 'your business started in ' + me.data.enteredValue
            };

            me.data.counts = {
                City: getCounts(data.City, me.data.enteredValue),
                County: getCounts(data.County, me.data.enteredValue),
                Metro: getCounts(data.Metro, me.data.enteredValue),
                State: getCounts(data.State, me.data.enteredValue),
                Nation: getCounts(data.Nation, me.data.enteredValue),
            };

            if (data['City']) {
                me.data.noData = false;
                me.data.chart.series['City'] = 
                    {

                        color: '#35B',
                        plots: data['City']
                    };
            }
            if (data['County']) {
                me.data.noData = false;
                me.data.chart.series['County'] =
                {

                    color: '#08D',
                    plots: data['County']
                };
            }
            if (data['Metro']) {
                me.data.noData = false;
                me.data.chart.series['Metro'] =
                {

                    color: '#6AC',
                    plots: data['Metro']
                };
            }
            if (data['State']) {
                me.data.noData = false;
                me.data.chart.series['State'] =
                {

                    color: '#FB1',
                    plots: data['State']
                };
            }
            if (data['Nation']) {
                me.data.noData = false;
                me.data.chart.series['Nation'] =
                {

                    color: '#F60',
                    plots: data['Nation']
                };
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