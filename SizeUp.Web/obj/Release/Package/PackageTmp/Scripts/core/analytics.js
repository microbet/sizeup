(function () {
    sizeup.core.namespace('sizeup.core');
    sizeup.core.analytics = function () {
        
        jQuery.ajaxSettings.traditional = true;
        var get = function (url, params, callback) {
            return $.get(url, params, 'json')
            .success(function (data, status) {
                if (status == 'success' && callback) { callback(data); }
            })
            .error(function (e, status) {
                if (status != 'abort' && callback) { callback(null); }
            });
        };

        var post = function (url, params, callback) {
            return $.post(url, params)
            .success(function (data, status) {
                if (status == 'success' && callback) { callback(data); }
            })
            .error(function (e, status) {
                if (status != 'abort' && callback) { callback(null); }
            });
        };

        var trackEvent = function (params) {
            if (!(typeof _gaq === 'undefined')) {
                _gaq.push(['_trackEvent',params.category, params.action, params.label, params.value]);
            }
        };

       

        var publicObj = {
            dashboardReportLoaded: function (params) {
                trackEvent({ category: 'Dashboard', action: 'reportLoaded', label: params.report, isInteraction: true });
            },
            dashboardIndustryChanged: function (params) {
                trackEvent({ category: 'Dashboard', action: 'industryChanged', label: params.industry, isInteraction: true });
            },
            dashboardPlaceChanged: function (params) {
                trackEvent({ category: 'Dashboard', action: 'placeChanged', label: params.place, isInteraction: true });
            },

            advertisingReportLoaded: function (params) {
                trackEvent({ category: 'Advertising', action: 'reportLoaded', label: params.attribute, isInteraction: true });
            },
            advertisingIndustryChanged: function (params) {
                trackEvent({ category: 'Advertising', action: 'industryChanged', label: params.industry, isInteraction: true });
            },
            advertisingPlaceChanged: function (params) {
                trackEvent({ category: 'Advertising', action: 'placeChanged', label: params.place, isInteraction: true });
            },
            advertisingAttributeChanged: function (params) {
                trackEvent({ category: 'Advertising', action: 'attributeChanged', label: params.attribute, isInteraction: true });
            },           
            advertisingAdvancedFilterChanged: function (params) {
                trackEvent({ category: 'Advertising', action: 'advancedFilters', label: params.attribute, isInteraction: true });
            },

            consumerExpenditureSelected: function (params) {
                trackEvent({ category: 'Competition', action: 'consumerExpendituresClicked', label: params.label, isInteraction: true });
            },
            consumerExpenditureTypeChanged: function (params) {
                trackEvent({ category: 'Competition', action: 'consumerExpendituresTypeChanged', label: params.label, isInteraction: true });
            },
            competitionTabLoaded: function (params) {
                trackEvent({ category: 'Competition', action: 'tabLoaded', label: params.tab, isInteraction: true });
            },
            competitionIndustryChanged: function (params) {
                trackEvent({ category: 'Competition', action: 'industryChanged', label: params.industry, isInteraction: true });
            },
            competitionPlaceChanged: function (params) {
                trackEvent({ category: 'Competition', action: 'placeChanged', label: params.place, isInteraction: true });
            },
            relatedCompetitor: function (params) {
                return get('/analytics/relatedIndustry/competitor/', params);
            },
            relatedSupplier: function (params) {
                return get('/analytics/relatedIndustry/supplier/', params);
            },
            relatedBuyer: function (params) {
                return get('/analytics/relatedIndustry/buyer/', params);
            },
            userSignin: function (params) {
                trackEvent({ category: 'User', action: 'signin', label: params.label, isInteraction: true });
            },
                     
            bestPlacesReportLoaded: function (params) {
                trackEvent({ category: 'BestPlaces', action: 'reportLoaded', label: params.label, isInteraction: true });
            },
            bestPlacesPlaceTypeChanged: function (params) {
                trackEvent({ category: 'BestPlaces', action: 'placeTypeChanged', label: params.placeType, isInteraction: true });
            },
            bestPlacesAttributeChanged: function (params) {
                trackEvent({ category: 'BestPlaces', action: 'attributeChanged', label: params.attribute, isInteraction: true });
            },
            bestPlacesIndustryChanged: function (params) {
                trackEvent({ category: 'BestPlaces', action: 'industryChanged', label: params.industry, isInteraction: true });
            },
            bestPlacesRegionChanged: function (params) {
                trackEvent({ category: 'BestPlaces', action: 'regionChanged', label: params.region, isInteraction: true });
            },
            bestPlacesAdvancedFilterChanged: function (params) {
                trackEvent({ category: 'BestPlaces', action: 'advancedFilters', label: params.attribute, isInteraction: true });
            },
            outgoingLink: function (params) {
                trackEvent({ category: 'outgoingLinks',action:'clicked', label: params.label, isInteraction: true });
            }
        };
        return publicObj;
    };
})();

