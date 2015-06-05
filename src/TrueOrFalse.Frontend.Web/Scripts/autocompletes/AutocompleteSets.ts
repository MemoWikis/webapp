interface SetItem {
    Id: number;
    Name : string;
    NumberOfQuestions : number;
    ImageUrl: string;

    NoResults : boolean;
}

class AutocompleteSets {
    OnAdd: (setId?: number) => void;
    OnRemove: (setId?: number) => void;

    _elemInput: JQuery;
    _nextSetIdx = 1;

    constructor(inputSelector: string) {

        var self = this;
        this._elemInput = $(inputSelector);
        if (this._elemInput.length == 0)
            return; 

        var elemContainer = this._elemInput.closest(".JS-Sets");

        var autocomplete = $(inputSelector).autocomplete({
            minLength: 0,
            source: function(request, response) {
                $.get("/Api/Sets/ByName?term=" + request.term, function (data) {
                    if (data.Items.length == 0)
                        response([{NoResults : true}]);
                    else
                        response(data.Items);
                });                
            },
            select: function (event, ui) {

                if (self.GetAlreadyAddedSets(elemContainer, ui.item.id).length > 0) {
                    var alreadyAddedCategories = self.GetAlreadyAddedSets(elemContainer, ui.item.id);

                    function bounce() { alreadyAddedCategories.closest(".added-cat").effect('bounce', null, 'fast'); }
                    bounce();
                    bounce();
                    bounce();
                    bounce();

                    return false;
                }

                $(inputSelector).data("set-id", ui.item.id);
                $(inputSelector).val(ui.item.Name);

                self.AddSet();

                return false;
            },
        });

        autocomplete.data("ui-autocomplete")._renderItem = function (ul, item: SetItem): any {

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

    AddSet() {

        var setIdx = this._nextSetIdx.toString();
        this._nextSetIdx++;
        var setText = this._elemInput.val();
        var setId = this._elemInput.data('set-id');

        this._elemInput.closest(".JS-SetInputContainer").before(
            "<div class='added-set' id='set-" + setIdx + "' style='display: none;'>" +
                "<a href='/Fragesaetze/ById?id=" + setId + "'>" + setText + "</a>" +
                "<input type='hidden' value='" + setId + "' name='set-" + setIdx + "'/>" +
                "<a href='#' id='delete-set-" + setIdx + "'><img alt='' src='/Images/Buttons/cross.png' /></a>" +
            "</div> ");

        this._elemInput.val('');
        this._elemInput.data('set-id', '');
        $("#delete-set-" + setIdx).click(function (e) {
            e.preventDefault();

            //animating = true;
            $("#set-" + setIdx).stop(true).animate({ opacity: 0 }, 250, function () {
                $(this).hide("blind", { direction: "horizontal" }, function () {
                    $(this).remove();
                    //animating = false;
                });
            });

        });
        $("#set-" + setIdx).show("blind", { direction: "horizontal" }, null, function () {
            //Workaround for jquery ui effect wrapper width rounding error
            $("#set-" + setIdx).css('min-width', parseInt($("#set-" + setIdx).css('width')) + 1 + 'px');
        });
    }

    GetAlreadyAddedSets(container: JQuery, setId: number) : JQuery {
        return container.find(".added-set input[value='" + setId + "']");        
    }
}