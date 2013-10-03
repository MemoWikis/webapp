var Page = (function () {
    function Page() { }
    Page.prototype.Init = function () {
        this.RowSelector = new RowSelector();
        this.ToQuestionSetModal = new ToQuestionSetModal();
    };
    return Page;
})();
$(function () {
    _page = new Page();
    _page.Init();
    new ValuationPerRow(".column-1", ValuationPerRowMode.Question);
});
