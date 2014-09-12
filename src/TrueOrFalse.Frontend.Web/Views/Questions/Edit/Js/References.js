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
        this.SearchFieldPlaceholder = "Suche nach Titel oder Zeitschrift/Zeitung";
    }
    return ReferenceArticle;
})(Reference);

var ReferenceVolumeChapter = (function (_super) {
    __extends(ReferenceVolumeChapter, _super);
    function ReferenceVolumeChapter() {
        _super.apply(this, arguments);
        this.FilterType = 8 /* VolumeChapter */;
        this.LabelText = "Beitrag in Sammelband suchen";
        this.SearchFieldPlaceholder = "Suche nach Titel oder Autor";
    }
    return ReferenceVolumeChapter;
})(Reference);

var ReferenceWebsiteArticle = (function (_super) {
    __extends(ReferenceWebsiteArticle, _super);
    function ReferenceWebsiteArticle() {
        _super.apply(this, arguments);
        this.FilterType = 9 /* WebsiteArticle */;
        this.LabelText = "Online-Artikel suchen";
        this.SearchFieldPlaceholder = "Suche nach Titel oder Autor";
    }
    return ReferenceWebsiteArticle;
})(Reference);

var ReferenceUi = (function () {
    function ReferenceUi() {
        var _this = this;
        this._nextRefIdx = 1;
        $("#AddReference").click(function (e) {
            e.preventDefault();

            $('#JS-ReferenceSearch').show();
            $('#AddReferenceControls').hide();

            $("#ReferenceType").change(function () {
                var referenceType = $('#ReferenceType option:selected').attr('value');
                if (referenceType == "Book")
                    _this.AddReferenceSearch(new ReferenceBook());
                if (referenceType == "Article")
                    _this.AddReferenceSearch(new ReferenceArticle());
                if (referenceType == "VolumeChapter")
                    _this.AddReferenceSearch(new ReferenceVolumeChapter());
                if (referenceType == "WebsiteArticle")
                    _this.AddReferenceSearch(new ReferenceWebsiteArticle());
                if (referenceType == "FreeText")
                    _this.AddFreetextReference();
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
        $('#AddFreeTextReference').hide();
        $('#ReferenceSearchInput').show().attr('placeholder', reference.SearchFieldPlaceholder);
        new AutocompleteCategories("#ReferenceSearchInput", true, reference.FilterType, "", true);
    };

    ReferenceUi.prototype.AddFreetextReference = function () {
        $('#ReferenceSearchInput').hide();
        $('#AddFreeTextReference').show();
    };

    ReferenceUi.ReferenceToJson = function () {
        var jsonReferences = $('.JS-ReferenceContainer:not(#JS-ReferenceSearch)').map(function (idx, elem) {
            var elemJ = $(elem);
            var result = new ReferenceJson();

            result.CategoryId = parseInt(elemJ.attr("data-cat-id"));
            result.ReferenceId = parseInt(elemJ.attr("data-ref-id"));
            result.AdditionalText = elemJ.find("[name='AdditionalInfo']").val();
            result.FreeText = elemJ.find("[name='FreeTextReference']").val();

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
    OnSelectForReference.prototype.OnSelect = function (autocomplete, referenceId) {
        var existingReferences = $('.JS-ReferenceContainer:not(#JS-ReferenceSearch)');
        var refIdxes = new Array;
        for (var i = 0; i < existingReferences.length; i++) {
            refIdxes.push(parseInt($(existingReferences[i]).attr('data-ref-idx')));
        }
        var nextRefIdx = 1;
        if (existingReferences.length != 0) {
            nextRefIdx = Math.max.apply(Math, refIdxes) + 1;
        }
        $("<div id='Ref-" + nextRefIdx + "' " + "class='JS-ReferenceContainer well'" + "data-ref-idx='" + nextRefIdx + "'" + "data-ref-id='" + autocomplete._referenceId + "'" + "data-cat-id='" + autocomplete._catId + "'>" + "<a id='delete-ref-" + nextRefIdx + "'" + " class='close' href ='#'>×</a>" + "</div>").insertBefore('#JS-ReferenceSearch');
        $("#delete-ref-" + nextRefIdx).click(function (e) {
            e.preventDefault();
            $("#delete-ref-" + nextRefIdx).closest('.JS-ReferenceContainer').remove();
        });

        autocomplete._elemInput.val("");
        $('#JS-ReferenceSearch').hide();
        $('#AddReferenceControls').show();

        if (autocomplete._catId != -1) {
            $.ajax({
                url: '/Fragen/Bearbeite/ReferencePartial?catId=' + autocomplete._catId,
                type: 'GET',
                success: function (data) {
                    $('#Ref-' + nextRefIdx).append(data).append("<div class='form-group' style='margin-bottom: 0;'>" + "<label class='columnLabel control-label' for='AdditionalInfo'>Ergänzungen zur Quelle</label>" + "<div class='columnControlsFull'>" + "<input class='InputRefAddition form-control input-sm' name='AdditionalInfo' type='text' placeholder='Seitenangaben, Zugriffsdatum etc.'/>" + "</div>" + "</div>");

                    $(window).trigger('referenceAdded' + referenceId);
                    $('.show-tooltip').tooltip();
                }
            });
        } else {
            $('#Ref-' + nextRefIdx).append("<div class='form-group' style='margin-bottom: 0;'>" + "<div class='columnControlsFull'>" + "<textarea class='FreeTextReference form-control' name='FreeTextReference' type='text' placeholder='Freitextquelle'></textarea>" + "</div>" + "</div>");
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
        $("#ReferenceSearchInput").data('category-id', '-1').trigger('initCategoryFromTxt');
    });
});
//# sourceMappingURL=References.js.map
