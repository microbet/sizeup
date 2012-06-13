(function () {
    sizeup.core.namespace('sizeup.maps');
    sizeup.maps.businessMap = function (opts) {

        var defaults = {
            mapSettings: sizeup.maps.mapOptions.getDefaults(),
            styles: sizeup.maps.mapStyles.getDefaults(),
            cityId: 0,
            radius: 100,
            primaryIndex: '',
            primaryIndexZoomFilter: 10
        };
        var dataLayer = new sizeup.core.data();
        var templates = new sizeup.core.templates();
        var me = {};
        me.opts = $.extend(true, defaults, opts);
        me.container = opts.container;
        me.overlay = null;
        me.data = {};

        var init = function () {

            me.map = new sizeup.maps.map({
                container: me.container,
                mapSettings: me.opts.mapSettings,
                styles: me.opts.styles
            });
        };

        var buildOverlay = function () {
            me.overlay = new google.maps.ImageMapType({
                getTileUrl: function (point, zoom) {
                    var url = '/tiles/businesses/';
                    var params = {
                        competitorIndustryIds: me.data.competitorIndustryIds,
                        buyerIndustryIds: me.data.buyerIndustryIds,
                        supplierIndustryIds: me.data.supplierIndustryIds,
                        x: point.x,
                        y: point.y,
                        zoom: zoom,
                        radius: opts.radius,
                        cityId: opts.cityId
                    };
                    if (zoom <= me.opts.primaryIndexZoomFilter) {
                        if (me.opts.primaryIndex == 'competitor') {
                            delete params.buyerIndustryIds;
                            delete params.supplierIndustryIds;
                        }
                        else if (me.opts.primaryIndex == 'buyer') {
                            delete params.competitorIndustryIds;
                            delete params.supplierIndustryIds;
                        }
                        else if (me.opts.primaryIndex == 'supplier') {
                            delete params.buyerIndustryIds;
                            delete params.competitorIndustryIds;
                        }
                    }

                    return jQuery.param.querystring(url, params);
                },
                tileSize: new google.maps.Size(256, 256)
            });
        };

        var setOverlay = function () {
            me.map.getNative().overlayMapTypes.clear();
            me.map.getNative().overlayMapTypes.push(me.overlay);
        };

       
        var addMarker = function (marker) {
            me.map.addMarker(marker);
        };

        var triggerEvent = function (event) {
            me.map.triggerEvent(event);
        };

        var fitBounds = function (latLngBounds) {
            me.map.fitBounds(latLngBounds);
        };



        var zoomChanged = function () {
            setOverlay();
        };

        var removeMarker = function (marker) {
            me.map.removeMarker(marker);
        };

        var getNative = function () {
            return me.map.getNative();
        };

        var setIndustryIds = function (obj) {
            me.data.competitorIndustryIds = obj.competitorIndustryIds;
            me.data.buyerIndustryIds = obj.buyerIndustryIds;
            me.data.supplierIndustryIds = obj.supplierIndustryIds;
            buildOverlay();
            setOverlay();
        };



        var publicObj = {
            getNative: function(){
                return getNative();
            },
            getContainer: function () {
                return me.container;
            },
            addMarker: function (marker) {
                addMarker(marker);
            },
            addPin: function (pin) {

            },
            triggerEvent: function (event) {
                triggerEvent(event);
            },
            fitBounds: function (latLgBounds) {
                fitBounds(latLgBounds);
            },
            removeMarker: function (marker) {
                removeMarker(marker);
            },
            setIndustryIds: function (obj) {
                setIndustryIds(obj);
            }
        };
        init();
        return publicObj;

    };
})();

















