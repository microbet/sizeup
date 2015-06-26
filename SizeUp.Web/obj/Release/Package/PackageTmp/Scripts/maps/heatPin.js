(function () {
    sizeup.core.namespace('sizeup.maps');
    sizeup.maps.heatPin = function (opts) {

        var defaults = {
            color: 'ffffff',
            position: null
        };
        var me = {};
        me.opts = $.extend(true, defaults, opts);

        var icon = new google.maps.MarkerImage(
                    '/pins/heat/marker?color=%23' + opts.color,
                    new google.maps.Size(25, 25),
                    null,
                    new google.maps.Point(12, 25)
                    );


        var shadow = new google.maps.MarkerImage(
                '/content/images/heatPinShadow.png',
                new google.maps.Size(25, 25),
                    null,
                new google.maps.Point(2, 25)
            );



        me._native = new google.maps.Marker({
            position: opts.position.getNative(),
            icon: icon,
            shadow: shadow,
            title: opts.title
        });




        var triggerEvent = function (event) {
            google.maps.event.trigger(me._native, event);
        };

        var getPosition = function () {
            return opts.position;
        };

        var bindEvent = function (event, func) {
            google.maps.event.addListener(me._native, event, func);
        };

        var publicObj = {
            getNative: function () {
                return me._native;
            },
            getPosition: function () {
                return getPosition();
            },
            triggerEvent: function (event) {
                triggerEvent(event);
            },
            bindEvent: function (event, func) {
                bindEvent(event, func);
            }
        };
        return publicObj;

    };
})();

















