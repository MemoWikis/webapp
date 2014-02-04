/// <reference path="../../../../scripts/typescript.defs/lib.d.ts" />
/// <reference path="../../../../scripts/mm.utils.ts" />
/// <reference path="../js/answerquestion.ts" />
var SolutionTypeTextEntry = (function () {
    function SolutionTypeTextEntry() {
        var answerQuestion = new AnswerQuestion(this);
        $("#txtAnswer").keypress(function () {
            answerQuestion.OnAnswerChange();
        });
    }
    SolutionTypeTextEntry.prototype.GetAnswerText = function () {
        return $("#txtAnswer").val();
    };

    SolutionTypeTextEntry.prototype.GetAnswerData = function () {
        return { answer: $("#txtAnswer").val() };
    };

    SolutionTypeTextEntry.prototype.OnNewAnswer = function () {
        $("#txtAnswer").focus();
        $("#txtAnswer").setCursorPosition(0);
    };
    return SolutionTypeTextEntry;
})();
;

$(function () {
    new SolutionTypeTextEntry();
});
//# sourceMappingURL=SolutionTypeText.js.map
