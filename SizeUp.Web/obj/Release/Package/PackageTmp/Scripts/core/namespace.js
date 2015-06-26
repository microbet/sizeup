(function () {
    window.sizeup = window.sizeup || {};
    window.sizeup.core = window.sizeup.core || {};
    window.sizeup.core.namespace = function (namespace) {
        var parts = namespace.split('.');
        var current = window;
        for (var x in parts) {
            current[parts[x]] = current[parts[x]] || {};
            current = current[parts[x]];
        }
    };
})();