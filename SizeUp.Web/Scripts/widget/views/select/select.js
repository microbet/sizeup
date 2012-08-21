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

            if (me.data.currentIndustry && me.data.currentPlace) {
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
                checkForData();
            }
        };

        var onSubmit = function () {
            if (me.hasData) {
                var currentCity = me.form.location.placeSelector.getSelection();
                var currentIndustry = me.form.industry.industrySelector.getSelection();
                dataLayer.setCurrentIndustry({ id: currentIndustry.Id });
                dataLayer.setCurrentPlace({ id: currentCity.Id });
                setSelectorLinks();
                showSelector();
            }
        };

        var setSelectorLinks = function () {
            var currentCity = me.form.location.placeSelector.getSelection();
            var currentIndustry = me.form.industry.industrySelector.getSelection();

            me.selector.myBusiness.attr('href', 'dashboard/' + currentCity.State.SEOKey + '/' + currentCity.County.SEOKey + '/' + currentCity.City.SEOKey + '/' + currentIndustry.SEOKey);
            me.selector.competition.attr('href', 'competition/' + currentCity.State.SEOKey + '/' + currentCity.County.SEOKey + '/' + currentCity.City.SEOKey + '/' + currentIndustry.SEOKey);
            me.selector.advertising.attr('href', 'advertising/' + currentCity.State.SEOKey + '/' + currentCity.County.SEOKey + '/' + currentCity.City.SEOKey + '/' + currentIndustry.SEOKey);
        };


        var checkForData = function () {
            var currentCity = me.form.location.placeSelector.getSelection();
            var currentIndustry = me.form.industry.industrySelector.getSelection();
            if (currentCity && currentIndustry) {
                var params = {
                    id: currentIndustry.Id,
                    placeId: currentCity.Id
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