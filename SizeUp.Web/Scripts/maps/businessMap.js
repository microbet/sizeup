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
        me.footer = me.container.find('.footer');
        me.overlay = null;
        me.data = {};
        me.activeOverlayFilter = 'all';
        var init = function () {

            me.opts.mapSettings.zoom = 20;
            me.map = new sizeup.maps.map({
                container: me.container.find('.map'),
                mapSettings: me.opts.mapSettings,
                styles: me.opts.styles
            });
            me.map.addEventListener('zoom_changed', zoomChanged);

            me.footer.find('input').change(filterChanged);
        };

        var filterChanged = function (e) {
            var target = $(e.target);
            me.activeOverlayFilter = target.attr('data-index');
            setOverlay();
        };

        var zoomChanged = function () {
            if (me.map.getZoom() <= me.opts.primaryIndexZoomFilter) {
                
                var primary = me.footer.find('.' + me.opts.primaryIndex + ' input');
                if (!primary.is(':checked')) {
                    primary.attr('checked', 'checked');
                }

                me.footer.find('.all input').attr('disabled', true).parent().addClass('disabled');
                me.footer.find('.all label .zoomMessage').show();
            }
            else {
                me.footer.find('.all input').attr('disabled', false).parent().removeClass('disabled');
                me.footer.find('.' + me.activeOverlayFilter + ' input').attr('checked', 'checked');
                me.footer.find('.all label .zoomMessage').hide();
            }
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

                    var filter = '';
                    if (me.footer.find('.competitor input').is(':checked')) {
                        filter = 'competitor';
                    }
                    else if (me.footer.find('.buyer input').is(':checked')) {
                        filter = 'buyer';
                    }
                    else if (me.footer.find('.supplier input').is(':checked')) {
                        filter = 'supplier';
                    }

                    if (filter == 'competitor') {
                        delete params.buyerIndustryIds;
                        delete params.supplierIndustryIds;
                    }
                    else if (filter == 'buyer') {
                        delete params.competitorIndustryIds;
                        delete params.supplierIndustryIds;
                    }
                    else if (filter == 'supplier') {
                        delete params.buyerIndustryIds;
                        delete params.competitorIndustryIds;
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

        var removeMarker = function (marker) {
            me.map.removeMarker(marker);
        };

        var getNative = function () {
            return me.map.getNative();
        };

        var setupFilter = function () {
            var indexCount = 0;
            if (me.data.competitorIndustryIds != null && me.data.competitorIndustryIds.length > 0) {
                indexCount = indexCount + 1;
            }
            if (me.data.buyerIndustryIds != null && me.data.buyerIndustryIds.length > 0) {
                indexCount = indexCount + 1;
            }
            if (me.data.supplierIndustryIds != null && me.data.supplierIndustryIds.length > 0) {
                indexCount = indexCount + 1;
            }

            if (indexCount > 1) {
                me.footer.show();
            }
            else {
                me.footer.hide();
            }
        };

        var getActiveIndexes = function () {
            var filter = {};
            if (me.footer.find('.competitor input').is(':checked')) {
                filter['competitor'] = true;
            }
            else if (me.footer.find('.buyer input').is(':checked')) {
                filter['buyer'] = true;
            }
            else if (me.footer.find('.supplier input').is(':checked')) {
                filter['supplier'] = true;
            }
            else {
                filter['competitor'] = true;
                filter['buyer'] = true;
                filter['supplier'] = true;
            }
            return filter;
        };

        var setIndustryIds = function (obj) {
            me.data.competitorIndustryIds = obj.competitorIndustryIds;
            me.data.buyerIndustryIds = obj.buyerIndustryIds;
            me.data.supplierIndustryIds = obj.supplierIndustryIds;
            setupFilter();
            buildOverlay();
            setOverlay();
        };

        var addEventListener = function (event, callback) {
            me.map.addEventListener(event, callback);
        };

        var setZoom = function(zoom){
            me.map.setZoom(zoom);
        };

        var setCenter = function (latLng) {
            me.map.setCenter(latLng);
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
            },
            addEventListener: function (event, callback) {
                addEventListener(event, callback);
            },
            setZoom: function (zoom) {
                setZoom(zoom);
            },
            getActiveIndexes: function () {
                return getActiveIndexes();
            },
            setCenter: function (latLng) {
                setCenter(latLng);
            }
        };
        init();
        return publicObj;

    };
})();

















