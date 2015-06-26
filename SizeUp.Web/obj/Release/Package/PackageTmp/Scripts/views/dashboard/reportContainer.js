(function () {
    sizeup.core.namespace('sizeup.views.dashboard.reportContainer');
    sizeup.views.dashboard.reportContainer = function (opts) {

        var defaults =
        {
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
        me.fadeTimeout = null;
        

        var init = function () {

            me.gauge = new sizeup.controls.gauge({ element: me.container.find('.gauge') });
            me.prompt = me.container.find('.prompt');
            me.runReport = me.container.find('.runReport');
            me.valueBox = me.container.find('.valueBox');
            me.body = me.container.find('.body');
            me.loading = me.container.find('.body .loading');
            me.reportContainer = me.container.find('.body .reportContainer');
            me.toggle = me.container.find('.reportToggle');

            me.gauge.hide();
            me.prompt.hide();
            me.runReport.hide();
            me.body.hide();
            me.reportContainer.hide();

            me.toggle.click(function () { reportToggleClicked(); });
            me.valueBox.blur(function () { onTextboxBlur(); });
            me.valueBox.keydown(function (e) { onTextboxKeypress(e); });

            me.valueBox.val('');
        };


        var reportToggleClicked = function () {
            if (me.body.is(':visible')) {
                collapseReport();
            } else {
                expandReport();
            }
        };

       
        var reportLoaded = function () {
            showReport();
        };

        var makeStale = function () {
            me.isReportStale = true;
            me.toggle.removeClass('active');
        };

        var makeFresh = function () {
            me.isReportStale = false;
            me.toggle.addClass('active');
        };

        var getReport = function () {
            showGauge();
            hideReport();
            me.body.show();
            me.toggle.addClass('open');
            var e =
            {
                callback: reportLoaded,
            }
            me.opts.events.runReport(e);
        };


        var showReport = function () {
            me.loading.hide();
            me.reportContainer.show();
            me.body.show();
            me.toggle.addClass('open');
        };

        var hideReport = function () {
            me.body.hide();
            me.loading.show();
            me.reportContainer.hide();
            me.toggle.removeClass('open');
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

        var hideGauge = function () {
            me.gauge.hide();
        };

        var fadeInPrompt = function (delay, callback) {
            me.fadeTimeout = setTimeout(function () {
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
                    makeFresh();
                    setValue(v);
                    getReport();
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
             if (e.keyCode == 8 || e.keyCode == 46) {
                makeStale();
            }
             else if (e.keyCode == 13) {
                 if (e.keyCode == 13) {
                     doSubmit();
                 }
             }
             else {
                 makeStale();
                 if (val != '') {
                     showRunReport();
                 } else {
                     hideAllControls();
                 }
             }
        };

        var setGauge = function (data) {
            me.gauge.setValue(data.value, data.tooltip);
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
            me.opts.events.valueChanged({ value: me.data.value });
        };

        var getValue = function () {
            return me.data.value;
        };

        var doGetReport = function () {
            makeFresh();
            getReport();
        };

        var forceSubmit = function () {
            clearTimeout(me.fadeTimeout);
            makeStale();
            doSubmit();
        };

        var expandReport = function () {
            if (!me.isReportStale) {
                showReport();
            }
        };

        var collapseReport = function () {
            hideReport();
        };

        var clearReport = function () {
            setValue('');
            hideAllControls();
            hideReport();
            fadeInPrompt(0);
        };

        var publicObj = {
            fadeInPrompt: function (delay, callback) {
                fadeInPrompt(delay, callback);
            },
            setValue: function (value) {
                setValue(value);
            },
            getValue: function () {
                return getValue();
            },
            setGauge: function (data) {
                setGauge(data);
            },
            hideGauge: function(){
                hideGauge();
            },
            doSubmit: function () {
                forceSubmit();
            },
            expandReport: function () {
                expandReport();
            },
            collapseReport: function () {
                collapseReport();
            },
            doGetReport: function () {
                doGetReport();
            },
            clearReport: function () {
                clearReport();
            }
        };
        init();
        return publicObj;

    };
})();