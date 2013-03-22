(function () {
    sizeup.core.namespace('sizeup.maps');
    sizeup.maps.heatMapOverlays = function (opts) {



        var defaults = {
            zoomExtent: {
                County: 12,
                Metro: 10,
                State: 5
            },
            placeId: 3051,
            industryId: 8589,
            tileUrl: '',
            colors: [
                '#F5F500',
                '#F5CC00',
                '#F5A300',
                '#F57A00',
                '#F55200',
                '#F52900',
                '#F50000'
            ],
            smallestGranularity: 'ZipCode'
        };
        var me = {};
        me.opts = $.extend(true, defaults, opts);
        
        var init = function () {
            /*if (me.opts.zoomExtent.County <= me.opts.zoomExtent.Metro) {
                me.opts.zoomExtent.Metro = null;
            }

            if (me.opts.zoomExtent.Metro <= me.opts.zoomExtent.State) {
                me.opts.zoomExtent.State = me.opts.zoomExtent.Metro - 1;
            }*/

           
           
        };
       
        var getZipCodeOverlays = function(){
            var overlays = [];


            overlays.push(new sizeup.maps.overlay({
                tileUrl: me.opts.tileUrl,
                tileParams: {
                    colors: me.opts.colors,
                    placeId: me.opts.placeId,
                    industryId: me.opts.industryId,
                    granularity: 'ZipCode',
                    boundingGranularity: 'County'
                },
                minZoom: me.opts.zoomExtent.County,
                maxZoom: 32
            }));


            if (me.opts.zoomExtent.Metro != null && me.opts.zoomExtent.County > me.opts.zoomExtent.Metro) {

                overlays.push(new sizeup.maps.overlay({
                    tileUrl: me.opts.tileUrl,
                    tileParams: {
                        colors: me.opts.colors,
                        placeId: me.opts.placeId,
                        industryId: me.opts.industryId,
                        granularity: 'County',
                        boundingGranularity: 'Metro'
                    },
                    minZoom: me.opts.zoomExtent.Metro,
                    maxZoom: me.opts.zoomExtent.County - 1
                }));

                overlays.push(new sizeup.maps.overlay({
                    tileUrl: me.opts.tileUrl,
                    tileParams: {
                        colors: me.opts.colors,
                        placeId: me.opts.placeId,
                        industryId: me.opts.industryId,
                        granularity: 'County',
                        boundingGranularity: 'State'
                    },
                    minZoom: me.opts.zoomExtent.State,
                    maxZoom: me.opts.zoomExtent.Metro - 1
                }));
            }

            if (me.opts.zoomExtent.Metro == null || me.opts.zoomExtent.County == me.opts.zoomExtent.Metro) {
                overlays.push(new sizeup.maps.overlay({
                    tileUrl: me.opts.tileUrl,
                    tileParams: {
                        colors: me.opts.colors,
                        placeId: me.opts.placeId,
                        industryId: me.opts.industryId,
                        granularity: 'County',
                        boundingGranularity: 'State'
                    },
                    minZoom: me.opts.zoomExtent.State,
                    maxZoom: me.opts.zoomExtent.County - 1
                }));
            }

            
            if (me.opts.zoomExtent.Metro != null && me.opts.zoomExtent.State > me.opts.zoomExtent.Metro) {
                overlays.push(new sizeup.maps.overlay({
                    tileUrl: me.opts.tileUrl,
                    tileParams: {
                        colors: me.opts.colors,
                        placeId: me.opts.placeId,
                        industryId: me.opts.industryId,
                        granularity: 'State'
                    },
                    minZoom: 0,
                    maxZoom: me.opts.zoomExtent.Metro - 1
                }));
            }

            if (me.opts.zoomExtent.State <= me.opts.zoomExtent.Metro) {
                overlays.push(new sizeup.maps.overlay({
                    tileUrl: me.opts.tileUrl,
                    tileParams: {
                        colors: me.opts.colors,
                        placeId: me.opts.placeId,
                        industryId: me.opts.industryId,
                        granularity: 'State'
                    },
                    minZoom: 0,
                    maxZoom: me.opts.zoomExtent.State - 1
                }));
            }

            return overlays;
        };

        var getCountyOverlays = function(){
            var overlays = [];

            if (me.opts.zoomExtent.Metro != null && me.opts.zoomExtent.County > me.opts.zoomExtent.Metro) {

                overlays.push(new sizeup.maps.overlay({
                    tileUrl: me.opts.tileUrl,
                    tileParams: {
                        colors: me.opts.colors,
                        placeId: me.opts.placeId,
                        industryId: me.opts.industryId,
                        granularity: 'County',
                        boundingGranularity: 'Metro'
                    },
                    minZoom: me.opts.zoomExtent.Metro,
                    maxZoom: 32
                }));


                overlays.push(new sizeup.maps.overlay({
                    tileUrl: me.opts.tileUrl,
                    tileParams: {
                        colors: me.opts.colors,
                        placeId: me.opts.placeId,
                        industryId: me.opts.industryId,
                        granularity: 'County',
                        boundingGranularity: 'State'
                    },
                    minZoom: me.opts.zoomExtent.State,
                    maxZoom: me.opts.zoomExtent.Metro - 1
                }));
            }
            if (me.opts.zoomExtent.Metro == null || me.opts.zoomExtent.County == me.opts.zoomExtent.Metro) {
                overlays.push(new sizeup.maps.overlay({
                    tileUrl: me.opts.tileUrl,
                    tileParams: {
                        colors: me.opts.colors,
                        placeId: me.opts.placeId,
                        industryId: me.opts.industryId,
                        granularity: 'County',
                        boundingGranularity: 'State'
                    },
                    minZoom: me.opts.zoomExtent.State,
                    maxZoom: 32
                }));
            }


            if (me.opts.zoomExtent.Metro != null && me.opts.zoomExtent.State > me.opts.zoomExtent.Metro) {
                overlays.push(new sizeup.maps.overlay({
                    tileUrl: me.opts.tileUrl,
                    tileParams: {
                        colors: me.opts.colors,
                        placeId: me.opts.placeId,
                        industryId: me.opts.industryId,
                        granularity: 'State'
                    },
                    minZoom: 0,
                    maxZoom: me.opts.zoomExtent.Metro - 1
                }));
            }

            if (me.opts.zoomExtent.State <= me.opts.zoomExtent.Metro) {
                overlays.push(new sizeup.maps.overlay({
                    tileUrl: me.opts.tileUrl,
                    tileParams: {
                        colors: me.opts.colors,
                        placeId: me.opts.placeId,
                        industryId: me.opts.industryId,
                        granularity: 'State'
                    },
                    minZoom: 0,
                    maxZoom: me.opts.zoomExtent.State - 1
                }));
            }

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

        var publicObj = {
            getOverlays: function () {
                return getOverlays();
            }
        };
        init();
        return publicObj;

    };
})();

