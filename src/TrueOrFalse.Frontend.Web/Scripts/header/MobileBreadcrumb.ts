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
            if (BreadCrumbCount < 4) {
                var element = document.getElementById("LastBreadcrumb");
                var lenght = element.textContent.length;
                if (lenght > 15) {
                    var text = element.textContent.substring(0, 15);
                    element.innerHTML = text + "..";
                }
            } else {
              var element = document.getElementById(BreadCrumbCount - 2 + "BreadCrumb");
              var lenght = element.textContent.length;
              if (lenght > 15) {
               var text = element.textContent.substring(0, 15);
               element.innerHTML = text + "..";
              }
            }
            break;
        case 2:
            if (BreadCrumbCount < 4) {
                var element = document.getElementById(BreadCrumbCount - 1 + "BreadCrumb");
                var lenght = element.textContent.length;
                if (lenght > 15) {
                    var text = element.textContent.substring(0, 15);
                    element.innerHTML = text + "..";
                }
            }
            else {
                var element = document.getElementById(BreadCrumbCount - 3 + "BreadCrumb");
                var lenght = element.textContent.length;
                if (lenght > 15) {
                    var text = element.textContent.substring(0, 15);
                    element.innerHTML = text + "..";
                }

            }
            break;
        case 3:

          
            if (BreadCrumbCount < 4) {
                var element = document.getElementById(BreadCrumbCount - 2 + "BreadCrumb");
                var lenght = element.textContent.length;
                if (lenght > 15) {
                    var text = element.textContent.substring(0, 15);
                    element.innerHTML = text + "..";
                }
            } else {
                var element = document.getElementById("LastBreadcrumb");
                var lenght = element.textContent.length;
                if (lenght > 15) {
                    var text = element.textContent.substring(0, 15);
                    element.innerHTML = text + "..";
                }
            }
            break;
        case 4:
            if (BreadCrumbCount < 4) {
                var element = document.getElementById(BreadCrumbCount - 3 + "BreadCrumb");
                var lenght = element.textContent.length;
                if (lenght > 15) {
                    var text = element.textContent.substring(0, 15);
                    element.innerHTML = text + "..";
                }
            }
            else {
                var element = document.getElementById(BreadCrumbCount - 1 + "BreadCrumb");
                var lenght = element.textContent.length;
                if (lenght > 15) {
                    var text = element.textContent.substring(0, 15);
                    element.innerHTML = text + "..";
                }
            }
            break;
        case 5:
            for (let i = 1; i++; i < (BreadCrumbCount - 3) ) {
                let element = document.getElementById(i + "BreadCrumb");
                let text = element.textContent.substring(0, 15);
                let lenght = element.textContent.length;
                if (lenght > 15) {
                    element.innerHTML = text + "..";
                }
            }
            break;
        case 6:
            if (BreadCrumbCount > 4) {
                document.getElementById(4 + "BreadCrumb").innerHTML = "..";
            }
            break;
        case 7:
            if (BreadCrumbCount > 5) {
                for (var i = 5; i++; i < (BreadCrumbCount - 3)) {
                 document.getElementById(i + "BreadCrumb").innerHTML = "..";
                }
            }
            break;
        case 8:
            if (BreadCrumbCount > 3) {
                document.getElementById(BreadCrumbCount - 3 + "BreadCrumb").innerHTML = "..";
            }
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