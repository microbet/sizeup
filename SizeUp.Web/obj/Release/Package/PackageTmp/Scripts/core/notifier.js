(function () {
    sizeup.core.namespace('sizeup.core.notifier');
    sizeup.core.notifier = function (callback) {

        var callbackList = [];
        var coreCallback = callback;
        var masterCallback = function () {
            var test = true;
            for (var x in callbackList) {
                test = test && callbackList[x].called;
            }
            if (test && coreCallback) {
                coreCallback();
            }
        };

        var publicObj = {
            getNotifier: function (callback) {
                var wrapper = {
                    called: false,
                    callback: callback
                };
                var cb = function (params) {
                    wrapper.called = true;
                    if (wrapper.callback) {
                        wrapper.callback(params);
                    }
                    masterCallback();
                };
                wrapper.func = cb;
                callbackList.push(wrapper);
                return cb;
            },
            setCallback: function (callback) {
                coreCallback = callback;
            }
        };
        return publicObj;
    };
})();

