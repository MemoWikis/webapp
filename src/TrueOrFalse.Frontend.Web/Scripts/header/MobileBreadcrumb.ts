window.onload = function (event) {
    var BreadCrumbContainerCount = getCount(document.getElementById('BreadCrumbContainer'));
    var BreadCrumbItemCount = BreadCrumbContainerCount - 3;

    if (BreadCrumbItemCount > 4) {
        for (var i = 3; i < (BreadCrumbItemCount - 1); i++) {
            $('#' + i + 'BreadCrumbContainer').attr('title', "Zur Themenseite " + document.getElementById(i + "BreadCrumb").innerText)
                .tooltip('fixTitle')
            document.getElementById(i + 'BreadCrumb').innerHTML = "...";
        }
    }
    if (BreadCrumbItemCount == 0) {
        $('#Breadcrumb').css('height', '55px');
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