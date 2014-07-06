
var ui: any;

function escape_regexp(text) {
    return text.replace(/[-[\]{}()*+?.,\\^$|#\s]/g, "\\$&");
}

$.expr[':'].textEquals = function (a, i, m) {
    return $(a).text().match(new RegExp("^" + escape_regexp(m[3]) + "$", "i")) != null;
};

enum AutoCompleteFilterType {
    None,
    Daily, 
    DailyIssue,
    Magazine
}

class AutocompleteCategories {

    private _isSingleSelect: boolean;
    private _filterType: AutoCompleteFilterType;

    OnAdd: any;
    OnRemove: any;
    
    constructor(
        inputSelector: string,
        isSingleSelect: boolean = false,
        filterType: AutoCompleteFilterType = AutoCompleteFilterType.None,
        selectorParentName : string = "") {

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

            if (self.OnAdd != null)
                self.OnAdd();

            if (self._isSingleSelect) {
                catIdx = inputSelector.substring(1);
                elemInput.closest(".JS-CatInputContainer").before(
                    "<div class='added-cat' id='cat-" + catIdx + "' style='display: none;'>" +
                        "<a href='/Kategorien/ByName?name=" + encodeURIComponent(catText) + "'>" + catText + "</a>" +
                        "<input id='hdd" + catIdx + "' type='hidden' value='" + catText + "'name='" + "hdd" + catIdx + "'/> " +
                        "<a href='#' id='delete-cat-" + catIdx + "'><i class='fa fa-pencil'></i></a>" +
                    "</div> ");
                elemInput.attr("type", "hidden").hide();
                var validator = $("#EditCategoryForm").validate();
                validator.element(elemInput);
            } else {
                elemInput.closest(".JS-CatInputContainer").before(
                    "<div class='added-cat' id='cat-" + catIdx + "' style='display: none;'>" +
                    "<a href='/Kategorien/ByName?name=" + encodeURIComponent(catText) + "' >" + catText + "</a>" +
                    "<input type='hidden' value='" + catText + "' name='cat-" + catIdx + "'/>" +
                    "<a href='#' id='delete-cat-" + catIdx + "'><img alt='' src='/Images/Buttons/cross.png' /></a>" +
                    "</div> ");                
            }

            elemInput.val('');
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
            source: function(request, response) {

                var params = "";
                if (self._filterType == AutoCompleteFilterType.Daily) {
                    params = "&type=Daily";
                }

                if (self._filterType == AutoCompleteFilterType.DailyIssue) {
                    params = "&type=DailyIssue&parentName=" + $("#hdd" + selectorParentName.substring(1)).val();
                }

                if (self._filterType == AutoCompleteFilterType.Magazine) {
                    params = "&type=Magazine";
                }

                $.get("/Api/Category/ByName?term=" + request.term + params, function(data) {
                    response(data);
                });
            },
            //focus: function (event, ui) {
            //    $(inputSelector).data("category-id", ui.item.id);
            //    $(inputSelector).val(ui.item.name);
            //    return false;
            //},
            select: function(event, ui) {
                $(inputSelector).data("category-id", ui.item.id);
                $(inputSelector).val(ui.item.name);

                if (elemContainer.find(".added-cat:textEquals('" + ui.item.name + "')").length > 0) {
                    return false;
                }

                addCat();
                return false;
            }
        });

        autocomplete.data("ui-autocomplete")._renderItem = function (ul, item): any {
            if (isCategoryEdit && categoryName == item.name)
                return "";

            return $("<li></li>")
                .data("ui-autocomplete-item", item)
                .append("<a class='CatListItem'><img src='" + item.imageUrl + "'/><div class='CatDescription'><span class='cat-name'>"
                + item.name + "</span><span class='NumberQuestions'>(" + item.numberOfQuestions + " Fragen)</span></div></a>")
                .appendTo(ul);
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
                alreadyAddedCategory.effect('bounce', null, 'fast', function () { animating = false; });
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
        }

        $(inputSelector).keypress(fnCheckTextAndAdd);
        $(inputSelector).bind("initCategoryFromTxt", addCat);        

    }
}

$(function () {
    new AutocompleteCategories("#txtNewRelatedCategory");
});