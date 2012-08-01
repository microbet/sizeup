(function () {

    sizeup.core.namespace('sizeup.controls');
    window.sizeup.controls.slider = function (opts) {

        var defaults = {
            onChange: function(){},
            min:1,
            max: 150,
            range: 'min',
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
                value: getMappingValue(me.opts.value, me.opts.range == 'max' ? me.opts.min : me.opts.max),
                range: me.opts.range
            };

            
           
            me.data.valueLabels = {};
            me.valueLabel = me.container.find('.valueLabel');
            me.valueLabel.children().each(function () {
                var i = $(this);
                me.data.valueLabels[i.attr('class')] = i.html();
                i.remove();
            });
            me.slider = me.container.find('.slider').slider(opts);
            var val = getValue();
            setValueLabel(val);
        };
       
        var getValue = function () {
            var val = me.slider.slider('value');
            return val;
        };

        var setValue = function (val) {
            me.slider.slider('value', val);
        };

        var getState = function (val) {
            var state = null;
            if (val == me.opts.min && me.opts.range == 'max') {
                state = 'off';
            }
            else if (val == me.opts.max && me.opts.range == 'min') {
                state = 'off';
            }
            else {
                state = 'value';
            }
            return state;
        };


        var getMappingValue = function (value, defaults) {
            var index = null;
            for (var x = 0; x < me.opts.mapping.length; x++) {
                i = me.opts.mapping[x];
                if (value == i.mappedValue) {
                    index = x;
                }
            }
            return index ? index : value ? value : defaults;
        };

        var getMapping = function (value) {
            var mapping = null;
            var i = null;
            for (var x = 0; x < me.opts.mapping.length; x++) {
                i = me.opts.mapping[x];
                if (value >= i.range.min && value < i.range.max) {
                    mapping = i;
                }
            }
            return mapping;
        };

       
        var onSlide = function (event, ui) {
            setValueLabel(ui.value);
        };

        var onChange = function (event, ui) {
            setValueLabel(ui.value);
            if (me.opts.onChange) {
                me.opts.onChange();
            }
        };

        var setValueLabel = function (val) {
            var state = getState(val);
            if (state === 'off') {
                me.container.addClass('noFilter');
            }
            else {
                me.container.removeClass('noFilter');
            }
            var label = me.data.valueLabels[state];
            var mapping = getMapping(val);
            var obj = { value: mapping && mapping.mappingLabel ? mapping : val };
            label = templates.bind(label, obj);
            me.valueLabel.html(label);
        };

        var getSliderValue = function () {
            var val = getValue();
            var state = getState(val);
            var mapping = getMapping(val);
            var obj = mapping && mapping.mappingLabel ? mapping : val;
            if (state === 'off') {
                obj = null;
            }
            return obj;
        };

        var setSliderValue = function (param) {
            var index = getMappingValue(param, me.opts.range == 'max' ? me.opts.min : me.opts.max);
            setValue(index);
            setValueLabel(index);

        };


        var publicObj = {
            getParam: function () {
                return getSliderValue();
            },
            setParam: function (param) {
                setSliderValue(param);
            }
        };
        init();
        return publicObj;
    };
})();













