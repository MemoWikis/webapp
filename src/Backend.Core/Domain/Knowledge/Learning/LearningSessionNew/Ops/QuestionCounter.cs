public record struct QuestionCounter(
    int InWuwi,
    int NotInWuwi,
    int CreatedByCurrentUser,
    int NotCreatedByCurrentUser,
    int Private,
    int Public,
    int NotLearned,
    int NeedsLearning,
    int NeedsConsolidation,
    int Solid,
    int Max);