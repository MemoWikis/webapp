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


        $(".column-Image .ImageContainer").hover(
            function () { $(this).find("label").show(); },
            function () {
                if (!$($(this).find("input")[0]).prop('checked')) {
                    $(this).find("label").hide();
                }
            }
            );

        $('.SelectAreaUpper').click(function () { _page.RowSelector.Toggle(new QuestionRow($(this).closest('.question-row'))); });
        $('#selectAll').click(function () { _page.RowSelector.SelectAll(); });
        $('#selectNone').click(function () { _page.RowSelector.DeselecttAll(); });
        $('#selectMemorizedByMe').click(function () { _page.RowSelector.SelectAllMemorizedByMe(); });
        $('#selectedNotMemorizedByMe').click(function () { _page.RowSelector.SelectAllNotMemorizedByMe(); });
        $('#selectCreatedByMe').click(function () { _page.RowSelector.SelectAllWhereIAmOwner(); });
        $('#selectNotCraetedByMe').click(function () { _page.RowSelector.SelectAllWhereIAmNotOwner(); });

        new Pin(PinRowType.Question);
        new QuestionDelete();

        FillSparklineTotals();
        fnInitImageDetailModal($('.ImageDetailModal'));
    }
  
}

$(function () {
    _page = new Page();
    _page.Init();
});