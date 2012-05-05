(function () {
    sizeup.core.namespace('sizeup.views.dashboard.healthcareCost');
    sizeup.views.dashboard.healthcareCost = function (opts) {

        var me = {};
        var dataLayer = new sizeup.core.data();
        me.opts = opts;
        me.data = {};
        me.container = opts.container;
        me.data.enteredValue = opts.revenue;

        var init = function () {


            me.source = new sizeup.controls.contentExpander(
                {
                    button: me.container.find('.reportContainer .links .source'),
                    contentPanel: me.container.find('.reportContainer .sourceContent')
                });

            me.reportContainer = new sizeup.views.dashboard.reportContainer(
                {
                    container: me.container
                });

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