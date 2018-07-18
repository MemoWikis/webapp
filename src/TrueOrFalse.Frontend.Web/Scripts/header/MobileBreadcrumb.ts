$(window).on("load", function () {
var offset = $('#LastBreadcrumb').offset();
var rightspace = ($(window).width() - ($('#LastBreadcrumb').offset().left + $('#LastBreadcrumb').outerWidth()));
    var index = 0;
if (rightspace < 0) {
    while (rightspace < 0) {
        index++;
        ShortenBreadcrumb(index);
    }
}
});

function ShortenBreadcrumb(index) {
    
}