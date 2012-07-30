(function () {

    sizeup.core.namespace('sizeup.core');
    window.sizeup.core.data = function () {

        jQuery.ajaxSettings.traditional = true;

        var get = function (url, params, callback) {
            return $.get(url, params, 'json')
            .success(function (data) { if (callback) { callback(data); } })
            .error(function (e) { if (callback) { callback(null); } });
        };

        var post = function (url, params, callback) {
            return $.post(url, params)
            .success(function (data) { if (callback) { callback(data); } })
            .error(function (e) { if (error) { callback(null); } });
        };

        var publicObj = {
            isAuthenticated: function ( callback) {
                return get('/api/user/authenticated/', null, callback);
            },
            searchIndustries: function (params, callback) {
                return get('/api/industry/search/', params, callback);
            },
            getIndustry: function (params, callback) {
                return get('/api/industry/', params, callback);
            },
            getIndustries: function (params, callback) {
                return get('/api/industry/list', params, callback);
            },
            getCurrentIndustry: function (callback) {
                return get('/api/industry/current', null, callback);
            },
            setCurrentIndustry: function (params, callback) {
                return post('/api/industry/current', params, callback);
            },
            hasData: function (params, callback) {
                return get('/api/industry/hasData/', params, callback);
            },

            searchPlaces: function (params, callback) {
                return get('/api/place/search/', params, callback);
            },
            getPlace: function (params, callback) {
                return get('/api/place/', params, callback);
            },
            getCurrentPlace: function (callback) {
                return get('/api/place/current', null, callback);
            },
            setCurrentPlace: function (params, callback) {
                return post('/api/place/current', params, callback);
            },
            getDetectedPlace: function (callback) {
                return get('/api/place/detected', null, callback);
            },

            getCity: function (params, callback) {
                return get('/api/city/', params, callback);
            },
            getCityBoundingBox: function (params, callback) {
                return get('/api/city/boundingbox/', params, callback);
            },
            getCityCentroid: function (params, callback) {
                return get('/api/city/centroid/', params, callback);
            },
           
            getCounty: function (params, callback) {
                return get('/api/county/', params, callback);
            },
            
            getMetro: function (params, callback) {
                return get('/api/metro/', params, callback);
            },

            getState: function (params, callback) {
                return get('/api/state/', params, callback);
            },

            getBusiness: function (params, callback) {
                return get('/api/business/', params, callback);
            },

            getBusinessAt: function (params, callback) {
                return get('/api/business/at', params, callback);
            },

            getBusinessesByIndustry: function (params, callback) {
                return get('/api/business/list', params, callback);
            },

            getAverageRevenueChart: function (params, callback) {
                return get('/api/averageRevenue/', params, callback);
            },
            getAverageRevenuePercentile: function (params, callback) {
                return get('/api/averageRevenue/percentile/', params, callback);
            },

            getAverageRevenueBandsByState: function (params, callback) {
                return get('/api/averageRevenue/bands/state/', params, callback);
            },

            getAverageRevenueBandsByCounty: function (params, callback) {
                return get('/api/averageRevenue/bands/county/', params, callback);
            },

            getAverageRevenueBandsByZip: function (params, callback) {
                return get('/api/averageRevenue/bands/zip/', params, callback);
            },

            getYearStartedChart: function (params, callback) {
                return get('/api/yearStarted/', params, callback);
            },

            getYearStartedCount: function (params, callback) {
                return get('/api/yearStarted/count/', params, callback);
            },

            getYearStartedPercentile: function (params, callback) {
                return get('/api/yearStarted/percentile/', params, callback);
            },


            getAverageSalaryChart: function (params, callback) {
                return get('/api/averageSalary/', params, callback);
            },
            getAverageSalaryPercentage: function (params, callback) {
                return get('/api/averageSalary/percentage/', params, callback);
            },

            getAverageSalaryBandsByState: function (params, callback) {
                return get('/api/averageSalary/bands/state/', params, callback);
            },

            getAverageSalaryBandsByCounty: function (params, callback) {
                return get('/api/averageSalary/bands/county/', params, callback);
            },

            getRevenuePerCapitaChart: function (params, callback) {
                return get('/api/RevenuePerCapita/', params, callback);
            },

            getRevenuePerCapitaPercentage: function (params, callback) {
                return get('/api/RevenuePerCapita/percentage/', params, callback);
            },

            getRevenuePerCapitaPercentile: function (params, callback) {
                return get('/api/RevenuePerCapita/percentile/', params, callback);
            },

            getRevenuePerCapitaBandsByState: function (params, callback) {
                return get('/api/RevenuePerCapita/bands/state/', params, callback);
            },

            getRevenuePerCapitaBandsByCounty: function (params, callback) {
                return get('/api/RevenuePerCapita/bands/county/', params, callback);
            },

            getRevenuePerCapitaBandsByZip: function (params, callback) {
                return get('/api/RevenuePerCapita/bands/zip/', params, callback);
            },

            getTotalRevenueBandsByState: function (params, callback) {
                return get('/api/TotalRevenue/bands/state/', params, callback);
            },

            getTotalRevenueBandsByCounty: function (params, callback) {
                return get('/api/TotalRevenue/bands/county/', params, callback);
            },

            getTotalRevenueBandsByZip: function (params, callback) {
                return get('/api/TotalRevenue/bands/zip/', params, callback);
            },



            getAverageEmployeesChart: function (params, callback) {
                return get('/api/AverageEmployees/', params, callback);
            },

            getAverageEmployeesPercentile: function (params, callback) {
                return get('/api/AverageEmployees/percentile/', params, callback);
            },

            getAverageEmployeesBandsByState: function (params, callback) {
                return get('/api/AverageEmployees/bands/state/', params, callback);
            },

            getAverageEmployeesBandsByCounty: function (params, callback) {
                return get('/api/AverageEmployees/bands/county/', params, callback);
            },

            getAverageEmployeesBandsByZip: function (params, callback) {
                return get('/api/AverageEmployees/bands/zip/', params, callback);
            },


            getEmployeesPerCapitaChart: function (params, callback) {
                return get('/api/EmployeesPerCapita/', params, callback);
            },

            getEmployeesPerCapitaPercentile: function (params, callback) {
                return get('/api/EmployeesPerCapita/percentile/', params, callback);
            },

            getEmployeesPerCapitaBandsByState: function (params, callback) {
                return get('/api/EmployeesPerCapita/bands/state/', params, callback);
            },

            getEmployeesPerCapitaBandsByCounty: function (params, callback) {
                return get('/api/EmployeesPerCapita/bands/county/', params, callback);
            },

            getEmployeesPerCapitaBandsByZip: function (params, callback) {
                return get('/api/EmployeesPerCapita/bands/zip/', params, callback);
            },



            getTurnoverChart: function (params, callback) {
                return get('/api/turnover/', params, callback);
            },

            getTurnoverPercentile: function (params, callback) {
                return get('/api/turnover/percentile/', params, callback);
            },


            getJobChangeChart: function (params, callback) {
                return get('/api/jobchange/', params, callback);
            },

            getRevenuePerCapitaChart: function (params, callback) {
                return get('/api/revenuePerCapita/', params, callback);
            },


            getWorkersCompChart: function (params, callback) {
                return get('/api/workersComp/', params, callback);
            },

            getWorkersCompPercentage: function (params, callback) {
                return get('/api/workersComp/percentage/', params, callback);
            },



            getBestPlacesToAdvertise: function (params, callback) {
                return get('/api/Advertising/', params, callback);
            },



        };
        return publicObj;
    };
})();
