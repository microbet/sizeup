(function () {
    sizeup.core.namespace('sizeup.views.business.find');
    sizeup.views.business.find = function (opts) {

        var me = {};
        var dataLayer = new sizeup.core.data();
        var notifier = new sizeup.core.notifier(function () { init(); });

        me.opts = opts;
        me.data = {};

        me.data.name = opts.name;

        if (opts.IndustryId) {
            dataLayer.getIndustry({ id: opts.industryId }, notifier.getNotifier(function (i) { me.data.industry = i; }));
        }
        if (opts.placeId) {
            dataLayer.getPlace({ id: opts.placeId }, notifier.getNotifier(function (i) { me.data.city = i; }));
        }
        
        var init = function () {
            me.form = {};
            me.errors = {};
            me.pager = {};
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

            
            me.hasErrors = false;
            me.errors.noIndustryMatches = $('#noIndustryMatchesMessage');
            me.errors.invalidCity = $('#invalidCityMessage');

            me.form.city.selector = sizeup.controls.placeSelector({
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


        var onIndustryChange = function (item) {
            if (!item) {
                me.errors.hasErrors = true;
                me.errors.noIndustryMatches.hide().fadeIn('slow');
            }
            else {
                me.errors.hasErrors = false;
                me.errors.noIndustryMatches.fadeOut('slow');
                me.form.industry.hiddenField.val(item.Id);
            }
        };

        var onCityChange = function (item) {
            if (!item) {
                me.errors.hasErrors = true;
                me.errors.invalidCity.hide().fadeIn('slow');
            }
            else {
                me.errors.hasErrors = false;
                me.errors.invalidCity.fadeOut('slow');
                me.form.city.hiddenField.val(item.Id);
            }
        };


        var onSubmit = function () {
            return !me.errors.hasErrors;// && validateCity() && validateIndustry();
        };


        var validateCity = function () {
            //me.form.city.selector.
        };

        var validateIndustry = function () {
            //me.form.industry.selector
        };


       
        

        var publicObj = {

        };
        //fires a fake notifier if we dont have any params
        notifier.getNotifier()();
        return publicObj;
        
    };
})();