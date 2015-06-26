(function () {
    sizeup.core.namespace('sizeup.views.dashboard.costEffectiveness');
    sizeup.views.dashboard.costEffectiveness = function (opts) {

        var me = {};
        var templates = new sizeup.core.templates(opts.container);
        me.opts = opts;
        me.data = {};
        me.container = opts.container;
        me.data.enteredEmployees = jQuery.bbq.getState().employees;
        me.data.enteredRevenue = jQuery.bbq.getState().revenue;
        me.data.enteredSalary = jQuery.bbq.getState().salary;
        me.data.description = {};
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
                        valueChanged: function () { }
                    },
                    inputFormat: function (val) {
                        var out = "";
                        if (val < 10) {
                            out = "Poor";
                        }
                        else if (val < 40) {
                            out = "Below Average";
                        }
                        else if (val <= 60) {
                            out = "Average";
                        }
                        else if (val <= 90) {
                            out = "Above Average";
                        }
                        else if (val > 90) {
                            out = "Superior";
                        }

                        return out;
                    }
                });

          
            me.sourceButton = new sizeup.controls.toggleButton(
            {
                button: me.container.find('.reportContainer .links .source'),
                onClick: function () { toggleSource(); }
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


            me.sourceContent = me.container.find('.reportContainer .sourceContent').hide();
            me.considerations = me.container.find('.reportContainer .considerations');
            me.resources = me.container.find('.reportContainer .resources');
            me.description = me.container.find('.reportContainer .description');

            me.map = new sizeup.maps.map({
                container: me.container.find('.mapWrapper.container .map')
            });

            me.textAlternative = me.container.find('.mapWrapper .textAlternative');
            me.textAlternative.click(textAlternativeClicked);

            $(window).bind('hashchange', function (e) { hashChanged(e); });

            me.noData = me.container.find('.reportContent.noDataError').removeClass('hidden').hide();
            me.reportData = me.container.find('.reportData');

        };

        var hashChanged = function (e) {
            var employees = e.getState('employees');
            var salary = e.getState('salary');
            var revenue = e.getState('revenue');
            me.data.enteredEmployees = employees;
            me.data.enteredSalary = salary;
            me.data.enteredRevenue = revenue;

            if (
                employees != null &&
                salary != null &&
                revenue != null
                ) {
                setupReport();
            }
            else if (!(employees != null &&
                salary != null &&
                revenue != null)) {
                me.reportContainer.clearReport();
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
            var url = '/accessibility/costEffectiveness/';
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

                me.reportContainer.setValue(me.data.gauge.value);


                sizeup.api.data.getZoomExtent({ placeId: me.opts.report.CurrentPlace.Id, width: me.map.getWidth() }, function (data) {
                    me.overlay = new sizeup.maps.heatMapOverlays({
                        attribute: sizeup.api.tiles.overlayAttributes.heatmap.costEffectiveness,
                        place: me.opts.report.CurrentPlace,
                        params: { industryId: me.opts.report.CurrentIndustry.Id },
                        zoomExtent: data,
                        attributeLabel: 'Cost Effectiveness',
                        format: function (val) { return sizeup.util.numbers.format.sigFig(val, 3); },
                        legendData: sizeup.api.data.getCostEffectivenessBands,
                        templates: templates
                    });
                    setHeatmap();
                });



                me.chart = new sizeup.charts.barChart({

                    valueFormat: function (val) { return sizeup.util.numbers.format.sigFig(val, 3); },
                    container: me.container.find('.chart .container'),
                    title: 'cost effectiveness ratio',
                    bars: me.data.chart.bars
                });
                me.chart.draw();

                me.table = new sizeup.charts.tableChart({
                    container: me.container.find('.table').hide(),
                    rowTemplate: templates.get('tableRow'),
                    rows: me.data.table
                });


                me.data.description.NAICS6 = me.opts.report.CurrentIndustry.NAICS6;
                me.data.description.Industry = me.opts.report.CurrentIndustry;

                me.description.html(templates.bind(templates.get("description"), me.data.description));

            }
        };




        var runReport = function (e) {
            new sizeup.core.analytics().dashboardReportLoaded({ report: 'costEffectiveness' });

            var notifier = new sizeup.core.notifier(function () {
                e.callback();
                displayReport();
            });

            var percentileData = {};
            var chartData = {};
            var percentileNotifier = new sizeup.core.notifier(notifier.getNotifier(function () { percentageDataReturned(percentileData); }));
            var chartNotifier = new sizeup.core.notifier(notifier.getNotifier(function () { chartDataReturned(chartData); }));


            if (me.data.enteredEmployees != null &&
                me.data.enteredSalary != null &&
                me.data.enteredRevenue != null) {
                
                sizeup.api.data.getCostEffectiveness({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.County.Id }, chartNotifier.getNotifier(function (data) { chartData.County = data; }));
                if (me.opts.report.CurrentPlace.Metro.Id) {
                    sizeup.api.data.getCostEffectiveness({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.Metro.Id }, chartNotifier.getNotifier(function (data) { chartData.Metro = data; }));
                }
                sizeup.api.data.getCostEffectiveness({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.State.Id }, chartNotifier.getNotifier(function (data) { chartData.State = data; }));
                sizeup.api.data.getCostEffectiveness({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.Nation.Id }, chartNotifier.getNotifier(function (data) { chartData.Nation = data; }));

                sizeup.api.data.getCostEffectivenessPercentage({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.County.Id, employees: me.data.enteredEmployees, salary: me.data.enteredSalary, revenue: me.data.enteredRevenue }, percentileNotifier.getNotifier(function (data) { percentileData.County = data; }));
                if (me.opts.report.CurrentPlace.Metro.Id) {
                    sizeup.api.data.getCostEffectivenessPercentage({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.Metro.Id, employees: me.data.enteredEmployees, salary: me.data.enteredSalary, revenue: me.data.enteredRevenue }, percentileNotifier.getNotifier(function (data) { percentileData.Metro = data; }));
                }
                sizeup.api.data.getCostEffectivenessPercentage({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.State.Id, employees: me.data.enteredEmployees, salary: me.data.enteredSalary, revenue: me.data.enteredRevenue }, percentileNotifier.getNotifier(function (data) { percentileData.State = data; }));
                sizeup.api.data.getCostEffectivenessPercentage({ industryId: me.opts.report.CurrentIndustry.Id, geographicLocationId: me.opts.report.CurrentPlace.Nation.Id, employees: me.data.enteredEmployees, salary: me.data.enteredSalary, revenue: me.data.enteredRevenue }, percentileNotifier.getNotifier(function (data) { percentileData.Nation = data; }));
            }
        };

        var chartDataReturned = function (data) {

            var ce = me.data.enteredRevenue / (me.data.enteredEmployees * me.data.enteredSalary);

            me.data.chart = {
                bars: {}
            };
            me.data.table = {};
            me.data.chart.bars['me'] =
                {
                    value: ce,
                    label: '',
                    name: 'My Business',
                    color: '#5b0'
                };


            me.data.table['me'] =
                {
                    name: 'My Business',
                    value: sizeup.util.numbers.format.sigFig(ce, 3)
                };

            me.data.noData = true;

            var indexes = ['County', 'Metro', 'State', 'Nation'];
            for (var x = 0; x < indexes.length; x++) {
                if (data[indexes[x]] != null) {
                    me.data.chart.bars[indexes[x]] =
                    {
                        value: data[indexes[x]].Value,
                        label: indexes[x],
                        name: data[indexes[x]].Name,
                        color: '#0af'
                    };

                    me.data.table[indexes[x]] = {
                        name: data[indexes[x]].Name,
                        value: sizeup.util.numbers.format.sigFig(data[indexes[x]].Value, 3)
                    };
                    me.data.noData = false;
                }
            }
            



        };

        var percentageDataReturned = function (data) {

            me.data.percentages = {};

            if (data.County) {
                var val = 50 + (data.County.Percentage / 2);
                var percentage = sizeup.util.numbers.format.percentage(Math.abs(data.County.Percentage));
                me.data.percentages.County = data.County.Percentage < 0 ? percentage + ' below average' : data.County.Percentage == 0 ? 'average' : percentage + ' above average';
            }
            if (data.Metro) {
                var val = 50 + (data.Metro.Percentage / 2);
                var percentage = sizeup.util.numbers.format.percentage(Math.abs(data.Metro.Percentage));
                me.data.percentages.Metro = data.Metro.Percentage < 0 ? percentage + ' below average' : data.Metro.Percentage == 0 ? 'average' : percentage + ' above average';
            }
            if (data.State) {
                var val = 50 + (data.State.Percentage / 2);
                var percentage = sizeup.util.numbers.format.percentage(Math.abs(data.State.Percentage));
                me.data.percentages.State = data.State.Percentage < 0 ? percentage + ' below average' : data.State.Percentage == 0 ? 'average' : percentage + ' above average';
            }
            if (data.Nation) {
                var val = 50 + (data.Nation.Percentage / 2);
                var percentage = sizeup.util.numbers.format.percentage(Math.abs(data.Nation.Percentage));
                me.data.percentages.Nation = data.Nation.Percentage < 0 ? percentage + ' below average' : data.Nation.Percentage == 0 ? 'average' : percentage + ' above average';
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


        var setupReport = function () {
            if (me.data.enteredEmployees != null &&
                me.data.enteredSalary != null &&
                me.data.enteredRevenue != null) {
                    me.reportContainer.doGetReport();
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



