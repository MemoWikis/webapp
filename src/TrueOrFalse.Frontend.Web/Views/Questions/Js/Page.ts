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


        $(".column-Image .image-container").hover(
            function () { $(this).find("label").show(); },
            function () {
                if (!$($(this).find("input")[0]).prop('checked')) {
                    $(this).find("label").hide();
                }
            }
            );

        $('.selectQuestion').change(function () { _page.RowSelector.Toggle(new Checkbox($(this))); });
        $('#selectAll').click(function () { _page.RowSelector.SelectAll(); });
        $('#selectNone').click(function () { _page.RowSelector.DeselecttAll(); });
        $('#selectMemorizedByMe').click(function () { _page.RowSelector.SelectAllMemorizedByMe(); });
        $('#selectCreatedByMe').click(function () { _page.RowSelector.SelectAllWhereIAmOwner(); });
        $('#selectedNotMemorizedByMe').click(function () { });
        $('#selectNotCraetedByMe').click(function () { _page.RowSelector.SelectAllWhereIAmNotOwner(); });

        new Pin(PinRowType.Question);
        new QuestionDelete();
    }
  
}

$(function () {
    _page = new Page();
    _page.Init();

});