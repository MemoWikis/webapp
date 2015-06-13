class SolutionTypeNumeric
    extends SolutionEntryBase
    implements ISolutionEntry {

    constructor(solutionEntry: SolutionEntry) {
        super(solutionEntry);
        this.IsGameMode = solutionEntry.IsGameMode;

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