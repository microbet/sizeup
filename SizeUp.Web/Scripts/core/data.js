(function () {

    sizeup.core.namespace('sizeup.core.data');
    window.sizeup.core.data = function () {

        var get = function (url, params, success, error) {
            $.getJSON(format(url, params))
            .success(function (data) { if (success) { success(data); } })
            .error(function (e) { if (error) { error(e); } });
        };

        var post = function (url, params, success, error) {
            $.post(url, params)
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
                get('/api/industry/search/?term={term}&maxResults={maxResults}', params, success, error);
            },
            getIndustry: function (params, success, error) {
                get('/api/industry/?id={id}', params, success, error);
            },
            getCurrentIndustry: function (success, error) {
                get('/api/industry/current', null, success, error);
            },
            setCurrentIndustry: function (params, success, error) {
                post('/api/industry/current', params, success, error);
            },
            hasData: function (params, success, error) {
                get('/api/industry/hasData/?id={id}&cityid={cityId}', params, success, error);
            },
            searchCities: function (params, success, error) {
                get('/api/city/search/?term={term}&maxResults={maxResults}', params, success, error);
            },
            getCity: function (params, success, error) {
                get('/api/city/?id={id}', params, success, error);
            },
            getCurrentCity: function (success, error) {
                get('/api/city/current', null, success, error);
            },
            setCurrentCity: function (params, success, error) {
                post('/api/city/current', params, success, error);
            },
            getDetectedCity: function (success, error) {
                get('/api/city/detected', null, success, error);
            },

            getSalaryChart: function (params, success, error) {
                get('/api/salary/?industryId={industryId}&countyId={countyId}', params, success, error);
            },
            getSalaryPercentile: function (params, success, error) {
                get('/api/salary/percentile/?industryId={industryId}&countyId={countyId}&value={value}', params, success, error);
            },



        };
        return publicObj;
    };
})();
