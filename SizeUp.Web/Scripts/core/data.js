(function () {

    sizeup.core.namespace('sizeup.core.data');
    window.sizeup.core.data = function () {

        var get = function (url, params, success) {
            $.getJSON(format(url, params), function (data) {if (success) { success(data); }});
        };

        var post = function (url, params, success) {
            $.post(url, params, function (data) { if (success) { success(data); } });
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

            searchIndustries: function (params, success) {
                get('/api/industry/search/?term={term}&maxResults={maxResults}', params, success);
            },
            getIndustry: function (params, success) {
                get('/api/industry/?id={id}', params, success);
            },
            getCurrentIndustry: function (success) {
                get('/api/industry/current', null, success);
            },
            setCurrentIndustry: function (params, success) {
                post('/api/industry/current', params, success);
            },
            hasData: function (params, success) {
                get('/api/industry/hasData/?id={id}&cityid={cityId}', params, success);
            },
            searchCities: function (params, success) {
                get('/api/city/search/?term={term}&maxResults={maxResults}', params, success);
            },
            getCity: function (params, success) {
                get('/api/city/?id={id}', params, success);
            },
            getCurrentCity: function (success) {
                get('/api/city/current', null, success);
            },
            setCurrentCity: function (params, success) {
                post('/api/city/current', params, success);
            },
            getDetectedCity: function (success) {
                get('/api/city/detected', null, success);
            }


        };
        return publicObj;
    };
})();
