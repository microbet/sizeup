(function () {

    sizeup.core.namespace('sizeup.core');
    window.sizeup.core.data = function () {

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

        var publicObj = {
            isAuthenticated: function (callback) {
                return get('/api/user/authenticated/', null, callback);
            },
            updateUserProfile: function (params, callback) {
                return post('/api/user/profile/', params, callback);
            },
            setPassword: function (params, callback) {
                return post('/api/user/password/', params, callback);
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






            getBoundingBox: function (params, callback) {
                return get('/api/geography/boundingbox/', params, callback);
            },
            getCentroid: function (params, callback) {
                return get('/api/geography/centroid/', params, callback);
            },
            getZoomExtent: function (params, callback) {
                return get('/api/geography/zoomExtent/', params, callback);
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
                return get('/api/averageRevenue/chart/', params, callback);
            },
            getAverageRevenuePercentile: function (params, callback) {
                return get('/api/averageRevenue/percentile/', params, callback);
            },

            getAverageRevenueBands: function(params, callback){
                return get('/api/averageRevenue/bands/', params, callback);
            },



            getYearStartedChart: function (params, callback) {
                return get('/api/yearStarted/chart/', params, callback);
            },

            getYearStartedCount: function (params, callback) {
                return get('/api/yearStarted/count/', params, callback);
            },

            getYearStartedPercentile: function (params, callback) {
                return get('/api/yearStarted/percentile/', params, callback);
            },



            getAverageSalaryChart: function (params, callback) {
                return get('/api/averageSalary/chart/', params, callback);
            },
            getAverageSalaryPercentage: function (params, callback) {
                return get('/api/averageSalary/percentage/', params, callback);
            },

            getAverageSalaryBands: function (params, callback) {
                return get('/api/averageSalary/bands/', params, callback);
            },



            getRevenuePerCapitaChart: function (params, callback) {
                return get('/api/RevenuePerCapita/chart/', params, callback);
            },

            getRevenuePerCapitaPercentile: function (params, callback) {
                return get('/api/RevenuePerCapita/percentile/', params, callback);
            },

            getRevenuePerCapitaBands: function (params, callback) {
                return get('/api/RevenuePerCapita/bands/', params, callback);
            },


            getTotalRevenueChart: function (params, callback) {
                return get('/api/TotalRevenue/chart/', params, callback);
            },

            getTotalRevenueBands: function (params, callback) {
                return get('/api/TotalRevenue/bands/', params, callback);
            },


            getAverageEmployeesChart: function (params, callback) {
                return get('/api/AverageEmployees/chart/', params, callback);
            },

            getAverageEmployeesPercentile: function (params, callback) {
                return get('/api/AverageEmployees/percentile/', params, callback);
            },

            getAverageEmployeesBands: function (params, callback) {
                return get('/api/AverageEmployees/bands/', params, callback);
            },



            getTotalEmployeesChart: function (params, callback) {
                return get('/api/TotalEmployees/chart/', params, callback);
            },

            getTotalEmployeesBands: function (params, callback) {
                return get('/api/TotalEmployees/bands/', params, callback);
            },




            getEmployeesPerCapitaChart: function (params, callback) {
                return get('/api/EmployeesPerCapita/chart/', params, callback);
            },

            getEmployeesPerCapitaPercentile: function (params, callback) {
                return get('/api/EmployeesPerCapita/percentile/', params, callback);
            },

            getEmployeesPerCapitaBands: function (params, callback) {
                return get('/api/EmployeesPerCapita/bands/', params, callback);
            },



            getCostEffectivenessChart: function (params, callback) {
                return get('/api/CostEffectiveness/chart/', params, callback);
            },

            getCostEffectivenessPercentage: function (params, callback) {
                return get('/api/CostEffectiveness/percentage/', params, callback);
            },

            getCostEffectivenessBands: function (params, callback) {
                return get('/api/CostEffectiveness/bands/', params, callback);
            },



            getTurnoverChart: function (params, callback) {
                return get('/api/turnover/chart/', params, callback);
            },

            getTurnoverPercentile: function (params, callback) {
                return get('/api/turnover/percentile/', params, callback);
            },


            getJobChangeChart: function (params, callback) {
                return get('/api/jobchange/chart/', params, callback);
            },

            getJobChangePercentile: function (params, callback) {
                return get('/api/jobchange/percentile/', params, callback);
            },

            


            getWorkersCompChart: function (params, callback) {
                return get('/api/workersComp/chart/', params, callback);
            },

            getWorkersCompPercentage: function (params, callback) {
                return get('/api/workersComp/percentage/', params, callback);
            },




            getHealthcareCostChart: function (params, callback) {
                return get('/api/healthcare/chart/', params, callback);
            },

            getHealthcareCostPercentage: function (params, callback) {
                return get('/api/healthcare/percentage/', params, callback);
            },



            getDemographics: function (params, callback) {
                return get('/api/demographics/', params, callback);
            },



            getDashboardValues: function (params, callback) {
                return get('/api/profile/dashboardValues/', params, callback);
            },

            setDashboardValues: function (params, callback) {
                return post('/api/profile/dashboardValues/', params, callback);
            },

            getCompetitionValues: function (params, callback) {
                return get('/api/profile/competitionValues/', params, callback);
            },

            setCompetitionValues: function (params, callback) {
                return post('/api/profile/competitionValues/', params, callback);
            },


            getConsumerExpenditureVariables: function (params, callback) {
                return get('/api/consumerExpenditures/variables/', params, callback);
            },

            getConsumerExpenditureVariable: function (params, callback) {
                return get('/api/consumerExpenditures/variable/', params, callback);
            },

            getConsumerExpenditureVariablePath: function (params, callback) {
                return get('/api/consumerExpenditures/variablePath/', params, callback);
            },

            getConsumerExpenditureVariableCrosswalk: function (params, callback) {
                return get('/api/consumerExpenditures/variableCrosswalk/', params, callback);
            },

            getConsumerExpenditureBands: function (params, callback) {
                return get('/api/consumerExpenditures/bands/', params, callback);
            },


            getBestPlacesToAdvertise: function (params, callback) {
                return get('/api/Advertising/', params, callback);
            },

            getBestPlacesToAdvertiseBands: function (params, callback) {
                return get('/api/Advertising/Bands', params, callback);
            },

            getBestPlacesToAdvertiseMinimumDistance: function (params, callback) {
                return get('/api/Advertising/MinimumDistance', params, callback);
            },

            

            getBestPlaces: function (params, callback) {
                return get('/api/bestPlaces/', params, callback);
            },
            getBestPlacesBands: function (params, callback) {
                return get('/api/bestPlaces/bands/', params, callback);
            },


            ///analytics

            trackPlaceIndustry: function (params) {
                return get('/analytics/placeIndustry/', params);
            },

            trackRelatedCompetitor: function (params) {
                return get('/analytics/relatedIndustry/competitor/', params);
            },
            trackRelatedSupplier: function (params) {
                return get('/analytics/relatedIndustry/supplier/', params);
            },
            trackRelatedBuyer: function (params) {
                return get('/analytics/relatedIndustry/buyer/', params);
            }
           

            



        };
        return publicObj;
    };
})();
