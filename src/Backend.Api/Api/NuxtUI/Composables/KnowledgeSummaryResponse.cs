public readonly record struct KnowledgeSummaryResponse(
    int NotLearned = 0,
    int NotLearnedPercentage = 0,
    int NeedsLearning = 0,
    int NeedsLearningPercentage = 0,
    int NeedsConsolidation = 0,
    int NeedsConsolidationPercentage = 0,
    int Solid = 0,
    int SolidPercentage = 0,
    int NotInWishKnowledge = 0,
    int NotInWishKnowledgePercentage = 0,
    int Total = 0,
    double KnowledgeStatusPoints = 0.0,
    double KnowledgeStatusPointsTotal = 0.0)
{
    public KnowledgeSummaryResponse(KnowledgeSummary knowledgeSummary) : this(
        knowledgeSummary.NotLearned,
        knowledgeSummary.NotLearnedPercentage,
        knowledgeSummary.NeedsLearning,
        knowledgeSummary.NeedsLearningPercentage,
        knowledgeSummary.NeedsConsolidation,
        knowledgeSummary.NeedsConsolidationPercentage,
        knowledgeSummary.Solid,
        knowledgeSummary.SolidPercentage,
        knowledgeSummary.NotInWishKnowledge,
        knowledgeSummary.NotInWishKnowledgePercentage,
        knowledgeSummary.Total,
        knowledgeSummary.KnowledgeStatusPoints,
        knowledgeSummary.KnowledgeStatusPointsTotal)
    {
    }
};
