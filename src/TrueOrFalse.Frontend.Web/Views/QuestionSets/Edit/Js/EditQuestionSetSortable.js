var QuestionSortable = (function () {
    function QuestionSortable() {
        var _this = this;
        this._ulQuestions = $("#ulQuestions");
        this._questionSetId = parseInt($("#questionSetContainer").attr("data-id"));
        this._ulQuestions.sortable({
            placeholder: "ui-state-highlight",
            cursor: "move",
            stop: function (event, ui) {
                _this.UpdateIndicies();
            }
        });
    }
    QuestionSortable.prototype.UpdateIndicies = function () {
        var lisItems = $("#ulQuestions li");
        var cmdItems = [];
        for(var i = 0; i < lisItems.length; i++) {
            var questionId = $(lisItems[i]).attr("data-id");
            cmdItems.push({
                "QuestionId": questionId,
                "NewIndex": i
            });
        }
        $.post("/QuestionSet/UpdateQuestionsOrder/", {
            "questionSetId": this._questionSetId,
            "newIndicies": JSON.stringify(cmdItems)
        });
    };
    return QuestionSortable;
})();
$(function () {
    var questionSortable = new QuestionSortable();
    $("#ulQuestions").disableSelection();
});
