public record struct AnswerQuestionDetailsResult(
    int QuestionId,
    KnowledgeStatus KnowledgeStatus,
    int PersonalProbability,
    string PersonalColor,
    int AvgProbability,
    int PersonalAnswerCount,
    int PersonalAnsweredCorrectly,
    int PersonalAnsweredWrongly,
    int OverallAnswerCount,
    int OverallAnsweredCorrectly,
    int OverallAnsweredWrongly,
    bool IsInWishKnowledge,
    AnswerQuestionDetailsPageItem[] Pages,
    QuestionVisibility Visibility,
    long DateNow,
    long EndTimer,
    MacroCreator Creator,
    DateTime CreationDate,
    int TotalViewCount,
    int WishKnowledgeCount,
    int LicenseId);

public record struct MacroCreator(int Id, string Name);

public record struct AnswerQuestionDetailsPageItem(
    int Id,
    string Name,
    int QuestionCount,
    string ImageUrl,
    string MiniImageUrl,
    int Visibility,
    bool IsSpoiler
);