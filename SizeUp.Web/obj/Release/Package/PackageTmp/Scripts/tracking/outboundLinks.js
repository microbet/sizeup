$(document).ready(function () {
    $('body').delegate('a[target="_blank"]', 'click', function () {
        var l = $(this);
        new sizeup.core.analytics().outgoingLink({ category: 'outgoingLinks', label: l.attr('href'), isInteraction: true });
    });
});