var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
/* A shared class */
var QuestionsSearch = (function (_super) {
    __extends(QuestionsSearch, _super);
    function QuestionsSearch() {
        var _this = this;
        _super.call(this);
        var filterSelector = "#txtCategoryFilter";
        var autoCompleteCategories = new AutocompleteCategories(filterSelector);
        autoCompleteCategories.OnAdd = function (categoryId) {
            _this._categories.push(categoryId);
            _this.SubmitSearch();
        };

        autoCompleteCategories.OnRemove = function (categoryId) {
            _this._categories = $.grep(_this._categories, function (x) {
                return x !== categoryId;
            });
            _this.SubmitSearch();
        };

        $(filterSelector).on("initCategoryIds", function (e, categoryId) {
            _this._categories.push(categoryId);
        });
    }
    return QuestionsSearch;
})(SearchInTabs);

$(function () {
    new QuestionsSearch();
});
//# sourceMappingURL=QuestionsSearch.js.map
