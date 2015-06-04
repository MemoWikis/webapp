class AutocompleteSets {
    OnAdd: (setId?: number) => void;
    OnRemove: (setId?: number) => void;

    _elemInput: JQuery;

    constructor(inputSelector: string) {

        var elemContainer = this._elemInput.closest(".JS-RelatedCategories");

        var autocomplete = $(inputSelector).autocomplete({
            minLength: 0,
            source: function(request, response) {
                $.get("/Api/Sets/ByName?term=" + request.term, function (data) {
                    response(data);
                });                
            }


        });

    }
 }