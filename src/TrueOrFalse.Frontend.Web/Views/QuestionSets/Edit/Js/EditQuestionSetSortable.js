var QuestionSortable = (function () {
    function QuestionSortable() {
        $("#sortable").sortable({
            placeholder: "ui-state-highlight",
            cursor: "move",
            stop: function (event, ui) {
                console.log(ui.item.index());
            }
        });
    }
    QuestionSortable.prototype.SendIndicies = function () {
    };
    return QuestionSortable;
})();
$(function () {
    $("#sortable").disableSelection();
});
