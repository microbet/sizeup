(function () {
    sizeup.core.namespace('sizeup.views.business.find');
    sizeup.views.business.find = function (opts) {

        var me = {};


        
        var dataLayer = new sizeup.core.data();
        var notifier = new sizeup.core.notifier(function () { init(); });

        me.opts = opts;
        me.data = {};

        me.data.name = opts.name;

        dataLayer.getIndustry({id: opts.industryId}, notifier.getNotifier(function (i) { me.data.industry = i; }));
        dataLayer.getCity({id: opts.cityId}, notifier.getNotifier(function (i) { me.data.city = i; }));
        

        var init = function () {
            me.form = {};
            me.errors = {};
            me.form.city = {};
            me.form.industry = {};
            me.form.business = {};

            me.form.container = $('#form');
            me.form.industry.textbox = $('#industry');
            me.form.industry.hiddenField = $('#industryId');
            me.form.business.textbox = $('#business');
            me.form.city.textbox = $('#city');
            me.form.city.hiddenField = $('#cityId');
            me.form.submit = $('#submit');

            me.errors.noIndustryMatches = $('#noIndustryMatchesMessage');
            me.errors.invalidCity = $('#invalidCityMessage');

            me.form.city.selector = sizeup.controls.citySelector({
                textbox: me.form.city.textbox,
                onChange: function (item) { onCityChange(item); }
            });

            me.form.industry.selector = sizeup.controls.industrySelector({
                textbox: me.form.industry.textbox,
                onChange: function (item) { onIndustryChange(item); }
            });

            if (me.data.city) {
                me.form.city.selector.setSelection(me.data.city);
            }
            if (me.data.industry) {
                me.form.industry.selector.setSelection(me.data.industry);
            }
            if (me.data.name) {
                me.form.business.textbox.val(me.data.name);
            }
           









            me.form.submit.click(onSubmit);

            me.form.container.hide().removeClass('hidden');
            me.errors.noIndustryMatches.hide().removeClass('hidden');
            me.errors.invalidCity.hide().removeClass('hidden');

            showForm();
        };

      

        var showForm = function () {
            me.form.container.show();
        };

        var showSelector = function () {
            me.form.container.hide("slide", { direction: "left" }, 500);
            me.selector.container.show("slide", { direction: "right" }, 500);
        };

        var onIndustryChange = function (item) {
            if (!item) {
                me.errors.noIndustryMatches.hide().fadeIn('slow');
            }
            else {
                me.errors.noIndustryMatches.fadeOut('slow');
                me.form.industry.hiddenField.val(item.Id);
            }
        };

        var onCityChange = function (item) {
            if (!item) {
                me.errors.invalidCity.hide().fadeIn('slow');
            }
            else {
                me.errors.invalidCity.fadeOut('slow');
                me.form.city.hiddenField.val(item.Id);
            }
        };


        var onSubmit = function () {

        };

      


       
        

        var publicObj = {

        };
        return publicObj;
        
    };
})();