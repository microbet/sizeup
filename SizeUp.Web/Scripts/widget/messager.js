(function () {
   
    var body = $('body');
    setInterval(function () {
        var obj = {
            width: body.width(),
            height: body.height()
        };
        window.top.postMessage(obj, "*");
    }, 125);


})();