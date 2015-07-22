class SolutionEntryBase {

    IsGameMode;
    IsLearningSession;

    constructor(solutionEntry: SolutionEntry) {
        this.IsGameMode = solutionEntry.IsGameMode;
    }
}