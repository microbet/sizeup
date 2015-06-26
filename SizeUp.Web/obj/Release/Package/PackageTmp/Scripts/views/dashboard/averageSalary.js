(function () {
    sizeup.core.namespace('sizeup.views.dashboard.averageSalary');
    sizeup.views.dashboard.averageSalary = function (opts) {

        var me = {};
        var templates = new sizeup.core.templates(opts.container);
        me.opts = opts;
        me.data = {};
        me.container = opts.container;
        me.data.enteredValue = jQuery.bbq.getState().salary;
        me.data.hasData = false;
        me.data.description = {};

        var init = function () {
            me.reportContainer = new sizeup.views.dashboard.reportContainer(
                {
                    container: me.container,
                    inputValidation: /^[0-9\.]+$/g,
                    inputCleaning: /^0$|^\$0$|[\$\,]|\.[0-9]*/g,
                    events:
                    {
                        runReport: runReport,
                        valueChanged: function (val) {
                            if (val.value == '') {
                                jQuery.bbq.removeState('salary');
                            }
                        }
                    },
                    inputFormat: function (val) {
                        return '$' + sizeup.util.numbers.format.addCommas(sizeup.util.numbers.format.round(val, 0));
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

            me.considerationToggle = new sizeup.controls.toggleButton(
                {
                    button: me.container.find('.reportSidebar .considerationToggle'),
                    onClick: function () { toggleConsiderations(); }
                });

            me.resourceToggle = new sizeup.controls.toggleButton(
                {
                    button: me.container.find('.reportSidebar .resourcesToggle'),
                    onClick: function () { toggleResources(); }
                });

            me.question = new sizeup.controls.question({
                answerClicked: function (index) { answerClicked(index); },
                answerCleared: function (index) { answerCleared(index); },
                questionContainer: me.container.find('.reportSidebar .question'),
                clearingButtons: [me.container.find('.reportSidebar .clearer')],
                answers: [
                        {
                            question: me.container.find('.reportSidebar .question .startup'),
                            answer: me.container.find('.reportSidebar .answer.startup'),
                            index: 'startup'
                        },
                        {
                            question: me.container.find('.reportSidebar .question .established'),
                            answer: me.container.find('.reportSidebar .answer.established'),
                            index: 'established'
                        }
                ]
            });

            me.map = new sizeup.maps.map({
                container: me.container.find('.mapWrapper.container .map')
            });

            me.textAlternative = me.container.find('.mapWrapper .textAlternative');
            me.textAlternative.click(textAlternativeClicked);


            var index = jQuery.bbq.getState('businessType');
            if (index) {
                me.question.showAnswer(index);
            }
            $(window).bind('hashchange', function (e) { hashChanged(e); });

            me.sourceContent = me.container.find('.reportContainer .sourceContent').hide();
            me.considerations = me.container.find('.reportContainer .considerations');
            me.resources = me.container.find('.reportContainer .resources');
            me.description = me.container.find('.reportContainer .description');



            me.reportData = me.container.find('.reportData');
            me.noData = me.container.find('.reportContent.noDataError').removeClass('hidden').hide();
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

        var toggleConsiderations = function () {
            me.considerations.toggleClass('collapsed', 1000);
        };

        var toggleResources = function () {
            me.resources.toggleClass('collapsed', 1000);
        };

        var answerClicked = function (index) {
            jQuery.bbq.pushState({ businessType: index });
        };

        var answerCleared = function () {
            jQuery.bbq.removeState('businessType');
        };

        var hashChanged = function (e) {
            var index = e.getState('businessType');
            me.question.showAnswer(index);
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


        var setHeatmap = function () {
            me.map.clearOverlays();
            var overlays = me.overlay.getOverlays();
            me.map.triggerEvent('resize');
            me.map.setCenter(new sizeup.maps.latLng({ lat: me.opts.centroid.Lat, lng: me.opts.centroid.Lng }));
            me.map.setZoom(me.overlay.getZoomExtent().County - 1);
            me.map.addEventListener('zoom_changed', mapZoomUpdated);
            for (var x in overlays) {
                me.map.addOverlay(overlays[x], 0);
            }

            setLegend();
        };

        var setLegend = function () {
            var z = me.map.getZoom();
            var callback = function (legend) {
                me.map.setLegend(legend);
            };

            me.overlay.getLegend(z, callback);
            me.data.textAlternative = me.overlay.getParams(z);
        };

        var mapZoomUpdated = function () {
            setLegend();
        };

        var textAlternativeClicked = function () {
            var url = '/accessibility/averageSalary/';
            window.open(jQuery.param.querystring(url, me.data.textAlternative), '_blank');
        };

      

        var displayReport = function () {

            if (me.data.noData) {
                me.noData.show();
                me.reportData.hide();
                me.reportContainer.hideGauge();
            }
            else {
                me.reportContainer.setGauge(me.data.gauge);
                me.reportData.show();
                me.noData.hide();

                sizeup.api.data.getZoomExtent({ placeId: me.opts.report.CurrentPlace.Id, width: me.map.getWidth() }, function (data) {
                    me.overlay = new sizeup.maps.heatMapOverlays({
                        attribute: sizeup.api.tiles.overlayAttributes.heatmap.averageSalary,
                        place: me.opts.report.CurrentPlace,
                        params: { industryId: me.opts.report.CurrentIndustry.Id },
                        zoomExtent: data,
                        attributeLabel: 'Average Salary',
                        format: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); },
                        legendData: sizeup.api.data.getAverageSalaryBands,
                        templates: templates
                    });
                    setHeatmap();
                });



                me.chart = new sizeup.charts.barChart({

                    valueFormat: function (val) { return '$' + sizeup.util.numbers.format.addCommas(Math.floor(val)); },
                    container: me.container.find('.chart .container'),
                    title: 'average annual salary',
                    bars: me.data.chart
                });
                me.chart.draw();

                me.table = new sizeup.charts.tableChart({
                    container: me.container.find('.table').hide(),
                    rowTemplate: templates.get('tableRow'),
                    rows: me.data.table
                });


                me.data.description.NAICS6 = me.opts.report.CurrentIndustry.NAICS6;
                me.data.description.Salary = me.data.table['Nation'].value;

                me.description.html(templates.bind(templates.get("description"), me.data.description));
            }
           
        };

        var runReport = function (e) {
            new sizeup.core.analytics().dashboardReportLoaded({ report: 'averageSalary' });

            var notifier = new sizeup.core.notifier(function () {
                e.callback();
                displayReport();
            });

            var percentileData = {};
            var chartData = {};
            var percentileNotifier = new sizeup.core.notifier(notifier.getNotifier(function () { percentileDataReturned(percentileData); }));
            var chartNotifier = new sizeup.core.notifier(notifier.getNotifier(function () { chartDataReturned(chartData); }));


            me.data.enteredValue = me.reportContainer.getValue();
            jQuery.bbq.pushState({ salary: me.data.enteredValue });
            sizeup.api.data.getAverageSalary({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.County.Id }, chartNotifier.getNotifier(function (data) { chartData.County = data; }));
            if (me.opts.report.CurrentPlace.Metro.Id) {
                sizeup.api.data.getAverageSalary({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.Metro.Id }, chartNotifier.getNotifier(function (data) { chartData.Metro = data; }));
            }
            sizeup.api.data.getAverageSalary({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.State.Id }, chartNotifier.getNotifier(function (data) { chartData.State = data; }));
            sizeup.api.data.getAverageSalary({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.Nation.Id }, chartNotifier.getNotifier(function (data) { chartData.Nation = data; }));

            sizeup.api.data.getAverageSalaryPercentage({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.County.Id, value: me.data.enteredValue}, percentileNotifier.getNotifier(function (data) { percentileData.County = data; }));
            if (me.opts.report.CurrentPlace.Metro.Id) {
                sizeup.api.data.getAverageSalaryPercentage({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.Metro.Id, value: me.data.enteredValue }, percentileNotifier.getNotifier(function (data) { percentileData.Metro = data; }));
            }
            sizeup.api.data.getAverageSalaryPercentage({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.State.Id, value: me.data.enteredValue}, percentileNotifier.getNotifier(function (data) { percentileData.State = data; }));
            sizeup.api.data.getAverageSalaryPercentage({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.Nation.Id, value: me.data.enteredValue }, percentileNotifier.getNotifier(function (data) { percentileData.Nation = data; }));
        };

        var percentileDataReturned = function (data) {

            me.data.percentages = {};

             if (data.County != null) {
                 var val = 50 + (data.County.Percentage / 2);
                 var percentage = sizeup.util.numbers.format.percentage(Math.abs(data.County.Percentage));
                 me.data.percentages.County = data.County.Percentage < 0 ? percentage + ' below average' : data.County.Percentage == 0 ? ' average' : percentage + ' Above average';
            }
             if (data.Metro != null) {
                var val = 50 + (data.Metro.Percentage / 2);
                var percentage = sizeup.util.numbers.format.percentage(Math.abs(data.Metro.Percentage));
                me.data.percentages.Metro = data.Metro.Percentage < 0 ? percentage + ' below average' : data.Metro.Percentage == 0 ? ' average' : percentage + ' Above average';
            }
             if (data.State != null) {
                var val = 50 + (data.State.Percentage / 2);
                var percentage = sizeup.util.numbers.format.percentage(Math.abs(data.State.Percentage));
                me.data.percentages.State = data.State.Percentage < 0 ? percentage + ' below average' : data.State.Percentage == 0 ? ' average' : percentage + ' Above average';
            }
             if (data.Nation != null) {
                var val = 50 + (data.Nation.Percentage / 2);
                var percentage = sizeup.util.numbers.format.percentage(Math.abs(data.Nation.Percentage));
                me.data.percentages.Nation = data.Nation.Percentage < 0 ? percentage + ' below average' : data.Nation.Percentage == 0 ? ' average' : percentage + ' Above average';
                me.data.gauge = {
                    value: val,
                    tooltip: data.Nation.Percentage < 0 ? percentage + ' Below Average' : data.Nation.Percentage == 0 ? 'Average' : percentage + ' Above Average'
                };
            }
            else {
                me.data.gauge = {
                    value: 0,
                    tooltip: 'No data'
                };
            }


            me.data.description = {
                Percentages: me.data.percentages
            }

        };

        var chartDataReturned = function (data) {
            me.data.chart = {};
            me.data.table = {};
            me.data.chart['me'] =
                {
                    value: me.data.enteredValue,
                    label: '',
                    name: 'My Business',
                    color: '#5b0'
                };
            

            me.data.table['me'] =
                {
                    name: 'My Business',
                    value: '$' + sizeup.util.numbers.format.addCommas(me.data.enteredValue)
                };
            
            me.data.noData = true;
            var indexes = ['County', 'Metro', 'State', 'Nation'];
            for (var x = 0; x < indexes.length; x++) {
                if (data[indexes[x]] != null) {
                    me.data.chart[indexes[x]] =
                    {
                        value: data[indexes[x]].Value,
                        label: indexes[x],
                        name: data[indexes[x]].Name,
                        color: '#0af'
                    };

                    me.data.table[indexes[x]] = {
                        name: data[indexes[x]].Name,
                        value: '$' + sizeup.util.numbers.format.addCommas(parseInt(data[indexes[x]].Value))
                    };
                    me.data.noData = false;
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


        var collapseReport = function () {
            me.reportContainer.collapseReport();
        };

        var expandReport = function () {
            me.reportContainer.expandReport();
        };


        var publicObj = {

            fadeInPrompt: function (delay, callback) {
                fadeInPrompt(delay, callback);
            },
            setupReport: function () {
                setupReport();
            },
            collapseReport: function () {
                collapseReport();
            },
            expandReport: function () {
                expandReport();
            }
        };
        init();
        return publicObj;

    };
})();