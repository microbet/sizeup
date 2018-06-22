var util = require('./util');

module.exports = function makeDataApi(getData) {
  return {

    findPlace: function (params, success, error) {
                var url = '/data/place/search/';
                return getData(url, params, success, error);
            },
            getPlace: function (params, success, error) {
                var url = '/data/place/';
                params.id = util.wrapAsArray(params.id);
                return getData(url, params, success, error);
            },
            getPlaceBySeokey: function (params, success, error) {
                if (typeof(params) == "string") {
                    keys = params.split("/");
                    params = {"stateSeokey": keys[0]};
                    if (keys.length > 1) { params.countySeokey = keys[1]; }
                    if (keys.length > 2) { params.placeSeokey = keys[2]; }
                    if (keys.length > 3) { throw "Too many elements in " + keys; }
                }
                return getData(
                    '/data/place/GetBySeokey/', params, success, error);
            },
            getDetected: function (params, success, error) {
                var url = '/data/place/SearchDetected/';
                return getData(url, params, success, error);
            },



    findIndustry: function (params, success, error) {
                var url = '/data/industry/search/';
                return getData(url, params, success, error);
            },
            getIndustry: function (params, success, error) {
                var url = '/data/industry/';
                params.id = util.wrapAsArray(params.id);
                return getData(url, params, success, error);
            },
            getIndustryBySeokey: function (params, success, error) {
                if (typeof(params) == "string") { params = {"seokey": params}; }
                return getData(
                    '/data/industry/GetBySeokey/', params, success, error);
            },



    getAverageRevenue: function (params, success, error) {
                var url = '/data/averageRevenue/chart/';
                return getData(url, params, success, error);
            },
            getAverageRevenuePercentile: function (params, success, error) {
                var url = '/data/averageRevenue/percentile/';
                return getData(url, params, success, error);
            },

            getAverageRevenueBands: function (params, success, error) {
                var url = '/data/averageRevenue/bands/';
                return getData(url, params, success, error);
            },


            getAverageEmployees: function (params, success, error) {
                var url = '/data/AverageEmployees/chart/';
                return getData(url, params, success, error);
            },

            getAverageEmployeesPercentile: function (params, success, error) {
                var url = '/data/AverageEmployees/percentile/';
                return getData(url, params, success, error);
            },

            getAverageEmployeesBands: function (params, success, error) {
                var url = '/data/AverageEmployees/bands/';
                return getData(url, params, success, error);
            },

            getAverageSalary: function (params, success, error) {
                var url = '/data/averageSalary/chart/';
                return getData(url, params, success, error);
            },
            getAverageSalaryPercentage: function (params, success, error) {
                var url = '/data/averageSalary/percentage/';
                return getData(url, params, success, error);
            },
            getAverageSalaryBands: function (params, success, error) {
                var url = '/data/averageSalary/bands/';
                return getData(url, params, success, error);
            },

            getCostEffectiveness: function (params, success, error) {
                var url = '/data/CostEffectiveness/chart/';
                return getData(url, params, success, error);
            },
            getCostEffectivenessPercentage: function (params, success, error) {
                var url = '/data/CostEffectiveness/percentage/';
                return getData(url, params, success, error);
            },
            getCostEffectivenessBands: function (params, success, error) {
                var url = '/data/CostEffectiveness/bands/';
                return getData(url, params, success, error);
            },

            getEmployeesPerCapita: function (params, success, error) {
                var url = '/data/EmployeesPerCapita/chart/';
                return getData(url, params, success, error);
            },

            getEmployeesPerCapitaPercentile: function (params, success, error) {
                var url = '/data/EmployeesPerCapita/percentile/';
                return getData(url, params, success, error);
            },

            getEmployeesPerCapitaBands: function (params, success, error) {
                var url = '/data/EmployeesPerCapita/bands/';
                return getData(url, params, success, error);
            },


            getHealthcareCost: function (params, success, error) {
                var url = '/data/healthcare/chart/';
                return getData(url, params, success, error);
            },

            getHealthcareCostPercentage: function (params, success, error) {
                var url = '/data/healthcare/percentage/';
                return getData(url, params, success, error);
            },

            getJobChange: function (params, success, error) {
                var url = '/data/jobchange/chart/';
                return getData(url, params, success, error);
            },

            getJobChangePercentile: function (params, success, error) {
                var url = '/data/jobchange/percentile/';
                return getData(url, params, success, error);
            },

            getRevenuePerCapita: function (params, success, error) {
                var url = '/data/RevenuePerCapita/chart/';
                return getData(url, params, success, error);
            },

            getRevenuePerCapitaPercentile: function (params, success, error) {
                var url = '/data/RevenuePerCapita/percentile/';
                return getData(url, params, success, error);
            },

            getRevenuePerCapitaBands: function (params, success, error) {
                var url = '/data/RevenuePerCapita/bands/';
                return getData(url, params, success, error);
            },

            getTotalEmployees: function (params, success, error) {
                var url = '/data/TotalEmployees/chart/';
                return getData(url, params, success, error);
            },

            getTotalEmployeesBands: function (params, success, error) {
                var url = '/data/TotalEmployees/bands/';
                return getData(url, params, success, error);
            },

            getTotalRevenue: function (params, success, error) {
                var url = '/data/TotalRevenue/chart/';
                return getData(url, params, success, error);
            },

            getTotalRevenueBands: function (params, success, error) {
                var url = '/data/TotalRevenue/bands/';
                return getData(url, params, success, error);
            },

            getTurnover: function (params, success, error) {
                var url = '/data/turnover/chart/';
                return getData(url, params, success, error);
            },

            getTurnoverPercentile: function (params, success, error) {
                var url = '/data/turnover/percentile/';
                return getData(url, params, success, error);
            },


            getWorkersComp: function (params, success, error) {
                var url = '/data/workersComp/chart/';
                return getData(url, params, success, error);
            },

            getWorkersCompPercentage: function (params, success, error) {
                var url = '/data/workersComp/percentage/';
                return getData(url, params, success, error);
            },

            getYearStarted: function (params, success, error) {
                var url = '/data/yearStarted/chart/';
                return getData(url, params, success, error);
            },

            getYearStartedPercentile: function (params, success, error) {
                var url = '/data/yearStarted/percentile/';
                return getData(url, params, success, error);
            },

            getBestIndustries: function (params, success, error) {
                var url = '/data/bestIndustries/';
                return getData(url, params, success, error);
            },




    getBestPlaces: function (params, success, error) {
                var url = '/data/bestPlaces/';
                return getData(url, params, success, error);
            },

            getBestPlacesBands: function (params, success, error) {
                var url = '/data/bestPlaces/Bands/';
                return getData(url, params, success, error);
            },





    getBestPlacesToAdvertise: function (params, success, error) {
                var url = '/data/Marketing/';
                return getData(url, params, success, error);
            },

            getBestPlacesToAdvertiseBands: function (params, success, error) {
                var url = '/data/Marketing/Bands/';
                return getData(url, params, success, error);
            },

            getBestPlacesToAdvertiseMinimumDistance: function (params, success, error) {
                var url = '/data/Marketing/MinimumDistance/';
                return getData(url, params, success, error);
            },




    getConsumerExpenditureVariables: function (params, success, error) {
                var url = '/data/consumerExpenditures/variables/';
                return getData(url, params, success, error);
            },

            getConsumerExpenditureVariable: function (params, success, error) {
                var url = '/data/consumerExpenditures/variable/';
                return getData(url, params, success, error);
            },

            getConsumerExpenditureVariablePath: function (params, success, error) {
                var url = '/data/consumerExpenditures/variablePath/';
                return getData(url, params, success, error);
            },

            getConsumerExpenditureVariableCrosswalk: function (params, success, error) {
                var url = '/data/consumerExpenditures/variableCrosswalk/';
                return getData(url, params, success, error);
            },

            getConsumerExpenditureBands: function (params, success, error) {
                var url = '/data/consumerExpenditures/bands/';
                return getData(url, params, success, error);
            },





    getBusinessesByIndustry: function (params, success, error) {
                var url = '/data/business/list/';
                return getData(url, params, success, error);
            },




    getBoundingBox: function (params, success, error) {
                var url = '/data/geography/boundingbox/';
                return getData(url, params, success, error);
            },
            getCentroid: function (params, success, error) {
                var url = '/data/geography/centroid/';
                return getData(url, params, success, error);
            },
            getZoomExtent: function (params, success, error) {
                var url = '/data/geography/zoomExtent/';
                return getData(url, params, success, error);
            },



    getDemographics: function (params, success, error) {
                var url = '/data/demographics/';
                return getData(url, params, success, error);
            },


		version: '1.0'
  };
}
