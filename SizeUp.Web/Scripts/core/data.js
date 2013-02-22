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
            getPlaceBoundingBox: function (params, callback) {
                return get('/api/place/boundingbox/', params, callback);
            },
            getPlaceCentroid: function (params, callback) {
                return get('/api/place/centroid/', params, callback);
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
            getCountyBoundingBox: function (params, callback) {
                return get('/api/county/boundingbox/', params, callback);
            },
            getCountyCentroid: function (params, callback) {
                return get('/api/county/centroid/', params, callback);
            },

            getMetro: function (params, callback) {
                return get('/api/metro/', params, callback);
            },
            getMetroBoundingBox: function (params, callback) {
                return get('/api/metro/boundingbox/', params, callback);
            },
            getMetroCentroid: function (params, callback) {
                return get('/api/metro/centroid/', params, callback);
            },


            getState: function (params, callback) {
                return get('/api/state/', params, callback);
            },
            getStateBoundingBox: function (params, callback) {
                return get('/api/state/boundingbox/', params, callback);
            },
            getStateCentroid: function (params, callback) {
                return get('/api/state/centroid/', params, callback);
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

            getTotalRevenueChart: function (params, callback) {
                return get('/api/TotalRevenue/', params, callback);
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

            getTotalEmployeesChart: function (params, callback) {
                return get('/api/TotalEmployees/', params, callback);
            },

            getTotalEmployeesBandsByState: function (params, callback) {
                return get('/api/TotalEmployees/bands/state/', params, callback);
            },

            getTotalEmployeesBandsByCounty: function (params, callback) {
                return get('/api/TotalEmployees/bands/county/', params, callback);
            },

            getTotalEmployeesBandsByZip: function (params, callback) {
                return get('/api/TotalEmployees/bands/zip/', params, callback);
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

            getCostEffectivenessChart: function (params, callback) {
                return get('/api/CostEffectiveness/', params, callback);
            },

            getCostEffectivenessBandsByState: function (params, callback) {
                return get('/api/CostEffectiveness/bands/state/', params, callback);
            },

            getCostEffectivenessBandsByCounty: function (params, callback) {
                return get('/api/CostEffectiveness/bands/county/', params, callback);
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

            getCostEffectivenessChart: function (params, callback) {
                return get('/api/CostEffectiveness/', params, callback);
            },

            getCostEffectivenessPercentage: function (params, callback) {
                return get('/api/CostEffectiveness/percentage/', params, callback);
            },

            getHealthcareCostChart: function (params, callback) {
                return get('/api/healthcare/', params, callback);
            },

            getHealthcareCostPercentage: function (params, callback) {
                return get('/api/healthcare/percentage/', params, callback);
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

            getCityDemographics: function (params, callback) {
                return get('/api/demographics/city', params, callback);
            },

            getCountyDemographics: function (params, callback) {
                return get('/api/demographics/county', params, callback);
            },

            getMetroDemographics: function (params, callback) {
                return get('/api/demographics/metro', params, callback);
            },

            getStateDemographics: function (params, callback) {
                return get('/api/demographics/state', params, callback);
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

            getConsumerExpenditureBandsByState: function (params, callback) {
                return get('/api/consumerExpenditures/bands/state/', params, callback);
            },

            getConsumerExpenditureBandsByCounty: function (params, callback) {
                return get('/api/consumerExpenditures/bands/county/', params, callback);
            },

            getConsumerExpenditureBandsByZip: function (params, callback) {
                return get('/api/consumerExpenditures/bands/zip/', params, callback);
            },


            getBestPlacesByCity: function (params, callback) {
                return get('/api/bestPlaces/city/', params, callback);
            },
            getBestPlacesByCounty: function (params, callback) {
                return get('/api/bestPlaces/county/', params, callback);
            },
            getBestPlacesByMetro: function (params, callback) {
                return get('/api/bestPlaces/metro/', params, callback);
            },
            getBestPlacesByState: function (params, callback) {
                return get('/api/bestPlaces/state/', params, callback);
            },


            getBestPlacesBandsByCity: function (params, callback) {
                return get('/api/bestPlaces/bands/city/', params, callback);
            },
            getBestPlacesBandsByCounty: function (params, callback) {
                return get('/api/bestPlaces/bands/county/', params, callback);
            },
            getBestPlacesBandsByMetro: function (params, callback) {
                return get('/api/bestPlaces/bands/metro/', params, callback);
            },
            getBestPlacesBandsByState: function (params, callback) {
                return get('/api/bestPlaces/bands/state/', params, callback);
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
