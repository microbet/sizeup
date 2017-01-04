(function () {
    sizeup.core.namespace('sizeup.views.user');
    sizeup.views.user.profile = function () {

        var defaults = {
            profileUrl: '/api/user/profile/',
            passwordUrl: '/api/user/password/'
        };
        var me = {};
        
        me.opts = $.extend(true, defaults, {});


        var init = function () {
            me.form = {};
            me.messages = {};
            me.form.container = $('#profileForm');
            me.form.submit = $('#saveChangesButton');
            me.form.fullName = $('#fullName');
            me.form.password = $('#passwordBox');
            me.form.confirmPassword = $('#confirmPasswordBox');
            me.form.isSubscribedBox = $('#optInBox');
            me.form.industrySubscriptionBox = $('#subscribeIndustry');
            
            me.messages.passwordsDontMatch = $('#passwordsDontMatch').removeClass('hidden').hide();
            me.messages.settingsSaved = $('#settingsSaved').removeClass('hidden').hide();
            
            me.form.container.submit(function (e) { onSubmit(); e.preventDefault(); return false; });

            sizeup.core.profile.getSubscribedIndustries(function (data) { me.form.industryMultiSelector.setMultipleSelections(data) });

            me.form.industryMultiSelector = sizeup.controls.industryMultiSelector({
                textbox: me.form.industrySubscriptionBox,
                onChange: function (item) { onIndustryChange(item); },
                onBlur: function (item) { onIndustryChange(item); }
            });

        };

        var onIndustryChange = function (item) {
            me.selectedIndustry = item;
        };

        var validatePassword = function () {
            var isValid = true;
            var p = me.form.password.val();
            var cp = me.form.confirmPassword.val();

            if ((p != '' || cp != '') && p!= cp) {
                me.messages.passwordsDontMatch.show();
                isValid = false;
            }
            return isValid;
        };


        var onSubmit = function () {
            me.messages.settingsSaved.hide();
            me.messages.passwordsDontMatch.hide();
            var isValid = validatePassword();
            var notifier = new sizeup.core.notifier(function () {
                me.messages.settingsSaved.show().delay(7500).fadeOut();
            });


            if (isValid) {
                var profile = {
                    FullName: $.trim(me.form.fullName.val()),
                    IsSubscribed: me.form.isSubscribedBox.is(':checked'),
                    IndustrySubscriptions: JSON.stringify($.map(me.form.industryMultiSelector.getSelection(), function (e, i) { return e.Id }))
                };
                var password = { password: me.form.password.val() };


                sizeup.core.profile.updateUserProfile(profile, notifier.getNotifier(function (data) {

                }));

                if (password.password != '') {
                    sizeup.core.profile.setPassword(password, notifier.getNotifier(function (data) {

                    }));
                }


            }
            me.form.password.val('');
            me.form.confirmPassword.val('');
            
        };

        var publicObj = {

        };
        init();
        return publicObj;

    };
})();