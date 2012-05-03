(function () {
    sizeup.core.namespace('sizeup.views.dashboard.revenue');
    sizeup.views.dashboard.revenue = function (opts) {

        var me = {};
        var dataLayer = new sizeup.core.data();
        me.opts = opts;
        me.data = {};
        me.container = opts.container;
        me.data.revenue = opts.revenue;

        var init = function () {
            var gauge = me.container.find('.gauge');
            

            me.source = new sizeup.controls.contentExpander(
                {
                    button: me.container.find('.reportContainer .links .source'),
                    contentPanel: me.container.find('.reportContainer .sourceContent')
                });

            me.gauge = new sizeup.controls.gauge({ element: gauge });
            me.prompt = me.container.find('.prompt');
            me.runReport = me.container.find('.runReport');
            me.valueBox = me.container.find('.valueBox');
            me.chartContainer = me.container.find('.chart .container');

            me.body = me.container.find('.body');
            me.loading = me.container.find('.body .loading');
            me.reportContainer = me.container.find('.body .reportContainer');
            
            gauge.hide().removeClass('hidden');
            me.prompt.hide().removeClass('hidden');
            me.runReport.hide().removeClass('hidden');
            me.body.hide().removeClass('hidden');
            me.reportContainer.hide().removeClass('hidden');

            me.valueBox.blur(function () { onTextboxBlur(); });
            me.valueBox.keypress(function (e) { onTextboxKeypress(e); });

            if (me.data.revenue) {
                me.valueBox.val(me.data.revenue);
                me.valueBox.blur();
            };
        };


  
        var displayReport = function () {
            me.loading.hide();
            me.reportContainer.show();
            me.chartContainer.empty();
            var _chart_AVERAGE_REVENUE = new gisp.charts.BarChart(me.chartContainer, { token: 'AVERAGE_REVENUE', grids: { horizontal: 3 }, gutters: { left: 45, top: 1, right: 50 }, bar: { height: 15, padding: 8 }, format: gisp.charts.DataFormat.DOLLARS, width: 0, legend: "average annual revenue per business" });
            _chart_AVERAGE_REVENUE.AddBar("BZ_0", 123, null, "", "My Business", "#5b0", "/main/business/MBPC.aspx?IDs=B:e461cc66-2946-4416-9417-fc381bd4b14c&NAICS=591205");
            _chart_AVERAGE_REVENUE.AddBar("CTY_0", 2621032.25806452, null, "City", "San Francisco, CA", "#0af", "/main/business/MBPC.aspx?IDs=G:246fa799-3bfe-4f81-ab34-16c312a71c31&NAICS=591205");
            _chart_AVERAGE_REVENUE.AddBar("CNTY_0", 2621032.25806452, null, "County", "San Francisco County, CA", "#0af", "/main/business/MBPC.aspx?IDs=G:2c43ef3a-a5ea-4865-a493-65617a8d8ba5&NAICS=591205");
            _chart_AVERAGE_REVENUE.AddBar("MTR_0", 3169992.39543726, null, "Metro", "San Francisco-Oakland-Fremont, CA ", "#0af", "/main/business/MBPC.aspx?IDs=G:12dba983-f064-4090-9819-16eaa725a137&NAICS=591205");
            _chart_AVERAGE_REVENUE.AddBar("ST_0", 2482152.34300892, null, "State", "California", "#0af", "/main/business/MBPC.aspx?IDs=G:76fa1c33-f5e8-498a-8647-df83a8e5ebf8&NAICS=591205");
            _chart_AVERAGE_REVENUE.AddBar("NTL", 2615169.88945303, null, "Nation", "USA", "#0af", null);
            _chart_AVERAGE_REVENUE.AddMarker("NTL_MED", 1281000, null, "National Median", "National Median", "#f60", null);
            _chart_AVERAGE_REVENUE.AddSource('#sourceLink.AVERAGE_REVENUEID', '#sourceContent.AVERAGE_REVENUEID');
            _chart_AVERAGE_REVENUE.RedrawChart(2000);
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

        var fadeInPrompt = function (callback) {
            me.prompt.fadeIn(500, callback);
        };

        var cleanInput = function (val) {
            return val.replace(/\$|\,/g, '');
        };

        var validateInput = function (val) {
            var v = cleanInput(val);
            return /[0-9]+/g.test(v);
        };

        var formatInput = function (val) {
            return '$' + sizeup.util.numbers.format.addCommas(val);
        };

        var doSubmit = function () {
            var v = cleanInput(me.valueBox.val());
            if (validateInput(v)) {
                me.valueBox.val(formatInput(v));
                me.data.revenue = v;
                getReport();
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
            fadeInPrompt: function (callback) {
                fadeInPrompt(callback);
            }
        };
        init();
        return publicObj;

    };
})();