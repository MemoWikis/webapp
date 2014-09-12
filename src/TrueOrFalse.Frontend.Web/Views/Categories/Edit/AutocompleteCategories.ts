
var ui: any;

interface CategoryItem {
    id: number;
    name: string;
    numberOfQuestions: number;
    imageUrl: string;
    type: string;
    html: string;
    isOnlyResult: boolean;
}

enum AutoCompleteFilterType {
    None,
    Book,
    Article,
    Daily, 
    DailyIssue,
    DailyArticle,
    Magazine,
    MagazineIssue,
    VolumeChapter,
    WebsiteArticle
}

class CompareType {
    static AreEqual(name: string, type: AutoCompleteFilterType) : boolean {
        
        if (name == "DailyIssue" && type == AutoCompleteFilterType.DailyIssue)
            return true;

        if (name == "MagazineIssue" && type == AutoCompleteFilterType.MagazineIssue)
            return true;

        if (name == "WebsiteArticle" && type == AutoCompleteFilterType.WebsiteArticle)
            return true;

        return false;
    }

    static IsReference(type: string) : boolean {
        if (type == "Book" ||
            type == "Daily" ||
            type == "DailyIssue" ||
            type == "DailyArticle" ||
            type == "Magazine" ||
            type == "MagazineIssue" ||
            type == "MagazineArticle" ||
            type == "VolumeChapter" ||
            type == "WebsiteArticle"
        )
            return true;

        return false;
    }
}

interface IAutocompleteOnSelect {
    OnSelect: (AutocompleteCategories, referenceId) => void;
}

class AutocompleteCategories {

    private _isSingleSelect: boolean;
    private _filterType: AutoCompleteFilterType;

    OnAdd: (categoryId?: number) => void;
    OnRemove: (categoryId?: number) => void;

    _isReference : boolean;
    _referenceId: number;
    _catId: number;
    _elemInput: JQuery;

