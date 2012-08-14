(function () {
    sizeup.core.namespace('sizeup.views.community');
    sizeup.views.community.find = function (opts) {

        var me = {};
        var dataLayer = new sizeup.core.data();
        var notifier = new sizeup.core.notifier(function () { init(); });

        me.opts = opts;
        me.data = {};

        
        var init = function () {
            me.form = {};
            me.errors = {};
            me.pager = {};
            me.form.state = {};


            me.form.container = $('#form');
            me.form.state.option = $('#state');
            me.form.state.hiddenField = $('#stateId');
            me.form.submit = $('#submit');

      
           
            if (me.form.state.hiddenField.val()) {
                me.form.state.option.val(me.form.state.hiddenField.val());
            }
           
            me.form.submit.click(onSubmit);

            me.form.container.hide().removeClass('hidden');

            showForm();
        };

      

        var showForm = function () {
            me.form.container.show();
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