class SolutionTypeSequence
    extends SolutionEntryBase
    implements ISolutionEntry
{
    constructor(solutionEntry: SolutionEntry) {
        super(solutionEntry);
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