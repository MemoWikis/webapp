var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
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
        var references = new Array();

        for (var reference in references) {
            this.AddReferenceSearch(reference);
        }

        $("#AddReference").click(function (e) {
            e.preventDefault();

            //$('#JS-ReferenceSearch').empty();
            var referenceType = $('#ReferenceType option:selected').attr('value');
            if (referenceType == "Book")
                _this.AddReferenceSearch(new ReferenceBook());
            if (referenceType == "Article")
                _this.AddReferenceSearch(new ReferenceArticle());
            if (referenceType == "VolumeChapter")
                _this.AddReferenceSearch(new ReferenceVolumeChapter());
            if (referenceType == "WebsiteArticle")
                _this.AddReferenceSearch(new ReferenceWebsiteArticle());
        });
    }
    ReferenceUi.prototype.AddReferenceSearch = function (reference) {
        //$("#JS-ReferenceSearch")
        $("#JS-References").append("<div class='well'>" + "<a class='close' data-dismiss='well' href ='#'>×</a>" + "<label class='control-label LabelInline'>" + reference.LabelText + "</label>" + "<div class='JS-CatInputContainer ControlInline'>" + "<input id='txtReference' class='form-control' name ='txtReference' type ='text' value ='' placeholder='" + reference.SearchFieldPlaceholder + "'/>" + "</div></div>");
        this.Init(reference.FilterType);
    };

    ReferenceUi.prototype.Init = function (filterType) {
        new AutocompleteCategories("#txtReference", true, filterType);
    };
    return ReferenceUi;
})();

$(function () {
    new ReferenceUi();
});
//# sourceMappingURL=References.js.map
