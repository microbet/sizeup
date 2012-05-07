(function () {
    sizeup.core.namespace('sizeup.views.dashboard.averageSalary');
    sizeup.views.dashboard.averageSalary = function (opts) {

        var me = {};
        var dataLayer = new sizeup.core.data();
        me.opts = opts;
        me.data = {};
        me.container = opts.container;
        me.data.enteredValue = opts.revenue;

        var init = function () {

            me.reportContainer = new sizeup.views.dashboard.reportContainer(
                {
                    container: me.container,
                    inputValidation: /^[0-9]+$/g,
                    inputCleaning: /[\$\,]/g,
                    events:
                    {
                        runReport: runReport,
                        valueChanged: function () { }
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
        };


        var displayReport = function (data) {

            var data = {

                grids: { horizontal: 3 }, gutters: { left: 45, top: 1, right: 50 }, bar: { height: 15, padding: 8 }, format: '',

                valueFormat: function(val){ return '$' + sizeup.util.numbers.format.addCommas(val);},
                container: me.container.find('.chart .container'),
                animationSpeed: 2000,
                title: 'average annual revenue per business',
                bars:[
                    {
                        value: 777,
                        label: '',
                        name: 'My Business',
                        color: '#5b0'
                    },
                    {
                        value: 2621032,
                        label: 'City',
                        name: 'San Francisco, CA',
                        color: '#0af'
                    },
                    {
                        value: 2621032,
                        label: 'County',
                        name: 'San Francisco County, CA',
                        color: '#0af'
                    },
                    {
                        value: 3169992,
                        label: 'Metro',
                        name: 'San Francisco-Oakland-Fremont, CA',
                        color: '#0af'
                    },
                    {
                        value: 2482152,
                        label: 'State',
                        name: 'California',
                        color: '#0af'
                    },
                    {
                        value: 2615169,
                        label: 'Nation',
                        name: 'USA', 
                        color: '#0af'
                    }
                ],
                marker: {
                    value: 123456,
                    label: 'National Median',
                    color: '#f60'
                }
            };
            var chart = new sizeup.charts.barChart(data);
            chart.draw();
        };

        var runReport = function (e) {
            setTimeout(function () {
                e.callback();
                displayReport();
            }, 250);
        };

        var fadeInPrompt = function (delay, callback) {
            me.reportContainer.fadeInPrompt(delay, callback);
        };


        var publicObj = {
            fadeInPrompt: function (delay, callback) {
                fadeInPrompt(delay, callback);
            }
        };
        init();
        return publicObj;

    };
})();