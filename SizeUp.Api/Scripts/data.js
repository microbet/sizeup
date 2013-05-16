(function () {
    window.sizeup = window.sizeup || {};
    window.sizeup.api = window.sizeup.api || {};
    window.sizeup.api.data = (function () {
       
        var me = {};

        

        var pub = {
            findPlace: function (params, success, error) {
                var url = '/data/place/search/';
                sizeup.api.loader.getData(url, params, success, error);
            },
            getPlace: function (params, success, error) {
                var url = '/data/place/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            findIndustry: function (params, success, error) {
                var url = '/data/industry/search/';
                sizeup.api.loader.getData(url, params, success, error);
            },
            getIndustry: function (params, success, error) {
                var url = '/data/industry/';
                sizeup.api.loader.getData(url, params, success, error);
            },
            getIndustries: function (params, success, error) {
                var url = '/data/industry/list/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getAverageRevenue: function (params, success, error) {
                var url = '/data/averageRevenue/chart/';
                sizeup.api.loader.getData(url, params, success, error);
            },
            getAverageRevenuePercentile: function (params, success, error) {
                var url = '/data/averageRevenue/percentile/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getAverageRevenueBands: function (params, success, error) {
                var url = '/data/averageRevenue/bands/';
                sizeup.api.loader.getData(url, params, success, error);
            },


            getAverageEmployees: function (params, success, error) {
                var url = '/data/AverageEmployees/chart/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getAverageEmployeesPercentile: function (params, success, error) {
                var url = '/data/AverageEmployees/percentile/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getAverageEmployeesBands: function (params, success, error) {
                var url = '/data/AverageEmployees/bands/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getAverageSalary: function (params, success, error) {
                var url = '/data/averageSalary/chart/';
                sizeup.api.loader.getData(url, params, success, error);
            },
            getAverageSalaryPercentage: function (params, success, error) {
                var url = '/data/averageSalary/percentage/';
                sizeup.api.loader.getData(url, params, success, error);
            },
            getAverageSalaryBands: function (params, success, error) {
                var url = '/data/averageSalary/bands/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getCostEffectiveness: function (params, success, error) {
                var url = '/data/CostEffectiveness/chart/';
                sizeup.api.loader.getData(url, params, success, error);
            },
            getCostEffectivenessPercentage: function (params, success, error) {
                var url = '/data/CostEffectiveness/percentage/';
                sizeup.api.loader.getData(url, params, success, error);
            },
            getCostEffectivenessBands: function (params, success, error) {
                var url = '/data/CostEffectiveness/bands/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getEmployeesPerCapita: function (params, success, error) {
                var url = '/data/EmployeesPerCapita/chart/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getEmployeesPerCapitaPercentile: function (params, success, error) {
                var url = '/data/EmployeesPerCapita/percentile/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getEmployeesPerCapitaBands: function (params, success, error) {
                var url = '/data/EmployeesPerCapita/bands/';
                sizeup.api.loader.getData(url, params, success, error);
            },


            getHealthcareCost: function (params, success, error) {
                var url = '/data/healthcare/chart/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getHealthcareCostPercentage: function (params, success, error) {
                var url = '/data/healthcare/percentage/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getJobChange: function (params, success, error) {
                var url = '/data/jobchange/chart/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getJobChangePercentile: function (params, success, error) {
                var url = '/data/jobchange/percentile/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getRevenuePerCapita: function (params, success, error) {
                var url = '/data/RevenuePerCapita/chart/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getRevenuePerCapitaPercentile: function (params, success, error) {
                var url = '/data/RevenuePerCapita/percentile/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getRevenuePerCapitaBands: function (params, success, error) {
                var url = '/data/RevenuePerCapita/bands/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getTotalEmployees: function (params, success, error) {
                var url = '/data/TotalEmployees/chart/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getTotalEmployeesBands: function (params, success, error) {
                var url = '/data/TotalEmployees/bands/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getTotalRevenue: function (params, success, error) {
                var url = '/data/TotalRevenue/chart/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getTotalRevenueBands: function (params, success, error) {
                var url = '/data/TotalRevenue/bands/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getTurnover: function (params, success, error) {
                var url = '/data/turnover/chart/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getTurnoverPercentile: function (params, success, error) {
                var url = '/data/turnover/percentile/';
                sizeup.api.loader.getData(url, params, success, error);
            },


            getWorkersComp: function (params, success, error) {
                var url = '/data/workersComp/chart/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getWorkersCompPercentage: function (params, success, error) {
                var url = '/data/workersComp/percentage/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getYearStarted: function (params, success, error) {
                var url = '/data/yearStarted/chart/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getYearStartedPercentile: function (params, success, error) {
                var url = '/data/yearStarted/percentile/';
                sizeup.api.loader.getData(url, params, success, error);
            },


            getBestPlaces: function (params, success, error) {
                var url = '/data/bestPlaces/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getBestPlacesBands: function (params, success, error) {
                var url = '/data/bestPlaces/Bands/';
                sizeup.api.loader.getData(url, params, success, error);
            },





            getBestPlacesToAdvertise: function (params, success, error) {
                var url = '/data/Advertising/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getBestPlacesToAdvertiseBands: function (params, success, error) {
                var url = '/data/Advertising/Bands/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getBestPlacesToAdvertiseMinimumDistance: function (params, success, error) {
                var url = '/data/Advertising/MinimumDistance/';
                sizeup.api.loader.getData(url, params, success, error);
            },






            getBoundingBox: function (params, success, error) {
                var url = '/data/geography/boundingbox/';
                sizeup.api.loader.getData(url, params, success, error);
            },
            getCentroid: function (params, success, error) {
                var url = '/data/geography/centroid/';
                sizeup.api.loader.getData(url, params, success, error);
            },
            getZoomExtent: function (params, success, error) {
                var url = '/data/geography/zoomExtent/';
                sizeup.api.loader.getData(url, params, success, error);
            },

            getDemographics: function (params, success, error) {
                var url = '/data/demographics/';
                sizeup.api.loader.getData(url, params, success, error);
            },




            version: '1.0'
        };



        return pub;
    })();
})();