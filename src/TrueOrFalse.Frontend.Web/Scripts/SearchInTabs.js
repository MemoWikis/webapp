var SearchInTabs = (function () {
    function SearchInTabs() {
        var _this = this;
        this._categories = [];
        this._elemContainer = $("#JS-SearchResult");

        $('#btnSearch').click(function (e) {
            e.preventDefault();
            _this.SubmitSearch();
        });

        $("#txtSearch").typeWatch({
            callback: function (value) {
                _this.SubmitSearch();
            },
            wait: 500,
            highlight: true,
            captureLength: 0
        });

        $("#txtSearch").keypress(function (e) {
            if (e.keyCode === 13) {
                e.preventDefault();
            }
        });
    }
    SearchInTabs.prototype.SubmitSearch = function () {
        var _this = this;
        this._elemContainer.html("<div style='text-align:center; padding-top: 30px;'>" + "<i class='fa fa-spinner fa-spin'></i>" + "</div>");

        $.ajax({
            url: $('#txtSearch').attr("formUrl") + "Api",
            data: {
                searchTerm: $('#txtSearch').val(),
                categories: this._categories
            },
            traditional: true,
            type: 'json',
            success: function (data) {
                _this._elemContainer.html(data.Html);

                var tabAmount = data.TotalInResult.toString() + " von " + data.TotalInSystem.toString();
                if (data.TotalInResult === data.TotalInSystem) {
                    tabAmount = data.TotalInSystem.toString();
                }

                Utils.SetElementValue("#resultCount", data.TotalInResult.toString() + " Fragen");
                Utils.SetElementValue2($(".JS-Tabs").find(".JS-" + data.Tab).find("span.JS-Amount"), tabAmount);

                var page = new Page();
                page.Init();

                $('.show-tooltip').tooltip();
                Images.Init();
            }
        });
    };
    return SearchInTabs;
})();
//# sourceMappingURL=SearchInTabs.js.map
