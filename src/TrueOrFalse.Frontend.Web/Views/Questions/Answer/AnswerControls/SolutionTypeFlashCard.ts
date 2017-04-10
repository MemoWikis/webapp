class SolutionTypeFlashCard
    extends AnswerEntryBase
    implements IAnswerEntry
{
    public SolutionType = SolutionType.FlashCard;

    constructor(answerEntry: AnswerEntry) {
        super(answerEntry);

        this.AnswerQuestion = new AnswerQuestion(this);
    }

    static GetChosenAnswers(): string {
        return "Work in progress";
    }

    GetAnswerText(): string {
        return "Work in progress";
    }

    GetAnswerData(): {} {
        return "Work in progress";
    }
};