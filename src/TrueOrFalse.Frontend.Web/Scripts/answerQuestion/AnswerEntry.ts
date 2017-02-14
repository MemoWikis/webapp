class AnswerEntry {

    IsGameMode: boolean;
    AnswerQuestion: AnswerQuestion;

    constructor(
        isGameMode: boolean = false) {

        this.IsGameMode = isGameMode;
    }

    public Init() {

        var solutionT = +$("#hddSolutionTypeNum").val();
        var answerEntry: IAnswerEntry;

        switch (solutionT) {
            case SolutionType.Date:
                answerEntry = new SolutionTypeDateEntry(this); break;
            case SolutionType.MultipleChoice:
                answerEntry = new SolutionTypeMultipleChoice(this); break;
            case SolutionType.Text:
                answerEntry = new SolutionTypeTextEntry(this); break;
            case SolutionType.Numeric:
                answerEntry = new SolutionTypeNumeric(this); break;
            case SolutionType.Sequence:
                answerEntry = new SolutionTypeSequence(this); break;
        };

        this.AnswerQuestion = answerEntry.AnswerQuestion;
    }

    public OnCorrectAnswer(fn: () => void) {
        this.AnswerQuestion.OnCorrectAnswer(fn);
    }

    public OnWrongAnswer(fn: () => void) {
        this.AnswerQuestion.OnWrongAnswer(fn);
    }
}