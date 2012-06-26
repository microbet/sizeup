(function () {
    sizeup.core.namespace('sizeup.views.dashboard.yearStarted');
    sizeup.views.dashboard.yearStarted = function (opts) {

        var me = {};
        var dataLayer = new sizeup.core.data();
        var templates = new sizeup.core.templates(opts.container);
        me.opts = opts;
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
                    button: me.container.find('.reportContainer .turnover .links .source'),
                    onClick: function () { toggleSource('turnover'); }
                });

            me.considerationToggle = new sizeup.controls.toggleButton(
                {
                    button: me.container.find('.reportSidebar.turnover .considerationToggle'),
                    onClick: function () { toggleConsiderations('turnover'); }
                });

            me.resourceToggle = new sizeup.controls.toggleButton(
                {
                    button: me.container.find('.reportSidebar.turnover .resourcesToggle'),
                    onClick: function () { toggleResources('turnover'); }
                });


            me.sourceContent = me.container.find('.reportContainer .turnover .sourceContent').hide();
            me.considerations = me.container.find('.reportContainer .turnover .considerations');
            me.resources = me.container.find('.reportContainer .turnover .resources');
            me.description = me.container.find('.reportContainer .turnover .description');





            me.noData = me.container.find('.noDataError').hide();
            me.reportData = me.container.find('.reportData');


            if (me.data.enteredValue) {
                me.reportContainer.setValue(me.data.enteredValue);
            }

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


        var displayReport = function () {

            me.reportContainer.setGauge(me.data.gauge);
            if (me.data.hasData) {
                me.noData.hide();
                me.reportData.show();
                me.table = new sizeup.charts.tableChart({
                    container: me.container.find('.turnover .table'),
                    rowTemplate: templates.get('turnoverTableRow'),
                    rows: me.data.turnover.table
                });

                me.reportContainer.setValue(sizeup.util.numbers.format.round(me.data.raw['County'].turnover, 0));

                me.data.description = {
                    Turnover: sizeup.util.numbers.format.percentage(me.data.raw['County'].turnover, 1),
                    NAICS4: me.opts.report.IndustryDetails.NAICS4,
                    Industry: me.opts.report.IndustryDetails.Industry
                };

                me.description.html(templates.bind(templates.get("turnoverDescription"), me.data.description));

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

            dataLayer.getYearStartedChart({ industryId: me.opts.report.IndustryDetails.Industry.Id, cityId: me.opts.report.Locations.City.Id,  countyId: me.opts.report.Locations.County.Id }, notifier.getNotifier(chartDataReturned));
            dataLayer.getYearStartedPercentile({ industryId: me.opts.report.IndustryDetails.Industry.Id, cityId: me.opts.report.Locations.City.Id, countyId: me.opts.report.Locations.County.Id }, notifier.getNotifier(percentileDataReturned));
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

        var chartDataReturned = function (data) {

            me.data.turnover.table = {};
            me.data.turnover.raw = {};
            var indexes = ['County', 'Metro', 'State', 'Nation'];
            for (var x = 0; x < indexes.length; x++) {
                if (data[indexes[x]] != null) {
                    me.data.turnover.raw[indexes[x]] = {
                        hires: data[indexes[x]].Hires,
                        separations: data[indexes[x]].Separations,
                        turnover: data[indexes[x]].Turnover
                    };

                    me.data.turnover.table[indexes[x]] = {
                        name: data[indexes[x]].Name,
                        hires: sizeup.util.numbers.format.addCommas(data[indexes[x]].Hires),
                        separations: sizeup.util.numbers.format.addCommas(data[indexes[x]].Separations),
                        turnover: sizeup.util.numbers.format.percentage(data[indexes[x]].Turnover, 2)
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