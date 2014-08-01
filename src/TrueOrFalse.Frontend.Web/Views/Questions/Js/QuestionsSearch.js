var QuestionsSearch = (function () {
    function QuestionsSearch() {
        var _this = this;
        this._elemContainer = $("#JS-SearchResult");
        var _self = this;

        $('#btnSearch').click(function (e) {
            e.preventDefault();
            _this.SubmitSearch();
        });

        $("#txtSearch").keypress(function (e) {
            if (e.keyCode === 13) {
                e.preventDefault();
                _self.SubmitSearch();
            }
        });
    }
    QuestionsSearch.prototype.SubmitSearch = function () {
        //window.location.href = $('#txtSearch').attr("formUrl") +
        //$('#txtSearch').val()
        //    .replace("Ersteller:\"", "Ersteller__")
        //    .replace("\"", "__")
        //    .replace("'", "__")
        //    .replace(":", "___")
        //    .replace("&", "_and_");
        var _this = this;
        this._elemContainer.html("<div style='text-align:center; padding-top: 30px;'>" + "<i class='fa fa-spinner fa-spin'></i>" + "</div>");

        $.post($('#txtSearch').attr("formUrl") + "Api", { searchTerm: $('#txtSearch').val() }, function (data) {
            _this._elemContainer.html(data.Html);
        });
    };
    return QuestionsSearch;
})();

$(function () {
    new QuestionsSearch();
});
//# sourceMappingURL=QuestionsSearch.js.map
