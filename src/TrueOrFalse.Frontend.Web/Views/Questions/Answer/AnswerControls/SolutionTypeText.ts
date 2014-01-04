/// <reference path="../../../../scripts/typescript.defs/lib.d.ts" />
/// <reference path="../../../../scripts/mm.utils.ts" />
/// <reference path="../js/answerquestion.ts" />

class SolutionTypeTextEntry implements ISolutionEntry
{
    GetAnswerText(): string {
        return $("#txtAnswer").val();
    }

    GetAnswerData(): {} {
        return { answer: $("#txtAnswer").val() };
    }

    OnNewAnswer() {
        $("#txtAnswer").focus();
        $("#txtAnswer").setCursorPosition(0);
    }
};

$(function() {
    var answerQuestion = new AnswerQuestion(new SolutionTypeTextEntry());
    $("#txtAnswer").keypress(()=> { answerQuestion.OnAnswerChange(); });
});

