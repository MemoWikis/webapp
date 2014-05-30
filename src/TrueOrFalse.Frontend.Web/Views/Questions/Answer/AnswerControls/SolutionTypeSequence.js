/// <reference path="../../../../scripts/typescript.defs/lib.d.ts" />
/// <reference path="../../../../scripts/utils.ts" />
/// <reference path="../js/answerquestion.ts" />
var SolutionTypeSequence = (function () {
    function SolutionTypeSequence() {
        var answerQuestion = new AnswerQuestion(this);
        $('.sequence-row').keydown(function () {
            answerQuestion.OnAnswerChange();
        });
    }
    SolutionTypeSequence.prototype.GetAnswerText = function () {
        return $.map($('.sequence-row'), function (x) {
            return $(x).val();
        }).join(", ");
    };

    SolutionTypeSequence.prototype.GetAnswerData = function () {
        return {
            answer: JSON.stringify($.map($('.sequence-row'), function (x) {
                return $(x).val();
            }))
        };
    };

    SolutionTypeSequence.prototype.OnNewAnswer = function () {
        $('.sequence-row').val("");
    };
    return SolutionTypeSequence;
})();
;

$(function () {
    new SolutionTypeSequence();
});
//# sourceMappingURL=SolutionTypeSequence.js.map
