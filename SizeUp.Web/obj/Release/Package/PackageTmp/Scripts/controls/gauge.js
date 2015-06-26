
(function () {

    sizeup.core.namespace('sizeup.controls.gauge');
    window.sizeup.controls.gauge = function (opts) {
        var el = opts.element;
        el.show();
        var me = this;
        var width = 87;
        var height = 55;
        var image = "/content/images/gauge.png";
        var canvas = Raphael(el[0], width, height);
        canvas.image(image, 0, 0, width, height);


        me._element = el;
        me._canvas = canvas;
        me._needleColor = "#900";
        me._needle = canvas.set();
        me._needle.push(canvas.circle(43, 42, 4).attr({ fill: "#999", stroke: me._needleColor, "stroke-opacity": 0.5 }));
        me._needle.push(canvas.path("M 42 38 l 0 33 -3 -3 4 10 4 -10 -3 3 0 -33 z").attr({ stroke: me._needleColor, fill: me._needleColor }));
        me._needle.rotate(180, 43, 42);  // work around IE bug when setting rotation angle to 0.
        me._intervalId = setInterval(function () { me._needle.animate({ rotation: 177 + Math.random() * 6 + " 43 42" }, 100); }, 150);
        me._value = 50;
        me._element.mouseover(function () { _flash(); });

        var _setValue = function (percentage, tooltip) {
            clearInterval(me._intervalId);
            var value = percentage * 180 * 0.01; // scale % to rotation degrees
            if (value < 0) value = 0;
            if (value > 180) value = 180;
            me._needle.attr({ title: tooltip });
            me._needle.animate({ rotation: value + 90 + " 43 42" }, 1500, "cubic-bezier(0.42, 0, 0.58, 1.0)");
            me._value = value;
            me._element.attr('title', tooltip);
        };


        var _flash = function () {
            me._needle.animate({ rotation: me._value + 87 + " 43 42" }, 100, "<>", function () {
                me._needle.animate({ rotation: me._value + 93 + " 43 42" }, 100, "<>", function () {
                    me._needle.animate({ rotation: me._value + 90 + " 43 42" }, 100, "<>");
                });
            });
        };


        var hide = function () {
            me._element.hide();
        };

        var show = function () {
            me._element.show();
        };

        var fadeIn = function () {
            me._element.fadeIn();
        };

        var fadeOut = function (callback) {
                me._element.fadeOut(1500, callback);
        };

           
       


        var publicObj = {
            setValue: function(percentage, tooltip){
                _setValue(percentage, tooltip);
            },
            show: function(){
                show();
            },
            hide: function(){
                hide();
            },
            fadeIn: function(){
                fadeIn();
            },
            fadeOut: function(callback){
                fadeOut(callback);
            }
        };

        return publicObj;
    };
})();
