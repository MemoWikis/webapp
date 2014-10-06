var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var ReferenceJson = (function () {
    function ReferenceJson() {
    }
    return ReferenceJson;
})();

var Reference = (function () {
    function Reference() {
        this.FilterType = 0 /* None */;
        this.LabelText = "";
        this.SearchFieldPlaceholder = "";
    }
    return Reference;
})();

var ReferenceBook = (function (_super) {
    __extends(ReferenceBook, _super);
    function ReferenceBook() {
        _super.apply(this, arguments);
        this.FilterType = 1 /* Book */;
        this.LabelText = "Buch suchen";
        this.SearchFieldPlaceholder = "Suche nach Buchtitel oder ISBN";
    }
    return ReferenceBook;
})(Reference);

var ReferenceArticle = (function (_super) {
    __extends(ReferenceArticle, _super);
    function ReferenceArticle() {
        _super.apply(this, arguments);
        this.FilterType = 2 /* Article */;
        this.LabelText = "Artikel suchen";
        this.SearchFieldPlaceholder = "Suche nach Titel / Zeitschrift / Zeitung";
    }
    return ReferenceArticle;
})(Reference);

var ReferenceVolumeChapter = (function (_super) {
    __extends(ReferenceVolumeChapter, _super);
    function ReferenceVolumeChapter() {
        _super.apply(this, arguments);
        this.FilterType = 7 /* VolumeChapter */;
        this.LabelText = "Beitrag in Sammelband suchen";
        this.SearchFieldPlaceholder = "Suche nach Titel oder Autor";
    }
    return ReferenceVolumeChapter;
})(Reference);

var ReferenceWebsiteArticle = (function (_super) {
    __extends(ReferenceWebsiteArticle, _super);
    function ReferenceWebsiteArticle() {
        _super.apply(this, arguments);
        this.FilterType = 8 /* WebsiteArticle */;
        this.LabelText = "Online-Artikel suchen";
        this.SearchFieldPlaceholder = "Suche nach Titel oder Autor";
    }
    return ReferenceWebsiteArticle;
})(Reference);

//enum ReferenceType {
//    MediaCategoryReference = 1,
//    FreeTextReference = 2,
//    UrlReference = 3,
//}
var ReferenceUi = (function () {
    function ReferenceUi() {
        var _this = this;
        $("#AddReference").click(function (e) {
            e.preventDefault();

            $('#JS-ReferenceSearch').show();
            $('#AddReferenceControls').hide();

            $("#ReferenceType").change(function () {
                var referenceSearchType = $('#ReferenceType option:selected').attr('value');

                if (referenceSearchType == "Book")
                    _this.AddReferenceSearch(new ReferenceBook());
                if (referenceSearchType == "Article")
                    _this.AddReferenceSearch(new ReferenceArticle());
                if (referenceSearchType == "VolumeChapter")
                    _this.AddReferenceSearch(new ReferenceVolumeChapter());
                if (referenceSearchType == "WebsiteArticle")
                    _this.AddReferenceSearch(new ReferenceWebsiteArticle());
                if (referenceSearchType == "FreeText")
                    _this.AddFreetextReference();
                if (referenceSearchType == "Url")
                    _this.AddUrlReference();
            });

            $("#ReferenceType").trigger('change');
        });

        $('#JS-HideReferenceSearch').click(function (e) {
            e.preventDefault();
            $('#JS-ReferenceSearch').hide();
            $('#AddReferenceControls').show();
        });
    }
    ReferenceUi.prototype.AddReferenceSearch = function (reference) {
        $('#AddFreeTextReference, #AddUrlReference').hide();
        $('#ReferenceSearchInput').closest('.JS-CatInputContainer').show();
        $('#ReferenceSearchInput').attr('placeholder', reference.SearchFieldPlaceholder).data('referenceType', 'MediaCategoryReference');
        new AutocompleteCategories("#ReferenceSearchInput", true, reference.FilterType, "", true);
    };

    ReferenceUi.prototype.AddFreetextReference = function () {
        $('#ReferenceSearchInput').closest('.JS-CatInputContainer').hide();
        $('#AddUrlReference').hide();
        $('#AddFreeTextReference').show();
    };

    ReferenceUi.prototype.AddUrlReference = function () {
        $('#ReferenceSearchInput').closest('.JS-CatInputContainer').hide();
        $('#AddFreeTextReference').hide();
        $('#AddUrlReference').show();
    };

    ReferenceUi.ReferenceToJson = function () {
        var jsonReferences = $('.JS-ReferenceContainer:not(#JS-ReferenceSearch)').map(function (idx, elem) {
            var elemJ = $(elem);
            var result = new ReferenceJson();

            result.CategoryId = parseInt(elemJ.attr("data-cat-id"));
            result.ReferenceId = parseInt(elemJ.attr("data-ref-id"));
            result.ReferenceType = elemJ.attr("data-ref-type");
            result.AdditionalText = elemJ.find("[name^='AdditionalInfo']").val();
            result.ReferenceText = elemJ.find("[name^='ReferenceText']").val();

            return result;
        }).toArray();

        return JSON.stringify(jsonReferences);
        ;
    };
    return ReferenceUi;
})();

