var Page = (function () {
    function Page() { }
    Page.prototype.Init = function () {
        this.RowSelector = new RowSelector();
        this.ToQuestionSetModal = new ToQuestionSetModal();
        $('#modalToQuestionSet').modal({
            show: true
        });
    };
    return Page;
})();
$(function () {
    _page = new Page();
    _page.Init();
});
