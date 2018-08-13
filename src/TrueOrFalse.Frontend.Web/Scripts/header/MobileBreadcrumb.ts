window.onload = function (event) {
    var i;
    var BreadCrumbCount = getCount(document.getElementById("BreadcrumbContainer")) - 3;

    if (BreadCrumbCount > 1) {

        for (i = 1; i < BreadCrumbCount; i++) {
            var BreadCrumbItem = $('#' + i + 'BreadCrumb')
            BreadCrumbItem.css({ width: "auto" }); 
            BreadCrumbItem.css('max-width', (BreadCrumbItem.outerWidth() + 1));
            $('#' + i + 'BreadCrumbContainer').css('max-width', (BreadCrumbItem.outerWidth() + 42));
            BreadCrumbItem.css({width: ""})
        }
        
        var LastBreadCrumbItem = $('#LastBreadCrumb');
        LastBreadCrumbItem.css({ width: "auto" });
        LastBreadCrumbItem.css('max-width', (LastBreadCrumbItem.outerWidth() + 1));
        $('#LastBreadCrumbContainer').css('max-width', (LastBreadCrumbItem.outerWidth() + 42));
        LastBreadCrumbItem.css({width: ""})
    }
    ResizeBreadcrumb();
    ReorientateMenu();
}

window.onresize = function (event) {
    ResizeBreadcrumb();
    ReorientateMenu();
};

function ResizeBreadcrumb() {
    var BreadCrumbCount = getCount(document.getElementById("BreadcrumbContainer")) - 3;
    var position = $(this).scrollTop();

    if (BreadCrumbCount > 2) {
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

        if (IsToLong) {
            ShortenBreadcrumb(1);
        }
    }
}

function ShortenBreadcrumb(index) {
    var BreadCrumbCount = getCount(document.getElementById("BreadcrumbContainer")) - 3;
    var position = $(this).scrollTop();

    switch (index) {
        case 1:
            var i;
            if (BreadCrumbCount < 3) {
                ShortenBreadcrumb(3);
            }

            if (position > 80) {
               var width = ((document.getElementById("BreadCrumbContainer").offsetWidth) - document.getElementById("LastBreadCrumb").offsetWidth - document.getElementById("BreadcrumbHome").offsetWidth - document.getElementById("BreadcrumbLogoSmall").offsetWidth - document.getElementById("StickyHeaderContainer").offsetWidth - document.getElementById((BreadCrumbCount - 1) + "BreadCrumb").offsetWidth) / (BreadCrumbCount - 2);
            } else {
                var width = ((document.getElementById("BreadCrumbContainer").offsetWidth) - document.getElementById("LastBreadCrumb").offsetWidth - document.getElementById("BreadcrumbHome").offsetWidth - document.getElementById((BreadCrumbCount - 1) + "BreadCrumb").offsetWidth) / (BreadCrumbCount - 2);
            }

            for (i = 1; i < BreadCrumbCount; i++) {
               document.getElementById(i + "BreadCrumb").style.width = (width - 41) + "px";
               document.getElementById(i + "BreadCrumbContainer").style.width = width + "px";
               $('#' + i + 'BreadCrumbContainer').attr('title', "Zur Themenseite " + document.getElementById(i + "BreadCrumb").innerText)
                   .tooltip('fixTitle')
            }

            if (IsToLong) {
                ShortenBreadcrumb(2);
            }
         break;
        case 2:
            var i;
            if (position > 80) {
                var width = ((document.getElementById("BreadCrumbContainer").offsetWidth) - document.getElementById("LastBreadCrumb").offsetWidth - document.getElementById("BreadcrumbHome").offsetWidth - document.getElementById("BreadcrumbLogoSmall").offsetWidth - document.getElementById("StickyHeaderContainer").offsetWidth) / (BreadCrumbCount -1);
            } else {
                var width = ((document.getElementById("BreadCrumbContainer").offsetWidth) - document.getElementById("LastBreadCrumb").offsetWidth - document.getElementById("BreadcrumbHome").offsetWidth) / (BreadCrumbCount -1);
            }

            for (i = 1; i < BreadCrumbCount; i++) {
                document.getElementById(i + "BreadCrumb").style.width = (width - 41) + "px";
                document.getElementById(i + "BreadCrumbContainer").style.width = width + "px";
                $('#' + i + 'BreadCrumbContainer').attr('title', "Zur Themenseite " + document.getElementById(i + "BreadCrumb").innerText)
                    .tooltip('fixTitle')
            }

            if (IsToLong) {
                ShortenBreadcrumb(3);
            }
            break;
        case 3:
            var i;
            if (position > 80) {
                var width = ((document.getElementById("BreadCrumbContainer").offsetWidth) - document.getElementById("BreadcrumbHome").offsetWidth - document.getElementById("BreadcrumbLogoSmall").offsetWidth - document.getElementById("StickyHeaderContainer").offsetWidth) / BreadCrumbCount;
            } else {
                var width = ((document.getElementById("BreadCrumbContainer").offsetWidth) - document.getElementById("BreadcrumbHome").offsetWidth) / BreadCrumbCount;
            }
            for (i = 1; i < BreadCrumbCount; i++) {
                    document.getElementById(i + "BreadCrumb").style.width = (width - 41) + "px";
                    document.getElementById(i + "BreadCrumbContainer").style.width = width + "px";
                    $('#' + i + 'BreadCrumbContainer').attr('title', "Zur Themenseite " + document.getElementById(i + "BreadCrumb").innerText)
                        .tooltip('fixTitle')
            }
            document.getElementById("LastBreadCrumb").style.width = (width - 41) + "px";
            document.getElementById("LastBreadCrumbContainer").style.width = width + "px";
            $('#LastBreadCrumbContainer').attr('title', document.getElementById("LastBreadCrumb").innerText)
                .tooltip('fixTitle')
            
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

function IsToLong() {
    var rightspace = ($(window).width() - ($('#LastBreadCrumbContainer').offset().left + $('#LastBreadCrumbContainer').outerWidth()));
    if (rightspace < 0) {
        return true;
    } else {
        return false;
    }
}