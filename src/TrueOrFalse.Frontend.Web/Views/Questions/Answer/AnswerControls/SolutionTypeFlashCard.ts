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
            e.preventDefault();
            if (!this.cardFlippedOnce) {
                this.cardFlippedOnce = true;
                $('#btnFlipCard').hide();
                $('#aSkipStep').hide();
                $('#buttons-answer').show();
                if ($(e.delegateTarget).attr('id') === "btnFlipCard")
                    $('#flashCardContent').click();
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
                this.answerRight = true;
                this.AnswerQuestion.ValidateAnswer();
            });

        $("#btnWrongAnswer").click(
            e => {
                e.preventDefault();
                this.answerRight = false;
                this.AnswerQuestion.ValidateAnswer();
            });

        $("#flashCard-dontCountAnswer").click(
            e => {
                $('#buttons-answer').hide();
            });
    }

     GetChosenAnswers(): string {
        return "";
    }

    GetAnswerText(): string {
        return "";
    }

    GetAnswerData(): {} {
        return { answer: this.answerRight ? "(Antwort gewusst)" : "(Antwort nicht gewusst)" };
    }
};