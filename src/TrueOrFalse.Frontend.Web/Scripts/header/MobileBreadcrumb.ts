window.onresize = function (event) {

var BreadCrumbCount = getCount(document.getElementById("BreadcrumbContainer")) - 3;
if (BreadCrumbCount > 1) {
    var offset = $('#LastBreadcrumb').offset();
    var rightspace = ($(window).width() - ($('#LastBreadcrumb').offset().left + $('#LastBreadcrumb').outerWidth()));
    if (rightspace < 0) {
        ShortenBreadcrumb(1);
        rightspace = ($(window).width() - ($('#LastBreadcrumb').offset().left + $('#LastBreadcrumb').outerWidth()))
        if (rightspace < 0) {
            ShortenBreadcrumb(2);
            rightspace = ($(window).width() - ($('#LastBreadcrumb').offset().left + $('#LastBreadcrumb').outerWidth()))
            if (rightspace < 0) {
                ShortenBreadcrumb(3);
                rightspace = ($(window).width() - ($('#LastBreadcrumb').offset().left + $('#LastBreadcrumb').outerWidth()))
            }
        }
    }
}
};

function ShortenBreadcrumb(index) {
    var BreadCrumbCount = getCount(document.getElementById("BreadcrumbContainer")) - 3;

    switch (index) {
        case 1:
         if (BreadCrumbCount < 3) {
            break;
         }
         var i;
         var width = ((document.getElementById("BreadcrumbContainer").offsetWidth) - document.getElementById("LastBreadcrumb").offsetWidth - document.getElementById("BreadcrumbHome").offsetWidth - document.getElementById((BreadCrumbCount - 1) + "BreadCrumb").offsetWidth) / (BreadCrumbCount - 2);
         for (i = 1; i < BreadCrumbCount - 1; i++) {
             document.getElementById(i + "BreadCrumb").style.width = (width - 41) + "px";
             document.getElementById(i + "BreadCrumbContainer").style.width = width + "px";
             $('#' + i + 'BreadCrumbContainer').attr('title', "Zur Themenseite " + document.getElementById(i + "BreadCrumb").innerText)
                 .tooltip('fixTitle')
         }
         break;
        case 2:
            if (BreadCrumbCount < 3) {
                break;
            }
            var i;
            var width = ((document.getElementById("BreadcrumbContainer").offsetWidth)- document.getElementById("BreadcrumbHome").offsetWidth) / BreadCrumbCount;
            for (i = 1; i < BreadCrumbCount; i++) {
                document.getElementById(i + "BreadCrumb").style.width = (width - 41) + "px";
                document.getElementById(i + "BreadCrumbContainer").style.width = width + "px";
                $('#' + i + 'BreadCrumbContainer').attr('title', "Zur Themenseite " + document.getElementById(i + "BreadCrumb").innerText)
                    .tooltip('fixTitle')
            }
            break;
        case 3:
            var i;
            var width = ((document.getElementById("BreadcrumbContainer").offsetWidth) - document.getElementById("BreadcrumbHome").offsetWidth) / BreadCrumbCount;
            document.getElementById("LastBreadcrumb").style.width = (width - 41) + "px";

            if (BreadCrumbCount > 2) {
                for (i = 1; i < BreadCrumbCount; i++) {
                    document.getElementById(i + "BreadCrumb").style.width = (width - 41) + "px";
                    document.getElementById(i + "BreadCrumbContainer").style.width = width + "px";
                    $('#' + i + 'BreadCrumbContainer').attr('title', "Zur Themenseite " + document.getElementById(i + "BreadCrumb").innerText)
                        .tooltip('fixTitle')
                }
            }
    }
}

function getCount(parent) {
    var relevantChildren = 0;
    var children = parent.childNodes.length;
    for (var i = 0; i < children; i++) {
        if (parent.childNodes[i].nodeType != 3) {
            relevantChildren++;
        }
    }
    return relevantChildren;
}