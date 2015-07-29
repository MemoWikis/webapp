interface IAnswerEntry {
    GetAnswerText(): string;
    GetAnswerData(): {};
    OnNewAnswer(): void;

    IsGameMode: boolean;
    IsLearningSession: boolean;
}