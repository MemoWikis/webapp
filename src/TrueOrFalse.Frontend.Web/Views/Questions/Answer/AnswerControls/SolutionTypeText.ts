/// <reference path="../../../../scripts/typescript.defs/lib.d.ts" />
/// <reference path="../../../../scripts/mm.utils.ts" />
/// <reference path="../js/answerquestion.ts" />

class SolutionTypeTextEntry {

    constructor() {
        var answerQuestion =
            new AnswerQuestion(
                () => { return $("#txtAnswer").val(); }, /* fnGetAnswerText */
                () => { return { answer: $("#txtAnswer").val() }; }, /* fnGetAnswerData */
                () => { /* fnOnNewAnswer */
                    $("#txtAnswer").focus();
                    $("#txtAnswer").setCursorPosition(0);}
            );

        $("#txtAnswer").keypress(() => { answerQuestion.OnAnswerChange(); });

    }
}


$(function () {
    new SolutionTypeTextEntry();
});