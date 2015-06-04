var AutocompleteSets = (function () {
    function AutocompleteSets(inputSelector) {
        var elemContainer = this._elemInput.closest(".JS-RelatedCategories");

        var autocomplete = $(inputSelector).autocomplete({
            minLength: 0,
            source: function (request, response) {
                $.get("/Api/Sets/ByName?term=" + request.term, function (data) {
                    response(data);
                });
            }
        });
    }
    return AutocompleteSets;
})();
//# sourceMappingURL=AutocompleteSets.js.map
