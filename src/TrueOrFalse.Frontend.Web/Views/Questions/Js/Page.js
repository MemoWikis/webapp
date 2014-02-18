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
    new ValuationPerRow(".column-Additional", 0 /* Question */);

    $(".column-Image .image-container").hover(function () {
        $(this).find("label").show();
    }, function () {
        if (!$($(this).find("input")[0]).prop('checked')) {
            $(this).find("label").hide();
        }
    });
});
//# sourceMappingURL=Page.js.map
