(function () {

    sizeup.core.namespace('sizeup.controls');
    window.sizeup.controls.rangeSlider = function (opts) {

        var defaults = {
            container: $('<div></div>'),
            label: $('<div></div>'),
            onChange: function () { },
            range: {min: 0, max: 99},
            mode: 'max',
            invert: false
        };


        var me = {};
        me.container = opts.container;
        me.opts = $.extend(true, defaults, opts);
        var templates = new sizeup.core.templates();
        
        me.test = 1;


        var init = function () {
            me.slider = me.container.find('.slider').addClass('off');
            me.label = new sizeup.controls.rangeLabel({
                container: me.opts.label
            });

            var opts = {
                slide: function (event, ui) { onSlide(event, ui); },
                change: function (event, ui) { onChange(event, ui); }
            };

            if ($.isArray(me.opts.range)) {
                opts.min = 0;
                opts.max = me.opts.range.length - 1;
                me.isMapped = true;
            }
            else {
                opts.min = me.opts.range.min;
                opts.max = me.opts.range.max;
                me.isMapped = false;
            }

            if (me.opts.mode == 'range') {
                opts.range = true;
                opts.min = opts.min - 1;
                opts.max = opts.max + 1;
                me.min = opts.min;
                me.max = opts.max;
                me.mode = me.opts.mode;
                if (me.isMapped) {
                    me.mappings = {};
                    me.mappings[opts.min] = { label: null, value: null };
                    for (var x in me.opts.range) {
                        me.mappings[x] = me.opts.range[x];
                    }
                    me.mappings[opts.max] = { label: null, value: null };
                }
                opts.values = getIndex(me.opts.value);
                if (opts.values[0] != me.min || opts.values[1] != me.max) {
                    me.slider.removeClass('off');
                }
                setLabel(opts.values);
            }
            else if (!me.opts.invert && me.opts.mode == 'min' || me.opts.invert && me.opts.mode == 'max') {
                opts.range = me.opts.mode;
                opts.min = opts.min - 1;
                me.min = opts.min;
                me.max = opts.max;
                me.mode = me.opts.mode;
                if (me.isMapped) {
                    me.mappings = {};
                    me.mappings[opts.min] = { label: null, value: null };
                    for (var x in me.opts.range) {
                        me.mappings[x] = me.opts.range[x];
                    }
                }
                opts.value = getIndex(me.opts.value);
                if (opts.value != me.min) {
                    me.slider.removeClass('off');
                }
                setLabel(opts.value);
            }
            else if (!me.opts.invert && me.opts.mode == 'max' || me.opts.invert && me.opts.mode == 'min') {
                opts.range = me.opts.mode;
                opts.max = opts.max + 1;
                me.min = opts.min;
                me.max = opts.max;
                me.mode = me.opts.mode;
                if (me.isMapped) {
                    me.mappings = {};
                    for (var x in me.opts.range) {
                        me.mappings[x] = me.opts.range[x];
                    }
                    me.mappings[opts.max] = { label: null, value: null };
                }
                opts.value = getIndex(me.opts.value);
                if (opts.value != me.max) {
                    me.slider.removeClass('off');
                }
                setLabel(opts.value);
            }
            me.slider.slider(opts);   
        };

        var onSlide = function (event, ui) {
            var index = 0;
            if (me.mode == 'range') {
                if(ui.values[0] == me.min && ui.values[1] == me.max){
                    me.slider.addClass('off');
                }
                else{
                    me.slider.removeClass('off');
                }
                index = ui.values;
            }
            else if (!me.opts.invert && me.opts.mode == 'min' || me.opts.invert && me.opts.mode == 'max') {
                if(ui.value == me.min){
                    me.slider.addClass('off');
                }
                else{
                    me.slider.removeClass('off');
                }
                index = ui.value;
            }
            else if (!me.opts.invert && me.opts.mode == 'max' || me.opts.invert && me.opts.mode == 'min') {
                if (ui.value == me.max) {
                    me.slider.addClass('off');
                }
                else {
                    me.slider.removeClass('off');
                }
                index = ui.value;
            }
            setLabel(index);
        };

        var onChange = function (event, ui) {
            onSlide(event, ui);
            if (me.opts.onChange) {
                me.opts.onChange();
            }
        };

        var setLabel = function (index) {
            me.label.setValues(getLabelValue(index));
        };

       
      

        var getLabelValue = function (index) {
            var mapping = getMapping(index);
            var labelObj = null;
            if (me.mode == 'range' && mapping != null) {
                labelObj = { min: mapping[0].label, max: mapping[1].label };
            }
            else if (mapping != null) {
                labelObj = { value: mapping.label };
            }
            return labelObj;
        };

        var getMapping = function (index) {
            var val = null;
            if (me.mode == 'range') {
                if (me.isMapped) {
                    val = [me.mappings[index[0]], me.mappings[index[1]]];
                }
                else {
                    var min = index[0] == me.min ? null : index[0];
                    var max = index[1] == me.max ? null : index[1];
                    val = [{ value: min, label: min }, { value: max, label: max }];
                }
                if (val[0].value == null && val[1].value == null) {
                    val = null;
                }
            }
            else {
                if (me.isMapped) {
                    if (me.mappings[index] != null) {
                        val = me.mappings[index];
                    }
                }
                else {
                    if (!me.opts.invert && me.opts.mode == 'min' || me.opts.invert && me.opts.mode == 'max') {
                        var v = index == me.min ? null : index;
                        val = { value: v, label: v };
                    }
                    else if (!me.opts.invert && me.opts.mode == 'max' || me.opts.invert && me.opts.mode == 'min') {
                        var v = index == me.max ? null : index;
                        val = { value: v, label: v };
                    }
                }
                if (val.value == null) {
                    val = null;
                }
            }
            return val;
        };



        var getIndexByValue = function (val) {
            var index = null;
            if (val != null) {
                if (me.isMapped) {
                    for (var x in me.mappings) {
                        if (me.mappings[x].value == val) {
                            index = x;
                        }
                    }
                }
                else {
                    index = val;
                }
            }
            return index;
        };


        var getIndex = function (value) {
            var index = null;
            if (me.mode == 'range') {
                index = [me.min, me.max];
                if(value!=null){
                    var min = getIndexByValue(value[0]);
                    var max = getIndexByValue(value[1]);
                    index[0] = min != null ? min : index[0];
                    index[1] = max != null ? max : index[1];
                }
            }
            else if (!me.opts.invert && me.opts.mode == 'min' || me.opts.invert && me.opts.mode == 'max') {
                index = getIndexByValue(value);
                if (index == null) {
                    index = me.min;
                }
            }
            else if (!me.opts.invert && me.opts.mode == 'max' || me.opts.invert && me.opts.mode == 'min') {
                index = getIndexByValue(value);
                if (index == null) {
                    index = me.max;
                }
            }
            return index;
        };



        var getValue = function () {
            var p = null;
            if (me.mode == 'range') {
                p = getLabelValue(me.slider.slider("values"));
            }
            else {
                p = getLabelValue(me.slider.slider("value"));
            }
            return p;
        };

        var getParam = function () {
            var p = null;
            if (me.mode == 'range') {
                var mapping = getMapping(me.slider.slider("values"));
                if (mapping != null) {
                    p = [mapping[0].value, mapping[1].value];
                }
            }
            else {
                var mapping = getMapping(me.slider.slider("value"));
                if (mapping != null) {
                    p = mapping.value;
                }
            }
            return p;
        };

        var setParam = function (param) {
            if (me.mode == 'range') {
                me.slider.slider("values", getIndex(param));
            }
            else {
                me.slider.slider("value", getIndex(param));
            }
        };


        var publicObj = {
            getParam: function(){
                return getParam();
            },
            setParam: function(param){
                setParam(param);
            },
            getValue: function () {
                return getValue();
            }
        };
        init();
        return publicObj;
    };
})();













