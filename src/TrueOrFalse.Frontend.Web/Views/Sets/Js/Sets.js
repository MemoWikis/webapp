/// <reference path="../../../Scripts/typescript.defs/jquery.d.ts" />
function SubmitSearchSets() {
    window.location.href = $('#txtSearch').attr("formUrl") + $('#txtSearch').val();
}

$(function () {
    $('#btnSearch').click(function () {
        SubmitSearchSets();
    });

    $(function () {
        $("#txtSearch").keypress(function (e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            if (code === 13) {
                SubmitSearchSets();
                e.preventDefault();
            }
        });
    });
});
//# sourceMappingURL=Sets.js.map
