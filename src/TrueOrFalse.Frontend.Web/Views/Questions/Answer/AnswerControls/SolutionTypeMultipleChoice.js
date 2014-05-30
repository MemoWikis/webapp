/// <reference path="../../../../scripts/typescript.defs/lib.d.ts" />
/// <reference path="../../../../scripts/utils.ts" />
/// <reference path="../js/answerquestion.ts" />
var SolutionTypeMultipleChoice = (function () {
    function SolutionTypeMultipleChoice() {
        var answerQuestion = new AnswerQuestion(this);
        $('input:radio[name=answer]').change(function () {
            answerQuestion.OnAnswerChange();
        });
    }
    SolutionTypeMultipleChoice.prototype.GetAnswerText = function () {
        var selected = $('input:radio[name=answer]:checked');
        return selected.length ? selected.val() : "";
    };

    SolutionTypeMultipleChoice.prototype.GetAnswerData = function () {
        return { answer: $('input:radio[name=answer]:checked').val() };
    };

    SolutionTypeMultipleChoice.prototype.OnNewAnswer = function () {
        $('input:radio[name=answer]:checked').prop('checked', false);
    };
    return SolutionTypeMultipleChoice;
})();
;

$(function () {
    new SolutionTypeMultipleChoice();
});
//# sourceMappingURL=SolutionTypeMultipleChoice.js.map
