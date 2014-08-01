class QuestionsSearch {

    _elemContainer: JQuery;

    constructor() {
        this._elemContainer = $("#JS-SearchResult");
        var _self = this;

        $('#btnSearch').click((e) => {
            e.preventDefault();
            this.SubmitSearch();
        } );

        $("#txtSearch").keypress(function (e) {
            if (e.keyCode === 13) {
                e.preventDefault();
                _self.SubmitSearch();
            }
        });
    }

    SubmitSearch() {
        //window.location.href = $('#txtSearch').attr("formUrl") +
        //$('#txtSearch').val()
        //    .replace("Ersteller:\"", "Ersteller__")
        //    .replace("\"", "__")
        //    .replace("'", "__")
        //    .replace(":", "___")
        //    .replace("&", "_and_");

        this._elemContainer.html(
            "<div style='text-align:center; padding-top: 30px;'>" +
                "<i class='fa fa-spinner fa-spin'></i>" +
            "</div>");

        $.post($('#txtSearch').attr("formUrl") + "Api", { searchTerm: $('#txtSearch').val() },
            (data) => { this._elemContainer.html(data.Html); }
        );

    }
}

$(function() {
    new QuestionsSearch();
})