﻿class AnswerEntry {

    IsGameMode: boolean;
    IsLearningSession: boolean;

    constructor(
        isGameMode: boolean = false,
        isLearningSession: boolean = false) {

        this.IsGameMode = isGameMode;
        this.IsLearningSession = isLearningSession;
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