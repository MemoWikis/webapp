//window.onload = function (event) {
//    var BreadCrumbContainerCount = getCount(document.getElementById('BreadCrumbTrail'));
//    var BreadCrumbItemCount = BreadCrumbContainerCount - 1;

//    if (BreadCrumbItemCount > 4) {
//        for (var i = 3; i < (BreadCrumbItemCount - 1); i++) {
//            $('#' + i + 'BreadCrumbContainer').attr('title', "Zur Themenseite " + document.getElementById(i + "BreadCrumb").innerText)
//                .tooltip('fixTitle')
//            document.getElementById(i + 'BreadCrumb').innerHTML = "...";
//        }
//    }
//    if (BreadCrumbItemCount <= 1) {
//        $('#Breadcrumb').css('height', '55px');
//    }

//}

//function getCount(parent) {
//    var relevantChildren = 0;
//    var children = parent.childNodes.length;
//    for (var i = 0; i < children; i++) {
//        if (parent.childNodes[i].nodeType != 3) {
//            relevantChildren++;
//        }
//    }
//    return relevantChildren;
//}


class BreadrumbMobile {

    private _masterMainWrapperInnerWidth: number;
    private _breadCrumbTrailWidth: number;
    private _breadCrumbContainerCount: number;


    constructor() {
        this._masterMainWrapperInnerWidth = parseInt($("#MasterMainContent").css("width")) - 230;
        this._breadCrumbTrailWidth = parseInt($("#BreadCrumbTrail").css("width"));
        this._breadCrumbContainerCount = $('#BreadCrumbTrail').children().length - 3;
        var i = 1;
        var j = 1;
        if (this._breadCrumbTrailWidth > this._masterMainWrapperInnerWidth) {
            $('#BreadCrumbTrail > div').eq(1).attr('title',
                    "Zur Themenseite " + $(i + "BreadCrumb").text())
                .tooltip('fixTitle');
            $('#BreadCrumbTrail > div').eq(1).children("span").children("a").text("...")
            this._masterMainWrapperInnerWidth = parseInt($("#MasterMainContent").css("width")) - 230;
        }

        while (this._breadCrumbTrailWidth > this._masterMainWrapperInnerWidth && i <= this._breadCrumbContainerCount) {
            try {
                $('#' + i + 'BreadCrumbContainer').attr('title',
                    "Zur Themenseite " + $(i + "BreadCrumb").text())
                    .tooltip('fixTitle');
                document.getElementById(i + 'BreadCrumb').innerHTML = "...";
                this._breadCrumbTrailWidth = parseInt($("#BreadCrumbTrail").css("width")) - 230;
                i++;
            } catch (e) {
                console.log("incorrect counter Breadcrumb");
                break;
            }

        }
    }
}
