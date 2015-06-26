(function () {
    sizeup.core.namespace('sizeup.maps');
    sizeup.maps.imagePin = function (opts) {

        var defaults = {
            color: 'ffffff',
            position: null
        };
        var me = {};
        me.opts = $.extend(true, defaults, opts);

        var icon = new google.maps.MarkerImage(
                    '/pins/business/pin/' + opts.color,
                    new google.maps.Size(15, 35),
                    null,
                    new google.maps.Point(7, 29)
                    );


        var shadow = new google.maps.MarkerImage(
                '/pins/business/pinshadow',
                new google.maps.Size(30, 35),
                    null,
                new google.maps.Point(7, 29)
            );

        

        me._native = new google.maps.Marker({
            position: opts.position.getNative(),
            icon: icon,
            shadow: shadow
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

















