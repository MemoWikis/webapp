interface SearchResultItem {
    ResultCount: number;
    Type: string;
    Item: any;

    NoResults: boolean;
    SearchUrl: string;
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

                switch (resultItem.Type) {

                    case "CategoriesHeader":
                        html = "<div class='SearchResultHeader'>Themen <div class='ResultCount'><a href='" + resultItem.Item.SearchUrl + "'>zeige " + resultItem.ResultCount + " Treffer</a></div></div>";
                        break;

                    case "Categories":
                        var item = <SetItem>resultItem.Item;

                        html =
                            "<div class='SearchResultItem'>" +
                                "<a href='" + item.ItemUrl + "'>" +
                                    "<img src=" + item.ImageUrl + "/>" +
                                    item.Name +
                                "</a>" +
                            "</div>";
                        break;

                    case "SetsHeader":
                        html = "<div class='SearchResultHeader'>Fragesätze <div class='ResultCount'><a href='" + resultItem.Item.SearchUrl + "'>zeige " + resultItem.ResultCount + " Treffer</a></div></div>";
                        break;

                    case "Sets":
                        item = <SetItem>resultItem.Item;

                        html =
                            "<div class='SearchResultItem'>" +
                                "<a href='" + item.ItemUrl + "'>" +
                                    "<img src=" + item.ImageUrl + "/>" +
                                    item.Name +
                                "</a>" +
                            "</div>";
                        break;

                    case "QuestionsHeader":
                        html = "<div class='SearchResultHeader'>Fragen <div class='ResultCount'><a href='" + resultItem.Item.SearchUrl + "'>zeige " + resultItem.ResultCount + " Treffer</a></div></div>";
                        break;

                    case "Questions":
                        item = <SetItem>resultItem.Item;

                        html =
                            "<div class='SearchResultItem'>" +
                                "<a href='" + item.ItemUrl + "'>" +
                                    "<img src=" + item.ImageUrl + "/>" +
                                    item.Name +
                                "</a>" +
                            "</div>";
                        break;

                    case "UsersHeader":
                        html = "<div class='SearchResultHeader'>Nutzer <div class='ResultCount'><a href='" + resultItem.Item.SearchUrl + "'>zeige " + resultItem.ResultCount + " Treffer</a></div></div>";
                        break;

                    case "Users":
                        item = <SetItem>resultItem.Item;

                        html =
                            "<div class='SearchResultItem'>" +
                                "<a href='" + item.ItemUrl + "'>" +
                                    "<img src=" + item.ImageUrl + "/>" +
                                    item.Name +
                                "</a>" +
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