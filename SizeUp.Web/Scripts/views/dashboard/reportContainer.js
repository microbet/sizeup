(function () {
    sizeup.core.namespace('sizeup.views.dashboard.reportContainer');
    sizeup.views.dashboard.reportContainer = function (opts) {

        var defaults =
        {
            displayValue:'',
            inputValidation: /.*/g,
            inputCleaning:new RegExp(''),
            inputFormat: function (val) { return val; },
            events: {
                runReport: function (e) { },
                valueChanged: function (e) { }
            }
        };
        var me = {};
        me.data = {};
        me.opts = $.extend(true, defaults, opts);
        me.container = opts.container;
        me.isReportStale = true;

        

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

            setValue(me.opts.displayValue);
        };



       
        var reportLoaded = function () {
            showReport();
        };

        var getReport = function () {
            showGauge();
            hideReport();
            me.body.show();
            var e =
            {
                callback: reportLoaded,
            }
            me.opts.events.runReport(e);
        };


        var showReport = function () {
            me.loading.hide();
            me.reportContainer.show();
        };

        var hideReport = function () {
            me.body.hide();
            me.loading.show();
            me.reportContainer.hide();
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
            var reg = new RegExp(me.opts.inputCleaning);
            return val.replace(reg, '');
        };

        var isValid = function (val) {
            var reg = new RegExp(me.opts.inputValidation);
            return reg.test(val);
        };


        var doSubmit = function () {
            if (me.isReportStale) {
                var v = $.trim(cleanInput(me.valueBox.val()));
                if (isValid(v)) {
                    me.isReportStale = false;
                    getReport();
                    setValue(v);
                }
                else if (v == '') {
                    hideAllControls();
                    hideReport();
                    fadeInPrompt(0);
                    setValue('');
                }
                else {
                    setValue('');
                }
            }
        };

        var onTextboxKeypress = function (e) {
            var val = $.trim(cleanInput(me.valueBox.val()));
            if (e.charCode != 0) {
                me.isReportStale = true;
                if (val != '') {
                    showRunReport();
                } else {
                    hideAllControls();
                }
            }
            else if (e.keyCode == 8 || e.keyCode == 46) {
                me.isReportStale = true;
            }
            else if (e.keyCode == 13) {
                if (e.keyCode == 13) {
                    doSubmit();
                }
            }
        };


        var onTextboxBlur = function () {
            doSubmit();
        };

        var setValue = function (val) {
            if (val != '') {
                me.data.value = val;
                me.valueBox.val(me.opts.inputFormat(val));
            }
            else {
                me.data.value = '';
                me.valueBox.val('');
            }
        };

        var getValue = function () {
            return me.data.value;
        };

        var publicObj = {
            fadeInPrompt: function (delay, callback) {
                fadeInPrompt(delay, callback);
            },
            setDisplayValue: function (value) {
                setValue(value);
            },
            getDisplayValue: function () {
                return getValue();
            }
        };
        init();
        return publicObj;

    };
})();