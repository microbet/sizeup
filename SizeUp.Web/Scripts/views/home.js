(function () {
    sizeup.core.namespace('sizeup.views.home');
    sizeup.views.home = function (opts) {

        var me = {};
        var dataLayer = new sizeup.core.data();
        var notifier = new sizeup.core.notifier(function () { init(); });

        dataLayer.getCurrentIndustry(notifier.getNotifier(function (i) { me.currentIndustry = i; }));
       // dataLayer.getCurrentCity(notifier.getNotifier(function (i) { me.currentCity = i; }));
       // dataLayer.getDetectedCity(notifier.getNotifier(function (i) { me.detectedCity = i; }));

        var init = function () {
            me.form = {};
            me.selector = {};

            me.form.industrySearch = {};

            //me.form.industrySearch.textbox = $('#searchIndustry');
           // me.form.industrySearch.prompt = me.form.industrySearch.textbox.val();
            me.form.submit = $('#continue');

            me.form.location = {};
            //me.form.location.textbox =$('#searchCommunity') ;
            //me.form.location.prompt = me.form.location.textbox.val();


            me.form.location.detectedLocation = $('#detectedLocation');
            me.form.location.enteredLocation = $('#enteredLocation');


           

            me.form.industrySelector = sizeup.controls.industrySelector({
                textbox: $('#searchIndustry'),
                onSelect: function (item) { onIndustrySelect(item); }
            });

            var onIndustrySelect = function (item) {
                
            };
            
        };
        
    };
})();