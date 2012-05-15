(function () {
    sizeup.core.namespace('sizeup.maps');
    sizeup.maps.polygonCache = sizeup.maps.polygonCache || new function(){

        var dataLayer = new sizeup.core.data();
        var me = {};
        var chunks = {
            zip: 25,
            county: 100,
            metro: 5,
            state: 7
        };
        var dataSources = {
            zip: dataLayer.getZipCodePolygons,
            county: dataLayer.getCountyPolygons,
            metro: null,
            state: dataLayer.getStatePolygons
        };

        var cache = {
            zip: [],
            county: [],
            metro: [],
            state:[]
        };


        var get = function (ids, index, callback) {
            var needs = [];
            for (var x in ids) {
                if (!cache[index][ids[x]] || cache[index][ids[x]] == '') {
                    cache[index][ids[x]] = '';
                    needs.push(ids[x]);
                }
            }
            var ret = function (data) {
                var polys = [];
                for (var x in data) {
                    cache[index][data[x].Id] = data[x].Polygon;
                }
                for (var x in ids) {
                    if (cache[index][ids[x]] != '') {
                        polys[ids[x]] = cache[index][ids[x]];
                    }
                }
                callback(polys);
            };
            if (needs.length > 0) {
                for (var x = 0; x < needs.length; x = x + chunks[index]) {
                    var partialNeeds = needs.slice(x, Math.min(needs.length, x + chunks[index]));
                    dataSources[index]({ ids: partialNeeds.join() }, ret);
                }
            }
        };

        var publicObj = {
            get: function (ids, index, callback) {
                return get(ids, index,  callback);
            }
        };
        return publicObj;
    };

    sizeup.maps.heatMap = function (opts) {

        var defaults = {
            mapSettings: sizeup.maps.mapOptions.getDefaults(),
            styles: sizeup.maps.mapStyles.getDefaults(),
            colors: [
                '#F5F500',
                '#F5CC00',
                '#F5A300',
                '#F57A00',
                '#F55200',
                '#F52900',
                '#F50000'
            ]
        };
        var dataLayer = new sizeup.core.data();
        var me = {};
        me.opts = $.extend(true, defaults, opts);
        me.container = opts.container;
        me.imageMap ={
            zip: null,
            county: null,
            metro: null,
            state:null
        };


        var init = function () {

            me.legend = me.container.find('.legend');
            me.map = new sizeup.maps.map({
                container: me.container.find('.container'),
                mapSettings: me.opts.mapSettings,
                styles: me.opts.styles
            });
            
            var zipOverlay = {
                getTileUrl: function (point, zoom) {
                    var url = "/tiles/salary/zip/";
                    var params = {
                        industryId: 1,
                        x: point.x,
                        y: point.y,
                        zoom: zoom,
                        colors: me.opts.colors.join(',')
                    };
                    return jQuery.param.querystring(url, params);
                },
                tileSize: new google.maps.Size(256, 256)
            };
            var countyOverlay = {
                getTileUrl: function (point, zoom) {
                    var url = "/tiles/salary/county/";
                    var params = {
                        industryId :1,
                        x : point.x,
                        y: point.y,
                        zoom: zoom,
                        colors: me.opts.colors.join(',')
                    };
                    return jQuery.param.querystring(url, params);
                },
                tileSize: new google.maps.Size(256, 256)
            };
            var stateOverlay = {
                getTileUrl: function (point, zoom) {
                    var url = "/tiles/salary/state/";
                    var params = {
                        industryId: 1,
                        x: point.x,
                        y: point.y,
                        zoom: zoom,
                        colors: me.opts.colors.join(',')
                    };
                    return jQuery.param.querystring(url, params);
                },
                tileSize: new google.maps.Size(256, 256)
            };
            me.imageMap["zip"] = new google.maps.ImageMapType(zipOverlay);
            me.imageMap["county"] =new  google.maps.ImageMapType(countyOverlay);
            me.imageMap["state"] = new google.maps.ImageMapType(stateOverlay);
          

           // me.map.addEventListener('bounds_changed', boundsChanged);
           // me.map.addEventListener('idle', boundsChanged);
            me.map.addEventListener('zoom_changed', zoomChanged);
            setOverlay();
            //updatePolygonResolution();
        };
        
        var setOverlay = function () {
            var z = me.map.getNative().getZoom();
            me.map.getNative().overlayMapTypes.clear();
            if (z >= 0 && z <= 4) {
                me.map.getNative().overlayMapTypes.push(me.imageMap['state']);
            }
            else if (z >= 5 && z <= 10) {
                me.map.getNative().overlayMapTypes.push(me.imageMap['county']);
            }
            else if (z >= 11 && z <= 18) {
                me.map.getNative().overlayMapTypes.push(me.imageMap['zip']);
            }
        };

        

       

        var zoomChanged = function () {
            setOverlay();
        };

        




        var publicObj = {
            getContainer: function () {
                return me.container;
            }
        };
        init();
        return publicObj;

    };
})();

















