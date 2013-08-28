(function () {
    sizeup.core.namespace('sizeup.widget.views.community');
    sizeup.widget.views.community.communityWithIndustry = function (opts) {


        var defaults = {
            mapSettings: sizeup.maps.mapOptions.getDefaults(),
            styles: sizeup.maps.mapStyles.getDefaults()
        };
        var me = {};

        me.opts = $.extend(true, defaults, opts);

        me.data = {};
        me.container = $('#community');
        var templates = new sizeup.core.templates(me.container);
        var notifier = new sizeup.core.notifier(function () { init(); });


        me.content = {};


        sizeup.api.data.getCentroid({ geographicLocationId: opts.location.CurrentPlace.City.Id }, notifier.getNotifier(function (data) { me.data.CityCenter = new sizeup.maps.latLng({ lat: data.Lat, lng: data.Lng }); }));
        sizeup.api.data.getBoundingBox({ geographicLocationId: opts.location.CurrentPlace.City.Id }, notifier.getNotifier(function (data) { me.data.BoundingBox = data; }));

        var init = function () {


            me.content.industryBox = me.container.find('#industryBox').hide().removeClass('hidden');
            me.content.changeIndustry = me.container.find('#changeIndustry');

            me.content.industrySelector = sizeup.controls.industrySelector({
                textbox: me.content.industryBox,
                onChange: function (item) { onIndustryChange(item); },
                onBlur: function (item) { onIndustryChange(item); }
            });

            me.content.placeBox = me.container.find('#placeBox').hide().removeClass('hidden');
            me.content.changePlace = me.container.find('#changePlace');

            me.content.placeSelector = sizeup.controls.placeSelector({
                textbox: me.content.placeBox,
                onChange: function (item) { onPlaceChange(item); },
                onBlur: function (item) { onPlaceChange(item); }
            });

            me.content.industrySelector.setSelection(me.opts.location.CurrentIndustry);
            me.content.changeIndustry.click(changeIndustryClicked);

            me.content.placeSelector.setSelection(me.opts.location.CurrentPlace);
            me.content.changePlace.click(changePlaceClicked);



            var bounds = new sizeup.maps.latLngBounds();
            bounds.extend(new sizeup.maps.latLng({ lat: me.data.BoundingBox.SouthWest.Lat, lng: me.data.BoundingBox.SouthWest.Lng }));
            bounds.extend(new sizeup.maps.latLng({ lat: me.data.BoundingBox.NorthEast.Lat, lng: me.data.BoundingBox.NorthEast.Lng }));


            me.content.map = new sizeup.maps.map({
                container: me.container.find('.mapWrapper.container .map')
            });

            me.content.map.addEventListener('zoom_changed', mapZoomUpdated);


            var borderOverlay = new sizeup.maps.overlay({
                attribute: sizeup.api.tiles.overlayAttributes.geographyBoundary,
                tileParams: {
                    geographicLocationId: opts.location.CurrentPlace.City.Id
                }
            });



            me.content.map.setCenter(me.data.CityCenter);
            me.content.map.fitBounds(bounds);
            me.content.map.addOverlay(borderOverlay, 1);

            var data = {
                averageRevenue: {},
                totalRevenue: {},
                averageEmployees: {},
                totalEmployees: {},
                averageSalary: {},
                costEffectiveness: {}
            };

            var notifiers = {
                averageRevenue: new sizeup.core.notifier(function(){initAverageRevenueChart(data.averageRevenue);}),
                totalRevenue: new sizeup.core.notifier(function () { initTotalRevenueChart(data.totalRevenue); }),
                averageEmployees: new sizeup.core.notifier(function () { initAverageEmployeesChart(data.averageEmployees); }),
                totalEmployees: new sizeup.core.notifier(function () { initTotalEmployeesChart(data.totalEmployees); }),
                averageSalary: new sizeup.core.notifier(function () { initAverageSalaryChart(data.averageSalary); }),
                costEffectiveness: new sizeup.core.notifier(function () { initCostEffectivenessChart(data.costEffectiveness); }),
            };

            sizeup.api.data.getAverageRevenue({ industryId: me.opts.location.CurrentIndustry.Id, geographicLocationId: me.opts.location.CurrentPlace.City.Id }, notifiers.averageRevenue.getNotifier(function (d) { data.averageRevenue.City = d; }));
            sizeup.api.data.getAverageRevenue({ industryId: me.opts.location.CurrentIndustry.Id, geographicLocationId: me.opts.location.CurrentPlace.County.Id }, notifiers.averageRevenue.getNotifier(function (d) { data.averageRevenue.County = d; }));
            if (me.opts.location.CurrentPlace.Metro.Id) {
                sizeup.api.data.getAverageRevenue({ industryId: me.opts.location.CurrentIndustry.Id, geographicLocationId: me.opts.location.CurrentPlace.Metro.Id }, notifiers.averageRevenue.getNotifier(function (d) { data.averageRevenue.Metro = d; }));
            }
            sizeup.api.data.getAverageRevenue({ industryId: me.opts.location.CurrentIndustry.Id, geographicLocationId: me.opts.location.CurrentPlace.State.Id }, notifiers.averageRevenue.getNotifier(function (d) { data.averageRevenue.State = d; }));
            sizeup.api.data.getAverageRevenue({ industryId: me.opts.location.CurrentIndustry.Id, geographicLocationId: me.opts.location.CurrentPlace.Nation.Id }, notifiers.averageRevenue.getNotifier(function (d) { data.averageRevenue.Nation = d; }));


            sizeup.api.data.getTotalRevenue({ industryId: me.opts.location.CurrentIndustry.Id, geographicLocationId: me.opts.location.CurrentPlace.City.Id }, notifiers.totalRevenue.getNotifier(function (d) { data.totalRevenue.City = d; }));
            sizeup.api.data.getTotalRevenue({ industryId: me.opts.location.CurrentIndustry.Id, geographicLocationId: me.opts.location.CurrentPlace.County.Id }, notifiers.totalRevenue.getNotifier(function (d) { data.totalRevenue.County = d; }));
            if (me.opts.location.CurrentPlace.Metro.Id) {
                sizeup.api.data.getTotalRevenue({ industryId: me.opts.location.CurrentIndustry.Id, geographicLocationId: me.opts.location.CurrentPlace.Metro.Id }, notifiers.totalRevenue.getNotifier(function (d) { data.totalRevenue.Metro = d; }));
            }
            sizeup.api.data.getTotalRevenue({ industryId: me.opts.location.CurrentIndustry.Id, geographicLocationId: me.opts.location.CurrentPlace.State.Id }, notifiers.totalRevenue.getNotifier(function (d) { data.totalRevenue.State = d; }));
            sizeup.api.data.getTotalRevenue({ industryId: me.opts.location.CurrentIndustry.Id, geographicLocationId: me.opts.location.CurrentPlace.Nation.Id }, notifiers.totalRevenue.getNotifier(function (d) { data.totalRevenue.Nation = d; }));


            sizeup.api.data.getAverageEmployees({ industryId: me.opts.location.CurrentIndustry.Id, geographicLocationId: me.opts.location.CurrentPlace.City.Id }, notifiers.averageEmployees.getNotifier(function (d) { data.averageEmployees.City = d; }));
            sizeup.api.data.getAverageEmployees({ industryId: me.opts.location.CurrentIndustry.Id, geographicLocationId: me.opts.location.CurrentPlace.County.Id }, notifiers.averageEmployees.getNotifier(function (d) { data.averageEmployees.County = d; }));
            if (me.opts.location.CurrentPlace.Metro.Id) {
                sizeup.api.data.getAverageEmployees({ industryId: me.opts.location.CurrentIndustry.Id, geographicLocationId: me.opts.location.CurrentPlace.Metro.Id }, notifiers.averageEmployees.getNotifier(function (d) { data.averageEmployees.Metro = d; }));
            }
            sizeup.api.data.getAverageEmployees({ industryId: me.opts.location.CurrentIndustry.Id, geographicLocationId: me.opts.location.CurrentPlace.State.Id }, notifiers.averageEmployees.getNotifier(function (d) { data.averageEmployees.State = d; }));
            sizeup.api.data.getAverageEmployees({ industryId: me.opts.location.CurrentIndustry.Id, geographicLocationId: me.opts.location.CurrentPlace.Nation.Id }, notifiers.averageEmployees.getNotifier(function (d) { data.averageEmployees.Nation = d; }));


            sizeup.api.data.getTotalEmployees({ industryId: me.opts.location.CurrentIndustry.Id, geographicLocationId: me.opts.location.CurrentPlace.City.Id }, notifiers.totalEmployees.getNotifier(function (d) { data.totalEmployees.City = d; }));
            sizeup.api.data.getTotalEmployees({ industryId: me.opts.location.CurrentIndustry.Id, geographicLocationId: me.opts.location.CurrentPlace.County.Id }, notifiers.totalEmployees.getNotifier(function (d) { data.totalEmployees.County = d; }));
            if (me.opts.location.CurrentPlace.Metro.Id) {
                sizeup.api.data.getTotalEmployees({ industryId: me.opts.location.CurrentIndustry.Id, geographicLocationId: me.opts.location.CurrentPlace.Metro.Id }, notifiers.totalEmployees.getNotifier(function (d) { data.totalEmployees.Metro = d; }));
            }
            sizeup.api.data.getTotalEmployees({ industryId: me.opts.location.CurrentIndustry.Id, geographicLocationId: me.opts.location.CurrentPlace.State.Id }, notifiers.totalEmployees.getNotifier(function (d) { data.totalEmployees.State = d; }));
            sizeup.api.data.getTotalEmployees({ industryId: me.opts.location.CurrentIndustry.Id, geographicLocationId: me.opts.location.CurrentPlace.Nation.Id }, notifiers.totalEmployees.getNotifier(function (d) { data.totalEmployees.Nation = d; }));



            sizeup.api.data.getAverageSalary({ industryId: me.opts.location.CurrentIndustry.Id, geographicLocationId: me.opts.location.CurrentPlace.County.Id }, notifiers.averageSalary.getNotifier(function (d) { data.averageSalary.County = d; }));
            if (me.opts.location.CurrentPlace.Metro.Id) {
                sizeup.api.data.getAverageSalary({ industryId: me.opts.location.CurrentIndustry.Id, geographicLocationId: me.opts.location.CurrentPlace.Metro.Id }, notifiers.averageSalary.getNotifier(function (d) { data.averageSalary.Metro = d; }));
            }
            sizeup.api.data.getAverageSalary({ industryId: me.opts.location.CurrentIndustry.Id, geographicLocationId: me.opts.location.CurrentPlace.State.Id }, notifiers.averageSalary.getNotifier(function (d) { data.averageSalary.State = d; }));
            sizeup.api.data.getAverageSalary({ industryId: me.opts.location.CurrentIndustry.Id, geographicLocationId: me.opts.location.CurrentPlace.Nation.Id }, notifiers.averageSalary.getNotifier(function (d) { data.averageSalary.Nation = d; }));



            sizeup.api.data.getCostEffectiveness({ industryId: me.opts.location.CurrentIndustry.Id, geographicLocationId: me.opts.location.CurrentPlace.County.Id }, notifiers.costEffectiveness.getNotifier(function (d) { data.costEffectiveness.County = d; }));
            if (me.opts.location.CurrentPlace.Metro.Id) {
                sizeup.api.data.getCostEffectiveness({ industryId: me.opts.location.CurrentIndustry.Id, geographicLocationId: me.opts.location.CurrentPlace.Metro.Id }, notifiers.costEffectiveness.getNotifier(function (d) { data.costEffectiveness.Metro = d; }));
            }
            sizeup.api.data.getCostEffectiveness({ industryId: me.opts.location.CurrentIndustry.Id, geographicLocationId: me.opts.location.CurrentPlace.State.Id }, notifiers.costEffectiveness.getNotifier(function (d) { data.costEffectiveness.State = d; }));
            sizeup.api.data.getCostEffectiveness({ industryId: me.opts.location.CurrentIndustry.Id, geographicLocationId: me.opts.location.CurrentPlace.Nation.Id }, notifiers.costEffectiveness.getNotifier(function (d) { data.costEffectiveness.Nation = d; }));


        };

        var changeIndustryClicked = function () {
            me.content.changeIndustry.hide();
            me.content.industryBox.show();
            me.content.industryBox.focus();
        };

        var onIndustryChange = function (i) {
            if (i != null && i.Id != me.opts.location.CurrentIndustry.Id) {
                me.content.changeIndustry.html(i.Name);
               // var p = { industry: me.data.competitor.primaryIndustry.Name };
               // new sizeup.core.analytics().competitionIndustryChanged(p);
                var url = document.location.pathname;
                url = url.replace(me.opts.location.CurrentIndustry.SEOKey, i.SEOKey);
                url = jQuery.param.querystring(url, jQuery.param.querystring(), 2);
                document.location = url;
            }
            else {
                me.content.changeIndustry.show();
                me.content.industryBox.hide();
                me.content.industrySelector.setSelection(me.opts.location.CurrentIndustry);
            }
        };

        var industryBoxBlur = function () {
            me.content.changeIndustry.show();
            me.content.industryBox.hide();
        };


        var changePlaceClicked = function () {
            me.content.changePlace.hide();
            me.content.placeBox.show();
            me.content.placeBox.focus();
        };

        var onPlaceChange = function (i) {
            if (i != null && i.Id != me.opts.location.CurrentPlace.Id) {
                me.content.changePlace.html(i.City.Name + ', ' + i.State.Abbreviation);
                //var p = { place: me.data.activePlace.City.Name + ', ' + me.data.activePlace.State.Abbreviation };
                //new sizeup.core.analytics().competitionPlaceChanged(p);
                var url = document.location.href;
                url = url.substring(0, url.indexOf('community'));
                url = url + 'community/' + i.State.SEOKey + '/' + i.County.SEOKey + '/' + i.City.SEOKey + '/' + me.opts.location.CurrentIndustry.SEOKey + '/';
                url = jQuery.param.querystring(url, jQuery.param.querystring(), 2);
                document.location = url;
            }
            else {
                me.content.changePlace.show();
                me.content.placeBox.hide();
                me.content.placeSelector.setSelection(me.opts.location.CurrentPlace);
            }
        };

        var placeBoxBlur = function () {
            me.content.changePlace.show();
            me.content.placeBox.hide();
        };



        var scrollContentToTop = function () {
            $('#contentWrap').scrollTop(0);
        };

        var mapZoomUpdated = function () {
            setLegend();
        };

        var setHeatmap = function () {
            var createOverlay = function () {
                clearHeatmap();
                if (me.data.activeHeatmap == 'averageRevenue') {
                    setAverageRevenueOverlays();
                }
                if (me.data.activeHeatmap == 'totalRevenue') {
                    setTotalRevenueOverlays();
                }
                if (me.data.activeHeatmap == 'averageEmployees') {
                    setAverageEmployeesOverlays();
                }
                if (me.data.activeHeatmap == 'totalEmployees') {
                    setTotalEmployeesOverlays();
                }
                if (me.data.activeHeatmap == 'averageSalary') {
                    setAverageSalaryOverlays();
                }
                if (me.data.activeHeatmap == 'costEffectiveness') {
                    setCostEffectivenessOverlays();
                }
                me.data.heatMapOverlays = me.overlay.getOverlays();
                for (var x in me.data.heatMapOverlays) {
                    me.content.map.addOverlay(me.data.heatMapOverlays[x], 0);
                }
                setLegend();
            };

            if (me.data.zoomExtent == null) {
                sizeup.api.data.getZoomExtent({ placeId: me.opts.location.CurrentPlace.Id, width: me.content.map.getWidth() }, function (data) {
                    me.data.zoomExtent = data;
                    createOverlay();
                });
            }
            else {
                createOverlay();
            }
        };

        var setLegend = function () {
            var z = me.content.map.getZoom();
            var callback = function (legend) {
                me.content.map.setLegend(legend);
            };
            if (me.overlay != null) {
                me.overlay.getLegend(z, callback);
            }
        };

        var clearHeatmap = function () {
            for (var x in me.data.heatMapOverlays) {
                me.content.map.removeOverlay(me.data.heatMapOverlays[x]);
            }
            me.overlay = null;
        };

        var clearLegend = function () {
            me.content.map.clearLegend();
        };

      

        var setAverageRevenueOverlays = function () {
            me.overlay = new sizeup.maps.heatMapOverlays({
                attribute: sizeup.api.tiles.overlayAttributes.heatmap.averageRevenue,
                place: me.opts.location.CurrentPlace,
                params: { industryId: me.opts.location.CurrentIndustry.Id },
                zoomExtent: me.data.zoomExtent,
                attributeLabel: 'Average Business Annual Revenue',
                format: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); },
                legendData: sizeup.api.data.getAverageRevenueBands,
                templates: templates
            });
        };

        //////////////////////////////////////////////////////////////////////////////////////


        var setTotalRevenueOverlays = function () {

            me.overlay = new sizeup.maps.heatMapOverlays({
                attribute: sizeup.api.tiles.overlayAttributes.heatmap.totalRevenue,
                place: me.opts.location.CurrentPlace,
                params: { industryId: me.opts.location.CurrentIndustry.Id },
                zoomExtent: me.data.zoomExtent,
                attributeLabel: 'Total Revenue',
                format: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); },
                legendData: sizeup.api.data.getTotalRevenueBands,
                templates: templates
            });
        };


        //////////////////////////////////////////////////////////////////////////////////////


        var setAverageEmployeesOverlays = function () {
            me.overlay = new sizeup.maps.heatMapOverlays({
                attribute: sizeup.api.tiles.overlayAttributes.heatmap.averageEmployees,
                place: me.opts.location.CurrentPlace,
                params: { industryId: me.opts.location.CurrentIndustry.Id },
                zoomExtent: me.data.zoomExtent,
                attributeLabel: 'Average Employees per business',
                format: function (val) { return sizeup.util.numbers.format.abbreviate(val); },
                legendData: sizeup.api.data.getAverageEmployeesBands,
                templates: templates
            });
        };


        //////////////////////////////////////////////////////////////////////////////////////
        var setTotalEmployeesOverlays = function () {
            me.overlay = new sizeup.maps.heatMapOverlays({
                attribute: sizeup.api.tiles.overlayAttributes.heatmap.totalEmployees,
                place: me.opts.location.CurrentPlace,
                params: { industryId: me.opts.location.CurrentIndustry.Id },
                zoomExtent: me.data.zoomExtent,
                attributeLabel: 'Total Employees',
                format: function (val) { return sizeup.util.numbers.format.abbreviate(val); },
                legendData: sizeup.api.data.getTotalEmployeesBands,
                templates: templates
            });
        };



        //////////////////////////////////////////////////////////////////////////////////////


        var setAverageSalaryOverlays = function () {
            me.overlay = new sizeup.maps.heatMapOverlays({
                attribute: sizeup.api.tiles.overlayAttributes.heatmap.averageSalary,
                place: me.opts.location.CurrentPlace,
                params: { industryId: me.opts.location.CurrentIndustry.Id },
                zoomExtent: me.data.zoomExtent,
                attributeLabel: 'Average Salary',
                smallestGranularity: 'County',
                format: function (val) { return sizeup.util.numbers.format.abbreviate(val); },
                legendData: sizeup.api.data.getAverageSalaryBands,
                templates: templates
            });
        };



        //////////////////////////////////////////////////////////////////////////////////////

        var setCostEffectivenessOverlays = function () {
            me.overlay = new sizeup.maps.heatMapOverlays({
                attribute: sizeup.api.tiles.overlayAttributes.heatmap.costEffectiveness,
                place: me.opts.location.CurrentPlace,
                params: { industryId: me.opts.location.CurrentIndustry.Id },
                zoomExtent: me.data.zoomExtent,
                attributeLabel: 'Cost Effectiveness',
                smallestGranularity: 'County',
                format: function (val) { return sizeup.util.numbers.format.round(val, 2); },
                legendData: sizeup.api.data.getCostEffectivenessBands,
                templates: templates
            });
        };



        var formatChartData = function (data, indexes) {
            var formattedData = {};
            var hasData = false;
            for (var x = 0; x < indexes.length; x++) {
                if (data[indexes[x]] != null) {
                    hasData = true;
                    formattedData[indexes[x]] =
                    {
                        value: data[indexes[x]].Value,
                        label: indexes[x],
                        name: data[indexes[x]].Name,
                        color: '#0af'
                    };
                }
            }
            return hasData ? formattedData : null;
        };



        var initAverageRevenueChart = function (data) {
            var container = me.container.find('#averageRevenue');
            container.find('.loading').remove();
            var indexes = ['City', 'County', 'Metro', 'State', 'Nation'];
            var formattedData = formatChartData(data, indexes);
            if (formattedData == null) {
                container.find('.noData').removeClass('hidden');
                container.find('.chartWrapper').remove();
            }
            else {
                var chart = new sizeup.charts.barChart({
                    valueFormat: function (val) { return '$' + sizeup.util.numbers.format.addCommas(Math.floor(val)); },
                    bar: { height: 13, padding: 4 },
                    gutters: { left: 50, top: 1 },
                    fillRight: false,
                    container: container.find('.chart'),
                    title: null,
                    bars: formattedData
                });
                chart.draw();


                container.find('.buttons .mapActivate').click(function () {
                    me.data.activeHeatmap = 'averageRevenue';
                    setHeatmap();
                    scrollContentToTop();

                });

                container.find('.buttons .mapClear').click(function () {
                    me.data.activeHeatmap = null;
                    clearHeatmap();
                    clearLegend();
                    scrollContentToTop();
                });
            }
        };

      

        var initTotalRevenueChart = function (data) {
            var container = me.container.find('#totalRevenue');
            container.find('.loading').remove();
            var indexes = ['City', 'County', 'Metro', 'State'];
            var formattedData = formatChartData(data, indexes);

            if (formattedData == null) {
                container.find('.noData').removeClass('hidden');
                container.find('.chartWrapper').remove();
            }
            else {
                var chart = new sizeup.charts.barChart({
                    valueFormat: function (val) { return '$' + sizeup.util.numbers.format.addCommas(Math.floor(val)); },
                    bar: { height: 13, padding: 4 },
                    gutters: { left: 50, top: 1 },
                    fillRight: false,
                    container: container.find('.chart'),
                    title: null,
                    bars: formattedData
                });
                chart.draw();


                container.find('.buttons .mapActivate').click(function () {
                    me.data.activeHeatmap = 'totalRevenue';
                    setHeatmap();
                    scrollContentToTop();
                });

                container.find('.buttons .mapClear').click(function () {
                    me.data.activeHeatmap = null;
                    clearHeatmap();
                    clearLegend();
                    scrollContentToTop();
                });
            }

        };

        var initAverageEmployeesChart = function (data) {
            var container = me.container.find('#averageEmployees');
            container.find('.loading').remove();
            var indexes = ['City', 'County', 'Metro', 'State', 'Nation'];
            var formattedData = formatChartData(data, indexes);

            if (formattedData == null) {
                container.find('.noData').removeClass('hidden');
                container.find('.chartWrapper').remove();
            }
            else {
                var chart = new sizeup.charts.barChart({
                    valueFormat: function (val) { return sizeup.util.numbers.format.addCommas(Math.floor(val)); },
                    bar: { height: 13, padding: 4 },
                    gutters: { left: 50, top: 1 },
                    fillRight: false,
                    container: container.find('.chart'),
                    title: null,
                    bars: formattedData
                });
                chart.draw();

                container.find('.buttons .mapActivate').click(function () {
                    me.data.activeHeatmap = 'averageEmployees';
                    setHeatmap();
                    scrollContentToTop();

                });

                container.find('.buttons .mapClear').click(function () {
                    me.data.activeHeatmap = null;
                    clearHeatmap();
                    clearLegend();
                    scrollContentToTop();
                });
            }
        };

        var initTotalEmployeesChart = function (data) {
            var container = me.container.find('#totalEmployees');
            container.find('.loading').remove();
            var indexes = ['City', 'County', 'Metro', 'State'];
            var formattedData = formatChartData(data, indexes);

            if (formattedData == null) {
                container.find('.noData').removeClass('hidden');
                container.find('.chartWrapper').remove();
            }
            else {
                var chart = new sizeup.charts.barChart({
                    valueFormat: function (val) { return sizeup.util.numbers.format.addCommas(Math.floor(val)); },
                    bar: { height: 13, padding: 4 },
                    gutters: { left: 50, top: 1 },
                    fillRight: false,
                    container: container.find('.chart'),
                    title: null,
                    bars: formattedData
                });
                chart.draw();

                container.find('.buttons .mapActivate').click(function () {
                    me.data.activeHeatmap = 'totalEmployees';
                    setHeatmap();
                    scrollContentToTop();

                });

                container.find('.buttons .mapClear').click(function () {
                    me.data.activeHeatmap = null;
                    clearHeatmap();
                    clearLegend();
                    scrollContentToTop();
                });
            }
        };


        var initAverageSalaryChart = function (data) {
            var container = me.container.find('#averageSalary');
            container.find('.loading').remove();
            var indexes = ['City', 'County', 'Metro', 'State', 'Nation'];
            var formattedData = formatChartData(data, indexes);

            if (formattedData == null) {
                container.find('.noData').removeClass('hidden');
                container.find('.chartWrapper').remove();
            }
            else {
                var chart = new sizeup.charts.barChart({
                    valueFormat: function (val) { return '$' + sizeup.util.numbers.format.addCommas(Math.floor(val)); },
                    bar: { height: 13, padding: 4 },
                    gutters: { left: 50, top: 1 },
                    fillRight: false,
                    container: container.find('.chart'),
                    title: null,
                    bars: formattedData
                });
                chart.draw();

                container.find('.buttons .mapActivate').click(function () {
                    me.data.activeHeatmap = 'averageSalary';
                    setHeatmap();
                    scrollContentToTop();

                });

                container.find('.buttons .mapClear').click(function () {
                    me.data.activeHeatmap = null;
                    clearHeatmap();
                    clearLegend();
                    scrollContentToTop();
                });
            }
        };


        var initCostEffectivenessChart = function (data) {
            var container = me.container.find('#costEffectiveness');
            container.find('.loading').remove();
            var indexes = ['City', 'County', 'Metro', 'State', 'Nation'];
            var formattedData = formatChartData(data, indexes);

            if (formattedData == null) {
                container.find('.noData').removeClass('hidden');
                container.find('.chartWrapper').remove();
            }
            else {
                var chart = new sizeup.charts.barChart({
                    valueFormat: function (val) { return sizeup.util.numbers.format.round(val, 1); },
                    bar: { height: 13, padding: 4 },
                    gutters: { left: 50, top: 1 },
                    fillRight: false,
                    container: container.find('.chart'),
                    title: null,
                    bars: formattedData
                });
                chart.draw();

                container.find('.buttons .mapActivate').click(function () {
                    me.data.activeHeatmap = 'costEffectiveness';
                    setHeatmap();
                    scrollContentToTop();

                });

                container.find('.buttons .mapClear').click(function () {
                    me.data.activeHeatmap = null;
                    clearHeatmap();
                    clearLegend();
                    scrollContentToTop();
                });
            }
        };



        var publicObj = {

        };
        return publicObj;

    };
})();