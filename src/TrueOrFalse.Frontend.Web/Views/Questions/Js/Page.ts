/// <reference path="ToQuestionSet.ts" />
/// <reference path="QuestionRowSelection.ts" />
/// <reference path="ValuationPerRow.ts" />

declare var _page: Page;

class Page
{
    RowSelector: RowSelector;
    ToQuestionSetModal: ToQuestionSetModal;

    Init() {
        this.RowSelector = new RowSelector();
        this.ToQuestionSetModal = new ToQuestionSetModal();
    }
  
}

$(function () {
    _page = new Page();
    _page.Init();
    new ValuationPerRow(".column-1");
});

