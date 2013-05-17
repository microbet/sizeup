(function () {
    sizeup.core.namespace('sizeup.views.home');
    sizeup.views.home = function (opts) {

        var me = {};
        var dataLayer = new sizeup.core.data();
        var notifier = new sizeup.core.notifier(function () { init(); });

        me.opts = opts;
        me.data = {};

        sizeup.core.profile.getCurrentIndustry(notifier.getNotifier(function (i) { me.data.currentIndustry = i; }));
        sizeup.core.profile.getCurrentPlace(notifier.getNotifier(function (i) { me.data.currentCity = i; }));
        dataLayer.getDetectedPlace(notifier.getNotifier(function (i) { me.data.detectedCity = i; }));

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
            me.form.location.detectedLocation = $('#detectedLocation');
            me.form.location.enteredLocation = $('#enteredLocation');
            me.form.location.cityTextbox = $('#searchCommunity');

            me.form.location.detectedLocation.find('.changeLocation').click(onChangeCityClicked);
            me.form.location.enteredLocation.find('.changeLocation').click(onChangeCityClicked);


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

            if (!me.data.currentCity && me.data.detectedCity) {
                me.form.location.detectedLocation.find('.locationText').html(me.data.detectedCity.City.Name + ', ' + me.data.detectedCity.State.Abbreviation);
                me.form.location.placeSelector.setSelection(me.data.detectedCity);
                me.selectedCity = me.data.detectedCity;
                showDetectedCity();
            }
            else if (me.data.currentCity) {
                me.form.location.enteredLocation.find('.locationText').html(me.data.currentCity.City.Name + ', ' + me.data.currentCity.State.Abbreviation);
                me.form.location.placeSelector.setSelection(me.data.currentCity);
                me.selectedCity = me.data.currentCity;
                showCurrentCity();
            }
            else {
                showPlaceSelector();
            }

            if (me.data.currentIndustry) {
                me.form.industry.industrySelector.setSelection(me.data.currentIndustry);
                me.selectedIndustry = me.data.currentIndustry;
            }

            me.form.submit.click(onSubmit);

            me.form.container.hide().removeClass('hidden');
            me.selector.container.hide().removeClass('hidden');

            me.errors.noIndustryMatches.hide().removeClass('hidden');
            me.errors.invalidCity.hide().removeClass('hidden');
            me.errors.noValuesEntered.hide().removeClass('hidden');
            showForm();
        };

        var isSubmitable = function () {
            return !me.form.location.placeSelector.hasFocus() && !me.form.industry.industrySelector.hasFocus();
        };


        var showDetectedCity = function () {
            me.form.location.detectedLocation.removeClass('hidden');
            me.form.location.cityTextbox.addClass('hidden');
            me.form.location.enteredLocation.addClass('hidden');
        };

        var showCurrentCity = function () {
            me.form.location.detectedLocation.addClass('hidden');
            me.form.location.cityTextbox.addClass('hidden');
            me.form.location.enteredLocation.removeClass('hidden');
        };

        var showPlaceSelector = function () {
            me.form.location.detectedLocation.addClass('hidden');
            me.form.location.cityTextbox.removeClass('hidden');
            me.form.location.enteredLocation.addClass('hidden');
        };

        var showForm = function () {
            me.form.container.show();
            me.selector.container.hide();
        };

        var showSelector = function () {
            new sizeup.core.analytics().placeIndustry({ placeId: me.selectedCity.Id, industryId: me.selectedIndustry.Id });
            me.form.container.hide("slide", { direction: "left" }, 500);
            me.selector.container.show("slide", { direction: "right" }, 500);
        };

        var onIndustryChange = function (item) {
            me.selectedIndustry = item;
            me.errors.noValuesEntered.hide();
            if (!item) {
                me.errors.noIndustryMatches.hide().fadeIn('slow');
            }
            else {
                me.errors.noIndustryMatches.fadeOut('slow');
            }
        };

        var onCityChange = function (item) {
            me.selectedCity = item;
            me.errors.noValuesEntered.hide();
            if (!item) {
                me.errors.invalidCity.hide().fadeIn('slow');
            }
            else {
                me.errors.invalidCity.fadeOut('slow');
                me.form.location.enteredLocation.find('.locationText').html(item.City.Name + ', ' + item.State.Abbreviation);
                showCurrentCity();
            }
        };

        var onChangeCityClicked = function () {
            showPlaceSelector();
        };

        var onSubmit = function () {
            if (isSubmitable()) {
                me.errors.noValuesEntered.hide();
                if (me.selectedCity == null || me.selectedIndustry == null) {
                    me.errors.invalidCity.hide();
                    me.errors.noValuesEntered.fadeIn("slow");
                }
                else {
                    sizeup.core.profile.setCurrentIndustry({ id: me.selectedIndustry.Id });
                    sizeup.core.profile.setCurrentPlace({ id: me.selectedCity.Id });
                    setSelectorLinks();
                    showSelector();
                }
            }
        };

        var setSelectorLinks = function () {
            me.selector.myBusiness.attr('href', 'dashboard/' + me.selectedCity.State.SEOKey + '/' + me.selectedCity.County.SEOKey + '/' + me.selectedCity.City.SEOKey + '/' + me.selectedIndustry.SEOKey);
            me.selector.competition.attr('href', 'competition/' + me.selectedCity.State.SEOKey + '/' + me.selectedCity.County.SEOKey + '/' + me.selectedCity.City.SEOKey + '/' + me.selectedIndustry.SEOKey);
            me.selector.advertising.attr('href', 'advertising/' + me.selectedCity.State.SEOKey + '/' + me.selectedCity.County.SEOKey + '/' + me.selectedCity.City.SEOKey + '/' + me.selectedIndustry.SEOKey);
        };

        

        var publicObj = {

        };
        return publicObj;
        
    };
})();