    constructor(
        inputSelector: string,
        isSingleSelect: boolean = false,
        filterType: AutoCompleteFilterType = AutoCompleteFilterType.None,
        selectorParent: string = "",
        isReference: boolean = false) {

        this._filterType = filterType;
        this._isReference = isReference;

        var self = this;
        this._isSingleSelect = isSingleSelect;

        this._elemInput = $(inputSelector);
        if (this._elemInput.length == 0)
            return; 

        var elemContainer = this._elemInput.closest(".JS-RelatedCategories");

        var isCategoryEdit = $("#isCategoryEdit").length == 1;
        var categoryName = "";
        if (isCategoryEdit)
            categoryName = $("#Name").val();

        var nextCatIdx = 1;

        function addCatWithoutTriggers(referenceId: number = -1) {
            addCat(true, referenceId);
        }

        function addCat(withoutTriggers: boolean = false, referenceId: number = -1) {

            var catIdx = nextCatIdx.toString();
            nextCatIdx++;
            var catText = $(inputSelector).val();
            var catId = $(inputSelector).data('category-id');

            self._referenceId = referenceId;
            self._catId = catId;


            if (self.OnAdd != null && !withoutTriggers)
                self.OnAdd(catId);

            if (isReference == false) {

                if (self._isSingleSelect) {
                    catIdx = inputSelector.substring(1);
                    self._elemInput.closest(".JS-CatInputContainer").before(
                        "<div class='added-cat SingleSelect' id='cat-" + catIdx + "' style='display: none;'>" +
                            "<a href='/Kategorien/ById?id=" + catId + "'>" + catText + "</a>" +
                            "<input id='hdd" + catIdx + "' type='hidden' value='" + catId + "'name='" + "hdd" + catIdx + "'/> " +
                            "<a href='#' id='delete-cat-" + catIdx + "'><i class='fa fa-pencil'></i></a>" +
                        "</div> ");
                    self._elemInput.attr("type", "hidden").hide();

                    if ($("#EditCategoryForm").length > 0) {
                        var validator = $("#EditCategoryForm").validate();
                        validator.element(self._elemInput);
                    }

                } else {
                    self._elemInput.closest(".JS-CatInputContainer").before(
                        "<div class='added-cat' id='cat-" + catIdx + "' style='display: none;'>" +
                            "<a href='/Kategorien/ById?id=" + catId + "'>" + catText + "</a>" +
                            "<input type='hidden' value='" + catId + "' name='cat-" + catIdx + "'/>" +
                            "<a href='#' id='delete-cat-" + catIdx + "'><img alt='' src='/Images/Buttons/cross.png' /></a>" +
                        "</div> ");
                }

                self._elemInput.val('');
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
                        self._elemInput.attr("type", "").show();

                });
                $("#cat-" + catIdx).show("blind", { direction: "horizontal" });
            } else {
                new OnSelectForReference().OnSelect(self, referenceId);
            }
        }

        var autocomplete = $(inputSelector).autocomplete({
            minLength: 0,
            source: function(request, response) {

                var params = "";
                if (self._filterType == AutoCompleteFilterType.Book) {
                    params = "&type=Book";
                }
                if (self._filterType == AutoCompleteFilterType.Article) {
                    params = "&type=Article";
                }
                if (self._filterType == AutoCompleteFilterType.Daily) {
                    params = "&type=Daily";
                }
                if (self._filterType == AutoCompleteFilterType.DailyIssue) {
                    params = "&type=DailyIssue&parentId=" + $("#hdd" + selectorParent.substring(1)).val();
                }
                if (self._filterType == AutoCompleteFilterType.Magazine) {
                    params = "&type=Magazine";
                }
                if (self._filterType == AutoCompleteFilterType.MagazineIssue) {
                    params = "&type=MagazineIssue&parentId=" + $("#hdd" + selectorParent.substring(1)).val();
                }
                if (self._filterType == AutoCompleteFilterType.VolumeChapter) {
                    params = "&type=VolumeChapter";
                }
                if (self._filterType == AutoCompleteFilterType.WebsiteArticle) {
                    params = "&type=WebsiteArticle";
                }

                $.get("/Api/Category/ByName?term=" + request.term + params, function(data) {
                    response(data);
                });
            },
            select: function (event, ui) {

                    if (ui.item.type == "CreateCategoryLink") {
                        return false;
                    }

                    $(inputSelector).data("category-id", ui.item.id);
                    $(inputSelector).val(ui.item.name);

                    if (self.GetAlreadyAddedCategories(elemContainer, ui.item.id).length > 0) {
                        return false;
                    }

                    addCat();
                
                return false;
            },
            open: function(event, ui) {
                $('.show-tooltip').tooltip();
            }
        });

        autocomplete.data("ui-autocomplete")._renderItem = function (ul, item: CategoryItem): any {
            if (isCategoryEdit && categoryName == item.name)
                return "";

            var html;
            if (CompareType.IsReference(item.type))
                {
                var jqueryReference = $(item.html);
                if (CompareType.AreEqual(item.type, AutoCompleteFilterType.WebsiteArticle)) {//Render link as plain text to avoid nested anchors
                    var linkContent = jqueryReference.find('.Url').text();
                    jqueryReference.find('.Url').text(linkContent);
                } else {
                    jqueryReference.find('.Url').remove();    
                }
                if (CompareType.AreEqual(item.type, AutoCompleteFilterType.DailyIssue) ||
                    CompareType.AreEqual(item.type, AutoCompleteFilterType.MagazineIssue))
                    jqueryReference.find('.PublicationDate').remove();
                    
                jqueryReference.find('.WikiUrl').remove();
                
                var jqueryReferenceHtml = $('<div></div>').append(jqueryReference).html();

                html = "<a class='CatListItem'>" +
                            "<img src='" + item.imageUrl + "'/>" +
                            "<div class='CatDescription'>" +
                                jqueryReferenceHtml +
                                "<span class='NumberQuestions'>(" + item.numberOfQuestions + " Fragen)</span>" +
                            "</div>" +
                        "</a>";
            }
            else if (item.type == "CreateCategoryLink") {

                var resultInfo = "Kein Treffer? Bitte weitertippen oder ";

                if (item.isOnlyResult)
                    resultInfo = "Leider kein Treffer. Bitte anderen Suchbegriff verwenden oder ";

                var linkText = "Kategorie in neuem Tab erstellen."; 

                if(self._isReference) {
                    linkText = "Quelle in neuem Tab erstellen.";
                } 

                html = "<a class='CatListItem'>" + linkText + "</a>";

                html = "<div class='CatListItem'>" +
                    resultInfo +
                    "<a href='#' class='PlainLink'>" +
                    linkText +
                    "</a>" +
                    "</div>";

            }
            
            else {
                html = "<a class='CatListItem'>" +
                            "<img src='" + item.imageUrl + "'/>" +
                            "<div class='CatDescription'>" +
                                "<span class='cat-name'>" + item.name + "</span>" +
                                "<span class='NumberQuestions'>(" + item.numberOfQuestions + " Fragen)</span>" +
                            "</div>" +
                        "</a>";
            }

            return $("<li></li>")
                .data("ui-autocomplete-item", item)
                .append(html)
                .appendTo(ul);
        };

        autocomplete.data("ui-autocomplete")._resizeMenu = function () {
            var left = self._elemInput.offset().left;
            var maxWidth = 420;
            var maxPossibleWidth = $(window).outerWidth() - left - 20;
            if(maxPossibleWidth < maxWidth)
                maxWidth = maxPossibleWidth;
            $(this.menu.element).css('max-width', maxWidth + 'px');
            $(this.menu.element).css('min-width', self._elemInput.outerWidth() + 'px');
        }

        var animating = false;
        function checkText() {
            var id = $(inputSelector).data('category-id');
            var alreadyAddedCategories = self.GetAlreadyAddedCategories(elemContainer, id);

            if (!animating && alreadyAddedCategories.length != 0) {
                animating = true;
                alreadyAddedCategories.closest(".added-cat").effect('bounce', null, 'fast', function () { animating = false; });
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
        }

        $(inputSelector).keypress(fnCheckTextAndAdd);
        $(inputSelector).unbind("initCategoryFromTxt");
        $(inputSelector).bind("initCategoryFromTxt", function (event, referenceId: number = -1) { addCatWithoutTriggers(referenceId); });
    }

    GetAlreadyAddedCategories(container : JQuery, id : string) : JQuery {
        return container.find(".added-cat input[value='" + id + "']");
    }
}

$(function () {
    new AutocompleteCategories("#txtNewRelatedCategory");
});