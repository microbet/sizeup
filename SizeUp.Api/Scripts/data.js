(function () {
    window.sizeup = window.sizeup || {};
    window.sizeup.api = window.sizeup.api || {};
    window.sizeup.api.data = (function () {
       
        var me = {};

        

        var pub = {
            findPlace: function (params, success, error) {
                var url = '/data/place/search/';
                return sizeup.api.loader.getData(url, params, success, error);
            },
            getPlace: function (params, success, error) {
                var url = '/data/place/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            findIndustry: function (params, success, error) {
                var url = '/data/industry/search/';
                return sizeup.api.loader.getData(url, params, success, error);
            },
            getIndustry: function (params, success, error) {
                var url = '/data/industry/';
                return sizeup.api.loader.getData(url, params, success, error);
            },
            getIndustries: function (params, success, error) {
                var url = '/data/industry/list/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getAverageRevenue: function (params, success, error) {
                var url = '/data/averageRevenue/chart/';
                return sizeup.api.loader.getData(url, params, success, error);
            },
            getAverageRevenuePercentile: function (params, success, error) {
                var url = '/data/averageRevenue/percentile/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getAverageRevenueBands: function (params, success, error) {
                var url = '/data/averageRevenue/bands/';
                return sizeup.api.loader.getData(url, params, success, error);
            },


            getAverageEmployees: function (params, success, error) {
                var url = '/data/AverageEmployees/chart/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getAverageEmployeesPercentile: function (params, success, error) {
                var url = '/data/AverageEmployees/percentile/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getAverageEmployeesBands: function (params, success, error) {
                var url = '/data/AverageEmployees/bands/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getAverageSalary: function (params, success, error) {
                var url = '/data/averageSalary/chart/';
                return sizeup.api.loader.getData(url, params, success, error);
            },
            getAverageSalaryPercentage: function (params, success, error) {
                var url = '/data/averageSalary/percentage/';
                return sizeup.api.loader.getData(url, params, success, error);
            },
            getAverageSalaryBands: function (params, success, error) {
                var url = '/data/averageSalary/bands/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getCostEffectiveness: function (params, success, error) {
                var url = '/data/CostEffectiveness/chart/';
                return sizeup.api.loader.getData(url, params, success, error);
            },
            getCostEffectivenessPercentage: function (params, success, error) {
                var url = '/data/CostEffectiveness/percentage/';
                return sizeup.api.loader.getData(url, params, success, error);
            },
            getCostEffectivenessBands: function (params, success, error) {
                var url = '/data/CostEffectiveness/bands/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getEmployeesPerCapita: function (params, success, error) {
                var url = '/data/EmployeesPerCapita/chart/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getEmployeesPerCapitaPercentile: function (params, success, error) {
                var url = '/data/EmployeesPerCapita/percentile/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getEmployeesPerCapitaBands: function (params, success, error) {
                var url = '/data/EmployeesPerCapita/bands/';
                return sizeup.api.loader.getData(url, params, success, error);
            },


            getHealthcareCost: function (params, success, error) {
                var url = '/data/healthcare/chart/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getHealthcareCostPercentage: function (params, success, error) {
                var url = '/data/healthcare/percentage/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getJobChange: function (params, success, error) {
                var url = '/data/jobchange/chart/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getJobChangePercentile: function (params, success, error) {
                var url = '/data/jobchange/percentile/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getRevenuePerCapita: function (params, success, error) {
                var url = '/data/RevenuePerCapita/chart/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getRevenuePerCapitaPercentile: function (params, success, error) {
                var url = '/data/RevenuePerCapita/percentile/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getRevenuePerCapitaBands: function (params, success, error) {
                var url = '/data/RevenuePerCapita/bands/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getTotalEmployees: function (params, success, error) {
                var url = '/data/TotalEmployees/chart/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getTotalEmployeesBands: function (params, success, error) {
                var url = '/data/TotalEmployees/bands/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getTotalRevenue: function (params, success, error) {
                var url = '/data/TotalRevenue/chart/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getTotalRevenueBands: function (params, success, error) {
                var url = '/data/TotalRevenue/bands/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getTurnover: function (params, success, error) {
                var url = '/data/turnover/chart/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getTurnoverPercentile: function (params, success, error) {
                var url = '/data/turnover/percentile/';
                return sizeup.api.loader.getData(url, params, success, error);
            },


            getWorkersComp: function (params, success, error) {
                var url = '/data/workersComp/chart/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getWorkersCompPercentage: function (params, success, error) {
                var url = '/data/workersComp/percentage/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getYearStarted: function (params, success, error) {
                var url = '/data/yearStarted/chart/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getYearStartedPercentile: function (params, success, error) {
                var url = '/data/yearStarted/percentile/';
                return sizeup.api.loader.getData(url, params, success, error);
            },


            getBestPlaces: function (params, success, error) {
                var url = '/data/bestPlaces/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getBestPlacesBands: function (params, success, error) {
                var url = '/data/bestPlaces/Bands/';
                return sizeup.api.loader.getData(url, params, success, error);
            },





            getBestPlacesToAdvertise: function (params, success, error) {
                var url = '/data/Advertising/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getBestPlacesToAdvertiseBands: function (params, success, error) {
                var url = '/data/Advertising/Bands/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getBestPlacesToAdvertiseMinimumDistance: function (params, success, error) {
                var url = '/data/Advertising/MinimumDistance/';
                return sizeup.api.loader.getData(url, params, success, error);
            },



            getConsumerExpenditureVariables: function (params, success, error) {
                var url = '/data/consumerExpenditures/variables/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getConsumerExpenditureVariable: function (params, success, error) {
                var url = '/data/consumerExpenditures/variable/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getConsumerExpenditureVariablePath: function (params, success, error) {
                var url = '/data/consumerExpenditures/variablePath/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getConsumerExpenditureVariableCrosswalk: function (params, success, error) {
                var url = '/data/consumerExpenditures/variableCrosswalk/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getConsumerExpenditureBands: function (params, success, error) {
                var url = '/data/consumerExpenditures/bands/';
                return sizeup.api.loader.getData(url, params, success, error);
            },




            
            getBusiness: function (params, success, error) {
                var url = '/data/business/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getBusinessAt: function (params, success, error) {
                var url = '/data/business/at/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getBusinessesByIndustry: function (params, success, error) {
                var url = '/data/business/list/';
                return sizeup.api.loader.getData(url, params, success, error);
            },



            
            getBoundingBox: function (params, success, error) {
                var url = '/data/geography/boundingbox/';
                return sizeup.api.loader.getData(url, params, success, error);
            },
            getCentroid: function (params, success, error) {
                var url = '/data/geography/centroid/';
                return sizeup.api.loader.getData(url, params, success, error);
            },
            getZoomExtent: function (params, success, error) {
                var url = '/data/geography/zoomExtent/';
                return sizeup.api.loader.getData(url, params, success, error);
            },

            getDemographics: function (params, success, error) {
                var url = '/data/demographics/';
                return sizeup.api.loader.getData(url, params, success, error);
            },




            version: '1.0'
        };



        return pub;
    })();
})();