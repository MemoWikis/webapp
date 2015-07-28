class AnswerEntryBase {

    IsGameMode;
    IsLearningSession;

    constructor(answerEntry: AnswerEntry) {
        this.IsGameMode = answerEntry.IsGameMode;
    }
}