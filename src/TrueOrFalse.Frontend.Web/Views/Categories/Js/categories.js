/// <reference path="../../../Scripts/typescript.defs/jquery.d.ts" />
function SubmitSearch() {
    window.location.href = "/Kategorien/Suche/" + $('#txtSearch').val();
}

$(function () {
    $('#btnSearch').click(function () {
        SubmitSearch();
    });

    $(function () {
        $("#txtSearch").keypress(function (e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            if (code === 13) {
                SubmitSearch();
                e.preventDefault();
            }
        });
    });
});
//# sourceMappingURL=categories.js.map
