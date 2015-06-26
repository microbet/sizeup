(function () {
   
    var body = $('body');
    setInterval(function () {
        var msg = 'resizeIframe:width=' + body.width() + '&height=' + body.height();
        window.top.postMessage(msg, "*");
    }, 125);


})();