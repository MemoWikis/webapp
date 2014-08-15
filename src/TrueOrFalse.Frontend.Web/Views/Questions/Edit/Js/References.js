var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var ReferenceType;
(function (ReferenceType) {
    ReferenceType[ReferenceType["Url"] = 0] = "Url";
    ReferenceType[ReferenceType["Book"] = 1] = "Book";
    ReferenceType[ReferenceType["Periodicals"] = 2] = "Periodicals";
})(ReferenceType || (ReferenceType = {}));

var Reference = (function () {
    function Reference(type) {
        this.LabelText = "";
        this.SearchFieldPlaceholder = "";
        this.Type = type;
    }
    Reference.prototype.ToHtml = function () {
        throw new Error('This method is abstract');
    };
    Reference.prototype.Init = function () {
        throw new Error('This method is abstract');
    };
    return Reference;
})();

var ReferenceUrl = (function (_super) {
    __extends(ReferenceUrl, _super);
    function ReferenceUrl(url) {
        _super.call(this, 0 /* Url */);
        this.Url = url;
    }
    return ReferenceUrl;
})(Reference);

var ReferencePeriodical = (function (_super) {
    __extends(ReferencePeriodical, _super);
    function ReferencePeriodical() {
        _super.call(this, 2 /* Periodicals */);
    }
    return ReferencePeriodical;
})(Reference);

var ReferenceBook = (function (_super) {
    __extends(ReferenceBook, _super);
    function ReferenceBook() {
        _super.call(this, 1 /* Book */);
        this.FilterType = 1 /* Book */;
        this.LabelText = "Buch suchen";
        this.SearchFieldPlaceholder = "Suche nach Buchtitel oder ISBN";
    }
    return ReferenceBook;
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
            $('#JS-ReferenceSearch').empty();
            var referenceType = $('#ReferenceType option:selected').attr('value');
            if (referenceType == "Book")
                _this.AddReferenceSearch(new ReferenceBook());
        });
    }
    ReferenceUi.prototype.AddReferenceSearch = function (reference) {
        debugger;
        $("#JS-ReferenceSearch").append("<label class='control-label LabelInline'>" + reference.LabelText + "</label>" + "<div class='JS-CatInputContainer ControlInline'>" + "<input id='txtReference' class='form-control' name ='txtReference' type ='text' value ='' placeholder='" + reference.SearchFieldPlaceholder + "'/>" + "</div>");
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
