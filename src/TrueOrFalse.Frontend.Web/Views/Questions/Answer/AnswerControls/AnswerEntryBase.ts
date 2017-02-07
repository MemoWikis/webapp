class AnswerEntryBase {

    IsGameMode;
    AnswerQuestion: AnswerQuestion;

    constructor(answerEntry: AnswerEntry) {
        this.IsGameMode = answerEntry.IsGameMode;
    }
}