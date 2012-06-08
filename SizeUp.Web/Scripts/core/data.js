(function () {

    sizeup.core.namespace('sizeup.core');
    window.sizeup.core.data = function () {

        jQuery.ajaxSettings.traditional = true;

        var get = function (url, params, success, error) {
            return $.get(url, params, 'json')

            .success(function (data) { if (success) { success(data); } })
            .error(function (e) { if (error) { error(e); } });
        };

        var post = function (url, params, success, error) {
            return $.post(url, params)
            .success(function (data) { if (success) { success(data); } })
            .error(function (e) { if (error) { error(e); } });
        };

        var publicObj = {
            isAuthenticated: function ( success, error) {
                return get('/api/user/authenticated/', null, success, error);
            },
            searchIndustries: function (params, success, error) {
                return get('/api/industry/search/', params, success, error);
            },
            getIndustry: function (params, success, error) {
                return get('/api/industry/', params, success, error);
            },
            getIndustries: function (params, success, error) {
                return get('/api/industry/list', params, success, error);
            },
            getCurrentIndustry: function (success, error) {
                return get('/api/industry/current', null, success, error);
            },
            setCurrentIndustry: function (params, success, error) {
                return post('/api/industry/current', params, success, error);
            },
            hasData: function (params, success, error) {
                return get('/api/industry/hasData/', params, success, error);
            },
            searchCities: function (params, success, error) {
                return get('/api/city/search/', params, success, error);
            },
            getCity: function (params, success, error) {
                return get('/api/city/', params, success, error);
            },
            getCityBoundingBox: function (params, success, error) {
                return get('/api/city/boundingbox/', params, success, error);
            },
            getCityCentroid: function (params, success, error) {
                return get('/api/city/centroid/', params, success, error);
            },
            getCurrentCity: function (success, error) {
                return get('/api/city/current', null, success, error);
            },
            setCurrentCity: function (params, success, error) {
                return post('/api/city/current', params, success, error);
            },
            getDetectedCity: function (success, error) {
                return get('/api/city/detected', null, success, error);
            },
            getCounty: function (params, success, error) {
                return get('/api/county/', params, success, error);
            },

            getMetro: function (params, success, error) {
                return get('/api/metro/', params, success, error);
            },

            getState: function (params, success, error) {
                return get('/api/state/', params, success, error);
            },

            getBusiness: function (success, error) {
                return get('/api/business/', null, success, error);
            },

            getBusinessesByIndustry: function (params, success, error) {
                return get('/api/business/list', params, success, error);
            },

            getAverageSalaryChart: function (params, success, error) {
                return get('/api/averageSalary/', params, success, error);
            },
            getAverageSalaryPercentage: function (params, success, error) {
                return get('/api/averageSalary/percentage/', params, success, error);
            },

            getAverageSalaryBandsByState: function (params, success, error) {
                return get('/api/averageSalary/bands/state/', params, success, error);
            },

            getAverageSalaryBandsByCounty: function (params, success, error) {
                return get('/api/averageSalary/bands/county/', params, success, error);
            },


            getTurnoverChart: function (params, success, error) {
                return get('/api/turnover/', params, success, error);
            },

            getTurnoverPercentile: function (params, success, error) {
                return get('/api/turnover/percentile/', params, success, error);
            },


            getJobChangeChart: function (params, success, error) {
                return get('/api/jobchange/', params, success, error);
            },

            getRevenuePerCapitaChart: function (params, success, error) {
                return get('/api/revenuePerCapita/', params, success, error);
            },


            



        };
        return publicObj;
    };
})();
