window.onload = function (event) {
    var i;
    var BreadCrumbCount = getCount(document.getElementById("BreadcrumbContainer")) - 3;

    if (BreadCrumbCount > 2) {

        for (i = 1; i < BreadCrumbCount; i++) {
            var BreadCrumbItem = $('#' + i + 'BreadCrumb')
            BreadCrumbItem.css({ width: "auto" }); 
            BreadCrumbItem.css('max-width', (BreadCrumbItem.outerWidth() + 1));
            $('#' + i + 'BreadCrumbContainer').css('max-width', (BreadCrumbItem.outerWidth() + 42));
            BreadCrumbItem.css({width: ""})
        }

        var LastBreadCrumbItem = $('#LastBreadcrumb');
        LastBreadCrumbItem.css({ width: "auto" });
        LastBreadCrumbItem.css('max-width', (LastBreadCrumbItem.outerWidth() + 1));
        $('#' + (BreadCrumbCount + 1) + 'BreadCrumbContainer').css('max-width', (BreadCrumbItem.outerWidth() + 42));
    }
    ResizeBreadcrumb();

}

window.onresize = function (event) {
    ResizeBreadcrumb();
};

function ResizeBreadcrumb() {
    var BreadCrumbCount = getCount(document.getElementById("BreadcrumbContainer")) - 3;
    if (BreadCrumbCount > 1) {
        var offset = $('#LastBreadcrumb').offset();
        var rightspace = ($(window).width() - ($('#LastBreadcrumb').offset().left + $('#LastBreadcrumb').outerWidth()));

        if ($('#1BreadCrumb').width() < 23.2) {
            document.getElementById("BreadcrumbHome").style.display = "none";
            document.getElementById("BreadcrumbLogoSmall").style.display = "block";
        }
        if ($('#1BreadCrumb').width() > 40) { 
                document.getElementById("BreadcrumbHome").style.display = "block";
                if (!(position > 80)) {
                    document.getElementById("BreadcrumbLogoSmall").style.display = "none";
                }
        }

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

        if (rightspace > 0) {
            var i;
            var position = $(this).scrollTop();
            if (position > 80) {
                var width = (document.getElementById("BreadcrumbContainer").offsetWidth - document.getElementById("BreadcrumbHome").offsetWidth - document.getElementById("BreadcrumbLogoSmall").offsetWidth - document.getElementById("StickyHeaderContainer").offsetWidth) / BreadCrumbCount;
            } else {
                var width = (document.getElementById("BreadcrumbContainer").offsetWidth - document.getElementById("BreadcrumbHome").offsetWidth) / BreadCrumbCount;
            }

            document.getElementById("LastBreadcrumb").style.width = (width - 41) + "px";
            if (BreadCrumbCount > 2) {
                for (i = 1; i < BreadCrumbCount; i++) {
                    document.getElementById(i + "BreadCrumb").style.width = (width - 41) + "px";
                    document.getElementById(i + "BreadCrumbContainer").style.width = width + "px";
                    $('#' + i + 'BreadCrumbContainer').attr('title', "Zur Themenseite")
                        .tooltip('fixTitle')
                }
            }

        }
    }
}

function ShortenBreadcrumb(index) {
    var BreadCrumbCount = getCount(document.getElementById("BreadcrumbContainer")) - 3;
    var position = $(this).scrollTop();

    switch (index) {
        case 1:
            if (BreadCrumbCount < 3) {
            break;
            }
            var i;
            if (position > 80) {
               var width = ((document.getElementById("BreadcrumbContainer").offsetWidth) - document.getElementById("LastBreadcrumb").offsetWidth - document.getElementById("BreadcrumbHome").offsetWidth - document.getElementById("BreadcrumbLogoSmall").offsetWidth - document.getElementById("StickyHeaderContainer").offsetWidth - document.getElementById((BreadCrumbCount - 1) + "BreadCrumb").offsetWidth) / (BreadCrumbCount - 2);
            } else {
               var width = ((document.getElementById("BreadcrumbContainer").offsetWidth) - document.getElementById("LastBreadcrumb").offsetWidth - document.getElementById("BreadcrumbHome").offsetWidth - document.getElementById((BreadCrumbCount - 1) + "BreadCrumb").offsetWidth) / (BreadCrumbCount - 2);
            }

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
            if (position > 80) {
                var width = ((document.getElementById("BreadcrumbContainer").offsetWidth) - document.getElementById("LastBreadcrumb").offsetWidth - document.getElementById("BreadcrumbHome").offsetWidth - document.getElementById("BreadcrumbLogoSmall").offsetWidth - document.getElementById("StickyHeaderContainer").offsetWidth) / (BreadCrumbCount -1);
            } else {
                var width = ((document.getElementById("BreadcrumbContainer").offsetWidth) - document.getElementById("LastBreadcrumb").offsetWidth - document.getElementById("BreadcrumbHome").offsetWidth) / (BreadCrumbCount -1);
            }

            for (i = 1; i < BreadCrumbCount; i++) {
                document.getElementById(i + "BreadCrumb").style.width = (width - 41) + "px";
                document.getElementById(i + "BreadCrumbContainer").style.width = width + "px";
                $('#' + i + 'BreadCrumbContainer').attr('title', "Zur Themenseite " + document.getElementById(i + "BreadCrumb").innerText)
                    .tooltip('fixTitle')
            }
            break;
        case 3:
            var i;
            if (position > 80) {
                var width = ((document.getElementById("BreadcrumbContainer").offsetWidth) - document.getElementById("BreadcrumbHome").offsetWidth - document.getElementById("BreadcrumbLogoSmall").offsetWidth - document.getElementById("StickyHeaderContainer").offsetWidth) / BreadCrumbCount;
            } else {
                var width = ((document.getElementById("BreadcrumbContainer").offsetWidth) - document.getElementById("BreadcrumbHome").offsetWidth) / BreadCrumbCount;
            }

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
