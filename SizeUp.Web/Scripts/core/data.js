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

            searchPlaces: function (params, success, error) {
                return get('/api/place/search/', params, success, error);
            },
            getPlace: function (params, success, error) {
                return get('/api/place/', params, success, error);
            },
            getCurrentPlace: function (success, error) {
                return get('/api/place/current', null, success, error);
            },
            setCurrentPlace: function (params, success, error) {
                return post('/api/place/current', params, success, error);
            },
            getDetectedPlace: function (success, error) {
                return get('/api/place/detected', null, success, error);
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
           
            getCounty: function (params, success, error) {
                return get('/api/county/', params, success, error);
            },
            
            getMetro: function (params, success, error) {
                return get('/api/metro/', params, success, error);
            },

            getState: function (params, success, error) {
                return get('/api/state/', params, success, error);
            },

            getBusiness: function (params, success, error) {
                return get('/api/business/', params, success, error);
            },

            getBusinessAt: function (params, success, error) {
                return get('/api/business/at', params, success, error);
            },

            getBusinessesByIndustry: function (params, success, error) {
                return get('/api/business/list', params, success, error);
            },

            getAverageRevenueChart: function (params, success, error) {
                return get('/api/averageRevenue/', params, success, error);
            },
            getAverageRevenuePercentile: function (params, success, error) {
                return get('/api/averageRevenue/percentile/', params, success, error);
            },

            getAverageRevenueBandsByState: function (params, success, error) {
                return get('/api/averageRevenue/bands/state/', params, success, error);
            },

            getAverageRevenueBandsByCounty: function (params, success, error) {
                return get('/api/averageRevenue/bands/county/', params, success, error);
            },

            getAverageRevenueBandsByZip: function (params, success, error) {
                return get('/api/averageRevenue/bands/zip/', params, success, error);
            },

            getYearStartedChart: function (params, success, error) {
                return get('/api/yearStarted/', params, success, error);
            },

            getYearStartedPercentile: function (params, success, error) {
                return get('/api/yearStarted/percentile', params, success, error);
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

            getRevenuePerCapitaChart: function (params, success, error) {
                return get('/api/RevenuePerCapita/', params, success, error);
            },

            getRevenuePerCapitaPercentage: function (params, success, error) {
                return get('/api/RevenuePerCapita/percentage/', params, success, error);
            },

            getRevenuePerCapitaPercentile: function (params, success, error) {
                return get('/api/RevenuePerCapita/percentile/', params, success, error);
            },

            getRevenuePerCapitaBandsByState: function (params, success, error) {
                return get('/api/RevenuePerCapita/bands/state/', params, success, error);
            },

            getRevenuePerCapitaBandsByCounty: function (params, success, error) {
                return get('/api/RevenuePerCapita/bands/county/', params, success, error);
            },

            getRevenuePerCapitaBandsByZip: function (params, success, error) {
                return get('/api/RevenuePerCapita/bands/zip/', params, success, error);
            },

            getTotalRevenueBandsByState: function (params, success, error) {
                return get('/api/TotalRevenue/bands/state/', params, success, error);
            },

            getTotalRevenueBandsByCounty: function (params, success, error) {
                return get('/api/TotalRevenue/bands/county/', params, success, error);
            },

            getTotalRevenueBandsByZip: function (params, success, error) {
                return get('/api/TotalRevenue/bands/zip/', params, success, error);
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


            getBestPlacesToAdvertise: function (params, success, error) {
                return get('/api/Advertising/', params, success, error);
            },



        };
        return publicObj;
    };
})();
