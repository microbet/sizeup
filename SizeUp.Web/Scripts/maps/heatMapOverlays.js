(function () {
    sizeup.core.namespace('sizeup.maps');
    sizeup.maps.heatMapOverlays = function (opts) {



        var defaults = {
            zoomExtent: {
                County: 12,
                Metro: 10,
                State: 5
            },
            place: {},
            params: {},
            tileUrl: '',
            colors: [
                '#F50000',
                '#F52900',
                '#F55200',
                '#F57A00',
                '#F5A300',
                '#F5CC00',
                '#F5F500'
            ],
            opacity:0.9,
            smallestGranularity: 'ZipCode',
            attributeLabel: 'Unknown',
            format: function (val) { return val; },
            legendData: function () { },
            templates: new sizeup.core.templates()
        };
        var me = {};
        me.xhr = null;
        me.opts = $.extend(true, defaults, opts);
        
        var init = function () {

        };
       
        var getZipCodeZoomLevels = function () {
            var levels = [];
            levels.push({
                granularity: 'ZipCode',
                boundingGranularity: 'County',
                minZoom: me.opts.zoomExtent.County,
                maxZoom: 32
            });

            if (me.opts.zoomExtent.Metro != null && me.opts.zoomExtent.County > me.opts.zoomExtent.Metro) {
                levels.push({
                    granularity: 'County',
                    boundingGranularity: 'Metro',
                    minZoom: me.opts.zoomExtent.Metro,
                    maxZoom: me.opts.zoomExtent.County - 1
                });

                levels.push({
                    granularity: 'County',
                    boundingGranularity: 'State',
                    minZoom: me.opts.zoomExtent.State,
                    maxZoom: me.opts.zoomExtent.Metro - 1
                });
            }

            if (me.opts.zoomExtent.Metro == null || me.opts.zoomExtent.County == me.opts.zoomExtent.Metro) {
                levels.push({
                    granularity: 'County',
                    boundingGranularity: 'State',
                    minZoom: me.opts.zoomExtent.State,
                    maxZoom: me.opts.zoomExtent.County - 1
                });
            }


            if (me.opts.zoomExtent.Metro != null && me.opts.zoomExtent.State > me.opts.zoomExtent.Metro) {
                levels.push({
                    granularity: 'State',
                    boundingGranularity: 'Nation',
                    minZoom: 0,
                    maxZoom: me.opts.zoomExtent.Metro - 1
                });
            }

            if (me.opts.zoomExtent.State <= me.opts.zoomExtent.Metro) {
                levels.push({
                    granularity: 'State',
                    boundingGranularity: 'Nation',
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
                    granularity: 'County',
                    boundingGranularity: 'Metro',
                    minZoom: me.opts.zoomExtent.Metro,
                    maxZoom: 32
                });
                levels.push({
                    granularity: 'County',
                    boundingGranularity: 'State',
                    minZoom: me.opts.zoomExtent.State,
                    maxZoom: me.opts.zoomExtent.Metro - 1
                });
            }
            if (me.opts.zoomExtent.Metro == null || me.opts.zoomExtent.County == me.opts.zoomExtent.Metro) {
                levels.push({
                    granularity: 'County',
                    boundingGranularity: 'State',
                    minZoom: me.opts.zoomExtent.State,
                    maxZoom: 32
                });
            }


            if (me.opts.zoomExtent.Metro != null && me.opts.zoomExtent.State > me.opts.zoomExtent.Metro) {
                levels.push({
                    granularity: 'State',
                    boundingGranularity: 'Nation',
                    minZoom: 0,
                    maxZoom: me.opts.zoomExtent.Metro - 1
                });
            }

            if (me.opts.zoomExtent.State <= me.opts.zoomExtent.Metro) {
                levels.push({
                    granularity: 'State',
                    boundingGranularity: 'Nation',
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
                    tileUrl: me.opts.tileUrl,
                    opacity: me.opts.opacity,
                    tileParams: $.extend(true, {
                        colors: me.opts.colors,
                        placeId: me.opts.place.Id,
                        granularity: zooms[z].granularity,
                        boundingGranularity : zooms[z].boundingGranularity
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
            if (me.opts.smallestGranularity == 'ZipCode') {
                overlays = getZipCodeOverlays();
            }
            else if (me.opts.smallestGranularity == 'County') {
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
            if (level.granularity == 'ZipCode') {
                params.granularity = 'ZIP code';
            }
            if (level.granularity == 'County') {
                params.granularity = 'county';
            }
            if (level.granularity == 'State') {
                params.granularity = 'state';
            }

            if (level.boundingGranularity == 'County') {
                params.boundingGranularity = me.opts.place.County.Name + ', ' + me.opts.place.State.Abbreviation;
            }
            if (level.boundingGranularity == 'Metro') {
                params.boundingGranularity = me.opts.place.Metro.Name;
            }
            if (level.boundingGranularity == 'State') {
                params.boundingGranularity = me.opts.place.State.Name;
            }
            if (level.boundingGranularity == 'Nation') {
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
                    colors: me.opts.colors,
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
            if (me.opts.smallestGranularity == 'ZipCode') {
                levels = getZipCodeZoomLevels();
            }
            else if (me.opts.smallestGranularity == 'County') {
                levels = getCountyZoomLevels();
            }
            for (var z in levels) {
                if (levels[z].minZoom <= zoom && levels[z].maxZoom >= zoom) {
                    level = levels[z];
                }
            };
            return $.extend(true, {
                granularity: level.granularity,
                boundingGranularity: level.boundingGranularity,
                placeId: me.opts.place.Id,
                bands: me.opts.colors.length
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

