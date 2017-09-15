interface IAnswerEntry {
    SolutionType: SolutionType;
    GetAnswerText(): string;
    GetAnswerData(): {};

    IsGameMode: boolean;
    AnswerQuestion: AnswerQuestion;
}