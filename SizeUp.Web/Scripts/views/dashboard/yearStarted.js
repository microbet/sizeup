(function () {
    sizeup.core.namespace('sizeup.views.dashboard.yearStarted');
    sizeup.views.dashboard.yearStarted = function (opts) {

        var me = {};
        var dataLayer = new sizeup.core.data();
        var templates = new sizeup.core.templates(opts.container);
        me.opts = opts;
        me.opts.startYear = 1986;
        me.opts.endYear = 2016;
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
                        runReport: runReport
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
            var index = jQuery.bbq.getState('businessType');
            if (index) {
                me.question.showAnswer(index);
            }
            $(window).bind('hashchange', function (e) { hashChanged(e); });


            me.sourceContent = me.container.find('.reportContainer .sourceContent').hide();
            me.considerations = me.container.find('.reportContainer  .considerations');
            me.resources = me.container.find('.reportContainer  .resources');
            me.description = me.container.find('.reportContainer  .description');





            me.noData = me.container.find('.noDataError').hide();
            me.reportData = me.container.find('.reportData');

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

        var displayReport = function () {

            me.reportContainer.setGauge(me.data.gauge);
            if (me.data.hasData) {
                me.noData.hide();
                me.reportData.show();
               

               
                me.chart = new sizeup.charts.lineChart({
                    container: me.container.find('.chart .container'),
                    series: me.data.chart.series,
                    marker: me.data.chart.marker
                });
                me.chart.draw();






                me.data.description = {
                    //Turnover: sizeup.util.numbers.format.percentage(me.data.raw['County'].turnover, 1),
                    //NAICS4: me.opts.report.IndustryDetails.NAICS4,
                    //Industry: me.opts.report.IndustryDetails.Industry
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
            jQuery.bbq.pushState({ yearStarted: me.data.enteredValue });
            dataLayer.getYearStartedChart({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id, startYear: me.opts.startYear, endYear: me.opts.endYear }, notifier.getNotifier(chartDataReturned));
            dataLayer.getYearStartedPercentile({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id, value: me.data.enteredValue }, notifier.getNotifier(percentileDataReturned));
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

            me.data.chart = {
                marker:null,
                series: data
            };


            /*
            var indexes = ['City', 'County', 'Metro', 'State', 'Nation'];
            for (var x = 0; x < indexes.length; x++) {
                if (data[indexes[x]] != null) {
                    me.data.raw[indexes[x]] = {
                        hires: data[indexes[x]].Hires,
                        separations: data[indexes[x]].Separations,
                        turnover: data[indexes[x]].Turnover
                    };

        
                }
            }*/
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