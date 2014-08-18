/// <reference path="ToQuestionSet.ts" />
/// <reference path="QuestionRowSelection.ts" />

var Page = (function () {
    function Page() {
    }
    Page.prototype.Init = function () {
        this.RowSelector = new RowSelector();
        this.ToQuestionSetModal = new ToQuestionSetModal();

        $(".column-Image .image-container").hover(function () {
            $(this).find("label").show();
        }, function () {
            if (!$($(this).find("input")[0]).prop('checked')) {
                $(this).find("label").hide();
            }
        });

        $('.selectQuestion').change(function () {
            _page.RowSelector.Toggle(new Checkbox($(this)));
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
    };
    return Page;
})();

$(function () {
    _page = new Page();
    _page.Init();
});
//# sourceMappingURL=Page.js.map
