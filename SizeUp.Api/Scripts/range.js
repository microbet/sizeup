﻿(function () {
    window.sizeup = window.sizeup || {};
    window.sizeup.api = window.sizeup.api || {};
    window.sizeup.api.range = function () {

        var me = {
            _min: null,
            _max: null
        };

        if (arguments.length == 1) {
            me._min = arguments[0];
        }
        else if (arguments.length == 2) {
            me._min = arguments[0];
            me._max = arguments[1];
        }

        me.min = function () {
            if (arguments.length > 0) {
                me._min = arguments[0];
            }
            else {
                return me._min;
            }
        };


        me.max = function () {
            if (arguments.length > 0) {
                me._max = arguments[0];
            }
            else {
                return me._max;
            }
        };


        var pub = {
            _type: function () { return 'sizeup.api.range' },
            toJSON: function(){ return 'new sizeup.api.range(' + me._min + ' , ' + me._max + ')'},
            min: me.min,
            max: me.max
        };
        return pub;
    };
})();