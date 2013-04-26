(function () {
    sizeup.core.namespace('sizeup.widget.views.select');
    sizeup.widget.views.select.select = function (opts) {

        var me = {};
        var dataLayer = new sizeup.core.data();
        var notifier = new sizeup.core.notifier(function () { init(); });

        me.opts = opts;
        me.data = {};

        dataLayer.getCurrentIndustry(notifier.getNotifier(function (i) { me.data.currentIndustry = i; }));
        dataLayer.getCurrentPlace(notifier.getNotifier(function (i) { me.data.currentPlace = i; }));

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
                onChange: function (item) { onCityChange(item); }
            });

            me.form.industry.industrySelector = sizeup.controls.industrySelector({
                textbox: me.form.industry.textbox,
                onChange: function (item) { onIndustryChange(item); }
            });

            if (me.data.currentIndustry) {
                me.form.industry.industrySelector.setSelection(me.data.currentIndustry);
            }

            if (me.data.currentPlace) {
                me.form.location.placeSelector.setSelection(me.data.currentPlace);
            }

            me.form.submit.click(onSubmit);

            me.form.container.hide().removeClass('hidden');
            me.selector.container.hide().removeClass('hidden');

            me.errors.noIndustryMatches.hide().removeClass('hidden');
            me.errors.invalidCity.hide().removeClass('hidden');         
            me.errors.noValuesEntered.hide().removeClass('hidden');
            showForm();
        };

        var showForm = function () {
            me.form.container.show();
            me.selector.container.hide();
        };

        var showSelector = function () {
            var currentCity = me.form.location.placeSelector.getSelection();
            var currentIndustry = me.form.industry.industrySelector.getSelection();
            new sizeup.core.analytics().placeIndustry({ placeId: currentCity.Id, industryId: currentIndustry.Id });
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
        };

        var onCityChange = function (item) {
            me.errors.noValuesEntered.hide();
            if (!item) {
                me.errors.invalidCity.hide().fadeIn('slow');
            }
            else {
                me.errors.invalidCity.fadeOut('slow');
            }
        };


        var onSubmit = function () {
            me.errors.noValuesEntered.hide();
            var currentCity = me.form.location.placeSelector.getSelection();
            var currentIndustry = me.form.industry.industrySelector.getSelection();
            if (currentCity == null || currentIndustry == null) {
                me.errors.invalidCity.hide();
                me.errors.noValuesEntered.fadeIn("slow");
            }
            else{
                var currentCity = me.form.location.placeSelector.getSelection();
                var currentIndustry = me.form.industry.industrySelector.getSelection();
                dataLayer.setCurrentIndustry({ id: currentIndustry.Id });
                dataLayer.setCurrentPlace({ id: currentCity.Id });
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
        };

        var setSelectorLinks = function () {
            me.selector.myBusiness.attr('href', 'dashboard/' + getUrlPath());
            me.selector.competition.attr('href', 'competition/' + getUrlPath());
            me.selector.advertising.attr('href', 'advertising/' + getUrlPath());
        };

        var getUrlPath = function () {
            var currentCity = me.form.location.placeSelector.getSelection();
            var currentIndustry = me.form.industry.industrySelector.getSelection();

            return currentCity.State.SEOKey + '/' + currentCity.County.SEOKey + '/' + currentCity.City.SEOKey + '/' + currentIndustry.SEOKey;
        };


        var publicObj = {

        };
        return publicObj;

    };
})();