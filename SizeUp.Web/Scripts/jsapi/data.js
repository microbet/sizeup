(function () {
    window.sizeup = window.sizeup || {};
    window.sizeup.api = window.sizeup.api || {};
    window.sizeup.api.data = (function () {
       
        var me = {};

        

        var pub = {
            findPlace: function (params, success, error) {
                var url = '/api/place/search/';
                sizeup.api.loader.getData(url, params, success, error);
            }
        };



        return pub;
    })();
})();