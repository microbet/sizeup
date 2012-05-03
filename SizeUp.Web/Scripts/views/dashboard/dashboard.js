(function () {
    sizeup.core.namespace('sizeup.views.dashboard.dashboard');
    sizeup.views.dashboard.dashboard = function (opts) {

        var me = {};
       /* var dataLayer = new sizeup.core.data();
        var notifier = new sizeup.core.notifier(function () { init(); });
        */
        me.opts = opts;
        me.data = {};

       /* me.data.name = opts.name;

        dataLayer.getIndustry({id: opts.industryId}, notifier.getNotifier(function (i) { me.data.industry = i; }));
        dataLayer.getCity({id: opts.cityId}, notifier.getNotifier(function (i) { me.data.city = i; }));
        */

        var init = function () {
            var rev = new sizeup.views.dashboard.revenue({ container: $('#revenue') });
            rev.fadeInPrompt();
     
        };

      

       
       
        

        var publicObj = {

        };
        init();
        return publicObj;
        
    };
})();