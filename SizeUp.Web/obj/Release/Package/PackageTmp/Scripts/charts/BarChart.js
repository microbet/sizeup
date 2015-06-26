(function () {
    sizeup.core.namespace('sizeup.charts.barChart');
    sizeup.charts.barChart = function (opts) {

        var defaults =
        {
            valueFormat:  function(val){ return val;},
            grids: { horizontal: 3 },
            gutters: { left: 45, top: 1, right: 50 },
            bar: { height: 15, padding: 8 },
            rangePadding: 0.1 
        };

        var me = {};
        me.opts = $.extend(true, defaults, opts);
        me.container = opts.container;


        var init = function () {
            for (var x in me.opts.bars) {
                var bar = me.opts.bars[x];
                addBar('bar' + x, bar.value, null, bar.label, bar.name, bar.color, '');
            }
            if (me.opts.marker) {
                var m = me.opts.marker;
                addMarker('marker', m.value, null, m.name, m.label,  m.color, '');
            }
        };

    
        me._parentElement = me.opts.container.empty()
        me._width = me.opts.width || me._parentElement.width();
        me._height = me.opts.height || me._parentElement.height();

        me._rangePadding = me.opts.rangePadding;
        me._bounds = { xMin: Infinity, xMax: -Infinity, yMin: Infinity, yMax: -Infinity };
        me._canvas = Raphael(me._parentElement.get(0), me._width, me._height);
        me._gutters = jQuery.extend({ left: 105, right: 105, top: 10, bottom: 40, color: "#e6e6e0" }, me.opts.gutters); 
        me._fillRight = me.opts.fillRight != null ? me.opts.fillRight : true;
        me._grids = jQuery.extend({ vertical: 10, horizontal: 7, color: "#969690" }, me.opts.grids);
        me._axisTextStyle = { fill: me._grids.color, "font-size": 10, "font-family": "Trebuchet MS, Arial, sans-serif" };
        me._barTextStyle = { fill: me._grids.color, "font-size": 12, "font-family": "Trebuchet MS, Arial, sans-serif" };
        me._legendTextStyle = { fill: me._grids.color, "font-size": 10, "font-family": "Trebuchet MS, Arial, sans-serif" };
        me._textOffset = $.browser.msie || $.browser.webkit ? 0 : 1;
        me._effectiveSize = { width: me._width - me._gutters.left - me._gutters.right, height: me._height - me._gutters.top - me._gutters.bottom };
        me._legend = me.opts.title;
        me._bars = {};
        var barOptions = jQuery.extend({ height: 15, padding: 8, margin: 15 }, me.opts.bar);
        me._barHeight = barOptions.height;
        me._barPadding = barOptions.padding;
        me._barsMargin = barOptions.margin;
        me._token = me.opts.token;

        var _getFormattedValue = function (pRawValue) {
            return me.opts.valueFormat(pRawValue);
        };

        var _drawGridlines = function () {
            var g = me._gutters;
            me._canvas.rect(g.left, g.top, me._width - me._gutters.left - (me._fillRight ? 0 : me._gutters.right), me._effectiveSize.height).attr({ stroke: g.color, fill: "90-#f6f6f0-#fff" });
        };

        var _drawAxes = function (bounds) {

            var xMin = bounds.min,
            xMax = bounds.max,
            xBase = me._gutters.left,
            yBase = me._gutters.top + me._effectiveSize.height;

            var xGridStepPx = me._effectiveSize.width / me._grids.horizontal;
            var xGridStep = (xMax - xMin) / me._grids.horizontal;

            var xGrids = [];
            for (var j = 0; j <= me._grids.horizontal; j++) {
                var xPos = xBase + j * xGridStepPx;
                xGrids.push("M", xPos, yBase + 3, "L", xPos, yBase);
                me._canvas.text(xPos, yBase + 12, _getFormattedValue(xMin + j * xGridStep)).attr(me._axisTextStyle);
            }
            me._canvas.path().attr({ path: xGrids }).attr({ "stroke-width": 1, stroke: me._grids.color }).toBack();
            if (me._legend != null && me._legend != "") {
                me._canvas.text(me._width / 2, me._height - 10, me._legend).attr(me._legendTextStyle).attr({ "text-anchor": "middle" });
            }
        };

        var _drawBar = function (bar, bounds, top) {

            var barWidth = (bar.value - bounds.min) / (bounds.max - bounds.min) * me._effectiveSize.width;
            if (barWidth < 2) barWidth = 2;
            var thisBar = me._canvas.rect(me._gutters.left, top, 0, me._barHeight).attr({ fill: bar.color, stroke: me._gutters.color, "stroke-width": 0.5, "fill-opacity": 0.75, cursor: "pointer", title: _getFormattedValue(bar.value) }).animate({ width: barWidth }, 1500, ">");
            var thisNameBox = me._canvas.text(me.left, top + me._textOffset + me._barHeight / 2, bar.name).attr(me._barTextStyle).attr({ "text-anchor": "start" });
            var thisDescBox = me._canvas.text(me._gutters.left + barWidth + 5, top + me._textOffset + me._barHeight / 2, bar.description).attr(me._barTextStyle).attr({ "text-anchor": "start", opacity: 0 }).animateWith(thisBar, { opacity: 1 }, 2500);

            (function (bar, nameText, descText, link) {
                bar.hover(
                    function () {
                        this.attr({ "fill-opacity": 1 });
                        nameText.attr({ fill: "#555" });
                        descText.attr({ fill: "#555" });
                    },
                    function () {
                        this.attr({ "fill-opacity": 0.75 });
                        nameText.attr({ fill: me._grids.color });
                        descText.attr({ fill: me._grids.color });
                    }
                );
                //bar.click(function (event) { window.location = link; });
            })(thisBar, thisNameBox, thisDescBox, bar.link);

        };


        var _drawMarker = function (bar, bounds) {

            var left = me._gutters.left + (bar.value - bounds.min) / (bounds.max - bounds.min) * me._effectiveSize.width;
            var thisBar = me._canvas.path(["M", left, me._gutters.top + me._effectiveSize.height, "L", left, me._gutters.top]).attr({ stroke: bar.color, "stroke-width": 4, opacity: 0.6, cursor: "pointer", title: bar.name + ' ' + _getFormattedValue(bar.value) });

            (function (outerRect) {
                outerRect.hover(
                function () {
                    this.animate({ "stroke-width": 8 }, 100, "<>");
                },
                function () {
                    this.animate({ "stroke-width": 4 }, 100, "<>");
                }
            );
            })(thisBar);

        };

        var _getSuperBounds = function (boundsA, boundsB) {

            var min = boundsA.min < boundsB.min ? boundsA.min : boundsB.min;
            var max = boundsA.max > boundsB.max ? boundsA.max : boundsB.max;
            min = parseFloat(min);
            max = parseFloat(max);
            return { min: min, max: max + max * me._rangePadding };
        };

        var addBar = function (key, value, bounds, name, description, color, link) {
            if (bounds == null) {
                bounds = { min: 0, max: value };
            }
            me._bars[key] = { value: value, name: name, description: description, color: color, bounds: bounds, link: link, visible: true };
        };

        var addMarker = function (key, value, bounds, name, description, color, link) {
            if (bounds == null) {
                bounds = { min: 0, max: value };
            }
            me._bars[key] = { value: value, name: name, description: description, color: color, bounds: bounds, link: link, visible: true, isMarker: true };
        };

        var reset = function () {
            var superBounds = null;
            var newHeight = me._gutters.top + me._gutters.bottom + me._barsMargin * 2;

            for (var thisBarKey in me._bars) {
                var thisBar = me._bars[thisBarKey];
                if (thisBar.visible) {
                    if (!thisBar.isMarker) {
                        newHeight += me._barHeight + me._barPadding * 2;
                    }
                    if (superBounds == null) {
                        superBounds = thisBar.bounds;
                    }
                    else {
                        superBounds = _getSuperBounds(thisBar.bounds, superBounds);
                    }
                }
            }

            me._parentElement.height(newHeight + "px");
            me._canvas.setSize(me._width, newHeight);
            me._height = newHeight;
            me._effectiveSize = { width: me._width - me._gutters.left - me._gutters.right, height: me._height - me._gutters.top - me._gutters.bottom };

            me._canvas.clear();
            _drawGridlines();
            _drawAxes(superBounds);

        };

        var redrawChart = function () {

            var superBounds = null;
            var newHeight = me._gutters.top + me._gutters.bottom + me._barsMargin * 2;

            for (var thisBarKey in me._bars) {
                var thisBar = me._bars[thisBarKey];
                if (thisBar.visible) {
                    if (!thisBar.isMarker) {
                        newHeight += me._barHeight + me._barPadding * 2;
                    }
                    if (superBounds == null) {
                        superBounds = thisBar.bounds;
                    }
                    else {
                        superBounds = _getSuperBounds(thisBar.bounds, superBounds);
                    }
                }
            }

            me._parentElement.height(newHeight + "px");
            me._canvas.setSize(me._width, newHeight);
            me._height = newHeight;
            me._effectiveSize = { width: me._width - me._gutters.left - me._gutters.right, height: me._height - me._gutters.top - me._gutters.bottom };

            me._canvas.clear();
            _drawGridlines();
            _drawAxes(superBounds);

            var barTop = me._gutters.top + me._barPadding + me._barsMargin;

            for (var thisBarKey in me._bars) {
                var thisBar = me._bars[thisBarKey];
                if (thisBar.visible) {
                    if (thisBar.isMarker) {
                        _drawMarker(thisBar, superBounds);
                    }
                    else {
                        _drawBar(thisBar, superBounds, barTop);
                        barTop += (me._barHeight + me._barPadding * 2);
                    }
                }
            }
        }

        var publicObj = {
            draw: function () {
                redrawChart(2000);
            },
            getContainer: function () {
                return me.container;
            }
        };
        init();
        return publicObj;

    };

})();

