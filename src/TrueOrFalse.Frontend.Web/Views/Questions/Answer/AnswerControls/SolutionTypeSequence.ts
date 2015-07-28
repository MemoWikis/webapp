class SolutionTypeSequence
    extends AnswerEntryBase
    implements IAnswerEntry
{
    constructor(answerEntry: AnswerEntry) {
        super(answerEntry);
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