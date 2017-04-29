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
            source: function (request, response) {
                $.get("/Api/Sets/ByName?term=" + request.term, function (data) {
                    if (data.Items.length == 0)
                        response([{ NoResults: true }]);
                    else
                        response(data.Items);
                });
            },
            select: function (event, ui) {

                self._elemInput.data("set-id", ui.item.Id);
                self._elemInput.val(ui.item.Name);

                return false;
            },
        });

        autocomplete.data("ui-autocomplete")._renderItem = <any>function (ul, item: SetItem): any {

            ul.addClass("AutocompleteHeader");

            var html = "";

            if (item.NoResults) {
                html =
                    "<div class='SetListItem'>" +
                    "Keine Treffer. Bitte weitertippen <br> oder anderen Suchbegriff verwenden." +
                    "</div>";
            } else {
                html =
                    "<div class='SetListItem'>" +
                    "<img src=" + item.ImageUrl + "/>"
                    + item.Name +
                    "</div>";
            }

            return $("<li></li>")
                .data("ui-autocomplete-item", item)
                .append(html)
                .appendTo(ul);
        }

    }
}

$(() => {
    new SearchBox();
});