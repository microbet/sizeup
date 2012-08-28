(function () {
    sizeup.core.namespace('sizeup.core');
    sizeup.core.profile = (function (opts) {
        var pub = {};

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
        return pub;
    })();
})();