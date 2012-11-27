(function () {
    sizeup.core.namespace('sizeup.core');
    sizeup.core.analytics = function () {
        var dataLayer = new sizeup.core.data();

        var trackEvent = function (params) {
            if (!(typeof _gaq === 'undefined')) {
                _gaq.push(['_trackEvent',params.category, params.action, params.label, params.value]);
            }
        };

        var trackInternal = function (params, func) {
            dataLayer[func](params);
        };



        var publicObj = {
            dashboardReportLoaded: function (params) {
                trackEvent({ category: 'Dashboard', action: 'reportLoaded', label: params.report, isInteraction: true });
            },
            advertisingReportLoaded: function (params) {
                trackEvent({ category: 'Advertising', action: 'reportLoaded', label: params.attribute, isInteraction: true });
            },
            advertisingFiltersClicked: function () {
                trackEvent({ category: 'Advertising', action: 'filtersClicked', label: null, isInteraction: true });
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
            relatedCompetitor: function (params) {
                trackInternal(params, 'trackRelatedCompetitor');
            },
            relatedSupplier: function (params) {
                trackInternal(params, 'trackRelatedSupplier');
            },
            relatedBuyer: function (params) {
                trackInternal(params, 'trackRelatedBuyer');
            },
            userSignin: function (params) {
                trackEvent({ category: 'User', action: 'signin', label: params.label, isInteraction: true });
            },
            placeIndustry: function (params) {
                trackInternal(params, 'trackPlaceIndustry');
            },
            topPlacesReportLoaded: function () {
                trackEvent({ category: 'TopPlaces', action: 'reportLoaded', label: null, isInteraction: true });
            },
            outgoingLink: function (params) {
                trackEvent({ category: 'outgoingLinks',action:'clicked', label: params.label, isInteraction: true });
            }
        };
        return publicObj;
    };
})();

