(function () {
    sizeup.core.namespace('sizeup.views.dashboard.healthcareCost');
    sizeup.views.dashboard.healthcareCost = function (opts) {

        var me = {};
        var templates = new sizeup.core.templates(opts.container);
        me.opts = opts;
        me.data = {};
        me.container = opts.container;
        me.data.enteredValue = jQuery.bbq.getState().healthcareCost;
        me.data.enteredEmployees = jQuery.bbq.getState().employees;
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
                                jQuery.bbq.removeState('healthcareCost');
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


            me.tableContainer = me.container.find('.tableContainer');

            me.sourceContent = me.container.find('.reportContainer .sourceContent').hide();
            me.considerations = me.container.find('.reportContainer .considerations');
            me.resources = me.container.find('.reportContainer .resources');
            me.description = me.container.find('.reportContainer .description');




            $(window).bind('hashchange', function (e) { hashChanged(e); });


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

        var runReport = function (e) {
            new sizeup.core.analytics().dashboardReportLoaded({ report: 'healthcareCost' });

            var notifier = new sizeup.core.notifier(function () {
                e.callback();
                displayReport();
            });

            me.data.enteredValue = me.reportContainer.getValue();
            jQuery.bbq.pushState({ healthcareCost: me.data.enteredValue });
            if (me.data.enteredEmployees) {
                sizeup.api.data.getHealthcareCost({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.State.Id, employees: me.data.enteredEmployees }, notifier.getNotifier(chartDataReturned));
                sizeup.api.data.getHealthcareCostPercentage({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.State.Id, value: me.data.enteredValue }, notifier.getNotifier(percentageDataReturned));
            }
            else {
                sizeup.api.data.getHealthcareCost({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.State.Id }, notifier.getNotifier(chartDataReturned));
                sizeup.api.data.getHealthcareCostPercentage({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.State.Id, value: me.data.enteredValue }, notifier.getNotifier(percentageDataReturned));
            }
        };

        var percentageDataReturned = function (data) {
            if (data) {
                var val = 50 - (data.Percentage / 2);
                var percentage = sizeup.util.numbers.format.percentage(Math.abs(data.Percentage));
                me.data.gauge = {
                    value: val,
                    tooltip: 'you' + (data.Percentage < 0 ? ' save ' : ' overpay ') + percentage + ' compared to the average'
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
            me.data.raw = data;
            me.data.table = {
                state: data.State == null ? 'no data' : '$' + sizeup.util.numbers.format.addCommas(data.State),
                stateRank: data.State == null ? '' : data.StateRank,

                industry: data.Industry == null ? 'no data' : '$' + sizeup.util.numbers.format.addCommas(data.Industry),
                industryRank: data.Industry == null ? '' : data.IndustryRank,

                firmSize: data.FirmSize == null ? 'no data' : '$' + sizeup.util.numbers.format.addCommas(data.FirmSize),
                firmSizeRank: data.FirmSize == null ? '' : data.FirmSizeRank,

                currentBusiness: '$' + sizeup.util.numbers.format.addCommas(me.data.enteredValue)
            };
        };


        var moreLessFormatter = function(value, compareValue){
            var out = "";
            if(value == compareValue){
                out = "the same";
            }
            else if(value < compareValue){
                out = '$' + sizeup.util.numbers.format.addCommas(compareValue - value) + " less";
            }
            else{
                out = '$' + sizeup.util.numbers.format.addCommas(value - compareValue) + " more";
            }
            return out;
        };


        var displayReport = function () {

            me.reportContainer.setGauge(me.data.gauge);
       
            me.reportData.show();


            me.tableContainer.html(templates.bind(templates.get("table"), me.data.table));

                
            me.data.description = {
                firmSizeDifference: moreLessFormatter(me.data.enteredValue, me.data.raw.FirmSize),
                industryDifference: moreLessFormatter(me.data.enteredValue, me.data.raw.Industry)
            };

            var description = '';
            if (me.data.enteredEmployees == null) {
                description = templates.bind(templates.get("noEmployeeDescription"), me.data.description);
            }
            else
            {
                description = templates.bind(templates.get("description"), me.data.description);
                if (me.data.enteredValue > me.data.raw.FirmSize) {
                    description = description + templates.bind(templates.get("overpayDescription"), me.data.description);
                }
            }
            me.description.html(description);              

        };

        var hashChanged = function (e) {
            var index = e.getState('employees');
            if (index != me.data.enteredEmployees && me.data.enteredValue) {
                me.data.enteredEmployees = index;
                runReport({ callback: function () { } });
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


