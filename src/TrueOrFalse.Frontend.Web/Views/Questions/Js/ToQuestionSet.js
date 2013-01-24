var ToQuestionSetModal = (function () {
    function ToQuestionSetModal() {
        $('#btnSelectionToSet').click(function () {
            _page.ToQuestionSetModal.Show();
        });
        this.Populate();
    }
    ToQuestionSetModal.prototype.Show = function () {
        this.Populate();
        $('#modalToQuestionSet').modal('show');
    };
    ToQuestionSetModal.prototype.Populate = function () {
        if(_page.RowSelector.Count() == 0) {
        } else {
        }
    };
    return ToQuestionSetModal;
})();
