/// <reference path="../../../Scripts/typescript.defs/jquery.d.ts" />

function SubmitSearch() {
    window.location.href = "/Nutzer/Suche/" + $('#txtSearch').val();
}

$(function () {
    $('#btnSearch').click(function () { SubmitSearch(); });

    $(function () {
        $("#txtSearch").keypress(function (e: any) {
            var code = (e.keyCode ? e.keyCode : e.which);
            if (code === 13) {
                SubmitSearch();
                e.preventDefault();
            }
        });
    });
});