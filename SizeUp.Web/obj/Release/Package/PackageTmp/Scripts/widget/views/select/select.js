(function () {
    sizeup.core.namespace('sizeup.widget.views.select');
    sizeup.widget.views.select.select = function (opts) {

        var me = {};

        var defaults = {};

        me.opts = $.extend(true, defaults, opts);

        me.data = {};

        var init = function () {
            
            me.form = {};
            me.selector = {};
            me.errors = {};
            me.form.location = {};
            me.form.industry = {};

            me.form.industry.textbox = $('#searchIndustry');
            me.form.container = $('#form');
            me.selector.container = $('#selector');
            me.selector.myBusiness = $('#myBusiness');
            me.selector.competition = $('#competition');
            me.selector.advertising = $('#advertising');

            me.errors.noIndustryMatches = $('#noIndustryMatchesMessage');
            me.errors.invalidCity = $('#invalidCityMessage');
            me.errors.noValuesEntered = $('#noValuesEntered');
            me.form.submit = $('#continue');
            me.form.location.cityTextbox = $('#searchCommunity');

  

            me.form.location.placeSelector = sizeup.controls.placeSelector({
                textbox: me.form.location.cityTextbox,
                onChange: function (item) { onCityChange(item); },
                onBlur: function (item) { onCityChange(item); }
            });

            me.form.industry.industrySelector = sizeup.controls.industrySelector({
                textbox: me.form.industry.textbox,
                onChange: function (item) { onIndustryChange(item); },
                onBlur: function (item) { onIndustryChange(item); }
            });

            if (me.opts.currentInfo.CurrentIndustry) {
                me.form.industry.industrySelector.setSelection(me.opts.currentInfo.CurrentIndustry);
                me.selectedIndustry = me.opts.currentInfo.CurrentIndustry;
            }

            if (me.opts.currentInfo.CurrentPlace.Id) {
                me.form.location.placeSelector.setSelection(me.opts.currentInfo.CurrentPlace);
                me.selectedPlace = me.opts.currentInfo.CurrentPlace;
            }

            me.form.submit.click(onSubmit);

            me.form.container.hide().removeClass('hidden');
            me.selector.container.hide().removeClass('hidden');

            me.errors.noIndustryMatches.hide().removeClass('hidden');
            me.errors.invalidCity.hide().removeClass('hidden');         
            me.errors.noValuesEntered.hide().removeClass('hidden');

            var params = jQuery.deparam.fragment();
            if (params.featureSelect) {
                setSelectorLinks();
                me.form.container.hide();
                me.selector.container.show();
            }
            else {
                showForm();
            }
        };

        var showForm = function () {
            me.form.container.show();
            me.selector.container.hide();
        };

        var showSelector = function () {
            me.form.container.hide("slide", { direction: "left" }, 500);
            me.selector.container.show("slide", { direction: "right" }, 500);
        };

        var onIndustryChange = function (item) {
            me.errors.noValuesEntered.hide();
            if (!item) {
                me.errors.noIndustryMatches.hide().fadeIn('slow');
            }
            else {
                me.errors.noIndustryMatches.fadeOut('slow');
            }
            me.selectedIndustry = item;
        };

        var onCityChange = function (item) {
            me.errors.noValuesEntered.hide();
            if (!item) {
                me.errors.invalidCity.hide().fadeIn('slow');
            }
            else {
                me.errors.invalidCity.fadeOut('slow');
            }
            me.selectedPlace = item;
        };

        var isSubmitable = function () {
            return !me.form.location.placeSelector.hasFocus() && !me.form.industry.industrySelector.hasFocus();
        };

        var onSubmit = function () {
            if (isSubmitable()) {
                me.errors.noValuesEntered.hide();
                if (me.selectedPlace == null || me.selectedIndustry == null) {
                    me.errors.invalidCity.hide();
                    me.errors.noValuesEntered.fadeIn("slow");
                }
                else {
                    setSelectorLinks();
                    if (me.opts.startFeature != null && me.opts.startFeature == 'Dashboard') {
                        window.location = '/widget/dashboard/' + getUrlPath();
                    }
                    else if (me.opts.startFeature != null && me.opts.startFeature == 'Competition') {
                        window.location = '/widget/competition/' + getUrlPath();
                    }
                    else if (me.opts.startFeature != null && me.opts.startFeature == 'Advertising') {
                        window.location = '/widget/advertising/' + getUrlPath();
                    }
                    else if (me.opts.startFeature != null && me.opts.startFeature == 'Community') {
                        window.location = '/widget/community/' + getUrlPath();
                    }
                    else {
                        showSelector();
                    }
                }
            }
        };

        var setSelectorLinks = function () {
            me.selector.myBusiness.attr('href', 'dashboard/' + getUrlPath());
            me.selector.competition.attr('href', 'competition/' + getUrlPath());
            me.selector.advertising.attr('href', 'advertising/' + getUrlPath());
        };

        var getUrlPath = function () {
            return jQuery.param.querystring(me.selectedPlace.State.SEOKey + '/' + me.selectedPlace.County.SEOKey + '/' + me.selectedPlace.City.SEOKey + '/' + me.selectedIndustry.SEOKey,jQuery.param.querystring(), 2 );
        };


        var publicObj = {

        };
        init();
        return publicObj;

    };
})();