(function () {
    sizeup.core.namespace('sizeup.views.home');
    sizeup.views.home = function (opts) {

        var me = {};
        var dataLayer = new sizeup.core.data();
        var notifier = new sizeup.core.notifier(function () { init(); });

        me.opts = opts;
        me.data = {};

        dataLayer.getCurrentIndustry(notifier.getNotifier(function (i) { me.data.currentIndustry = i; }));
        dataLayer.getCurrentCity(notifier.getNotifier(function (i) { me.data.currentCity = i; }));
        dataLayer.getDetectedCity(notifier.getNotifier(function (i) { me.data.detectedCity = i; }));

        var init = function () {
            me.hasData = false;
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

            me.errors.noData = $('#noDataMessage');
            me.errors.noIndustryMatches = $('#noIndustryMatchesMessage');
            me.errors.invalidCity = $('#invalidCityMessage');
            me.form.submit = $('#continue');
            me.form.location.detectedLocation = $('#detectedLocation');
            me.form.location.enteredLocation = $('#enteredLocation');
            me.form.location.cityTextbox = $('#searchCommunity');

            me.form.location.detectedLocation.find('.changeLocation').click(onChangeCityClicked);
            me.form.location.enteredLocation.find('.changeLocation').click(onChangeCityClicked);


            me.form.location.citySelector = sizeup.controls.citySelector({
                textbox: me.form.location.cityTextbox,
                onChange: function (item) { onCityChange(item); }
            });

            me.form.industry.industrySelector = sizeup.controls.industrySelector({
                textbox: me.form.industry.textbox,
                onChange: function (item) { onIndustryChange(item); }
            });

            if (!me.data.currentCity && me.data.detectedCity) {
                me.form.location.detectedLocation.find('.locationText').html(me.data.detectedCity.FullName);
                me.form.location.citySelector.setSelection(me.data.detectedCity);
                showDetectedCity();
            }
            else if (me.data.currentCity) {
                me.form.location.enteredLocation.find('.locationText').html(me.data.currentCity.FullName);
                me.form.location.citySelector.setSelection(me.data.currentCity);
                showCurrentCity();
            }
            else {
                showCitySelector();
            }

            if (me.data.currentIndustry) {
                me.form.industry.industrySelector.setSelection(me.data.currentIndustry);
            }

            if (me.data.currentIndustry && (me.data.currentCity || me.data.detectedCity)) {
                me.hasData = true;
            }

            me.form.submit.click(onSubmit);

            me.form.container.hide().removeClass('hidden');
            me.selector.container.hide().removeClass('hidden');

            me.errors.noIndustryMatches.hide().removeClass('hidden');
            me.errors.invalidCity.hide().removeClass('hidden');
            me.errors.noData.hide().removeClass('hidden');
            showForm();
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

        var showCitySelector = function () {
            me.form.location.detectedLocation.addClass('hidden');
            me.form.location.cityTextbox.removeClass('hidden');
            me.form.location.enteredLocation.addClass('hidden');
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
            if (!item) {
                me.errors.noIndustryMatches.hide().fadeIn('slow');
            }
            else {
                me.errors.noIndustryMatches.fadeOut('slow');
                checkForData();
            }
        };

        var onCityChange = function (item) {
            if (!item) {
                me.errors.invalidCity.hide().fadeIn('slow');
            }
            else {
                me.errors.invalidCity.fadeOut('slow');
                me.form.location.enteredLocation.find('.locationText').html(item.FullName);
                showCurrentCity();
                checkForData();
            }
        };

        var onChangeCityClicked = function () {
            showCitySelector();
        };

        var onSubmit = function () {
            if (me.hasData) {
                var currentCity = me.form.location.citySelector.getSelection();
                var currentIndustry = me.form.industry.industrySelector.getSelection();
                dataLayer.setCurrentIndustry({ id: currentIndustry.Id });
                dataLayer.setCurrentCity({ id: currentCity.Id });
                setSelectorLinks();
                showSelector();
            }
        };

        var setSelectorLinks = function () {
            var currentCity = me.form.location.citySelector.getSelection();
            var currentIndustry = me.form.industry.industrySelector.getSelection();

            me.selector.myBusiness.attr('href', 'dashboard/' + currentCity.State + '/' + currentCity.SEOKey + '/' + currentIndustry.SEOKey);
            me.selector.competition.attr('href', 'competition/' + currentCity.State + '/' + currentCity.SEOKey + '/' + currentIndustry.SEOKey);
            me.selector.advertising.attr('href', 'advertising/' + currentCity.State + '/' + currentCity.SEOKey + '/' + currentIndustry.SEOKey);
        };


        var checkForData = function () {
            var currentCity = me.form.location.citySelector.getSelection();
            var currentIndustry = me.form.industry.industrySelector.getSelection();
            if (currentCity && currentIndustry) {
                var params = {
                    id: currentIndustry.Id,
                    cityId: currentCity.Id
                }
                dataLayer.hasData(params, function (isValid) {
                    if (!isValid) {
                        me.errors.noData.hide().fadeIn('slow');
                    }
                    else {
                        me.errors.noData.fadeOut('slow');
                    }
                    me.hasData = isValid;
                });
            }
        };

       
        

        var publicObj = {

        };
        return publicObj;
        
    };
})();