(function () {
    sizeup.core.namespace('sizeup.views.dashboard.averageSalary');
    sizeup.views.dashboard.averageSalary = function (opts) {

        var me = {};
        var dataLayer = new sizeup.core.data();
        me.opts = opts;
        me.data = {};
        me.container = opts.container;
        me.data.enteredValue = jQuery.bbq.getState().salary;
        me.data.hasData = false;

        var init = function () {
            me.reportContainer = new sizeup.views.dashboard.reportContainer(
                {
                    container: me.container,
                    inputValidation: /^[0-9]+$/g,
                    inputCleaning: /[\$\,]/g,
                    events:
                    {
                        runReport: runReport,
                        valueChanged: function (val) {  }
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


            me.description = me.container.find('.description');
            me.sourceContent = me.container.find('.reportContainer .sourceContent').hide();

            me.noData = me.container.find('.noDataError').hide();
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
      
        var displayReport = function () {

            me.reportContainer.setGauge(me.data.gauge);
            if (me.data.hasData) {
                me.noData.hide();
                me.reportData.show();

                me.map = new sizeup.maps.heatMap({
                    container: me.container.find('.reportContainer .map'),
                    overlays:[
                        {
                            tileUrl: "/tiles/salary/state/",
                            legendSource: function (callback) {
                                dataLayer.getSalaryBandsByState({ 
                                    industryId: me.opts.industryId,
                                    bands: 7 
                                }, callback);
                            },
                            legendTitle: 'Average Salary by state in the USA',
                            legendFormat: function(val){ return '$' + sizeup.util.numbers.format.abbreviate(val);},
                            industryId: me.opts.industryId,
                            minZoom: 0,
                            maxZoom: 4,
                            colors:[
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
                            tileUrl: "/tiles/salary/county/",
                            legendSource: function (callback) {
                                dataLayer.getSalaryBandsByCounty({
                                    industryId: me.opts.industryId,
                                    bands: 7,
                                    boundingEntityId: 's' + me.opts.locations.state.Id
                                }, callback);
                            },
                            legendTitle: 'Average Salary by county in ' + me.opts.locations.state.Name,
                            legendFormat: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); },
                            industryId: me.opts.industryId,
                            minZoom: 5,
                            maxZoom: 8,
                            boundingEntityId: 's' + me.opts.locations.state.Id,
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
                            tileUrl: "/tiles/salary/county/",
                            legendSource:function (callback) {
                                dataLayer.getSalaryBandsByCounty({
                                    industryId: me.opts.industryId,
                                    bands: 7,
                                    boundingEntityId: me.opts.locations.metro ? 'm' + me.opts.locations.metro.Id : 's' + me.opts.locations.state.Id
                                }, callback); 
                            },
                            legendTitle: 'Average Salary by county in ' +   (me.opts.locations.metro ? me.opts.locations.metro.Name + ' (Metro)' : me.opts.locations.state.Name),
                            legendFormat: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); },
                            industryId: me.opts.industryId,
                            minZoom: 9,
                            maxZoom: 32,
                            boundingEntityId: me.opts.locations.metro ? 'm' + me.opts.locations.metro.Id : 's' + me.opts.locations.state.Id,
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
                me.map.setCenter(me.opts.center);
                me.chart = new sizeup.charts.barChart({

                    valueFormat: function(val){ return '$' + sizeup.util.numbers.format.addCommas(Math.floor(val));},
                    container: me.container.find('.chart .container'),
                    title: 'average annual revenue per business',
                    bars: me.data.chart
                });
                me.chart.draw();

                me.table = new sizeup.charts.tableChart({
                    container: me.container.find('.table').hide(),
                    rows:me.data.table
                });

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
            jQuery.bbq.pushState({ salary: me.data.enteredValue });
            dataLayer.getSalaryChart({ industryId: me.opts.industryId, countyId: me.opts.locations.county.Id }, notifier.getNotifier(chartDataReturned));
            dataLayer.getSalaryPercentile({ industryId: me.opts.industryId, countyId: me.opts.locations.county.Id, value: me.data.enteredValue }, notifier.getNotifier(percentileDataReturned));
        };

        var percentileDataReturned = function (data) {
            if (data) {
                me.data.hasData = true;
                var val = 50 + (data.Percentile / 2);
                var percentage = sizeup.util.numbers.format.percentage(Math.abs(data.Percentile));
                me.data.gauge = {
                    value: val,
                    tooltip: data.Percentile < 0 ? percentage + ' Below Average' : percentage + ' Above Average'
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
            me.data.chart = [
                {
                    value: me.data.enteredValue,
                    label: '',
                    name: 'My Business',
                    color: '#5b0'
                }
            ]

            me.data.table = [
                {
                    name: 'My Business',
                    value: '$' + sizeup.util.numbers.format.addCommas(me.data.enteredValue)
                }
            ]
       
            var indexes = ['County', 'Metro', 'State', 'Nation'];
            for (var x = 0; x < indexes.length; x++) {
                if (data[indexes[x]] != null) {
                    me.data.chart.push(
                    {
                        value: parseInt(data[indexes[x]].Value),
                        label: indexes[x],
                        name: data[indexes[x]].Name,
                        color: '#0af'
                    });

                    me.data.table.push({
                        name: data[indexes[x]].Name,
                        value: '$' + sizeup.util.numbers.format.addCommas(parseInt(data[indexes[x]].Value))
                    });
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


        var publicObj = {

            fadeInPrompt: function (delay, callback) {
                fadeInPrompt(delay, callback);
            },
            setupReport: function () {
                setupReport();
            }
        };
        init();
        return publicObj;

    };
})();