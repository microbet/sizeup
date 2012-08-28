(function () {
    sizeup.core.namespace('sizeup.views.shared');
    sizeup.views.shared.signin = function (opts) {

        var defaults = {
            signinUrl: '/api/user/signin/'
        };
        var me = {};

        me.opts = $.extend(true, defaults, opts);
        me.container = opts.container.hide().removeClass('hidden');
        me.toggle = opts.toggle;

        me.signinForm = {
            emailBox: me.container.find('.signInEmail'),
            passwordBox: me.container.find('.signInPassword'),
            rememberMeBox: me.container.find('.remeberMe'),
            submit: me.container.find('.button'),
            error: me.container.find('.error').hide().removeClass('hidden')
        };

        me.signinForm.submit.click(function () { signinPressed(); });

        me.toggle.click(function () { toggleSigninForm(); });
       


        var toggleSigninForm = function () {
            me.container.slideToggle();
        };


        var signinPressed = function () {
            me.signinForm.error.hide();
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

        var publicObj = {
            toggleSigninForm: function () {
                toggleSigninForm();
            }
        };
        return publicObj;
    };

})();


