(function () {
    sizeup.core.namespace('sizeup.views.home');
    sizeup.views.home = function (opts) {
        
        var me = {};
        var notifier = new sizeup.core.notifier(function () { init(); });

        var defaults = {};
       
        me.opts = $.extend(true, defaults, opts);

        me.data = {};

        sizeup.core.profile.getDetectedPlace(notifier.getNotifier(function (i) { me.data.detectedCity = i; }));

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
            me.selector.competition = $('#competing');
            me.selector.advertising = $('#advertising');

            
            me.errors.noIndustryMatches = $('#noIndustryMatchesMessage');
            me.errors.invalidCity = $('#invalidCityMessage');
            me.errors.noValuesEntered = $('#noValuesEntered');
            me.form.submit = $('#inline-continue, #row-continue');
            me.form.location.detectedLocation = $('#detectedLocation');
            me.form.location.enteredLocation = $('#enteredLocation');
            me.form.location.cityTextbox = $('#searchCommunity');
            me.form.location.cityTextboxAddon = $('#location-container .input-group-addon');

            me.form.location.detectedLocation.find('.changeLocation').click(onChangeCityClicked);
            me.form.location.enteredLocation.find('.changeLocation').click(onChangeCityClicked);


            //me.form.location.placeSelector = sizeup.controls.placeSelector({
            //    textbox: me.form.location.cityTextbox,
            //    onChange: function (item) { onCityChange(item); },
            //    onBlur: function (item) { onCityChange(item); }
            //});

            me.form.industry.industrySelector = sizeup.controls.industrySelector({
                textbox: me.form.industry.textbox,
                onChange: function (item) { onIndustryChange(item); },
                onBlur: function (item) { onIndustryChange(item); }
            });

            if (!me.opts.currentInfo.CurrentPlace.Id && me.data.detectedCity) {
                me.form.location.detectedLocation.find('.locationText').html(me.data.detectedCity.City.Name + ', ' + me.data.detectedCity.State.Abbreviation);
                //me.form.location.placeSelector.setSelection(me.data.detectedCity);
                me.selectedCity = me.data.detectedCity;
                showDetectedCity();
            }
            else if (me.opts.currentInfo.CurrentPlace.Id) {
                me.form.location.enteredLocation.find('.locationText').html(me.opts.currentInfo.CurrentPlace.City.Name + ', ' + me.opts.currentInfo.CurrentPlace.State.Abbreviation);
                //me.form.location.placeSelector.setSelection(me.opts.currentInfo.CurrentPlace);
                me.selectedCity = me.opts.currentInfo.CurrentPlace;
                //showCurrentCity();
            }
            else {
                showPlaceSelector();
            }

            if (me.opts.currentInfo.CurrentIndustry) {
                me.form.industry.industrySelector.setSelection(me.opts.currentInfo.CurrentIndustry);
                me.selectedIndustry = me.opts.currentInfo.CurrentIndustry;
            }

            me.selectedBusinessStatus = 'established';
            var date = new Date();
            date.setTime(date.getTime() + (7 * 24 * 60 * 60 * 1000)); // 7 days

            if (me.opts.currentInfo.CurrentBusinessStatus) 
                me.selectedBusinessStatus = me.opts.currentInfo.CurrentBusinessStatus;
            else 
                document.cookie = "businessStatus=" + me.selectedBusinessStatus + "; expires=" + date.toGMTString() + ";";
            
            $('input[id="startup"], input[id="business"]').on('click', function () {
                me.selectedBusinessStatus = $(this).val();
                document.cookie = "businessStatus=" + me.selectedBusinessStatus + ";";
            });


            me.form.submit.click(onSubmit);

            me.form.container.hide().removeClass('hidden');
            me.selector.container.hide().removeClass('hidden');

            me.errors.noIndustryMatches.hide().removeClass('hidden');
            me.errors.invalidCity.hide().removeClass('hidden');
            me.errors.noValuesEntered.hide().removeClass('hidden');
            showForm();
        };

        var isSubmitable = function () {
            //return !me.form.location.placeSelector.hasFocus() && !me.form.industry.industrySelector.hasFocus();
            return !me.form.industry.industrySelector.hasFocus();
        };


        var showDetectedCity = function () {
            me.form.location.detectedLocation.removeClass('hidden');
            me.form.location.cityTextbox.addClass('hidden');
            me.form.location.cityTextboxAddon.addClass('hidden');
            me.form.location.enteredLocation.addClass('hidden');
        };

        var showCurrentCity = function () {
            me.form.location.detectedLocation.addClass('hidden');
            me.form.location.cityTextbox.addClass('hidden');
            me.form.location.cityTextboxAddon.addClass('hidden');
            me.form.location.enteredLocation.removeClass('hidden');
        };

        var showPlaceSelector = function () {
            me.form.location.detectedLocation.addClass('hidden');
            me.form.location.cityTextbox.removeClass('hidden');
            me.form.location.cityTextboxAddon.removeClass('hidden');
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
                //showCurrentCity();
            }
        };

        var onChangeCityClicked = function () {
            showPlaceSelector();
        };

        var onSubmit = function () {
            var current_fs, next_fs; //fieldsets

            if (isSubmitable()) {
                me.errors.noValuesEntered.hide();
                //if (me.selectedCity == null || me.selectedIndustry == null) {
                if ( me.selectedIndustry == null) {
                    me.errors.invalidCity.hide();
                    me.errors.noValuesEntered.fadeIn("slow");
                }
                else {
                    setSelectorLinks();
                    current_fs = $(this).closest('fieldset');
                    next_fs = $(this).closest('fieldset').next();
                    current_fs.hide();
                    next_fs.show();
                    next_fs.css({ 'position': 'inherit', 'margin-top': '2.6em', 'width': '100%' });
                    setTimeout(function () {$("#step-two .container .card").toggleClass("flipped");}, 1);
                    setTimeout(function () {
                        $("#step-two-back").show();
                        $("#login-form h1").addClass("col-lg-6").addClass("col-lg-offset-2")
                            .addClass("col-md-7").addClass("col-md-offset-2")
                            .addClass("col-sm-10")
                            .addClass("col-xs-12");
                    }, 1000);
                }
            }
        };

        var setSelectorLinks = function () {
            //me.selector.myBusiness.attr('href', 'demo/test/' + me.selectedCity.State.SEOKey + '/' + me.selectedCity.County.SEOKey + '/' + me.selectedCity.City.SEOKey + '/' + me.selectedIndustry.SEOKey);
            //me.selector.competition.attr('href', 'demo/test/' + me.selectedCity.State.SEOKey + '/' + me.selectedCity.County.SEOKey + '/' + me.selectedCity.City.SEOKey + '/' + me.selectedIndustry.SEOKey);
            //me.selector.advertising.attr('href', 'demo/test/' + me.selectedCity.State.SEOKey + '/' + me.selectedCity.County.SEOKey + '/' + me.selectedCity.City.SEOKey + '/' + me.selectedIndustry.SEOKey);
            me.selector.myBusiness.attr('href', 'demo/test?c=' + window.demoLocation + '&t=ftyTAnI86i');
            me.selector.competition.attr('href', 'demo/test?c=' + window.demoLocation + '&t=ftyTAnI86i');
            me.selector.advertising.attr('href', 'demo/test?c=' + window.demoLocation + '&t=ftyTAnI86i');

        };

        
        var publicObj = {

        };
        return publicObj;
        
    };
})();