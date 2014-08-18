var ui;

var AutoCompleteFilterType;
(function (AutoCompleteFilterType) {
    AutoCompleteFilterType[AutoCompleteFilterType["None"] = 0] = "None";
    AutoCompleteFilterType[AutoCompleteFilterType["Book"] = 1] = "Book";
    AutoCompleteFilterType[AutoCompleteFilterType["Article"] = 2] = "Article";
    AutoCompleteFilterType[AutoCompleteFilterType["Daily"] = 3] = "Daily";
    AutoCompleteFilterType[AutoCompleteFilterType["DailyIssue"] = 4] = "DailyIssue";
    AutoCompleteFilterType[AutoCompleteFilterType["DailyArticle"] = 5] = "DailyArticle";
    AutoCompleteFilterType[AutoCompleteFilterType["Magazine"] = 6] = "Magazine";
    AutoCompleteFilterType[AutoCompleteFilterType["MagazineIssue"] = 7] = "MagazineIssue";
    AutoCompleteFilterType[AutoCompleteFilterType["VolumeChapter"] = 8] = "VolumeChapter";
    AutoCompleteFilterType[AutoCompleteFilterType["WebsiteArticle"] = 9] = "WebsiteArticle";
})(AutoCompleteFilterType || (AutoCompleteFilterType = {}));

var CompareType = (function () {
    function CompareType() {
    }
    CompareType.AreEqual = function (name, type) {
        if (name == "DailyIssue" && type == 4 /* DailyIssue */)
            return true;

        if (name == "MagazineIssue" && type == 7 /* MagazineIssue */)
            return true;

        if (name == "WebsiteArticle" && type == 9 /* WebsiteArticle */)
            return true;

        return false;
    };

    CompareType.IsReference = function (type) {
        if (type == "Book" || type == "Daily" || type == "DailyIssue" || type == "DailyArticle" || type == "Magazine" || type == "MagazineIssue" || type == "MagazineArticle" || type == "VolumeChapter" || type == "WebsiteArticle")
            return true;

        return false;
    };
    return CompareType;
})();

