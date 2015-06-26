(function () {
    sizeup.core.namespace('sizeup.views.user.signin');
    sizeup.views.user.signin = function () {

        var me = {};
        
        var init = function () {
            me.form = {};
            me.error = {};
            me.form.submit = $('#signin');
            me.form.email = $('#emailBox');
            me.form.password = $('#passwordBox');

            me.error.emailRequired = $('.emailRequired');
            me.error.invalidEmail = $('.emailInvalid');
            me.error.passwordRequired = $('.passwordRequired');

            me.form.email.blur(validateEmail);
            me.form.password.blur(validatePassword);

            $('#signinForm').submit(onSubmit);


            me.error.emailRequired.hide().removeClass('hidden');
            me.error.invalidEmail.hide().removeClass('hidden');
            me.error.passwordRequired.hide().removeClass('hidden');
        };



        var validateEmail = function () {
            var isValid = true;
            var emailRegex = /\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/;
            var emailVal = $.trim(me.form.email.val());
            if (emailVal == '') {
                me.error.emailRequired.show();
                isValid = false;
            }
            else if (!emailRegex.test(emailVal)) {
                me.error.invalidEmail.show();
                isValid = false;
            }
            else {
                me.error.emailRequired.hide();
                me.error.invalidEmail.hide();
            }
            return isValid;
        };

        var validatePassword = function () {
            var isValid = true;
            if ($.trim(me.form.password.val()) == '') {
                me.error.passwordRequired.show();
                isValid = false;
            }
            else {
                me.error.passwordRequired.hide();
            }
            return isValid;
        };


        var onSubmit = function () {
            var isValid = validateEmail() && validatePassword();
            return isValid;
        };

       



        var publicObj = {

        };
        init();
        return publicObj;

    };
})();