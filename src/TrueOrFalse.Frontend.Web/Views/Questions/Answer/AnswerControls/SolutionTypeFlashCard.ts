class SolutionTypeFlashCard
    extends AnswerEntryBase
    implements IAnswerEntry
{
    public SolutionType = SolutionType.FlashCard;
    private answerRight: boolean;
    private cardFlippedOnce: boolean;

    constructor(answerEntry: AnswerEntry) {
        super(answerEntry);

        this.AnswerQuestion = new AnswerQuestion(this);
        //special code for FlashCard
        $("#flashCardContent, #btnFlipCard").click(e => {
            if (!this.cardFlippedOnce) {
                this.cardFlippedOnce = true;
                $('#btnFlipCard').hide();
                $('#buttons-answer').show();
            }
        });

        $("#btnRightAnswer, #btnWrongAnswer")
            .click(
            e => {
                e.preventDefault();
                $('#hddTimeRecords').attr('data-time-of-answer', $.now());
            });

        $("#btnRightAnswer").click(
            e => {
                e.preventDefault();
                this.AnswerQuestion.AnsweredCorrectly = true;
                this.AnswerQuestion.ValidateAnswer();
            });

        $("#btnWrongAnswer").click(
            e => {
                e.preventDefault();
                this.AnswerQuestion.AnsweredCorrectly = false;
                this.AnswerQuestion.ValidateAnswer();
            });
    }

     GetChosenAnswers(): string {
        return "";
    }

    GetAnswerText(): string {
        return "";
    }

    GetAnswerData(): {} {
        return { answer: this.answerRight ? "true" : "false" };
    }
};