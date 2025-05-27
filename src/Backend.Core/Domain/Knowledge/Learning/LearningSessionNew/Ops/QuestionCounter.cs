public record struct QuestionCounter(
    int InWishKnowledge,
    int NotInWishKnowledge,
    int CreatedByCurrentUser,
    int NotCreatedByCurrentUser,
    int Private,
    int Public,
    int NotLearned,
    int NeedsLearning,
    int NeedsConsolidation,
    int Solid,
    int Max);