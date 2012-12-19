(function () {
    sizeup.core.namespace('sizeup.controls');
    sizeup.controls.printButton = function (opts) {

        var defaults = {
            onClick: function () { }
        };
        var me = {};
        me.opts = $.extend(true, defaults, opts);
        

        var init = function () {
            me.button = opts.button;
            me.button.click(function () { onClick(); });
        };

        var createWindow = function () {
            var URL = window.location;
            var name = "_blank";
            var specs = "channelmode=0,directories=0,fullscreen=0,height=600,location=0,menubar=0,scrollbars=1,status=0,toolbar=0,width=850";
            var win = window.open(URL, name, specs);
            return win;
        };

        var onClick = function () {
            var win = createWindow();
            win.print();
            win.close();
        };

        var publicObj = {

        };
        init();
        return publicObj;

    };
})();