interface SearchResultItem {
    Type: string;
    Item: any;

    NoResults: boolean;
}

class SearchBox
{
    _elemInput: JQuery;

    constructor() {

        var self = this;
        this._elemInput = $("#headerSearchBox");
        if (this._elemInput.length == 0)
            return;

        var autocomplete = this._elemInput.autocomplete({
            minLength: 0,
            source(request, response) {
                $.get("/Api/Search/ByName?term=" + request.term, data => {
                    if (data.Items.length == 0)
                        response([{ NoResults: true }]);
                    else
                        response(data.Items);
                });
            },
            select(event, ui) {

                self._elemInput.data("set-id", ui.item.Id);
                self._elemInput.val(ui.item.Name);

                return false;
            }
        });

        autocomplete.data("ui-autocomplete")._renderItem = <any>function (ul, resultItem: SearchResultItem): any {

            ul.addClass("AutocompleteHeader");

            var html = "";

            if (resultItem.NoResults) {
                html =
                    "<div class='SearchResultItem'>" +
                        "Keine Treffer. Bitte weitertippen <br> oder anderen Suchbegriff verwenden." +
                    "</div>";
            } else {

                console.log(resultItem);

                switch (resultItem.Type) {

                    case "CategoriesHeader":
                        html = "<div class='SearchResultItem'>Header Categories</div>";
                        break;

                    case "Categories":
                        var item = <SetItem>resultItem.Item;

                        html =
                            "<div class='SearchResultItem'>" +
                                "<img src=" + item.ImageUrl + "/>" +
                                item.Name +
                            "</div>";
                        break;

                    case "SetsHeader":
                        html = "<div class='SearchResultItem'>Header Sets</div>";
                        break;

                    case "Sets":
                        item = <SetItem>resultItem.Item;

                        html =
                            "<div class='SearchResultItem'>" +
                                "<img src=" + item.ImageUrl + "/>" +
                                item.Name +
                            "</div>";
                        break;

                    case "QuestionsHeader":
                        html = "<div class='SearchResultItem'>Header Questions</div>";
                        break;

                    case "Questions":
                        item = <SetItem>resultItem.Item;

                        html =
                            "<div class='SearchResultItem'>" +
                                "<img src=" + item.ImageUrl + "/>" +
                                item.Name +
                            "</div>";
                        break;

                    case "UsersHeader":
                        html = "<div class='SearchResultItem'>Header Users</div>";
                        break;

                    case "Users":
                        item = <SetItem>resultItem.Item;

                        html =
                            "<div class='SearchResultItem'>" +
                                "<img src=" + item.ImageUrl + "/>" +
                                item.Name +
                            "</div>";
                        break;
                }
            }

            return $("<li></li>")
                .data("ui-autocomplete-item", resultItem)
                .append(html)
                .appendTo(ul);
        }

    }
}

$(() => {
    new SearchBox();
});