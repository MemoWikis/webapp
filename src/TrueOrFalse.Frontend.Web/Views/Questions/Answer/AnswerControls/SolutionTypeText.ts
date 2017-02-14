class SolutionTypeTextEntry
    extends AnswerEntryBase
    implements IAnswerEntry
{
    constructor(answerEntry: AnswerEntry) {
        super(answerEntry);

        this.AnswerQuestion = new AnswerQuestion(this);

        $("#txtAnswer").keypress(() => { this.AnswerQuestion.OnAnswerChange(); });    
    }

    GetAnswerText(): string {
        return $("#txtAnswer").val();
    }

    GetAnswerData(): {} {
        return { answer: $("#txtAnswer").val() };
    }

    OnNewAnswer() {
        $("#txtAnswer").focus();
        $("#txtAnswer").setCursorPosition(0);
        $("#txtAnswer").select();
    }
};