/// <reference path="QuestionRowSelection.ts" />

declare var _page: Page;

class Page
{
    RowSelector: RowSelector;

    Init() {
        this.RowSelector = new RowSelector();

        $('.SelectAreaCheckbox').on("click" , (function(e) {
            e.preventDefault();
            _page.RowSelector.Toggle(new QuestionRow($(this).closest('.question-row')));
        }));
        $('#selectAll').click(() => { _page.RowSelector.SelectAll(); });
        $('#selectNone').click(() => { _page.RowSelector.DeselecttAll(); });
        $('#selectMemorizedByMe').click(() => { _page.RowSelector.SelectAllMemorizedByMe(); });
        $('#selectedNotMemorizedByMe').click(() => { _page.RowSelector.SelectAllNotMemorizedByMe(); });
        $('#selectCreatedByMe').click(() => { _page.RowSelector.SelectAllWhereIAmOwner(); });
        $('#selectNotCreatedByMe').click(() => { _page.RowSelector.SelectAllWhereIAmNotOwner(); });

        new Pin(PinType.Question);
        new QuestionRowDelete(QuestionRowDeleteSourcePage.QuestionRow);

        FillSparklineTotals();
    }
}

$(() => {
    _page = new Page();
    _page.Init();
});