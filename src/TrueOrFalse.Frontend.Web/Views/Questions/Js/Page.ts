/// <reference path="ToQuestionSet.ts" />
/// <reference path="QuestionRowSelection.ts" />

declare var _page: Page;

class Page
{
    RowSelector: RowSelector;
    ToQuestionSetModal: ToQuestionSetModal;

    Init() {
        this.RowSelector = new RowSelector();
        this.ToQuestionSetModal = new ToQuestionSetModal();
        $('#modalToQuestionSet').modal({
            show: true
        });
    }
  
}

$(function () {
    _page = new Page();
    _page.Init();
});

