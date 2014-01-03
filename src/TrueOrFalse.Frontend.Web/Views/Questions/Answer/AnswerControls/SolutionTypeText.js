/// <reference path="../../../../scripts/typescript.defs/lib.d.ts" />
/// <reference path="../../../../scripts/mm.utils.ts" />
/// <reference path="../js/answerquestion.ts" />
var SolutionTypeTextEntry = (function () {
    function SolutionTypeTextEntry() {
        var answerQuestion = new AnswerQuestion(function () {
            return $("#txtAnswer").val();
        }, function () {
            return { answer: $("#txtAnswer").val() };
        }, function () {
            $("#txtAnswer").focus();
            $("#txtAnswer").setCursorPosition(0);
        });

        $("#txtAnswer").keypress(function () {
            answerQuestion.OnAnswerChange();
        });
    }
    return SolutionTypeTextEntry;
})();

$(function () {
    new SolutionTypeTextEntry();
});
//# sourceMappingURL=SolutionTypeText.js.map
