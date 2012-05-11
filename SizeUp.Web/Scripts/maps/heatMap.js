(function () {
    sizeup.core.namespace('sizeup.maps');
    sizeup.maps.polygonCache = sizeup.maps.polygonCache || new function(){

        var dataLayer = new sizeup.core.data();
        var me = {};
        var chunks = {
            zip: 25,
            county: 10,
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

        me.data = {
            polygons: {
                zip: [],
                county: [],
                metro: [],
                state:[]
            },
            bands: {
                zip: null,
                county: null,
                metro: null,
                state:null
            }
        };

        var init = function () {

            me.legend = me.container.find('.legend');
            me.map = new sizeup.maps.map({
                container: me.container.find('.container'),
                mapSettings: me.opts.mapSettings,
                styles: me.opts.styles
            });
            me.map.addEventListener('bounds_changed', boundsChanged);
            me.map.addEventListener('idle', boundsChanged);
            me.map.addEventListener('zoom_changed', zoomChanged);

            updatePolygonResolution();
        };
        
       
        var buildPolygon = function (polyData, visible) {
            var latlngs = [];
            for (var x = 0; x < polyData.Paths.length; x++) {
                var poly = []
                latlngs.push(poly);
                for (var y = 0; y < polyData.Paths[x].length; y++) {
                    poly.push(new sizeup.maps.latLng({ lat: polyData.Paths[x][y].Lat, lng: polyData.Paths[x][y].Lng }).getNative());
                }
            };
            var c = me.opts.colors[Math.floor((Math.random() * me.opts.colors.length))];
            var p = new sizeup.maps.polygon({
                visible: visible,
                clickable: false,
                fillColor: c,
                fillOpacity: 0.6,
                strokeColor: c,
                strokeOpacity: 1,
                strokeWeight: 1
            });
            p.getNative().setPaths(latlngs);
            return p;
        };

        var switchPolygonResolution = function (level) {
            for (var x in me.data.polygons) {
                if (x == level) {
                    for (var y in me.data.polygons[x]) {
                        me.data.polygons[x][y].setVisible(true);
                    }
                }
                else {
                    for (var y in me.data.polygons[x]) {
                        me.data.polygons[x][y].setVisible(false);
                    }
                }
            }
        };

        var updatePolygonResolution = function () {
            var oldRes = me.polygonResolution;
            var z = me.map.getZoom();
            if (z < 6) {
                me.polygonResolution = "state";
            }
            else if (z < 10) {
                me.polygonResolution = "county";
            }
            else if (z < 12) {
                me.polygonResolution = "zip";
            }


            if (oldRes != me.polygonResolution) {
                switchPolygonResolution(me.polygonResolution);
            }
        };

        var zoomChanged = function () {
            updatePolygonResolution();
        };

        var boundsChanged = function () {
            var bounds = me.map.getBounds();
            var sw = bounds.getSouthWest();
            var ne = bounds.getNorthEast();

            if (!(me.statesXHR) || me.statesXHR.statusText == "OK") {
                if (me.polygonResolution == 'state') {
                    me.statesXHR = dataLayer.getStatesInBounds({ sw: sw.lat() + ' ' + sw.lng(), ne: ne.lat() + ' ' + ne.lng(), buffer: 100000 }, function(d){ boundsReturned(d, 'state');});
                }
                else if (me.polygonResolution == 'county') {
                    me.statesXHR = dataLayer.getCountiesInBounds({ sw: sw.lat() + ' ' + sw.lng(), ne: ne.lat() + ' ' + ne.lng(), buffer: 10000 },  function(d){ boundsReturned(d, 'county');});
                }
                else if (me.polygonResolution == 'zip') {
                    me.statesXHR = dataLayer.getZipCodesInBounds({ sw: sw.lat() + ' ' + sw.lng(), ne: ne.lat() + ' ' + ne.lng(), buffer: 2500 }, function (d) { boundsReturned(d, 'zip');});
                }
            }
            
        };

        var boundsReturned = function (data, index) {
            //opts.dataSources.state(bandsReturned);

            sizeup.maps.polygonCache.get(data, index, function (polys) {
                for (var x in polys) {
                    if (!me.data.polygons[index][x]) {
                        var p = buildPolygon(polys[x], me.polygonResolution == index);
                        me.data.polygons[index][x] = p;
                        me.map.addPolygon(p);
                    }
                }
            });
        };

        var countyBoundsReturned = function (data) {
            sizeup.maps.polygonCache.get(data, 'county',  function (polys) {
                for (var x in polys) {
                    if (!me.data.polygons.county[x]) {
                        var p = buildPolygon(polys[x], me.polygonResolution == 'county');
                        me.data.polygons.county[x] = p;
                        me.map.addPolygon(p);
                    }
                }
            });
        };

        var zipCodeBoundsReturned = function (data) {
            sizeup.maps.polygonCache.get(data,'zip', function (polys) {
                for (var x in polys) {
                    if (!me.data.polygons.zip[x]) {
                        var p = buildPolygon(polys[x], me.polygonResolution == 'zip');
                        me.data.polygons.zip[x] = p;
                        me.map.addPolygon(p);
                    }
                }
            });
        };


        var bandsReturned = function (data) {
            
        }


        var publicObj = {
            getContainer: function () {
                return me.container;
            }
        };
        init();
        return publicObj;

    };
})();

















