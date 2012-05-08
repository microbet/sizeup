(function () {
    sizeup.core.namespace('sizeup.views.dashboard.averageSalary');
    sizeup.views.dashboard.averageSalary = function (opts) {

        var me = {};
        var dataLayer = new sizeup.core.data();
        me.opts = opts;
        me.data = {};
        me.container = opts.container;
        me.data.enteredValue = sizeup.core.urlParams.getParams().salary;

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

            me.source = new sizeup.controls.contentExpander(
                {
                    button: me.container.find('.reportContainer .links .source'),
                    contentPanel: me.container.find('.reportContainer .sourceContent')
                });

            if (me.data.enteredValue) {
                me.reportContainer.setValue(me.data.enteredValue);
            }
        };


        var displayReport = function () {

            me.reportContainer.setGauge(me.data.gauge);
            me.map = new sizeup.maps.map({
                container: me.container.find('.reportContainer .map')
            });

            var chart = new sizeup.charts.barChart(me.data.chart);
            chart.draw();
        };

        var runReport = function (e) {
            var notifier = new sizeup.core.notifier(function () {
                e.callback();
                displayReport();
            });

            me.data.enteredValue = me.reportContainer.getValue();
            sizeup.core.urlParams.add({ salary: me.data.enteredValue });
            dataLayer.getSalaryChart({ industryId: 8589, countyId: 222 }, notifier.getNotifier(chartDataReturned));
            dataLayer.getSalaryPercentile({ industryId: 8589, countyId: 222, value: me.data.enteredValue }, notifier.getNotifier(percentileDataReturned));
           
        };

        var percentileDataReturned = function (data) {

            var val = 50 + (data.Percentile/2);
            var percentage = sizeup.util.numbers.format.percentage(Math.abs(data.Percentile));
            me.data.gauge = {
                value: val,
                tooltip: data.Percentile < 0 ? percentage + ' Below Average' : percentage + ' Above Average'
            };
        };

        var chartDataReturned = function (data) {
            me.data.chart = {

                valueFormat: function(val){ return '$' + sizeup.util.numbers.format.addCommas(Math.floor(val));},
                container: me.container.find('.chart .container'),
                title: 'average annual revenue per business',
                bars:[
                    {
                        value: me.data.enteredValue,
                        label: '',
                        name: 'My Business',
                        color: '#5b0'
                    }
                ]
            };
          
        
            var indexes = ['County', 'Metro', 'State', 'Nation'];
            for (var x = 0; x < indexes.length; x++) {
                if (data[indexes[x]] != null) {
                    me.data.chart.bars.push(
                    {
                        value: parseInt(data[indexes[x]].Value),
                        label: indexes[x],
                        name: data[indexes[x]].Name,
                        color: '#0af'
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