(function () {
    sizeup.core.namespace('sizeup.views.user.resetPassword');
    sizeup.views.user.resetPassword = function () {

        var me = {};

        var init = function () {
            me.form = {};
            me.error = {};
            me.form.submit = $('#continue');
            me.form.password = $('#passwordBox');
            me.error.passwordRequired = $('#passwordRequired');
            me.form.password.blur(validatePassword);
            $('#resetForm').submit(onSubmit);
            me.error.passwordRequired.hide().removeClass('hidden');
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

            var isValid = true;
            isValid = validatePassword();
            return isValid;
        };





        var publicObj = {

        };
        init();
        return publicObj;

    };
})();