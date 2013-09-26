function SubmitSearch() {
    window.location.href = $('#txtSearch').attr("formUrl") + $('#txtSearch').val();
}
$(function () {
    $('#btnSearch').click(function () {
        SubmitSearch();
    });
    $(function () {
        $("#txtSearch").keypress(function (e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            if(code === 13) {
                SubmitSearch();
                e.preventDefault();
            }
        });
    });
    new ValuationPerRow(".column-2");
});
