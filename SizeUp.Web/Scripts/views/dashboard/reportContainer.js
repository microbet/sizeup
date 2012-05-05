(function () {
    sizeup.core.namespace('sizeup.views.dashboard.reportContainer');
    sizeup.views.dashboard.reportContainer = function (opts) {

        var me = {};
        me.data = {};
        me.opts = opts;
        me.container = opts.container;
        me.data.displayValue = opts.displayValue;

        me.opts.events = me.opts.events || {};
        me.events = {
            runReport: me.opts.events.runReport || function () { },
            valueChanged: me.opts.events.valueChanged || function () { }
        };
        

        var init = function () {
            me.gauge = new sizeup.controls.gauge({ element: me.container.find('.gauge') });
            me.prompt = me.container.find('.prompt');
            me.runReport = me.container.find('.runReport');
            me.valueBox = me.container.find('.valueBox');
            me.body = me.container.find('.body');
            me.loading = me.container.find('.body .loading');
            me.reportContainer = me.container.find('.body .reportContainer');

            me.gauge.hide();
            me.prompt.hide();
            me.runReport.hide();
            me.body.hide();
            me.reportContainer.hide();


            me.valueBox.blur(function () { onTextboxBlur(); });
            me.valueBox.keypress(function (e) { onTextboxKeypress(e); });

            if (me.data.enteredValue) {
                me.valueBox.val(me.data.enteredValue);
                me.valueBox.blur();
            };
        };



       

        var getReport = function () {
            showGauge();
            me.body.show();
            displayReport();
        };

        var hideAllControls = function () {
            me.gauge.hide();
            me.prompt.hide();
            me.runReport.hide();
        };

        var showRunReport = function () {
            me.gauge.hide();
            me.prompt.hide();
            me.runReport.show();
        };

        var showGauge = function () {
            me.gauge.fadeIn();
            me.prompt.hide();
            me.runReport.hide();
        };

        var fadeInPrompt = function (delay, callback) {
            setTimeout(function () {
                me.prompt.fadeIn(500, callback);
            }, delay);
        };

        var cleanInput = function (val) {
            return val.replace(/\$|\,/g, '');
        };

        var validateInput = function (val) {
            var v = cleanInput(val);
            return /^[0-9]+$/g.test(v);
        };

        var formatInput = function (val) {
            return '$' + sizeup.util.numbers.format.addCommas(val);
        };

        var doSubmit = function () {
            var v = cleanInput(me.valueBox.val());
            if (validateInput(v)) {
                me.valueBox.val(formatInput(v));
                me.data.enteredValue = v;
                getReport();
            }
            else if (v == '') {
                hideAllControls();
                me.body.hide();
                fadeInPrompt(0);
            }
            else {
                me.valueBox.val('');
            }
        };

        var onTextboxKeypress = function (e) {
            var v = me.valueBox.val();
            if ($.trim(v) != '') {
                showRunReport();
                if (e.keyCode == 13) {
                    doSubmit();
                }
            } else {
                hideAllControls();
            }
        };


        var onTextboxBlur = function () {
            doSubmit();
        };

        var publicObj = {
            fadeInPrompt: function (delay, callback) {
                fadeInPrompt(delay, callback);
            },
            setDisplayValue: function (value) {

            },
            getDisplayValue: function () {

            }
        };
        init();
        return publicObj;

    };
})();