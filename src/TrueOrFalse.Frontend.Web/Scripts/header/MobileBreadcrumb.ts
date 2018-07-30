window.onresize = function (event) {
var offset = $('#LastBreadcrumb').offset();
var rightspace = ($(window).width() - ($('#LastBreadcrumb').offset().left + $('#LastBreadcrumb').outerWidth()));
    var index = 0;
    while (rightspace < 0) {
        index++;
        if (index < 40) {
            ShortenBreadcrumb(index);
        } else {
            break;
        }
    }
};

function ShortenBreadcrumb(index) {
    var BreadCrumbCount = getCount(document.getElementById("BreadcrumbContainer")) - 3;
    switch (index) {
        case 1:
         var i;
         var width = ((document.getElementById("BreadcrumbContainer").offsetWidth) - document.getElementById("LastBreadcrumb").offsetWidth - document.getElementById("BreadcrumbHome").offsetWidth - document.getElementById((BreadCrumbCount - 1) + "BreadCrumb").offsetWidth) / (BreadCrumbCount -2);
         for (i = 1; i < BreadCrumbCount - 1; i++) {
             document.getElementById(i + "BreadCrumb").style.width = (width - 41) + "px";
             document.getElementById(i + "BreadCrumbContainer").style.width = width + "px";

         }
         break;
        case 2:
            var i;
            var width = ((document.getElementById("BreadcrumbContainer").offsetWidth)- document.getElementById("BreadcrumbHome").offsetWidth) / BreadCrumbCount;
            for (i = 1; i < BreadCrumbCount; i++) {
                document.getElementById(i + "BreadCrumb").style.width = (width - 41) + "px";
                document.getElementById(i + "BreadCrumbContainer").style.width = width + "px";
            }
            document.getElementById("LastBreadcrumb").style.width = (width -41) + "px";
            break;
       
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