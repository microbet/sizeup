(function () {

    sizeup.core.namespace('sizeup.core.data');
    window.sizeup.core.data = function () {

        var get = function (url, params, success, error) {
            return $.getJSON(format(url, params))
            .success(function (data) { if (success) { success(data); } })
            .error(function (e) { if (error) { error(e); } });
        };

        var post = function (url, params, success, error) {
            return $.post(url, params)
            .success(function (data) { if (success) { success(data); } })
            .error(function (e) { if (error) { error(e); } });
        };

        var format = function (url, params) {
            url = url.replace(new RegExp('{.*?}', 'gi'), function (i) {
                var c = params;
                var items = i.substring(1, i.length - 1).split('.');
                for (var x in items) {
                    if (!(typeof c === "undefined")) {
                        c = c[items[x]];
                    }
                }
                if (typeof c === "undefined" || c === null) {
                    c = '';
                }
                return encodeURIComponent(c);
            });
            return url;
        };


        var publicObj = {

            searchIndustries: function (params, success, error) {
                return get('/api/industry/search/?term={term}&maxResults={maxResults}', params, success, error);
            },
            getIndustry: function (params, success, error) {
                return get('/api/industry/?id={id}', params, success, error);
            },
            getCurrentIndustry: function (success, error) {
                return get('/api/industry/current', null, success, error);
            },
            setCurrentIndustry: function (params, success, error) {
                return post('/api/industry/current', params, success, error);
            },
            hasData: function (params, success, error) {
                return get('/api/industry/hasData/?id={id}&cityid={cityId}', params, success, error);
            },
            searchCities: function (params, success, error) {
                return get('/api/city/search/?term={term}&maxResults={maxResults}', params, success, error);
            },
            getCity: function (params, success, error) {
                return get('/api/city/?id={id}', params, success, error);
            },
            getCityBoundingBox: function (params, success, error) {
                return get('/api/city/boundingbox/?id={id}', params, success, error);
            },
            getCityCentroid: function (params, success, error) {
                return get('/api/city/centroid/?id={id}', params, success, error);
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
                return get('/api/county/?id={id}', params, success, error);
            },

            getMetro: function (params, success, error) {
                return get('/api/metro/?id={id}', params, success, error);
            },

            getState: function (params, success, error) {
                return get('/api/state/?id={id}', params, success, error);
            },

            getSalaryChart: function (params, success, error) {
                return get('/api/salary/?industryId={industryId}&countyId={countyId}', params, success, error);
            },
            getSalaryPercentile: function (params, success, error) {
                return get('/api/salary/percentile/?industryId={industryId}&countyId={countyId}&value={value}', params, success, error);
            },

            getSalaryBandsByState: function (params, success, error) {
                return get('/api/salary/bands/state/?industryId={industryId}&bands={bands}', params, success, error);
            },

            getSalaryBandsByCounty: function (params, success, error) {
                return get('/api/salary/bands/county/?industryId={industryId}&bands={bands}&boundingEntityId={boundingEntityId}', params, success, error);
            }

          

            



        };
        return publicObj;
    };
})();
