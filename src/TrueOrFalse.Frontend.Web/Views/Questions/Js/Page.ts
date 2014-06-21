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
    }
  
}

$(function () {
    _page = new Page();
    _page.Init();

    $(".column-Image .image-container").hover(
        function() { $(this).find("label").show(); },
        function () {
            if (!$($(this).find("input")[0]).prop('checked')) {
                $(this).find("label").hide();   
            }
        }
    );

    new PinRow(PinRowType.Question);

});