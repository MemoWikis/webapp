/// <reference path="../../../../scripts/typescript.defs/lib.d.ts" />
/// <reference path="../../../../scripts/mm.utils.ts" />
/// <reference path="../js/answerquestion.ts" />
var SolutionTypeTextEntry = (function () {
    function SolutionTypeTextEntry() {
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
    var answerQuestion = new AnswerQuestion(new SolutionTypeTextEntry());
    $("#txtAnswer").keypress(function () {
        answerQuestion.OnAnswerChange();
    });
});
//# sourceMappingURL=SolutionTypeText.js.map
