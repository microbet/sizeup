function checkWidth() {
    var w = window,
		d = document,
		e = d.documentElement,
		g = d.getElementsByTagName('body')[0],
		x = w.innerWidth || e.clientWidth || g.clientWidth,
		y = w.innerHeight || e.clientHeight || g.clientHeight;

    // business/startup toggle
    /*if(x <=356 || screen.width <= 356){sid
		$('label[for=business]').text("Business");
	} else {
		$('label[for=business]').text("Business Owner");
	}*/

    // slider disappering / advertisement map
    if (x <= 768 || screen.width <= 768) {
        $('.enterprise.sidebar').hide();
        $('#slider-column').hide();
        $('#slider-sibling-column').children().css('padding-left', '0px');
        $('#advertisement-map').hide();
        $('.section-end span').hide();
    } else {
        $('.enterprise.sidebar').show();
        $('#slider-column').show();
        $('#slider-sibling-column').children().css('padding-left', 'inherit');
        $('#advertisement-map').show();
        $('.section-end span').show();
    }

    if (x <= 1199 || screen.width <= 1199) {
        $('#slider-sibling-column h4').css('margin-top', '10px');
        $('#decisions img').parent().removeClass('text-center').addClass('text-left');
        $('#decisions .copy').css('margin-top', '20px');
    } else {
        $('#slider-sibling-column h4').css('margin-top', '0px');
        $('#decisions img').parent().removeClass('text-left').addClass('text-center');
        $('#decisions .copy').css('margin-top', '0');
    }

    // hide animations if width is too small
    if (x <= 780 || screen.width <= 780) {
        $('#competition #competition-carousel-wrapper').css('padding-bottom', '0em').hide();
    } else {
        $('#competition #competition-carousel-wrapper').css('padding-bottom', '0em').show();
    }

    // industry/location inputs
    if (x <= 426 || screen.width <= 426) {
        if ($('#industry-container span').is(':visible')) {
            $('#industry-container span').hide();
        }
        if ($('#location-container span').is(':visible')) {
            $('#location-container span').hide();
        }
    } else {
        if (!$('#industry-container span').is(':visible')) {
            $('#industry-container span').show();
        }

        if (!$('#location-container span').is(':visible')) {
            $('#location-container span').show();
        }
    }

    // learn more buttons
    if (x <= 1199 || screen.width <= 1199) {
        $('#decisions a.left-col').hide();
        $('#decisions a.right-col').show();
    } else {
        $('#decisions a.left-col').show();
        $('#decisions a.right-col').hide();
    }

    // switch based on device width
    if (x < 613 || screen.width < 613) {
        $('#inline-switch').hide();
        $('#row-switch').show();
    } else {
        $('#inline-switch').show();
        $('#row-switch').hide();
    }

    // copy in decisions section
    if (x <= 550 || screen.width <= 550) {
        $('#decisions .copy p:first-child').css('margin-top', '0.5em');
        $('#decisions .copy a').css('margin-bottom', '1em');
    } else {
        $('#decisions .copy p:first-child').css('margin-top', '0');
        $('#decisions .copy a').css('margin-bottom', '0');
    }

    // swap footer section / toggle advertisement-map
    if (x <= 547 || screen.width <= 547) {
        $('#footer-sm').show();
        $('#footer').hide();
    } else {
        $('#footer-sm').hide();
        $('#footer').show();
    }

    if (x <= 380 || screen.width <= 380) {
        $('#login-btn').css('margin-top', '1.5em');
    } else {
        $('#login-btn').css('margin-top', '2em');
    }
}

//window.onload = checkWidth;
window.onresize = checkWidth;

$(window).scroll(function () {
    $('#animatedDesktop').each(function () {
        var imagePos = $(this).offset().top;

        var topOfWindow = $(window).scrollTop();
        if (imagePos < topOfWindow + 380) {
            $(this).addClass("slideDown");
        }
    });

    $('#animatedFlyer').each(function () {
        var imagePos = $(this).offset().top;

        var topOfWindow = $(window).scrollTop();
        if (imagePos < topOfWindow + 460) {
            $(this).addClass("slideRight");
        }
    });

    $('#animatedLegend').each(function () {
        var imagePos = $(this).offset().top;

        var topOfWindow = $(window).scrollTop();
        if (imagePos < topOfWindow + 555) {
            $(this).addClass("slideLeft");

            $('#competition .carousel-control').css('visibility', 'visible');
            $(window).unbind("scroll");
            setTimeout(function () {
                $('.slideDown').css('visibility', 'visible').removeClass('slideDown');
                $('.slideLeft').css('visibility', 'visible').removeClass('slideLeft');
                $('.slideRight').css('visibility', 'visible').removeClass('slideRight');
                $('#carousel-competition-generic').carousel();
            }, 3000);
        }
    });
});