/// <reference path="../../../Scripts/ValuationPerRow.ts" />
/// <reference path="../../../Scripts/typescript.defs/jquery.d.ts" />

function SubmitSearch() {
    window.location.href = $('#txtSearch').attr("formUrl") + $('#txtSearch').val();
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

    new ValuationPerRow(".column-3", ValuationPerRowMode.Set);
});