function SubmitSearch() {
    window.location.href = "/FrageSaetze/Suche/" + $('#txtSearch').val();
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
});
