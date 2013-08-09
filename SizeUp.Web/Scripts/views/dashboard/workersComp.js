(function () {
    sizeup.core.namespace('sizeup.views.dashboard.workersComp');
    sizeup.views.dashboard.workersComp = function (opts) {

        var me = {};
        var templates = new sizeup.core.templates(opts.container);
        me.opts = opts;
        me.data = {};
        me.container = opts.container;
        me.data.enteredValue = jQuery.bbq.getState().workersComp;
        me.data.hasData = false;
        me.data.description = {};

        var init = function () {

            me.reportContainer = new sizeup.views.dashboard.reportContainer(
                {
                    container: me.container,
                    inputValidation: /^[0-9\.]+$/g,
                    inputCleaning: /[\$\,]/g,
                    events:
                    {
                        runReport: runReport,
                        valueChanged: function (val) {
                            if (val.value == '') {
                                jQuery.bbq.removeState('workersComp');
                            }
                        }
                    },
                    inputFormat: function (val) {
                        return '$' + sizeup.util.numbers.format.addCommas(val);
                    }
                });

            me.sourceButton = new sizeup.controls.toggleButton(
                 {
                     button: me.container.find('.reportContainer .links .source'),
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


            me.sourceContent = me.container.find('.reportContainer .sourceContent').hide();
            me.considerations = me.container.find('.reportContainer .considerations');
            me.resources = me.container.find('.reportContainer .resources');
            me.description = me.container.find('.reportContainer .description');

            me.noData = me.container.find('.reportContent.noDataError').removeClass('hidden').hide();
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

        var displayReport = function () {
            me.reportContainer.setGauge(me.data.gauge);
            if (me.data.hasData) {
                me.noData.hide();
                me.reportData.show();
               
                me.table = new sizeup.charts.tableChart({
                    container: me.container.find('.table'),
                    rowTemplate: templates.get('tableRow'),
                    rows: me.data.table
                });

               
                me.description.html(templates.bind(templates.get("description"), me.data.description));
            }
            else {
                me.noData.show();
                me.reportData.hide();
                me.reportContainer.hideGauge();
            }


        };


        var runReport = function (e) {
            new sizeup.core.analytics().dashboardReportLoaded({ report: 'workersComp' });

            var notifier = new sizeup.core.notifier(function () {
                e.callback();
                displayReport();
            });

            me.data.enteredValue = me.reportContainer.getValue();
            jQuery.bbq.pushState({ workersComp: me.data.enteredValue });

            sizeup.api.data.getWorkersComp({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.State.Id }, notifier.getNotifier(function (data) { chartDataReturned({ State: data }); }));
            sizeup.api.data.getWorkersCompPercentage({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.State.Id, value: me.data.enteredValue }, notifier.getNotifier(function (data) { percentageDataReturned({ State: data }); }));
        };

        var percentageDataReturned = function (data) {
            if (data) {
                var val = 50 - (data.State.Percentage / 2);
                var percentage = sizeup.util.numbers.format.percentage(Math.abs(data.State.Percentage));

                me.data.description = {
                    Percentage: data.State.Percentage < 0 ? percentage + ' less' : data.State.Percentage == 0 ? 'the same' : percentage + ' greater'
                };

                me.data.gauge = {
                    value: val,
                    tooltip: 'You' + (data.State.Percentage < 0 ? ' save ' : ' overpay ') + percentage + ' compared to the average'
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

            me.data.table = {};
            me.data.table['me'] =
                {
                    name: 'My Business',
                    value: '$' + sizeup.util.numbers.format.addCommas(new Number(me.data.enteredValue).toFixed(2))
                };


            var indexes = ['State'];
            for (var x = 0; x < indexes.length; x++) {
                if (data[indexes[x]] != null) {
                    me.data.hasData = true;
                    me.data.table[indexes[x]] = {
                        name: data[indexes[x]].Name,
                        rank: data[indexes[x]].Rank,
                        value: '$' + sizeup.util.numbers.format.addCommas(data[indexes[x]].Average)
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