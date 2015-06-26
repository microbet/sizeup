(function () {
    sizeup.core.namespace('sizeup.maps');
    sizeup.maps.businessMarker = function (opts) {

        var defaults = {
            section: '',
            index: '',
            position: null
        };
        var me = {};
        me.opts = $.extend(true, defaults, opts);

        var icon = new google.maps.MarkerImage(
                    '/pins/business/marker/' + opts.section + '/' + opts.index,
                    new google.maps.Size(20, 29),
                    null,
                    new google.maps.Point(5, 23)
                    );

        var iconHover = new google.maps.MarkerImage(
                '/pins/business/marker/' + opts.section + '/' + opts.index + '/highlight',
                new google.maps.Size(20, 29),
                null,
                new google.maps.Point(5, 23)
                );

        var shadow = new google.maps.MarkerImage(
                '/pins/business/markershadow',
                new google.maps.Size(40, 29),
                null,
                new google.maps.Point(5, 23)
            );

        

        me._native = new google.maps.Marker({
            position: opts.position.getNative(),
            icon: icon,
            shadow: shadow
        });

        google.maps.event.addListener(me._native, 'mouseover', function () {
            me._native.setIcon(iconHover);
            me._native.setZIndex(google.maps.Marker.MAX_ZINDEX + 1);
        });
        google.maps.event.addListener(me._native, 'mouseout', function () {
            me._native.setIcon(icon);
            me._native.setZIndex(google.maps.Marker.MAX_ZINDEX + 1);
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

















