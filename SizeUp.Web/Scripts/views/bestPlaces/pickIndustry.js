﻿(function () {
    sizeup.core.namespace('sizeup.views.bestPlaces');
    sizeup.views.bestPlaces.pickIndustry = function (opts) {

        var me = {};
        var dataLayer = new sizeup.core.data();
        var notifier = new sizeup.core.notifier(function () { init(); });

        me.opts = opts;
        me.data = {};

        dataLayer.getCurrentIndustry(notifier.getNotifier(function (i) { me.data.currentIndustry = i; }));

        var init = function () {
            me.hasData = false;
            me.form = {};
            me.errors = {};
            me.form.industry = {};

            me.form.container = $('#pickIndustry .form');
            me.form.industry.textbox = $('#searchIndustry');
            me.errors.noIndustryMatches = $('#noIndustryMatchesMessage');
            me.form.submit = $('#continue');
         
            me.form.industry.industrySelector = sizeup.controls.industrySelector({
                textbox: me.form.industry.textbox,
                onChange: function (item) { onIndustryChange(item); },
                onBlur: function (item) { onIndustryChange(item); }
            });

            if (me.data.currentIndustry) {
                me.form.industry.industrySelector.setSelection(me.data.currentIndustry);
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
            me.errors.noIndustryMatches.hide();
            me.hasData = false;
            if (!item) {
                me.errors.noIndustryMatches.hide().fadeIn('slow');
            }
            else {
                me.errors.noIndustryMatches.fadeOut('slow');
            }
        };

      
     
        var onSubmit = function () {
            var currentIndustry = me.form.industry.industrySelector.getSelection();
            if (currentIndustry == null) {
                me.errors.noIndustryMatches.hide().fadeIn('slow');
            }
            else {
                dataLayer.setCurrentIndustry({ id: currentIndustry.Id }, function () {
                    window.location.href = currentIndustry.SEOKey;
                });
            }
        };

       

        

        var publicObj = {

        };
        return publicObj;
        
    };
})();