var AutocompleteCategories = (function () {
    function AutocompleteCategories(inputSelector, isSingleSelect, filterType, selectorParent, onSelect, isReference) {
        if (typeof isSingleSelect === "undefined") { isSingleSelect = false; }
        if (typeof filterType === "undefined") { filterType = 0 /* None */; }
        if (typeof selectorParent === "undefined") { selectorParent = ""; }
        if (typeof onSelect === "undefined") { onSelect = null; }
        if (typeof isReference === "undefined") { isReference = false; }
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
                self.OnAdd(catId);

            if (isReference == false) {
                if (self._isSingleSelect) {
                    catIdx = inputSelector.substring(1);
                    elemInput.closest(".JS-CatInputContainer").before("<div class='added-cat SingleSelect' id='cat-" + catIdx + "' style='display: none;'>" + "<a href='/Kategorien/ById?id=" + catId + "'>" + catText + "</a>" + "<input id='hdd" + catIdx + "' type='hidden' value='" + catId + "'name='" + "hdd" + catIdx + "'/> " + "<a href='#' id='delete-cat-" + catIdx + "'><i class='fa fa-pencil'></i></a>" + "</div> ");
                    elemInput.attr("type", "hidden").hide();

                    if ($("#EditCategoryForm").length > 0) {
                        var validator = $("#EditCategoryForm").validate();
                        validator.element(elemInput);
                    }
                } else {
                    elemInput.closest(".JS-CatInputContainer").before("<div class='added-cat' id='cat-" + catIdx + "' style='display: none;'>" + "<a href='/Kategorien/ById?id=" + catId + "'>" + catText + "</a>" + "<input type='hidden' value='" + catId + "' name='cat-" + catIdx + "'/>" + "<a href='#' id='delete-cat-" + catIdx + "'><img alt='' src='/Images/Buttons/cross.png' /></a>" + "</div> ");
                }

                elemInput.val('');
                $(inputSelector).data('category-id', '');
                $("#delete-cat-" + catIdx).click(function (e) {
                    e.preventDefault();
                    if (self.OnRemove != null)
                        self.OnRemove(catId);
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
            } else {
                $.ajax({
                    url: '/Fragen/Bearbeite/ReferencePartial?catId=' + catId,
                    type: 'GET',
                    success: function (data) {
                        elemInput.closest('.JS-ReferenceContainer').append(data).append("<div class='form-group'>" + "<label class='columnLabel control-label' for='ReferenceAddition-" + catId + "'>Ergänzungen zur Quelle</label>" + "<div class='columnControlsFull'>" + "<input class='InputRefAddition form-control' name='ReferenceAddition-" + catId + "' type='text' placeholder='Seitenangaben etc.'/>" + "</div>" + "</div>").append("<input type='hidden' value='" + catId + "' name='ref-" + catIdx + "'/>");
                        elemInput.closest('.JS-ReferenceSearch').remove();
                        $('.show-tooltip').tooltip();
                    }
                });
            }
        }

        var autocomplete = $(inputSelector).autocomplete({
            minLength: 0,
            source: function (request, response) {
                var params = "";
                if (self._filterType == 1 /* Book */) {
                    params = "&type=Book";
                }
                if (self._filterType == 2 /* Article */) {
                    params = "&type=Article";
                }
                if (self._filterType == 3 /* Daily */) {
                    params = "&type=Daily";
                }
                if (self._filterType == 4 /* DailyIssue */) {
                    params = "&type=DailyIssue&parentId=" + $("#hdd" + selectorParent.substring(1)).val();
                }
                if (self._filterType == 6 /* Magazine */) {
                    params = "&type=Magazine";
                }
                if (self._filterType == 7 /* MagazineIssue */) {
                    params = "&type=MagazineIssue&parentId=" + $("#hdd" + selectorParent.substring(1)).val();
                }
                if (self._filterType == 8 /* VolumeChapter */) {
                    params = "&type=VolumeChapter";
                }
                if (self._filterType == 9 /* WebsiteArticle */) {
                    params = "&type=WebsiteArticle";
                }

                $.get("/Api/Category/ByName?term=" + request.term + params, function (data) {
                    response(data);
                });
            },
            select: function (event, ui) {
                //debugger;
                //if (onSelect == null) {
                $(inputSelector).data("category-id", ui.item.id);
                $(inputSelector).val(ui.item.name);

                if (self.GetAlreadyAddedCategories(elemContainer, ui.item.id).length > 0) {
                    return false;
                }

                addCat();

                //}
                //else {
                //    new onSelect(catId, catIdx, catText);
                //}
                return false;
            },
            open: function (event, ui) {
                $('.show-tooltip').tooltip();
            }
        });

        autocomplete.data("ui-autocomplete")._renderItem = function (ul, item) {
            if (isCategoryEdit && categoryName == item.name)
                return "";

            var html;

            //debugger;
            if (CompareType.IsReference(item.type)) {
                var jqueryReference = $(item.html);
                if (CompareType.AreEqual(item.type, 9 /* WebsiteArticle */)) {
                    var linkContent = jqueryReference.find('.Url').text();
                    jqueryReference.find('.Url').text(linkContent);
                } else {
                    jqueryReference.find('.Url').remove();
                }
                if (CompareType.AreEqual(item.type, 4 /* DailyIssue */) || CompareType.AreEqual(item.type, 7 /* MagazineIssue */))
                    jqueryReference.find('.PublicationDate').remove();

                jqueryReference.find('.WikiUrl').remove();

                var jqueryReferenceHtml = $('<div></div>').append(jqueryReference).html();

                html = "<a class='CatListItem'>" + "<img src='" + item.imageUrl + "'/>" + "<div class='CatDescription'>" + jqueryReferenceHtml + "<span class='NumberQuestions'>(" + item.numberOfQuestions + " Fragen)</span>" + "</div>" + "</a>";
            } else {
                html = "<a class='CatListItem'>" + "<img src='" + item.imageUrl + "'/>" + "<div class='CatDescription'>" + "<span class='cat-name'>" + item.name + "</span>" + "<span class='NumberQuestions'>(" + item.numberOfQuestions + " Fragen)</span>" + "</div>" + "</a>";
            }

            return $("<li></li>").data("ui-autocomplete-item", item).append(html).appendTo(ul);
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
//# sourceMappingURL=AutocompleteCategories.js.map
