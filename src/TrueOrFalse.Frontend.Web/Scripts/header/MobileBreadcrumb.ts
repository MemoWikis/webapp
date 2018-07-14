$(window).on("load", function () {
var offset = $('#Breadcrumblast').offset();
var rightspace = ($(window).width() - ($('#Breadcrumblast').offset().left + $('#Breadcrumblast').outerWidth()));
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