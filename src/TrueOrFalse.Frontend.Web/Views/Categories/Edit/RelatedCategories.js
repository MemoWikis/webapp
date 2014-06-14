var ui;

function escape_regexp(text) {
    return text.replace(/[-[\]{}()*+?.,\\^$|#\s]/g, "\\$&");
}

$.expr[':'].textEquals = function (a, i, m) {
    return $(a).text().match(new RegExp("^" + escape_regexp(m[3]) + "$", "i")) != null;
};

var AutocompleteCategories = (function () {
    function AutocompleteCategories(inputSelector, isSingleSelect, singleSelectInputName) {
        if (typeof isSingleSelect === "undefined") { isSingleSelect = false; }
        if (typeof singleSelectInputName === "undefined") { singleSelectInputName = ""; }
        var self = this;
        this.isSingleSelect = isSingleSelect;

        var elemInput = $(inputSelector);
        var elemContainer = elemInput.closest(".JS-RelatedCategories");

        var isCategoryEdit = $("#isCategoryEdit").length == 1;
        var categoryName = "";
        if (isCategoryEdit)
            categoryName = $("#Name").val();

        var nextCatId = 1;
        function addCat() {
            var catId = nextCatId;
            nextCatId++;
            var catText = $(inputSelector).val();

            if (self.isSingleSelect) {
                catId = 999;
                elemInput.closest(".JS-CatInputContainer").before("<div class='added-cat' id='cat-" + catId + "' style='display: none;'>" + "<a href='/Kategorien/" + catText + "/" + catId + "'>" + catText + "</a>" + "<input type='hidden' value='" + catText + "' name='" + this.singleSelectInputName + "'/> " + "<a href='#' id='delete-cat-" + catId + "'><i class='fa fa-pencil'></i></a>" + "</div> ");
                elemInput.hide();
            } else {
                elemInput.closest(".JS-CatInputContainer").before("<div class='added-cat' id='cat-" + catId + "' style='display: none;'>" + "<a href='/Kategorien/" + catText + "/" + catId + "'>" + catText + "</a>" + "<input type='hidden' value='" + catText + "' name='cat-" + catId + "'/>" + "<a href='#' id='delete-cat-" + catId + "'><img alt='' src='/Images/Buttons/cross.png' /></a>" + "</div> ");
            }

            elemInput.val('');
            $("#delete-cat-" + catId).click(function (e) {
                e.preventDefault();
                animating = true;
                $("#cat-" + catId).stop(true).animate({ opacity: 0 }, 250, function () {
                    $(this).hide("blind", { direction: "horizontal" }, function () {
                        $(this).remove();
                        animating = false;
                    });
                });

                if (self.isSingleSelect)
                    elemInput.show();
            });
            $("#cat-" + catId).show("blind", { direction: "horizontal" });
        }

        $(inputSelector).autocomplete({
            minLength: 0,
            source: function (request, response) {
                var type = "";
                if (self.isSingleSelect) {
                    type = "&type=Daily";
                }

                $.get("/Api/Category/ByName?term=" + request.term + type, function (data) {
                    response(data);
                });
            },
            focus: function (event, ui) {
                $(inputSelector).data("category-id", ui.item.id);
                $(inputSelector).val(ui.item.name);
                return false;
            },
            select: function (event, ui) {
                $(inputSelector).data("category-id", ui.item.id);
                $(inputSelector).val(ui.item.name);

                if (elemContainer.find(".added-cat:textEquals('" + ui.item.name + "')").length > 0) {
                    return false;
                }

                addCat();
                return false;
            }
        }).data("ui-autocomplete")._renderItem = function (ul, item) {
            if (isCategoryEdit && categoryName == item.name)
                return "";

            return $("<li></li>").data("ui-autocomplete-item", item).append("<a class='CatListItem'><img src='" + item.imageUrl + "'/><div class='CatDescription'><span class='cat-name'>" + item.name + "</span><span class='NumberQuestions'>(" + item.numberOfQuestions + " Fragen)</span></div></a>").appendTo(ul);
        };

        var animating = false;
        function checkText() {
            var text = $(inputSelector).val();

            //var matchesInAutomcompleteList = elemContainer.find($(".ui-autocomplete li .cat-name:textEquals('" + text + "')"));
            var alreadyAddedCategory = elemContainer.find(".added-cat:textEquals('" + text + "')");

            //if (matchesInAutomcompleteList.length != 0 && alreadyAddedCategory.length == 0) {
            //    if ($(inputSelector).val() != matchesInAutomcompleteList.text()) {
            //        $(inputSelector).val(matchesInAutomcompleteList.text());
            //    }
            //}
            if (!animating && alreadyAddedCategory.length != 0) {
                animating = true;
                alreadyAddedCategory.effect('bounce', null, 'fast', function () {
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

                    if (elemContainer.find(".added-cat:textEquals('" + ui.item.name + "')").length == 0) {
                        addCat();
                    }
                }
            }
        };

        $(inputSelector).keypress(fnCheckTextAndAdd);
        $(inputSelector).bind("initCategoryFromTxt", addCat);
    }
    return AutocompleteCategories;
})();

$(function () {
    new AutocompleteCategories("#txtNewRelatedCategory");
});
//# sourceMappingURL=RelatedCategories.js.map
