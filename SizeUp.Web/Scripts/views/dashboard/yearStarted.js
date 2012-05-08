(function () {
    sizeup.core.namespace('sizeup.views.dashboard.yearStarted');
    sizeup.views.dashboard.yearStarted = function (opts) {

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
                    events:
                    {
                        runReport: runReport,
                        valueChanged: function () { }
                    },
                    inputFormat: function (val) {
                        return val;
                    }
                });

            me.source = new sizeup.controls.contentExpander(
                {
                    button: me.container.find('.reportContainer .links .source'),
                    contentPanel: me.container.find('.reportContainer .sourceContent')
                });



        };

        var runReport = function (e) {
            e.callback();
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