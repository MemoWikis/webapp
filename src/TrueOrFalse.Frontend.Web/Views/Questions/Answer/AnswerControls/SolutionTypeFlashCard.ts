class SolutionTypeFlashCard
    extends AnswerEntryBase
    implements IAnswerEntry
{
    public SolutionType = SolutionType.FlashCard;

    constructor(answerEntry: AnswerEntry) {
        super(answerEntry);

        this.AnswerQuestion = new AnswerQuestion(this);
        //special code for FlashCard

        $("#btnRightAnswer").click(
            e => {
                e.preventDefault();
                this.AnswerQuestion.ValidateAnswer();
            });

        $("#btnWrongAnswer").click(
            e => {
                e.preventDefault();
                this.AnswerQuestion.ValidateAnswer();
            });
    }

    static GetChosenAnswers(): string {
        return "";
    }

    GetAnswerText(): string {
        return "";
    }

    GetAnswerData(): {} {
        return "";
    }
};