var OnSelectForReference = (function () {
    function OnSelectForReference() {
    }
    OnSelectForReference.prototype.OnSelect = function (autocomplete, referenceId, referenceType) {
        var existingReferences = $('.JS-ReferenceContainer:not(#JS-ReferenceSearch)');
        var refIdxes = new Array;
        for (var i = 0; i < existingReferences.length; i++) {
            refIdxes.push(parseInt($(existingReferences[i]).attr('data-ref-idx')));
        }
        var nextRefIdx = 1;
        if (existingReferences.length != 0) {
            nextRefIdx = Math.max.apply(Math, refIdxes) + 1;
        }
        $("<div id='Ref-" + nextRefIdx + "' " + "class='JS-ReferenceContainer well'" + "data-ref-idx='" + nextRefIdx + "'" + "data-ref-id='" + autocomplete._referenceId + "'" + "data-ref-type='" + referenceType + "'" + "data-cat-id='" + autocomplete._catId + "'>" + "<a id='delete-ref-" + nextRefIdx + "' class='close show-tooltip' href ='#' data-toggle='tooltip' title = 'Quellenangabe löschen' data-placement = 'top'>×</a>" + "</div>").insertBefore('#JS-ReferenceSearch');
        $("#delete-ref-" + nextRefIdx).click(function (e) {
            e.preventDefault();
            $("#delete-ref-" + nextRefIdx).closest('.JS-ReferenceContainer').remove();
        });

        autocomplete._elemInput.val("");
        $('#JS-ReferenceSearch').hide();
        $('#AddReferenceControls').show();

        if (referenceType == "MediaCategoryReference") {
            $.ajax({
                url: '/Fragen/Bearbeite/ReferencePartial?catId=' + autocomplete._catId,
                type: 'GET',
                success: function (data) {
                    $('#Ref-' + nextRefIdx).append(data).append("<div class='form-group' style='margin-bottom: 0;'>" + "<label class='columnLabel control-label' for='AdditionalInfo-" + nextRefIdx + "'>Ergänzungen zur Quelle</label>" + "<div class='columnControlsFull'>" + "<input class='InputRefAddition form-control input-sm' name='AdditionalInfo-" + nextRefIdx + "' type='text' placeholder='Seitenangaben, Zugriffsdatum etc.'/>" + "</div>" + "</div>");

                    $(window).trigger('referenceAdded' + referenceId);
                    $('.show-tooltip').tooltip();
                }
            });
        } else {
            var fnInitReferenceTextValidation = function () {
                $('.ReferenceText').each(function () {
                    $(this).rules('add', {
                        required: true,
                        messages: {
                            required: "Bitte fülle dieses Pflichtfeld aus (oder lösche diese Quelle)."
                        }
                    });
                });
            };

            if (referenceType == "FreeTextReference") {
                $('#Ref-' + nextRefIdx).append("<div class='form-group' style='margin-bottom: 0;'>" + "<label class='RequiredField columnLabel control-label' for='ReferenceText-" + nextRefIdx + "'>Freitextquelle</label>" + "<div class='columnControlsFull'>" + "<textarea class='ReferenceText form-control input-sm' name='ReferenceText-" + nextRefIdx + "' type='text' placeholder='Quellenangabe'></textarea>" + "</div>" + "</div>");
                fnInitReferenceTextValidation();
            }
            if (referenceType == "UrlReference") {
                $('#Ref-' + nextRefIdx).append("<div class='form-group' style='margin-bottom: 0;'>" + "<label class='RequiredField columnLabel control-label' for='ReferenceText-" + nextRefIdx + "'>Url</label>" + "<div class='columnControlsFull'>" + "<input class='ReferenceText form-control input-sm' name='ReferenceText-" + nextRefIdx + "' type='text' placeholder='Bitte hier nur die Url eingeben'/>" + "<a href='#' id='TestLink-" + nextRefIdx + "' style='display: none;' target='_blank'>Link testen (in neuem Tab öffnen)</a>" + "</div>" + "</div>" + "<div class='form-group' style='margin-bottom: 0;'>" + "<label class='columnLabel control-label' for='AdditionalInfo-" + nextRefIdx + "'>Ergänzungen zur Quelle</label>" + "<div class='columnControlsFull'>" + "<textarea class='AdditionalInfo form-control input-sm' name='AdditionalInfo-" + nextRefIdx + "' type='text' placeholder='Zugriffsdatum etc.'></textarea>" + "</div>" + "</div>");
                fnInitReferenceTextValidation();

                var inputReferenceText = $('[name=ReferenceText-' + nextRefIdx + ']');
                inputReferenceText.bind('input blur', function (e) {
                    if ($(this).val() == "") {
                        $('#TestLink-' + nextRefIdx).hide();
                    } else if (e.type == "blur") {
                        var urlValue = inputReferenceText.val();
                        if (inputReferenceText.val().substring(0, 7) != "http://" && inputReferenceText.val().substring(0, 8) != "https://") {
                            urlValue = "http://" + urlValue;
                        }
                        inputReferenceText.val(urlValue);
                        $('#TestLink-' + nextRefIdx).show().attr('href', urlValue);
                    }
                });
            }
            $('#ReferenceSearchInput').data('referenceType', '');
            $(window).trigger('referenceAdded' + autocomplete._referenceId);
            $('.show-tooltip').tooltip();
        }
    };
    return OnSelectForReference;
})();

$(function () {
    new ReferenceUi();
    $('#AddFreeTextReference button').click(function (e) {
        e.preventDefault();
        $("#ReferenceSearchInput").data('category-id', '-1').data('referenceType', 'FreeTextReference').trigger('initCategoryFromTxt');
    });
    $('#AddUrlReference button').click(function (e) {
        e.preventDefault();
        $("#ReferenceSearchInput").data('category-id', '-1').data('referenceType', 'UrlReference').trigger('initCategoryFromTxt');
    });
});
//# sourceMappingURL=References.js.map
