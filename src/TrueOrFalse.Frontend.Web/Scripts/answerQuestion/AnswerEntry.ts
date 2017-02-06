class AnswerEntry {

    IsGameMode: boolean;

    _fnOnCorrectAnswer: () => void;
    _fnOnWrongAnswer: () => void;

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

        //answerEntry.AnswerQuestion.
    }

    public OnCorrectAnswer(fn: () => void) {
        this._fnOnCorrectAnswer = fn;
    }

    public OnWrongAnswer(fn: () => void) {
        this._fnOnWrongAnswer = fn;
    }
}