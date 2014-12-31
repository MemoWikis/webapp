/// <reference path="ToQuestionSet.ts" />
/// <reference path="QuestionRowSelection.ts" />

var Page = (function () {
    function Page() {
    }
    Page.prototype.Init = function () {
        this.RowSelector = new RowSelector();
        this.ToQuestionSetModal = new ToQuestionSetModal();

        $(".column-Image .ImageContainer").hover(function () {
            $(this).find("label").show();
        }, function () {
            if (!$($(this).find("input")[0]).prop('checked')) {
                $(this).find("label").hide();
            }
        });

        $('.SelectAreaUpper').click(function () {
            _page.RowSelector.Toggle(new QuestionRow($(this).closest('.question-row')));
        });
        $('#selectAll').click(function () {
            _page.RowSelector.SelectAll();
        });
        $('#selectNone').click(function () {
            _page.RowSelector.DeselecttAll();
        });
        $('#selectMemorizedByMe').click(function () {
            _page.RowSelector.SelectAllMemorizedByMe();
        });
        $('#selectCreatedByMe').click(function () {
            _page.RowSelector.SelectAllWhereIAmOwner();
        });
        $('#selectedNotMemorizedByMe').click(function () {
        });
        $('#selectNotCraetedByMe').click(function () {
            _page.RowSelector.SelectAllWhereIAmNotOwner();
        });

        new Pin(0 /* Question */);
        new QuestionDelete();

        fnInitImageDetailModal($('.ImageDetailModal'));
    };
    return Page;
})();

$(function () {
    _page = new Page();
    _page.Init();
});
//# sourceMappingURL=Page.js.map
