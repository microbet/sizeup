(function () {
   
        $(document).ready(function () {
            var total = $('html').height();
            var head = $('#header').outerHeight();
            var h = (total - head);
            $('#contentWrap').css('max-height', h);
            $('#contentWrap').css('min-height', h);
            $('#contentWrap').css('height', h);
        });


})();