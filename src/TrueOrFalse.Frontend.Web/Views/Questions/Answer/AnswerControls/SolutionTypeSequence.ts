/// <reference path="../../../../scripts/typescript.defs/lib.d.ts" />
/// <reference path="../../../../scripts/mm.utils.ts" />
/// <reference path="../js/answerquestion.ts" />

class SolutionTypeSequence implements ISolutionEntry
{
    constructor() {
        var answerQuestion = new AnswerQuestion(this);
        $('.sequence-row').keydown(function () {
            answerQuestion.OnAnswerChange();
        });
    }

    GetAnswerText(): string {
        return $.map($('.sequence-row'), function (x) {
            return $(x).val();
        }).join(", ");
    }

    GetAnswerData(): {} {
        return {
            answer: JSON.stringify($.map($('.sequence-row'), function (x) {
                return $(x).val();
            }))
        };
    }

    OnNewAnswer() {
        $('.sequence-row').val("");
    }
};

$(function() {
    new SolutionTypeSequence();
});