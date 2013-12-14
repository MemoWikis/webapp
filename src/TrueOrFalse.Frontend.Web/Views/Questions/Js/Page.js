/// <reference path="ToQuestionSet.ts" />
/// <reference path="QuestionRowSelection.ts" />
/// <reference path="../../../Scripts/MM.ValuationPerRow.ts" />

var Page = (function () {
    function Page() {
    }
    Page.prototype.Init = function () {
        this.RowSelector = new RowSelector();
        this.ToQuestionSetModal = new ToQuestionSetModal();
    };
    return Page;
})();

$(function () {
    _page = new Page();
    _page.Init();
    new ValuationPerRow(".column-1", 0 /* Question */);
});
//# sourceMappingURL=Page.js.map
