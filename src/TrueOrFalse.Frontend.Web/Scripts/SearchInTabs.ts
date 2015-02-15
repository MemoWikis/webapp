﻿class SearchInTabs{

    _elemContainer: JQuery;
    _categories: Array<number> = [];

    constructor() {
        this._elemContainer = $("#JS-SearchResult");

        $('#btnSearch').click((e) => {
            e.preventDefault();
            this.SubmitSearch();
        });

        $("#txtSearch").typeWatch({
            callback: value => { this.SubmitSearch(); },
            wait: 500,
            highlight: true,
            captureLength: 0
        });

        $("#txtSearch").keypress(e => {
            if (e.keyCode === 13) {
                e.preventDefault();
            }
        });
    }

    SubmitSearch() {

        this._elemContainer.html(
            "<div style='text-align:center; padding-top: 30px;'>" +
            "<i class='fa fa-spinner fa-spin'></i>" +
            "</div>");

        $.ajax({
            url: $('#txtSearch').attr("formUrl") + "Api",
            data: {
                searchTerm: $('#txtSearch').val(),
                categories: this._categories
            },
            traditional: true,
            type: 'json',
            success: (data: SearchInTabsResult) => {
                this._elemContainer.html(data.Html);

                var tabAmount = data.TotalInResult.toString() + " von " + data.TotalInSystem.toString();
                if (data.TotalInResult === data.TotalInSystem) {
                    tabAmount = data.TotalInSystem.toString();
                }

                Utils.SetElementValue("#resultCount", data.TotalInResult.toString() + " Fragen");
                Utils.SetElementValue2($(".JS-Tabs")
                    .find(".JS-" + data.Tab)
                    .find("span.JS-Amount"), tabAmount);

                var page = new Page();
                page.Init();

                $('.show-tooltip').tooltip();
                Images.Init();
            }
        });
    }
}