public readonly record struct KnowledgeStatusCountsResponse(
    int NotLearned = 0,
    int NotLearnedPercentage = 0,
    int NeedsLearning = 0,
    int NeedsLearningPercentage = 0,
    int NeedsConsolidation = 0,
    int NeedsConsolidationPercentage = 0,
    int Solid = 0,
    int SolidPercentage = 0,
    int NotLearnedPercentageOfTotal = 0,
    int NeedsLearningPercentageOfTotal = 0,
    int NeedsConsolidationPercentageOfTotal = 0,
    int SolidPercentageOfTotal = 0,
    int Total = 0)
{
    public KnowledgeStatusCountsResponse(KnowledgeStatusCounts knowledgeStatusCounts) : this(
        knowledgeStatusCounts.NotLearned,
        knowledgeStatusCounts.NotLearnedPercentage,
        knowledgeStatusCounts.NeedsLearning,
        knowledgeStatusCounts.NeedsLearningPercentage,
        knowledgeStatusCounts.NeedsConsolidation,
        knowledgeStatusCounts.NeedsConsolidationPercentage,
        knowledgeStatusCounts.Solid,
        knowledgeStatusCounts.SolidPercentage,
        knowledgeStatusCounts.NotLearnedPercentageOfTotal,
        knowledgeStatusCounts.NeedsLearningPercentageOfTotal,
        knowledgeStatusCounts.NeedsConsolidationPercentageOfTotal,
        knowledgeStatusCounts.SolidPercentageOfTotal,
        knowledgeStatusCounts.Total)
    {
    }
}

public readonly record struct KnowledgeSummaryResponse(
    int Total = 0,
    double KnowledgeStatusPoints = 0.0,
    double KnowledgeStatusPointsTotal = 0.0,
    KnowledgeStatusCountsResponse InWishKnowledge = new KnowledgeStatusCountsResponse(),
    KnowledgeStatusCountsResponse NotInWishKnowledge = new KnowledgeStatusCountsResponse(),
    KnowledgeStatusCountsResponse TotalDetailed = new KnowledgeStatusCountsResponse())
{
    public KnowledgeSummaryResponse(KnowledgeSummary knowledgeSummary) : this(
        knowledgeSummary.NotInWishKnowledgePercentage,
        knowledgeSummary.TotalCount,
        knowledgeSummary.KnowledgeStatusPoints,
        knowledgeSummary.KnowledgeStatusPointsTotal,
        new KnowledgeStatusCountsResponse(knowledgeSummary.InWishKnowledge),
        new KnowledgeStatusCountsResponse(knowledgeSummary.NotInWishKnowledge),
        new KnowledgeStatusCountsResponse(knowledgeSummary.Total))
    {
    }
};
