(function () {
    sizeup.core.namespace('sizeup.views.dashboard.turnover');
    sizeup.views.dashboard.turnover = function (opts) {

        var me = {};
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
                        if (isNaN(val)) {
                            return 'N/A';
                        } else {
                            return sizeup.util.numbers.format.percentage(val, 0);
                        }
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


            me.reportData = me.container.find('.reportData');
            me.noData = me.container.find('.reportContent.noDataError').removeClass('hidden').hide();

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

            if (me.data.noData) {
                me.noData.show();
                me.reportData.hide();
                me.reportContainer.hideGauge();
            }
            else {
                me.reportContainer.setGauge(me.data.gauge);
                me.reportData.show();
                me.noData.hide();


                me.turnover.table = new sizeup.charts.tableChart({
                    container: me.container.find('.turnover .table'),
                    rowTemplate: templates.get('turnoverTableRow'),
                    rows: me.data.turnover.table
                });

                me.reportContainer.setValue(sizeup.util.numbers.format.round(me.data.turnover.raw['County'].turnover, 0));
                if (isNaN(me.data.turnover.raw['County'].turnover)) {
                    me.reportContainer.hideGauge();
                }


                me.data.turnover.description.NAICS4 = me.opts.report.CurrentIndustry.NAICS4;
                me.data.turnover.description.Industry = me.opts.report.CurrentIndustry;
                me.data.turnover.description.HasData = me.data.turnover.raw['County'].turnover != 'no data';

                me.turnover.description.html(templates.bind(templates.get("turnoverDescription"), me.data.turnover.description));




                me.jobChange.table = new sizeup.charts.tableChart({
                    container: me.container.find('.jobChange .table'),
                    rowTemplate: templates.get('jobChangeTableRow'),
                    rows: me.data.jobChange.table
                });




                me.data.jobChange.description.NAICS4 = me.opts.report.CurrentIndustry.NAICS4;
                me.data.jobChange.description.Industry = me.opts.report.CurrentIndustry;
                me.data.jobChange.description.HasData = me.data.jobChange.raw['County'].turnover != 'no data';
                me.jobChange.description.html(templates.bind(templates.get("jobChangeDescription"), me.data.jobChange.description));

            }
        };

        var runReport = function (e) {
            new sizeup.core.analytics().dashboardReportLoaded({ report: 'turnover' });

            var notifier = new sizeup.core.notifier(function () {
                e.callback();
                displayReport();
            });
            var turnoverPercentileData = {};
            var turnoverChartData = {};
            var jobChangeChartData = {};
            var jobChangePercentileData  = {};
            var turnoverPercentileNotifier = new sizeup.core.notifier(notifier.getNotifier(function () { turnoverPercentileDataReturned(turnoverPercentileData); }));
            var turnoverChartNotifier = new sizeup.core.notifier(notifier.getNotifier(function () { turnoverChartDataReturned(turnoverChartData); }));
            var jobChangeChartNotifier = new sizeup.core.notifier(notifier.getNotifier(function () { jobChangeChartDataReturned(jobChangeChartData); }));
            var jobChangePercentileNotifier = new sizeup.core.notifier(notifier.getNotifier(function () { jobChangePercentileDataReturned(jobChangePercentileData); }));

            
            sizeup.api.data.getTurnover({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.County.Id }, turnoverChartNotifier.getNotifier(function (data) { turnoverChartData.County = data; }));
            if (me.opts.report.CurrentPlace.Metro.Id) {
                sizeup.api.data.getTurnover({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.Metro.Id }, turnoverChartNotifier.getNotifier(function (data) { turnoverChartData.Metro = data; }));
            }
            sizeup.api.data.getTurnover({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.State.Id }, turnoverChartNotifier.getNotifier(function (data) { turnoverChartData.State = data; }));
            sizeup.api.data.getTurnover({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.Nation.Id }, turnoverChartNotifier.getNotifier(function (data) { turnoverChartData.Nation = data; }));

            if (me.opts.report.CurrentPlace.Metro.Id) {
                sizeup.api.data.getTurnoverPercentile({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.County.Id, boundingGeographicLocationId: me.opts.report.CurrentPlace.Metro.Id }, turnoverPercentileNotifier.getNotifier(function (data) { turnoverPercentileData.Metro = data; }));
            }
            sizeup.api.data.getTurnoverPercentile({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.County.Id, boundingGeographicLocationId: me.opts.report.CurrentPlace.State.Id }, turnoverPercentileNotifier.getNotifier(function (data) { turnoverPercentileData.State = data; }));
            sizeup.api.data.getTurnoverPercentile({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.County.Id, boundingGeographicLocationId: me.opts.report.CurrentPlace.Nation.Id }, turnoverPercentileNotifier.getNotifier(function (data) { turnoverPercentileData.Nation = data; }));


            sizeup.api.data.getJobChange({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.County.Id }, jobChangeChartNotifier.getNotifier(function (data) { jobChangeChartData.County = data; }));
            if (me.opts.report.CurrentPlace.Metro.Id) {
                sizeup.api.data.getJobChange({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.Metro.Id }, jobChangeChartNotifier.getNotifier(function (data) { jobChangeChartData.Metro = data; }));
            }
            sizeup.api.data.getJobChange({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.State.Id }, jobChangeChartNotifier.getNotifier(function (data) { jobChangeChartData.State = data; }));
            sizeup.api.data.getJobChange({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.Nation.Id }, jobChangeChartNotifier.getNotifier(function (data) { jobChangeChartData.Nation = data; }));

            if (me.opts.report.CurrentPlace.Metro.Id) {
                sizeup.api.data.getJobChangePercentile({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.County.Id, boundingGeographicLocationId: me.opts.report.CurrentPlace.Metro.Id }, jobChangePercentileNotifier.getNotifier(function (data) { jobChangePercentileData.Metro = data; }));
            }
            sizeup.api.data.getJobChangePercentile({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.County.Id, boundingGeographicLocationId: me.opts.report.CurrentPlace.State.Id }, jobChangePercentileNotifier.getNotifier(function (data) { jobChangePercentileData.State = data; }));
            sizeup.api.data.getJobChangePercentile({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.County.Id, boundingGeographicLocationId: me.opts.report.CurrentPlace.Nation.Id }, jobChangePercentileNotifier.getNotifier(function (data) { jobChangePercentileData.Nation = data; }));



        };


        var turnoverPercentileDataReturned = function (data) {
            me.data.turnover.percentiles = {};

            if (data.Metro) {
                me.data.turnover.percentiles.Metro = data.Metro.Percentile < 1 ? 'more than 99%' : data.Metro.Percentile > 99 ? 'less than 99%' : 'less than or equal to ' + sizeup.util.numbers.format.percentage(data.Metro.Percentile);
            }
            if (data.State) {
                me.data.turnover.percentiles.State = data.State.Percentile < 1 ? 'more than 99%' : data.State.Percentile > 99 ? 'less than 99%' : 'less than or equal to ' + sizeup.util.numbers.format.percentage(data.State.Percentile);
            }
            if (data.Nation) {
                me.data.turnover.percentiles.Nation = data.Nation.Percentile < 1 ? 'more than 99%' : data.Nation.Percentile > 99 ? 'less than 99%' : 'less than or equal to ' + sizeup.util.numbers.format.percentage(data.Nation.Percentile);
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


            me.data.turnover.description = {
                Percentiles: me.data.turnover.percentiles
            }
        };


        var jobChangePercentileDataReturned = function (data) {
            me.data.jobChange.percentiles = {};

            if (data.Metro) {
                me.data.jobChange.percentiles.Metro = data.Metro.Percentile < 1 ? 'less than 99%' : data.Metro.Percentile > 99 ? 'greater than 99%' : 'greater than or equal to ' + sizeup.util.numbers.format.percentage(data.Metro.Percentile);
            }
            if (data.State) {
                me.data.jobChange.percentiles.State = data.State.Percentile < 1 ? 'less than 99%' : data.State.Percentile > 99 ? 'greater than 99%' : 'greater than or equal to ' + sizeup.util.numbers.format.percentage(data.State.Percentile);
            }
            if (data.Nation) {
                me.data.jobChange.percentiles.Nation = data.Nation.Percentile < 1 ? 'less than 99%' : data.Nation.Percentile > 99 ? 'greater than 99%' : 'greater than or equal to ' + sizeup.util.numbers.format.percentage(data.Nation.Percentile);
            }

            me.data.jobChange.description = {
                Percentiles: me.data.jobChange.percentiles
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