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
    }
    ReferenceBook.prototype.ToHtml = function () {
        return "" + "<label class='control-label LabelInline'>Buch suchen</label>" + "<div class='JS-CatInputContainer ControlInline'>" + "<input id='txtReferenceBook' class='form-control' name ='txtReferenceBook' type ='text' value ='' placeholder='Suche nach Titel oder ISSN'/>" + "</div>";
    };

    ReferenceBook.prototype.Init = function () {
        new AutocompleteCategories("#txtReferenceBook", true, 0 /* None */);
    };
    return ReferenceBook;
})(Reference);

var ReferenceUi = (function () {
    function ReferenceUi() {
        var _this = this;
        var references = new Array();
        for (var reference in references) {
            this.AddReference(reference);
        }

        $("#addReference").click(function (e) {
            e.preventDefault();
            _this.AddReference(new ReferenceBook());
        });
    }
    ReferenceUi.prototype.AddReference = function (reference) {
        $("#JS-References").append(reference.ToHtml());
        reference.Init();
    };
    return ReferenceUi;
})();

$(function () {
    new ReferenceUi();
});
//# sourceMappingURL=References.js.map
