(function () {
    sizeup.core.namespace('sizeup.core');
    sizeup.core.profile = (function (opts) {
        var pub = {};

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


        var isWidget = function () {
            return document.location.pathname.indexOf('/widget') == 0;
        };

        var getURLPrefix = function () {
            var url = '/user';
            if (isWidget()) {
                url = '/widget/user';
            }
            return url;
        };

        var getReturnQueryString = function () {
            var url = '';
            if (document.location.search.toLowerCase().indexOf("returnurl=") > -1) {
                url = document.location.search + document.location.hash;
            }
            else {
                url = "?returnurl=" + encodeURIComponent(document.location.pathname + document.location.search + document.location.hash);
            }
            return url;
        };

        var parseQueryString = function (qs) {
            var obj = {};
            var qs = qs.split('&');
            for (var x = 0; x < qs.length; x++) {
                var item = qs[x].split('=');
                obj[item[0]] = item[1];
            }
            return obj;
        };

        pub.register = function () {
            document.location = getURLPrefix() + "/register" + getReturnQueryString();
        };

        pub.signIn = function () {
            document.location = getURLPrefix() + "/signin" + getReturnQueryString();
        };

        pub.signOut = function () {
            document.location = getURLPrefix() + "/signout" + getReturnQueryString();
        };

        pub.updateUserProfile = function (params, callback) {
            return post('/api/user/profile/', params, callback);
        }; 

        pub.setPassword = function (params, callback) {
            return post('/api/user/password/', params, callback);
        };

        pub.getDetectedPlace = function (callback) {
            return get('/api/place/detected', null, callback);
        };

        pub.getDashboardValues = function (params, callback) {
            return get('/api/profile/dashboardValues/', params, callback);
        };

        pub.setDashboardValues = function (params, callback) {
            var url = '/api/profile/dashboardValues/';
            var p = parseQueryString(jQuery.param.querystring());
            if (p.wt!=null) {
                url = jQuery.param.querystring(url, { wt: decodeURIComponent(p.wt) });
            }
            return post(url, params, callback);
        };

        pub.getCompetitionValues = function (params, callback) {
            return get('/api/profile/competitionValues/', params, callback);
        };

        pub.setCompetitionValues = function (params, callback) {
            var url = '/api/profile/competitionValues/';
            var p = parseQueryString(jQuery.param.querystring());
            if (p.wt != null) {
                url = jQuery.param.querystring(url, { wt: decodeURIComponent(p.wt) });
            }
            return post(url, params, callback);
        };

        return pub;
    })();
})();