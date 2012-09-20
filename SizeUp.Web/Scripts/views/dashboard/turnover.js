(function () {
    sizeup.core.namespace('sizeup.views.dashboard.turnover');
    sizeup.views.dashboard.turnover = function (opts) {

        var me = {};
        var dataLayer = new sizeup.core.data();
        var templates = new sizeup.core.templates(opts.container);
        me.opts = opts;
        me.data = {};
        me.container = opts.container;
        me.data.description = {};
        me.data.hasData = false;
        me.data.turnover = {};
        me.data.jobChange = {};
        me.turnover = {};
        me.jobChange = {};

        var init = function () {

            me.reportContainer = new sizeup.views.dashboard.reportContainer(
                {
                    container: me.container,
                    inputValidation: /^[0-9]+$/g,
                    inputCleaning: /[%]/g,
                    events:
                    {
                        runReport: runReport,
                        //valueChanged: function () { }
                    },
                    inputFormat: function (val) {
                        return sizeup.util.numbers.format.percentage(val,0);
                    }
                });

            me.turnover.sourceButton = new sizeup.controls.toggleButton(
                {
                    button: me.container.find('.reportContainer .turnover .links .source'),
                    onClick: function () { toggleSource('turnover'); }
                });

            me.turnover.considerationToggle = new sizeup.controls.toggleButton(
                {
                    button: me.container.find('.reportSidebar.turnover .considerationToggle'),
                    onClick: function () { toggleConsiderations('turnover'); }
                });

            me.turnover.resourceToggle = new sizeup.controls.toggleButton(
                {
                    button: me.container.find('.reportSidebar.turnover .resourcesToggle'),
                    onClick: function () { toggleResources('turnover'); }
                });


            me.turnover.sourceContent = me.container.find('.reportContainer .turnover .sourceContent').hide();
            me.turnover.considerations = me.container.find('.reportContainer .turnover .considerations');
            me.turnover.resources = me.container.find('.reportContainer .turnover .resources');
            me.turnover.description = me.container.find('.reportContainer .turnover .description');

        
            me.jobChange.sourceButton = new sizeup.controls.toggleButton(
                {
                    button: me.container.find('.reportContainer .jobChange .links .source'),
                    onClick: function () { toggleSource('jobChange'); }
                });

            me.jobChange.considerationToggle = new sizeup.controls.toggleButton(
                {
                    button: me.container.find('.reportSidebar.jobChange .considerationToggle'),
                    onClick: function () { toggleConsiderations('jobChange'); }
                });

            me.jobChange.resourceToggle = new sizeup.controls.toggleButton(
                {
                    button: me.container.find('.reportSidebar.jobChange .resourcesToggle'),
                    onClick: function () { toggleResources('jobChange'); }
                });

            me.jobChange.question = new sizeup.controls.question({
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
                me.jobChange.question.showAnswer(index);
            }
            $(window).bind('hashchange', function (e) { hashChanged(e); });


            me.jobChange.sourceContent = me.container.find('.reportContainer .jobChange .sourceContent').hide();
            me.jobChange.considerations = me.container.find('.reportContainer .jobChange .considerations');
            me.jobChange.description = me.container.find('.reportContainer .jobChange .description');




            me.noData = me.container.find('.noDataError').hide();
            me.reportData = me.container.find('.reportData');
       
            me.reportContainer.setValue('');
            
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

        var answerClicked = function (index) {
            jQuery.bbq.pushState({ businessType: index });
        };

        var answerCleared = function () {
            jQuery.bbq.removeState('businessType');
        };

        var hashChanged = function (e) {
            var index = e.getState('businessType');
            if (index) {
                me.jobChange.question.showAnswer(index);
            }
            else {
                me.jobChange.question.clearAnswer();
            }
        };

        var displayReport = function () {

            me.reportContainer.setGauge(me.data.gauge);
            if (me.data.hasData) {
                me.noData.hide();
                me.reportData.show();
                me.turnover.table = new sizeup.charts.tableChart({
                    container: me.container.find('.turnover .table'),
                    rowTemplate: templates.get('turnoverTableRow'),
                    rows: me.data.turnover.table
                });

                me.reportContainer.setValue(sizeup.util.numbers.format.round(me.data.turnover.raw['County'].turnover, 0));

                me.data.turnover.description = {
                    Turnover: sizeup.util.numbers.format.percentage(me.data.turnover.raw['County'].turnover,2),
                    NAICS4: me.opts.report.IndustryDetails.NAICS4,
                    Industry: me.opts.report.IndustryDetails.Industry
                };

                me.turnover.description.html(templates.bind(templates.get("turnoverDescription"), me.data.turnover.description));





                me.jobChange.table = new sizeup.charts.tableChart({
                    container: me.container.find('.jobChange .table'),
                    rowTemplate: templates.get('jobChangeTableRow'),
                    rows: me.data.jobChange.table
                });

                me.data.jobChange.description = {
                    NetJobChange: me.data.jobChange.raw['County'].netJobChange,
                    NAICS4: me.opts.report.IndustryDetails.NAICS4,
                    Industry: me.opts.report.IndustryDetails.Industry
                };

                me.jobChange.description.html(templates.bind(templates.get("jobChangeDescription"), me.data.jobChange.description));


            }
            else {
                me.noData.show();
                me.reportData.hide();
            }



        };

        var runReport = function (e) {
            new sizeup.core.analytics().dashboardReportLoaded({ report: 'turnover' });

            var notifier = new sizeup.core.notifier(function () {
                e.callback();
                displayReport();
            });
            dataLayer.getTurnoverChart({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id }, notifier.getNotifier(turnoverChartDataReturned));
            dataLayer.getJobChangeChart({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id }, notifier.getNotifier(jobChangeChartDataReturned));
            dataLayer.getTurnoverPercentile({ industryId: me.opts.report.IndustryDetails.Industry.Id, placeId: me.opts.report.CurrentPlace.Id }, notifier.getNotifier(percentileDataReturned));
        };


        var percentileDataReturned = function (data) {
            if (data) {
                me.data.hasData = true;
                var percentile = sizeup.util.numbers.format.ordinal(data.Percentile);
                me.data.gauge = {
                    value: data.Percentile,
                    tooltip: percentile + ' Percentile'
                };
            }
            else {
                me.data.gauge = {
                    value: 0,
                    tooltip: 'No data'
                };
            }
        };

        var turnoverChartDataReturned = function (data) {

            me.data.turnover.table = {};
            me.data.turnover.raw = {};
            var indexes = ['County', 'Metro', 'State', 'Nation'];
            for (var x = 0; x < indexes.length; x++) {
                if (data[indexes[x]] != null) {
                    me.data.turnover.raw[indexes[x]] = {
                        hires: data[indexes[x]].Hires == null ? 'no data' : data[indexes[x]].Hires,
                        separations: data[indexes[x]].Separations == null ? 'no data' : data[indexes[x]].Separations,
                        turnover: data[indexes[x]].Turnover == null ? 'no data' : data[indexes[x]].Turnover
                    };

                    me.data.turnover.table[indexes[x]] = {
                        name: data[indexes[x]].Name,
                        hires: data[indexes[x]].Hires == null ? 'no data' : sizeup.util.numbers.format.addCommas(data[indexes[x]].Hires),
                        separations: data[indexes[x]].Separations == null ? 'no data' : sizeup.util.numbers.format.addCommas(data[indexes[x]].Separations),
                        turnover: data[indexes[x]].Turnover == null ? 'no data' : sizeup.util.numbers.format.percentage(data[indexes[x]].Turnover, 2)
                    };
                }
            }
        };

        var jobChangeChartDataReturned = function (data) {

            me.data.jobChange.table = {};
            me.data.jobChange.raw = {};
            var indexes = ['County', 'Metro', 'State', 'Nation'];
            for (var x = 0; x < indexes.length; x++) {
                if (data[indexes[x]] != null) {
                    me.data.jobChange.raw[indexes[x]] = {
                        jobGains: data[indexes[x]].JobGains== null ? 'no data' : data[indexes[x]].JobGains,
                        jobLosses: data[indexes[x]].JobLosses == null ? 'no data' : data[indexes[x]].JobLosses,
                        netJobChange: data[indexes[x]].NetJobChange == null ? 'no data' : data[indexes[x]].NetJobChange
                    };

                    me.data.jobChange.table[indexes[x]] = {
                        name: data[indexes[x]].Name,
                        jobGains: data[indexes[x]].JobGains == null ? 'no data' : sizeup.util.numbers.format.addCommas(data[indexes[x]].JobGains),
                        jobLosses: data[indexes[x]].JobLosses == null ? 'no data' : sizeup.util.numbers.format.addCommas(data[indexes[x]].JobLosses),
                        netJobChange: data[indexes[x]].NetJobChange == null ? 'no data' : sizeup.util.numbers.format.addCommas(data[indexes[x]].NetJobChange)
                    };
                }
            }
        };
        

        var setupReport = function () {
            me.reportContainer.doGetReport();
        };

        var collapseReport = function () {
            me.reportContainer.collapseReport();
        };

        var expandReport = function () {
            me.reportContainer.expandReport();
        };


        var publicObj = {

            collapseReport: function () {
                collapseReport();
            },
            expandReport: function () {
                expandReport();
            },
            setupReport: function () {
                setupReport();
            }
        };
        init();
        return publicObj;

    };
})();