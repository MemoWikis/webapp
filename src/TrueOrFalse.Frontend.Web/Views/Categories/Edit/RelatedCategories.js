var ui;

var AutoCompleteFilterType;
(function (AutoCompleteFilterType) {
    AutoCompleteFilterType[AutoCompleteFilterType["None"] = 0] = "None";
    AutoCompleteFilterType[AutoCompleteFilterType["Daily"] = 1] = "Daily";
    AutoCompleteFilterType[AutoCompleteFilterType["DailyIssue"] = 2] = "DailyIssue";
    AutoCompleteFilterType[AutoCompleteFilterType["Magazine"] = 3] = "Magazine";
    AutoCompleteFilterType[AutoCompleteFilterType["MagazineIssue"] = 4] = "MagazineIssue";
})(AutoCompleteFilterType || (AutoCompleteFilterType = {}));

var AutocompleteCategories = (function () {
    function AutocompleteCategories(inputSelector, isSingleSelect, filterType, selectorParentName) {
        if (typeof isSingleSelect === "undefined") { isSingleSelect = false; }
        if (typeof filterType === "undefined") { filterType = 0 /* None */; }
        if (typeof selectorParentName === "undefined") { selectorParentName = ""; }
        this._filterType = filterType;

        var self = this;
        this._isSingleSelect = isSingleSelect;

        var elemInput = $(inputSelector);
        var elemContainer = elemInput.closest(".JS-RelatedCategories");

        var isCategoryEdit = $("#isCategoryEdit").length == 1;
        var categoryName = "";
        if (isCategoryEdit)
            categoryName = $("#Name").val();

        var nextCatIdx = 1;
        function addCat() {
            var catIdx = nextCatIdx.toString();
            nextCatIdx++;
            var catText = $(inputSelector).val();
            var catId = $(inputSelector).data('category-id');

            if (self.OnAdd != null)
                self.OnAdd();

            if (self._isSingleSelect) {
                catIdx = inputSelector.substring(1);
                elemInput.closest(".JS-CatInputContainer").before("<div class='added-cat SingleSelect' id='cat-" + catIdx + "' style='display: none;'>" + "<a href='/Kategorien/ById?id=" + catId + "'>" + catText + "</a>" + "<input id='hdd" + catIdx + "' type='hidden' value='" + catId + "'name='" + "hdd" + catIdx + "'/> " + "<a href='#' id='delete-cat-" + catIdx + "'><i class='fa fa-pencil'></i></a>" + "</div> ");
                elemInput.attr("type", "hidden").hide();
                var validator = $("#EditCategoryForm").validate();
                validator.element(elemInput);
            } else {
                elemInput.closest(".JS-CatInputContainer").before("<div class='added-cat' id='cat-" + catIdx + "' style='display: none;'>" + "<a href='/Kategorien/ById?id=" + catId + "'>" + catText + "</a>" + "<input type='hidden' value='" + catId + "' name='cat-" + catIdx + "'/>" + "<a href='#' id='delete-cat-" + catIdx + "'><img alt='' src='/Images/Buttons/cross.png' /></a>" + "</div> ");
            }

            elemInput.val('');
            $(inputSelector).data('category-id', '');
            $("#delete-cat-" + catIdx).click(function (e) {
                e.preventDefault();
                if (self.OnRemove != null)
                    self.OnRemove();
                animating = true;
                $("#cat-" + catIdx).stop(true).animate({ opacity: 0 }, 250, function () {
                    $(this).hide("blind", { direction: "horizontal" }, function () {
                        $(this).remove();
                        animating = false;
                    });
                });

                if (self._isSingleSelect)
                    elemInput.attr("type", "").show();
            });
            $("#cat-" + catIdx).show("blind", { direction: "horizontal" });
        }

        var autocomplete = $(inputSelector).autocomplete({
            minLength: 0,
            source: function (request, response) {
                var params = "";
                if (self._filterType == 1 /* Daily */) {
                    params = "&type=Daily";
                }

                if (self._filterType == 2 /* DailyIssue */) {
                    params = "&type=DailyIssue&parentName=" + $("#hdd" + selectorParentName.substring(1)).val();
                }

                if (self._filterType == 3 /* Magazine */) {
                    params = "&type=Magazine";
                }

                if (self._filterType == 4 /* MagazineIssue */) {
                    params = "&type=MagazineIssue&parentName=" + $("#hdd" + selectorParentName.substring(1)).val();
                }

                $.get("/Api/Category/ByName?term=" + request.term + params, function (data) {
                    response(data);
                });
            },
            select: function (event, ui) {
                $(inputSelector).data("category-id", ui.item.id);
                $(inputSelector).val(ui.item.name);

                if (self.GetAlreadyAddedCategories(elemContainer, ui.item.id).length > 0) {
                    return false;
                }

                addCat();
                return false;
            }
        });

        autocomplete.data("ui-autocomplete")._renderItem = function (ul, item) {
            if (isCategoryEdit && categoryName == item.name)
                return "";

            return $("<li></li>").data("ui-autocomplete-item", item).append("<a class='CatListItem'>" + "<img src='" + item.imageUrl + "'/>" + "<div class='CatDescription'>" + "<span class='cat-name'>" + item.name + "</span>" + "<span class='NumberQuestions'>(" + item.numberOfQuestions + " Fragen)</span>" + "</div>" + "</a>").appendTo(ul);
        };

        var animating = false;
        function checkText() {
            var id = $(inputSelector).data('category-id');
            var alreadyAddedCategories = self.GetAlreadyAddedCategories(elemContainer, id);

            if (!animating && alreadyAddedCategories.length != 0) {
                animating = true;
                alreadyAddedCategories.closest(".added-cat").effect('bounce', null, 'fast', function () {
                    animating = false;
                });
            }
            setTimeout(checkText, 250);
        }
        checkText();

        var fnCheckTextAndAdd = function (event) {
            if (event.keyCode == 13) {
                event.preventDefault();

                if (ui != undefined) {
                    checkText();

                    if (self.GetAlreadyAddedCategories(elemContainer, ui.item.id).length == 0) {
                        addCat();
                    }
                }
            }
        };

        $(inputSelector).keypress(fnCheckTextAndAdd);
        $(inputSelector).bind("initCategoryFromTxt", addCat);
    }
    AutocompleteCategories.prototype.GetAlreadyAddedCategories = function (container, id) {
        return container.find(".added-cat input[value='" + id + "']");
    };
    return AutocompleteCategories;
})();

$(function () {
    new AutocompleteCategories("#txtNewRelatedCategory");
});
//# sourceMappingURL=RelatedCategories.js.map
