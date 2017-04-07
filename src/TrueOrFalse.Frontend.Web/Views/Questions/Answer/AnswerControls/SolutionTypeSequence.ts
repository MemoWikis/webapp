class SolutionTypeSequence
    extends AnswerEntryBase
    implements IAnswerEntry
{
    public SolutionType = SolutionType.Sequence;

    constructor(answerEntry: AnswerEntry) {
        super(answerEntry);
        this.AnswerQuestion = new AnswerQuestion(this);
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
};