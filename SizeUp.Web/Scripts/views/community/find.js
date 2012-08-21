(function () {
    sizeup.core.namespace('sizeup.views.community');
    sizeup.views.community.find = function (opts) {

        var me = {};
        var dataLayer = new sizeup.core.data();
        var notifier = new sizeup.core.notifier(function () { init(); });

        me.opts = opts;
        me.data = {};

        if (opts.industryId) {
            dataLayer.getIndustry({ id: opts.industryId }, notifier.getNotifier(function (i) { me.data.industry = i; }));
        }
        var init = function () {
            me.form = {};
            me.errors = {};
            me.pager = {};
            me.form.state = {};
            me.form.industry = {};

            me.form.container = $('#form');
            me.form.state.option = $('#state');
            me.form.state.hiddenField = $('#stateId');
            me.form.submit = $('#submit');
            me.form.industry.textbox = $('#industry');
            me.form.industry.hiddenField = $('#industryId');
      
            me.hasErrors = false;
            me.errors.noIndustryMatches = $('#noIndustryMatchesMessage');


            me.form.industry.selector = sizeup.controls.industrySelector({
                textbox: me.form.industry.textbox,
                onChange: function (item) { onIndustryChange(item); }
            });

            if (me.data.industry) {
                me.form.industry.selector.setSelection(me.data.industry);
            }

            if (me.form.state.hiddenField.val()) {
                me.form.state.option.val(me.form.state.hiddenField.val());
            }
           
            me.form.submit.click(onSubmit);

            me.form.container.hide().removeClass('hidden');
            me.errors.noIndustryMatches.hide().removeClass('hidden');
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

      

        var onSubmit = function () {
            me.form.state.hiddenField.val(me.form.state.option.val());
            return !me.errors.hasErrors;
        };


       
       
        

        var publicObj = {

        };
        //fires a fake notifier if we dont have any params
        notifier.getNotifier()();
        return publicObj;
        
    };
})();