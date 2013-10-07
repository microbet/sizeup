(function () {
    sizeup.core.namespace('sizeup.maps');
    sizeup.maps.heatMapOverlays = function (opts) {



        var defaults = {
            zoomExtent: {
                County: 12,
                Metro: 10,
                State: 5
            },
            startColor: '#ff0000',
            endColor: '#ffff00',
            bands: 5,
            place: {},
            params: {},
            opacity:0.9,
            smallestGranularity: sizeup.api.granularity.ZIP_CODE,
            attributeLabel: 'Unknown',
            format: function (val) { return val; },
            legendData: function () { },
            templates: new sizeup.core.templates()
        };
        var me = {};
        me.xhr = null;
        me.opts = $.extend(true, defaults, opts);
        var heatmapOpts = { startColor: me.opts.startColor, endColor: me.opts.endColor, colors: me.opts.bands };
        var heatmapColors = new sizeup.maps.heatmapColors(heatmapOpts);
        me.opts.colors = heatmapColors.getColors();


        var init = function () {

        };
       
        var getZipCodeZoomLevels = function () {
            var levels = [];
            levels.push({
                granularity: sizeup.api.granularity.ZIP_CODE,
                boundingGeographicLocationId: me.opts.place.County.Id,
                minZoom: me.opts.zoomExtent.County,
                maxZoom: 92
            });

            if (me.opts.zoomExtent.Metro != null && me.opts.zoomExtent.County > me.opts.zoomExtent.Metro) {
                levels.push({
                    granularity: sizeup.api.granularity.COUNTY,
                    boundingGeographicLocationId: me.opts.place.Metro.Id,
                    minZoom: me.opts.zoomExtent.Metro,
                    maxZoom: me.opts.zoomExtent.County - 1
                });

                levels.push({
                    granularity: sizeup.api.granularity.COUNTY,
                    boundingGeographicLocationId: me.opts.place.State.Id,
                    minZoom: me.opts.zoomExtent.State,
                    maxZoom: me.opts.zoomExtent.Metro - 1
                });
            }

            if (me.opts.zoomExtent.Metro == null || me.opts.zoomExtent.County == me.opts.zoomExtent.Metro) {
                levels.push({
                    granularity: sizeup.api.granularity.COUNTY,
                    boundingGeographicLocationId: me.opts.place.State.Id,
                    minZoom: me.opts.zoomExtent.State,
                    maxZoom: me.opts.zoomExtent.County - 1
                });
            }


            if (me.opts.zoomExtent.Metro != null && me.opts.zoomExtent.State > me.opts.zoomExtent.Metro) {
                levels.push({
                    granularity: sizeup.api.granularity.STATE,
                    boundingGeographicLocationId: me.opts.place.Nation.Id,
                    minZoom: 0,
                    maxZoom: me.opts.zoomExtent.Metro - 1
                });
            }

            if (me.opts.zoomExtent.Metro == null || me.opts.zoomExtent.State <= me.opts.zoomExtent.Metro) {
                levels.push({
                    granularity: sizeup.api.granularity.STATE,
                    boundingGeographicLocationId: me.opts.place.Nation.Id,
                    minZoom: 0,
                    maxZoom: me.opts.zoomExtent.State - 1
                });
            }

            return levels;
        };

        var getCountyZoomLevels = function () {
            var levels = [];

            if (me.opts.zoomExtent.Metro != null && me.opts.zoomExtent.County > me.opts.zoomExtent.Metro) {
                levels.push({
                    granularity: sizeup.api.granularity.COUNTY,
                    boundingGeographicLocationId: me.opts.place.Metro.Id,
                    minZoom: me.opts.zoomExtent.Metro,
                    maxZoom: 92
                });
                levels.push({
                    granularity: sizeup.api.granularity.COUNTY,
                    boundingGeographicLocationId: me.opts.place.State.Id,
                    minZoom: me.opts.zoomExtent.State,
                    maxZoom: me.opts.zoomExtent.Metro - 1
                });
            }
            if (me.opts.zoomExtent.Metro == null || me.opts.zoomExtent.County == me.opts.zoomExtent.Metro) {
                levels.push({
                    granularity: sizeup.api.granularity.COUNTY,
                    boundingGeographicLocationId: me.opts.place.State.Id,
                    minZoom: me.opts.zoomExtent.State,
                    maxZoom: 32
                });
            }


            if (me.opts.zoomExtent.Metro != null && me.opts.zoomExtent.State > me.opts.zoomExtent.Metro) {
                levels.push({
                    granularity: sizeup.api.granularity.STATE,
                    boundingGeographicLocationId: me.opts.place.Nation.Id,
                    minZoom: 0,
                    maxZoom: me.opts.zoomExtent.Metro - 1
                });
            }

            if (me.opts.zoomExtent.Metro == null || me.opts.zoomExtent.State <= me.opts.zoomExtent.Metro) {
                levels.push({
                    granularity: sizeup.api.granularity.STATE,
                    boundingGeographicLocationId: me.opts.place.Nation.Id,
                    minZoom: 0,
                    maxZoom: me.opts.zoomExtent.State - 1
                });
            }

            return levels;
        };

        var buildOverlays = function (zooms) {
            var overlays = [];
            for (var z in zooms) {
                var p = {
                    attribute: me.opts.attribute,
                    opacity: me.opts.opacity,
                    tileParams: $.extend(true, {
                        startColor: me.opts.startColor,
                        endColor: me.opts.endColor,
                        bands: me.opts.bands,
                        boundingGeographicLocationId: zooms[z].boundingGeographicLocationId,
                        granularity: zooms[z].granularity
                    }, me.opts.params),
                    minZoom: zooms[z].minZoom,
                    maxZoom: zooms[z].maxZoom
                };
                overlays.push(new sizeup.maps.overlay(p));
            };
            return overlays;
        };


        var getZipCodeOverlays = function(){
            var overlays = buildOverlays(getZipCodeZoomLevels());
            return overlays;
        };

        var getCountyOverlays = function(){
            var overlays = buildOverlays(getCountyZoomLevels());
            return overlays;
        };


        var getOverlays = function () {
            var overlays = [];
            if (me.opts.smallestGranularity == sizeup.api.granularity.ZIP_CODE) {
                overlays = getZipCodeOverlays();
            }
            else if (me.opts.smallestGranularity == sizeup.api.granularity.COUNTY) {
                overlays = getCountyOverlays();
            }
            return overlays;
        };

        var getTitle = function (zoom) {
            var level = getParams(zoom);
            var titleTemplate = '{{attribute}} by {{granularity}} in {{boundingGranularity}}';
            var params = {
                attribute: me.opts.attributeLabel
            };
            if (level.granularity == sizeup.api.granularity.ZIP_CODE) {
                params.granularity = 'ZIP code';
            }
            if (level.granularity == sizeup.api.granularity.COUNTY) {
                params.granularity = 'county';
            }
            if (level.granularity == sizeup.api.granularity.STATE) {
                params.granularity = 'state';
            }

            if (level.boundingGeographicLocationId == me.opts.place.County.Id) {
                params.boundingGranularity = me.opts.place.County.Name + ', ' + me.opts.place.State.Abbreviation;
            }
            if (level.boundingGeographicLocationId == me.opts.place.Metro.Id) {
                params.boundingGranularity = me.opts.place.Metro.Name;
            }
            if (level.boundingGeographicLocationId == me.opts.place.State.Id) {
                params.boundingGranularity = me.opts.place.State.Name;
            }
            if (level.boundingGeographicLocationId == me.opts.place.Nation.Id) {
                params.boundingGranularity = 'the USA';
            }

            return me.opts.templates.bind(titleTemplate, params);
        };

        var getZoomExtent = function () {
            return me.opts.zoomExtent;
        };

        var getLegend = function (zoom, callback) {
            var innerCallback = function (data) {
                me.xhr = null;
                var legend = new sizeup.maps.legend({
                    templates: me.opts.templates,
                    title: getTitle(zoom),
                    items: data,
                    startColor: me.opts.startColor,
                    endColor: me.opts.endColor,
                    format: me.opts.format
                });
                if (callback!=null && data!=null) {
                    callback(legend);
                }
            };

            if (me.xhr != null) {
                me.xhr.abort();
            }
            me.xhr =  me.opts.legendData(getParams(zoom), innerCallback);
        };

        var getParams = function (zoom) {
            var levels = [];
            var level = null;
            if (me.opts.smallestGranularity == sizeup.api.granularity.ZIP_CODE) {
                levels = getZipCodeZoomLevels();
            }
            else if (me.opts.smallestGranularity == sizeup.api.granularity.COUNTY) {
                levels = getCountyZoomLevels();
            }
            for (var z in levels) {
                if (levels[z].minZoom <= zoom && levels[z].maxZoom >= zoom) {
                    level = levels[z];
                }
            };
            return $.extend(true, {
                granularity: level.granularity,
                boundingGeographicLocationId: level.boundingGeographicLocationId,
                bands: me.opts.bands
            }, me.opts.params);
        };


        var publicObj = {
            getOverlays: function () {
                return getOverlays();
            },
            getZoomExtent: function () {
                return getZoomExtent();
            },
            getLegend: function (zoom, callback) {
                getLegend(zoom, callback);
            },
            getParams: function (zoom) {
                return getParams(zoom);
            },
            getTitle: function (zoom) {
                return getTitle(zoom);
            }
        };
        init();
        return publicObj;

    };
})();

