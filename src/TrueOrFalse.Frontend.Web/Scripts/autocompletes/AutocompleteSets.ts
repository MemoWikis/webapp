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

                if (self.GetAlreadyAddedSets(elemContainer, ui.item.Id).length > 0) {
                    var alreadyAddedSets = self.GetAlreadyAddedSets(elemContainer, ui.item.Id);

                    for (var i = 0; i < 5; i++) {
                        alreadyAddedSets.closest(".added-set").effect('bounce', null, 'fast');
                    }

                    return false;
                }

                $(inputSelector).data("set-id", ui.item.Id);
                $(inputSelector).val(ui.item.Name);

                self.AddSet();

                return false;
            },
        });

        autocomplete.data("ui-autocomplete")._renderItem = <any>function (ul, item: SetItem): any {

            var html = "";

            if (item.NoResults) {
                html =
                    "<div class='SetListItem'>" +
                        "Keine Treffer. Bitte weitertippen <br> oder anderen Suchbegriff verwenden." +
                    "</div>";
            } else {
                html =
                    "<div class='SetListItem'>" +
                        "<img src=" + item.ImageUrl + "/>" +
                        "<div class='SetDescription'>" +
                            "<span class='set-name'>" + item.Name + "</span>" +
                            "<span class='NumberQuestions'>(" + item.NumberOfQuestions + " Fragen)</span>" +
                        "</div>" +
                    "</div>";
            }

            return $("<li></li>")
                .data("ui-autocomplete-item", item)
                .append(html)
                .appendTo(ul);            
        }

        $(inputSelector).unbind("initSetFromTxt");
        $(inputSelector).bind("initSetFromTxt", () => { this.AddSet(); });
    }

    AddSet() {

        var setIdx = this._nextSetIdx.toString();
        this._nextSetIdx++;
        var setText = this._elemInput.val();
        var setId = this._elemInput.data('set-id');

        this._elemInput.closest(".JS-SetInputContainer").before(
            "<div class='added-set' id='set-" + setIdx + "' style='display: none; float: left; margin-right: 9px;'>" +
                "<a href='/Fragesaetze/ById/" + setId + "'>" +
                    "<span class='label label-set show-tooltip' data-placement='top' data-original-title='Gehe zum Fragesatz'>" + setText + "</span>" +
                "</a>" +
                "<input type='hidden' value='" + setId + "' name='set-" + setIdx + "'/>" +
                "<a href='#' id='delete-set-" + setIdx +"' style='margin-left: 4px;'><img alt='' src='/Images/Buttons/cross.png' /></a>" +
            "</div> ");

        this._elemInput.val('');
        this._elemInput.data('set-id', '');
        $("#delete-set-" + setIdx).click(function (e) {
            e.preventDefault(); 

            $("#set-" + setIdx).stop(true).animate({ opacity: 0 }, 250, function () {
                $(this).hide("blind", { direction: "horizontal" }, function () {
                    $(this).remove();
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