var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var SolutionMetaData = (function () {
    function SolutionMetaData() {
    }
    return SolutionMetaData;
})();

var DatePrecision;
(function (DatePrecision) {
    DatePrecision[DatePrecision["Day"] = 1] = "Day";
    DatePrecision[DatePrecision["Month"] = 2] = "Month";
    DatePrecision[DatePrecision["Year"] = 3] = "Year";
    DatePrecision[DatePrecision["Decade"] = 4] = "Decade";
    DatePrecision[DatePrecision["Century"] = 5] = "Century";
    DatePrecision[DatePrecision["Millenium"] = 6] = "Millenium";
})(DatePrecision || (DatePrecision = {}));

var SolutionMetadataDate = (function (_super) {
    __extends(SolutionMetadataDate, _super);
    function SolutionMetadataDate() {
        _super.call(this);
        this.IsDate = true;
    }
    return SolutionMetadataDate;
})(SolutionMetaData);

var SolutionMetadataNumber = (function (_super) {
    __extends(SolutionMetadataNumber, _super);
    function SolutionMetadataNumber() {
        _super.call(this);
        this.IsNumber = true;
    }
    return SolutionMetadataNumber;
})(SolutionMetaData);

var SolutionMetadataText = (function (_super) {
    __extends(SolutionMetadataText, _super);
    function SolutionMetadataText() {
        _super.call(this);
        this.IsText = true;
    }
    return SolutionMetadataText;
})(SolutionMetaData);
//# sourceMappingURL=SolutionMetaData.js.map
