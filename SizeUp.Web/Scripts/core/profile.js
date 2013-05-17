﻿(function () {
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

        pub.register = function () {
            document.location = getURLPrefix() + "/register" + getReturnQueryString();
        };

        pub.signIn = function () {
            document.location = getURLPrefix() + "/signin" + getReturnQueryString();
        };

        pub.signOut = function () {
            document.location = getURLPrefix() + "/signout" + getReturnQueryString();
        };

        pub.isAuthenticated = function (callback) {
            return get('/api/user/authenticated/', null, callback);
        };

        pub.updateUserProfile = function (params, callback) {
            return post('/api/user/profile/', params, callback);
        }; 

        pub.setPassword = function (params, callback) {
            return post('/api/user/password/', params, callback);
        };

        pub.getCurrentIndustry = function (callback) {
            return get('/api/industry/current', null, callback);
        };
        pub.setCurrentIndustry = function (params, callback) {
            return post('/api/industry/current', params, callback);
        };

        pub.getCurrentPlace = function (callback) {
            return get('/api/place/current', null, callback);
        };

        pub.setCurrentPlace = function (params, callback) {
            return post('/api/place/current', params, callback);
        };

        pub.getDashboardValues = function (params, callback) {
            return get('/api/profile/dashboardValues/', params, callback);
        };

        pub.setDashboardValues = function (params, callback) {
            return post('/api/profile/dashboardValues/', params, callback);
        };

        pub.getCompetitionValues = function (params, callback) {
            return get('/api/profile/competitionValues/', params, callback);
        };

        pub.setCompetitionValues = function (params, callback) {
            return post('/api/profile/competitionValues/', params, callback);
        };

        return pub;
    })();
})();