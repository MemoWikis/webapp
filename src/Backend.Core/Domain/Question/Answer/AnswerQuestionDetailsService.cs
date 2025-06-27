public record struct AnswerQuestionDetailsResult(
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
    bool IsInWishknowledge,
    AnswerQuestionDetailsPageItem[] Pages,
    QuestionVisibility Visibility,
    long DateNow,
    long EndTimer,
    MacroCreator Creator,
    DateTime CreationDate,
    int TotalViewCount,
    int WishknowledgeCount,
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