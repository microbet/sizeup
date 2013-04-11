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
        };



        return pub;
    })();
})();