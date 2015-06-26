(function () {
    sizeup.core.namespace('sizeup.charts');
    sizeup.charts.lineChart = function (opts) {
        var defaults =
        {

        };

        var me = {};
        me.opts = $.extend(true, defaults, opts);
        me.container = opts.container;


        var init = function () {

            _drawGridlines();

            for (var x in me.opts.series) {
                var serie = me.opts.series[x];
                addSeries(x, getDataX(serie.plots), getDataY(serie.plots), getBounds(serie.plots), serie.color, '{y} new businesses in {x}', true);
            }
            if (me.opts.marker) {
                var m = me.opts.marker;
                addMarkers('marker', [me.opts.xBounds.min, m.value, me.opts.xBounds.max], [0, 1, 0], { xMin: me.opts.xBounds.min, xMax: me.opts.xBounds.max, yMin: 0, yMax: 1 }, m.color, m.label, true);
            }
        };

        var getDataX = function (series) {
            var a = [];
            for (var x in series) {
                a.push(series[x].Key);
            }
            return a;
        };

        var getDataY = function (series) {
            var a = [];
            for (var x in series) {
                a.push(series[x].Value);
            }
            return a;
        };

        var getBounds = function (series) {
            var b = {
                xMax: series[0].Key,
                xMin: series[0].Key,
                yMax: series[0].Value,
                yMin: series[0].Value,
            };

            for (var x in series) {
                b.xMax = Math.max(series[x].Key, b.xMax);
                b.yMax = Math.max(series[x].Value, b.yMax);
                b.xMin = Math.min(series[x].Key, b.xMin);
                b.yMin = Math.min(series[x].Value, b.yMin);
            }
            return b;
        };



        var el = me.container.empty();
        me._width = el.width();
        me._height = el.height();
        me._bounds = { xMin: Infinity, xMax: -Infinity, yMin: Infinity, yMax: -Infinity };
        me._canvas = Raphael(el.get(0), me._width, me._height);
        me._gutters = { left: 30, top: 20, right: 20, bottom: 20, color: "#e6e6e0" };
        me._grids = { vertical: 10, horizontal: 5, color: "#c6c6c0" };
        me._axisTextStyle = { fill: "#969690", "font-size": 10, "font-family": "Trebuchet MS, Arial, sans-serif" };
        me._effectiveSize = { width: me._width - me._gutters.left - me._gutters.right, height: me._height - me._gutters.top - me._gutters.bottom };
        me._textOffset = $.browser.msie || $.browser.webkit ? 3 : 1;
        me._series = {};
        el.append("<div class=\"fader\" style=\"width:" + me._width + "px;height:" + me._height + "px;background-color:#fff;position:absolute;margin-top:-" + me._height + "px;\"></div>");
        me._fader = $(".fader", el);


        var _drawGridlines = function () {
            var g = me._gutters;
            me._canvas.rect(g.left, g.top, me._effectiveSize.width, me._effectiveSize.height).attr({ stroke: g.color, fill: "90-#f6f6f6-#fff" });
        };

        var _drawAxes = function (bounds) {

            var xMin = bounds.xMin,
                xMax = bounds.xMax,
                yMin = bounds.yMin,
                yMax = bounds.yMax,
                xBase = me._gutters.left,
                yBase = me._gutters.top + me._effectiveSize.height;

            var xGridStepPx = me._effectiveSize.width / me._grids.horizontal;
            var xGridStep = (xMax - xMin) / me._grids.horizontal;

            var xGrids = [];
            for (var j = 0; j <= me._grids.horizontal; j++) {
                var xPos = xBase + j * xGridStepPx;
                xGrids.push("M", xPos, yBase + 5, "L", xPos, yBase);
                me._canvas.text(xPos, yBase + 12 + me._textOffset, parseInt(xMin + j * xGridStep)).attr(me._axisTextStyle);
            }

            me._canvas.path().attr({ path: xGrids }).attr({ "stroke-width": 1, stroke: me._grids.color }).toBack();

            var verticalGrids = yMax < me._grids.vertical ? yMax : me._grids.vertical;

            var yGridStepPx = me._effectiveSize.height / verticalGrids;
            var yGridStep = Math.round((yMax - yMin) / verticalGrids);

            var yGrids = [];
            if (yMax > 1) {
                for (var j = 0; j <= verticalGrids; j++) {
                    var yPos = yBase - j * yGridStepPx;
                    yGrids.push("M", xBase - 3, yPos, "L", xBase, yPos);
                    if (yPos != yBase) {
                        me._canvas.text(xBase - 6, yPos + me._textOffset, yMin + j * yGridStep).attr(me._axisTextStyle).attr({ "text-anchor": "end" });
                    }
                }
                me._canvas.path().attr({ path: yGrids }).attr({ "stroke-width": 1, stroke: me._grids.color }).toBack();
            }
        };

        var _drawSeries = function (series, bounds) {

            var xData = series.xData,
                yData = series.yData,
                xMin = bounds.xMin,
                xMax = bounds.xMax,
                yMin = bounds.yMin,
                yMax = bounds.yMax,
                xStep = me._effectiveSize.width / (bounds.xMax - bounds.xMin),
                yStep = me._effectiveSize.height / (bounds.yMax - bounds.yMin),
                color = series.color;

            var dataPoints = [];

            var xBase = me._gutters.left,
                yBase = me._gutters.top + me._effectiveSize.height;
            var lineData = ["M"];

            for (var i = 0; i < xData.length; i++) {
                var thisX = Math.round(xBase + (xData[i] - xMin) * xStep);
                var thisY = Math.round(yBase - (yData[i] - yMin) * yStep);
                if (thisX != null && thisY != null) {
                    lineData.push(thisX, thisY);
                    dataPoints.push({ x: thisX, y: thisY, title: series.pointDescriptionTemplate.replace("{x}", xData[i]).replace("{y}", yData[i]) });
                }
            }

            me._canvas.path().attr({ path: lineData, stroke: color, "stroke-width": 2, "stroke-opacity": 0.5 });

            // draw circles on "top" of the line to keep the hover events uninterrupted
            for (var i = 0; i < dataPoints.length; i++) {
                var thisPointData = dataPoints[i];
                var thisPoint = me._canvas.circle(thisPointData.x, thisPointData.y, 3).attr({ fill: color, stroke: color, "stroke-width": 6, "stroke-opacity": 0.01, "fill-opacity": 1, cursor: "pointer", title: thisPointData.title });
                (function (point) {
                    point.hover(
                        function () {
                            this.animate({ "stroke-opacity": 0.75 }, 50);
                        },
                        function () {
                            this.animate({ "stroke-opacity": 0.01 }, 50);
                        }
                        );
                })(thisPoint);
            }
        };


        var _drawMarkers = function (series, bounds) {

            var xData = series.xData,
                yData = series.yData,
                xMin = bounds.xMin,
                xMax = bounds.xMax,
                yMin = bounds.yMin,
                yMax = bounds.yMax,
                xStep = me._effectiveSize.width / (bounds.xMax - bounds.xMin),
                yStep = me._effectiveSize.height / (bounds.yMax - bounds.yMin),
                color = series.color;

            var dataPoints = me._canvas.set();

            var xBase = me._gutters.left,
                yBase = me._gutters.top + me._effectiveSize.height;
            var lineData = ["M"];

            for (var i = 0; i < xData.length; i++) {
                if (yData[i] == 0) {
                    continue;
                }

                var thisX = Math.round(xBase + (xData[i] - xMin) * xStep);
                var thisY = me._gutters.top;

                if (thisX < xBase) {
                    thisX = xBase;
                }

                lineData.push(thisX, thisY, "L", thisX, me._gutters.top + me._effectiveSize.height);

                var thisTitle = series.pointDescriptionTemplate.replace("{x}", xData[i]).replace("{y}", yData[i]);
                var thisPoint = me._canvas.path().attr({ path: lineData }).attr({ stroke: color, "stroke-width": 4, opacity: 0.8, cursor: "pointer", title: thisTitle });

                (function (outerRect) {
                    outerRect.hover(
                        function () {
                            this.animate({ "stroke-width": 8 }, 100, "<>");
                        },
                        function () {
                            this.animate({ "stroke-width": 4 }, 100, "<>");
                        }
                    );
                })(thisPoint);

            }
        };

        var addSeries = function (key, xData, yData, bounds, color, pointDescriptionTemplate, visible) {
            me._series[key] = { xData: xData, yData: yData, color: color, bounds: bounds, visible: visible, pointDescriptionTemplate: pointDescriptionTemplate, isMarker: false };
        };

        var showSeries = function (key) {
            me._series[key].visible = true;
        };

        var addMarkers = function (key, xData, yData, bounds, color, pointDescriptionTemplate, visible) {
            me._series[key] = { xData: xData, yData: yData, color: color, bounds: bounds, visible: visible, pointDescriptionTemplate: pointDescriptionTemplate, isMarker: true };
        };

        var hideSeries = function (key) {
            me._series[key].visible = false;
        };

        var _showChart = function () {
            me._canvas.safari();
        };

        var _hideChart = function () {
            me._canvas.safari();
        };

        var redrawChart = function (fadeDelay) {
            if (parseInt(fadeDelay)) {
                me._fader.fadeIn(fadeDelay, function () { redrawChartAction(fadeDelay); });
            }
            else {
                me._fader.hide();
                redrawChartAction();
            }
        };

        var redrawChartAction = function (fadeDelay) {

            var superBounds = {
                xMin: me.opts.xBounds.min,
                xMax: me.opts.xBounds.max,
                yMin: 0,
                yMax: 0
            };

            for (var thisSeriesKey in me._series) {
                var thisSeries = me._series[thisSeriesKey];
                if (thisSeries.visible) {
                    if (superBounds == null) {
                        superBounds = thisSeries.bounds;
                    }
                    else {
                        superBounds = getSuperBounds(thisSeries.bounds, superBounds);
                    }
                }
            }

            me._canvas.clear();

            _drawGridlines();
            _drawAxes(superBounds);

            for (var thisSeriesKey in me._series) {
                var thisSeries = me._series[thisSeriesKey];
                if (thisSeries.visible) {
                    if (thisSeries.isMarker) {
                        _drawMarkers(thisSeries, superBounds);
                    }
                    else {
                        _drawSeries(thisSeries, superBounds);
                    }
                }
            }
            if (parseInt(fadeDelay)) {
                me._fader.fadeOut(fadeDelay);
            }
        };

        var getSuperBounds = function (boundsA, boundsB) {

            var xMin = boundsA.xMin < boundsB.xMin ? boundsA.xMin : boundsB.xMin;
            var xMax = boundsA.xMax > boundsB.xMax ? boundsA.xMax : boundsB.xMax;
            var yMin = boundsA.yMin < boundsB.yMin ? boundsA.yMin : boundsB.yMin;
            var yMax = boundsA.yMax > boundsB.yMax ? boundsA.yMax : boundsB.yMax;

            return { xMin: xMin, xMax: xMax, yMin: yMin, yMax: yMax };
        };

       

        var publicObj = {
            draw: function () {
                redrawChart(500);
            },
            getContainer: function () {
                return me.container;
            },
            showSeries: function (key) {
                showSeries(key);
            },
            hideSeries: function (key) {
                hideSeries(key);
            }
        };
        init();
        return publicObj;

    };
})();


