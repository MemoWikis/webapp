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
    }
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

var ReferenceBooks = (function (_super) {
    __extends(ReferenceBooks, _super);
    function ReferenceBooks() {
        _super.call(this, 1 /* Book */);
    }
    return ReferenceBooks;
})(Reference);

var ReferenceUi = (function () {
    function ReferenceUi() {
        var references = new Array();
        references.push(new ReferenceUrl("http://someUrl"));
        references.push(new ReferenceUrl("http://someOtherUrl"));

        for (var reference in references) {
            this.AddReference(reference);
        }
    }
    ReferenceUi.prototype.AddReference = function (reference) {
        $("#references").append($("<div class='xxs-stack col-xs-4'>").append($("<select class='form-control'>").append($("<option>Url</option>")).append($("<optgroup label='Offline'>").append($("<option>Buch</option>")).append($("<option>Zeitung/Zeitschrift</option>"))))).append($("<div class='xxs-stack col-xs-8'>").append($("<input class='form-control' type='text' />")));
    };
    return ReferenceUi;
})();

$(function () {
    new ReferenceUi();
});
//# sourceMappingURL=References.js.map
