class SolutionTypeNumeric
    extends AnswerEntryBase
    implements IAnswerEntry
{

    public SolutionType = SolutionType.Numeric;

    constructor(answerEntry: AnswerEntry) {
        super(answerEntry);

        this.AnswerQuestion = new AnswerQuestion(this);
    }

    GetAnswerText(): string {
         return $("#txtAnswer").val();
    }

    GetAnswerData(): {} {
         return { answer: $("#txtAnswer").val() };
    }
}