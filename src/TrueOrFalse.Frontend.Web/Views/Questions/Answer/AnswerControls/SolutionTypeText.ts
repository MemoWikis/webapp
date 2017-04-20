class SolutionTypeTextEntry
    extends AnswerEntryBase
    implements IAnswerEntry
{

    public SolutionType = SolutionType.Text;

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
};