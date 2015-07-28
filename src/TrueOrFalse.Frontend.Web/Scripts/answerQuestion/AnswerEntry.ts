interface IAnswerEntry {
    GetAnswerText(): string;
    GetAnswerData(): {};
    OnNewAnswer(): void;

    IsGameMode : boolean;
}

class AnswerEntry {

    IsGameMode: boolean;

    constructor(isGameMode : boolean = false) {
        this.IsGameMode = isGameMode;
    }

    public Init() {

        var solutionT = +$("#hddSolutionTypeNum").val();

        switch (solutionT) {
            case SolutionType.Date:
                new SolutionTypeDateEntry(this); break;
            case SolutionType.MultipleChoice:
                new SolutionTypeMultipleChoice(this); break;
            case SolutionType.Text:
                new SolutionTypeTextEntry(this); break;
            case SolutionType.Numeric:
                new SolutionTypeNumeric(this); break;
            case SolutionType.Sequence:
                new SolutionTypeSequence(this); break;
        };         
    }
}