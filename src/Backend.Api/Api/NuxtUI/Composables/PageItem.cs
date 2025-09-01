public readonly record struct PageItem(
    int Id,
    string Name,
    string ImgUrl,
    int? QuestionCount,
    KnowledgeSummaryResponse KnowledgebarData,
    int? Popularity = null);

public readonly record struct KnowledgeSummaryResponse(
    int NotLearned = 0,
    int NotLearnedPercentage = 0,
    int NeedsLearning = 0,
    int NeedsLearningPercentage = 0,
    int NeedsConsolidation = 0,
    int NeedsConsolidationPercentage = 0,
    int Solid = 0,
    int SolidPercentage = 0,
    int NotInWishknowledge = 0,
    int NotInWishknowledgePercentage = 0,
    int Total = 0,
    double KnowledgeStatusPoints = 0.0,
    double KnowledgeStatusPointsTotal = 0.0);