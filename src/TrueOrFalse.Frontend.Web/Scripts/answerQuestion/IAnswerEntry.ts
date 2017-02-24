interface IAnswerEntry {
    SolutionType: SolutionType;
    GetAnswerText(): string;
    GetAnswerData(): {};
    OnNewAnswer(): void;

    IsGameMode: boolean;
    AnswerQuestion: AnswerQuestion;
}