(function () {
    sizeup.core.namespace('sizeup.views.shared');
    sizeup.views.shared.signin = function (opts) {

        var defaults = {
            signinUrl: '/api/user/signin/',
            toggle: $('<a href="javascript:void(0);"></a>')
        };
        var me = {};

        me.opts = $.extend(true, defaults, opts);
        me.container = opts.container.hide().removeClass('hidden');
        me.toggle = me.opts.toggle;

        me.signinForm = {
            emailBox: me.container.find('.signInEmail'),
            passwordBox: me.container.find('.signInPassword'),
            rememberMeBox: me.container.find('.remeberMe'),
            submit: me.container.find('.button'),
            error: me.container.find('.error').hide().removeClass('hidden'),
            form: me.container.find('.signinForm'),
            resetLink: me.container.find('.error.failed .resetLink'),
            resetMessage: me.container.find('.error.failed .passwordReset').hide().removeClass('hidden'),
            resendVerifiyLink: me.container.find('.error.unvalidated .resendVerifiyLink'),
            resendVerifiyMessage: me.container.find('.error.unvalidated .resendVerifyMessage').hide().removeClass('hidden')
        };

        me.signinForm.form.submit(function (e) { signinPressed(); e.preventDefault(); return false; });
        me.signinForm.resetLink.click(function () { resetPassword(); });
        me.signinForm.resendVerifiyLink.click(function () { resendVerification(); });
        me.toggle.click(function () { toggleSigninForm(); });
       


        var toggleSigninForm = function () {
            me.container.slideToggle();
        };

        var openForm = function () {
            if (!me.container.is(':visible')) {
                me.container.slideToggle();
            }
        };

        var closeForm = function () {
            if (me.container.is(':visible')) {
                me.container.slideToggle();
                clearForm();
            }
        };

        var clearForm = function () {
            me.signinForm.emailBox.val('');
            me.signinForm.passwordBox.val('');
        };

        var signinPressed = function () {
            me.signinForm.error.hide();
            me.signinForm.resetMessage.hide();
            me.signinForm.resendVerifiyMessage.hide();

            var obj = {
                email: me.signinForm.emailBox.val(),
                password: me.signinForm.passwordBox.val(),
                persist: me.signinForm.rememberMeBox.is(':checked')
            };

            if (obj.email != '' && obj.password != '') {
                $.ajax({
                    type: 'POST',
                    url: me.opts.signinUrl,
                    data: obj,
                    success: function (data) { signinPostComplete(data); }
                });
                me.signinForm.emailBox.removeClass('error');
                me.signinForm.passwordBox.removeClass('error');
            }
            else {
                if (obj.email == '') {
                    me.signinForm.emailBox.addClass('error');
                }
                if (obj.password == '') {
                    me.signinForm.passwordBox.addClass('error');
                }
            }
        };

        var signinPostComplete = function (data) {
            if (data == 'ok') {
                new sizeup.core.analytics().userSignin({ label: 'quickSignin' });
                window.location.reload();
            }
            else if (data == 'locked') {
                me.container.find('.error.locked').show();
            }
            else if (data == 'unvalidated') {
                me.container.find('.error.unvalidated').show();
            }
            else {
                me.container.find('.error.failed').show();
            }
        };

        var resetPassword = function () {

            var obj = {
                email: me.signinForm.emailBox.val()
            };

            if (obj.email != '') {
                $.ajax({
                    type: 'POST',
                    url: '/api/user/resetPassword',
                    data: obj,
                    success: function () { resetPasswordComplete(); }
                });
            }
        };

        var resetPasswordComplete = function () {
            me.signinForm.resetMessage.show();
        };

        var resendVerification = function () {

            var obj = {
                email: me.signinForm.emailBox.val()
            };

            if (obj.email != '') {
                $.ajax({
                    type: 'POST',
                    url: '/api/user/SendVerification',
                    data: obj,
                    success: function () { resendVerificationComplete(); }
                });
            }
        };

        var resendVerificationComplete = function () {
            me.signinForm.resendVerifiyMessage.show();
        };

        var publicObj = {
            toggleSigninForm: function () {
                toggleSigninForm();
            },
            openForm: function () {
                openForm();
            },
            closeForm: function(){
                closeForm();
            },
            clearForm: function () {
                clearForm();
            }
        };
        return publicObj;
    };

})();


