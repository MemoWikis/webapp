class SolutionTypeNumeric
    extends AnswerEntryBase
    implements IAnswerEntry {

    constructor(answerEntry: AnswerEntry) {
        super(answerEntry);
        this.IsGameMode = answerEntry.IsGameMode;

        var answerQuestion = new AnswerQuestion(this);
        $("#txtAnswer").keypress(() => { answerQuestion.OnAnswerChange(); });    
    }

    GetAnswerText(): string {
         return $("#txtAnswer").val();
    }

    GetAnswerData(): {} {
         return { answer: $("#txtAnswer").val() };
    }

    OnNewAnswer(): void {
        $("#txtAnswer").focus();
        $("#txtAnswer").setCursorPosition(0);
        $("#txtAnswer").select();        
    }
}