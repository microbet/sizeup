(function () {
    sizeup.core.namespace('sizeup.core');
    sizeup.core.analytics = function () {
        var dataLayer = new sizeup.core.data();

        var trackEvent = function (params) {
            if (!typeof _gaq === 'undefined') {
                _gaq._trackEvent(params.category, params.action, params.label, params.value, !params.isInteraction);
            }
        };

        var trackInternal = function (params, func) {
            dataLayer[func](params);
        };



        var publicObj = {
            dashboardReportLoaded: function (params) {
                trackEvent({ category: 'Dashboard', action: 'reportLoaded', label: params.report, isInteraction: true });
            },
            competitionTabLoaded: function (params) {
                trackEvent({ category: 'Competition', action: 'tabLoaded', label: params.tab, isInteraction: true });
            },
            advertisingReportLoaded: function () {
                trackEvent({ category: 'Advertising', action: 'reportLoaded', label: null, isInteraction: true });
            },
            advertisingFiltersClicked: function () {
                trackEvent({ category: 'Advertising', action: 'filtersClicked', label: null, isInteraction: true });
            },
            userSignin: function (params) {
                trackEvent({ category: 'User', action: 'signin', label: params.label, isInteraction: true });
            },
            placeIndustry: function (params) {
                trackInternal(params, 'trackPlaceIndustry');
            }
        };
        return publicObj;
    };
})();

