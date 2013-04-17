(function () {
    window.sizeup = window.sizeup || {};
    window.sizeup.api = window.sizeup.api || {};
    window.sizeup.api.data = (function () {
       
        var me = {};

        

        var pub = {
            findPlace: function (params, success, error) {
                var url = '/api/place/search/';
                sizeup.api.loader.getData(url, params, success, error);
            },
            getPlace: function (params, success, error) {
                var url = '/api/place/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            findIndustry: function (params, success, error) {
                var url = '/api/industry/search/';
                sizeup.api.loader.getData(url, params, success, error);
            },
            getIndustry: function (params, success, error) {
                var url = '/api/industry/';
                sizeup.api.loader.getData(url, params, success, error);
            },
            getIndustries: function (params, success, error) {
                var url = '/api/industry/list/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getAverageRevenue: function (params, success, error) {
                var url = '/api/averageRevenue/chart/';
                sizeup.api.loader.getData(url, params, success, error);
            },
            getAverageRevenuePercentile: function (params, success, error) {
                var url = '/api/averageRevenue/percentile/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getAverageRevenueBands: function (params, success, error) {
                var url = '/api/averageRevenue/bands/';
                sizeup.api.loader.getData(url, params, success, error);
            },


            getAverageEmployees: function (params, success, error) {
                var url = '/api/AverageEmployees/chart/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getAverageEmployeesPercentile: function (params, success, error) {
                var url = '/api/AverageEmployees/percentile/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getAverageEmployeesBands: function (params, success, error) {
                var url = '/api/AverageEmployees/bands/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getAverageSalary: function (params, success, error) {
                var url = '/api/averageSalary/chart/';
                sizeup.api.loader.getData(url, params, success, error);
            },
            getAverageSalaryPercentage: function (params, success, error) {
                var url = '/api/averageSalary/percentage/';
                sizeup.api.loader.getData(url, params, success, error);
            },
            getAverageSalaryBands: function (params, success, error) {
                var url = '/api/averageSalary/bands/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getCostEffectiveness: function (params, success, error) {
                var url = '/api/CostEffectiveness/chart/';
                sizeup.api.loader.getData(url, params, success, error);
            },
            getCostEffectivenessPercentage: function (params, success, error) {
                var url = '/api/CostEffectiveness/percentage/';
                sizeup.api.loader.getData(url, params, success, error);
            },
            getCostEffectivenessBands: function (params, success, error) {
                var url = '/api/CostEffectiveness/bands/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getEmployeesPerCapita: function (params, success, error) {
                var url = '/api/EmployeesPerCapita/chart/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getEmployeesPerCapitaPercentile: function (params, success, error) {
                var url = '/api/EmployeesPerCapita/percentile/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getEmployeesPerCapitaBands: function (params, success, error) {
                var url = '/api/EmployeesPerCapita/bands/';
                sizeup.api.loader.getData(url, params, success, error);
            },


            getHealthcareCost: function (params, success, error) {
                var url = '/api/healthcare/chart/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getHealthcareCostPercentage: function (params, success, error) {
                var url = '/api/healthcare/percentage/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getJobChange: function (params, success, error) {
                var url = '/api/jobchange/chart/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getJobChangePercentile: function (params, success, error) {
                var url = '/api/jobchange/percentile/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getRevenuePerCapita: function (params, success, error) {
                var url = '/api/RevenuePerCapita/chart/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getRevenuePerCapitaPercentile: function (params, success, error) {
                var url = '/api/RevenuePerCapita/percentile/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getRevenuePerCapitaBands: function (params, success, error) {
                var url = '/api/RevenuePerCapita/bands/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getTotalEmployees: function (params, success, error) {
                var url = '/api/TotalEmployees/chart/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getTotalEmployeesBands: function (params, success, error) {
                var url = '/api/TotalEmployees/bands/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getTotalRevenue: function (params, success, error) {
                var url = '/api/TotalRevenue/chart/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getTotalRevenueBands: function (params, success, error) {
                var url = '/api/TotalRevenue/bands/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getTurnover: function (params, success, error) {
                var url = '/api/turnover/chart/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getTurnoverPercentile: function (params, success, error) {
                var url = '/api/turnover/percentile/';
                sizeup.api.loader.getData(url, params, success, error);
            },


            getWorkersComp: function (params, success, error) {
                var url = '/api/workersComp/chart/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getWorkersCompPercentage: function (params, success, error) {
                var url = '/api/workersComp/percentage/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getYearStarted: function (params, success, error) {
                var url = '/api/yearStarted/chart/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getYearStartedPercentile: function (params, success, error) {
                var url = '/api/yearStarted/percentile/';
                sizeup.api.loader.getData(url, params, success, error);
            },


            getBestPlaces: function (params, success, error) {
                var url = '/api/bestPlaces/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getBestPlacesBands: function (params, success, error) {
                var url = '/api/bestPlaces/Bands/';
                sizeup.api.loader.getData(url, params, success, error);
            }

        };



        return pub;
    })();
})();