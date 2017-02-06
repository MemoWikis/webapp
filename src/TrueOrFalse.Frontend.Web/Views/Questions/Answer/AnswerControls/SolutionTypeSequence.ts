class SolutionTypeSequence
    extends AnswerEntryBase
    implements IAnswerEntry
{
    constructor(answerEntry: AnswerEntry) {
        super(answerEntry);
        this.AnswerQuestion = new AnswerQuestion(this);

        $('.sequence-row').keydown(() => { this.AnswerQuestion.OnAnswerChange(); });
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