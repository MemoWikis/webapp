interface QuestionSearchResult {
    Html : string;
    TotalInResult: number;
    TotalInSystem: number;
    Tab : string;
}

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

        this._elemContainer.html(
            "<div style='text-align:center; padding-top: 30px;'>" +
                "<i class='fa fa-spinner fa-spin'></i>" +
            "</div>");

        $.post($('#txtSearch').attr("formUrl") + "Api", { searchTerm: $('#txtSearch').val() },
            (data: QuestionSearchResult) => {
                this._elemContainer.html(data.Html);

                var tabAmount = data.TotalInResult.toString() + " von " + data.TotalInSystem.toString();
                if (data.TotalInResult == data.TotalInSystem) {
                    tabAmount = data.TotalInSystem.toString();
                }

                Utils.SetElementValue("#resultCount", data.TotalInResult.toString() + " Fragen");
                Utils.SetElementValue2($(".JS-Tabs")
                    .find(".JS-" + data.Tab)
                    .find("span.JS-Amount"), tabAmount);
                
            }
        );

    }
}

$(function() {
    new QuestionsSearch();
})