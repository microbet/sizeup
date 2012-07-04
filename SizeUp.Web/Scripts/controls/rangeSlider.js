(function () {

    sizeup.core.namespace('sizeup.controls');
    window.sizeup.controls.rangeSlider = function (opts) {

        var defaults = {
            onChange: function () { },
            min: 0,
            max: 4,
            values: [null,null],
            mapping: [ ]
        };


        var me = {};
        me.container = opts.container;
        me.opts = $.extend(true, defaults, opts);
        var templates = new sizeup.core.templates();
        me.data = {};


        var init = function () {
            var opts = {
                slide: function (event, ui) { onSlide(event, ui); },
                change: function (event, ui) { onChange(event, ui); },
                min: me.opts.min,
                max: me.opts.max,
                values: getMappingValues(me.opts.values, [me.opts.min,me.opts.max]),
                range: true
            };

            me.data.valueLabels = {};
            me.valueLabel = me.container.find('.valueLabel');
            me.valueLabel.children().each(function () {
                var i = $(this);
                me.data.valueLabels[i.attr('class')] = i.html();
                i.remove();
            });
            me.slider = me.container.find('.slider').slider(opts);
            var vals = getValues();
            setValueLabel(vals);
        };

        var getValues = function () {
            var val = me.slider.slider('values');
            return val;
        };

        var getState = function (vals) {
            var state = null;
            if (vals[0] == me.opts.min && vals[1] == me.opts.max) {
                state = 'off';
            }
            else if (vals[0] != me.opts.min && vals[1] == me.opts.max) {
                state = 'min';
            }
            else if (vals[0] == me.opts.min && vals[1] != me.opts.max) {
                state = 'max';
            }
            else {
                state = 'range';
            }
            return state;
        };

       
  

        var onSlide = function (event, ui) {
            setValueLabel(ui.values);
        };

        var onChange = function (event, ui) {
            setValueLabel(ui.values);
            if (me.opts.onChange) {
                me.opts.onChange();
            }
        };

        var getMappingValue = function (value, defaults) {
            var index = null;
            for (var x = 0; x < me.opts.mapping.length; x++) {
                i = me.opts.mapping[x];
                if (value == i.mappedValue) {
                    index = x;
                }
            }
            return index ? index : !isNaN(value) && value ? value : defaults;
        };

        var getMappingValues = function (values, defaults) {
            var vals = [getMappingValue(values[0], defaults[0]), getMappingValue(values[1], defaults[1])];
            return vals;
        };


        var getMapping = function(value){
            var mapping = null;
            var i = null;
            for (var x = 0; x < me.opts.mapping.length; x++) {
                i = me.opts.mapping[x];
                if (value >= i.range.min && value < i.range.max) {
                    mapping = i;
                }
            }
            return mapping;
        }

        var getMappings = function (vals) {
            var mappings = { min: null, max: null };
            mappings.min = getMapping(vals[0]);
            mappings.max = getMapping(vals[1]);
            return mappings;
        };


        var setValueLabel = function (vals) {
            var state = getState(vals);
            if (state === 'off') {
                me.container.addClass('noFilter');
            }
            else {
                me.container.removeClass('noFilter');
            }
            var label = me.data.valueLabels[state];
            var mappings = getMappings(vals);
            var obj = { min: mappings.min ? mappings.min.mappedLabel : vals[0], max: mappings.max ? mappings.max.mappedLabel : vals[1] };
            label = templates.bind(label, obj);
            me.valueLabel.html(label);
        };

        var getSliderValues = function () {
            var vals = getValues();
            var state = getState(vals);
            var mappings = getMappings(vals);
            var obj = [mappings.min ? mappings.min.mappedValue : vals[0], mappings.max ? mappings.max.mappedValue : vals[1]];
            if (state === 'min') {
                obj[1] = null;
            }
            else if (state === 'max') {
                obj[0] = null;
            }
            else if (state === 'off') {
                obj = null;
            }
            return obj;
        };

       

        var publicObj = {
            getParam: function () {
                return getSliderValues();
            }
        };
        init();
        return publicObj;
    };
})();













