var __extends = this.__extends || function (d, b) {
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var SolutionMetaData = (function () {
    function SolutionMetaData() { }
    return SolutionMetaData;
})();
var DatePrecision;
(function (DatePrecision) {
    DatePrecision._map = [];
    DatePrecision.Day = 1;
    DatePrecision.Month = 2;
    DatePrecision.Year = 3;
    DatePrecision.Decade = 4;
    DatePrecision.Century = 5;
    DatePrecision.Millenium = 6;
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
