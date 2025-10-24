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
    int? NotInWishKnowledgeCount = null,
    int? NotInWishKnowledgePercentage = null)
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
        knowledgeStatusCounts.NotInWishKnowledgeCount,
        knowledgeStatusCounts.NotInWishKnowledgePercentage)
    {
    }
}

public readonly record struct KnowledgeSummaryResponse(
    int TotalCount = 0,
    double KnowledgeStatusPoints = 0.0,
    double KnowledgeStatusPointsTotal = 0.0,
    KnowledgeStatusCountsResponse InWishKnowledge = new KnowledgeStatusCountsResponse(),
    KnowledgeStatusCountsResponse NotInWishKnowledge = new KnowledgeStatusCountsResponse(),
    KnowledgeStatusCountsResponse Total = new KnowledgeStatusCountsResponse())
{
    public KnowledgeSummaryResponse(KnowledgeSummary knowledgeSummary) : this(
        TotalCount: knowledgeSummary.TotalCount,
        KnowledgeStatusPoints: knowledgeSummary.KnowledgeStatusPoints,
        KnowledgeStatusPointsTotal: knowledgeSummary.KnowledgeStatusPointsTotal,
        InWishKnowledge: new KnowledgeStatusCountsResponse(knowledgeSummary.InWishKnowledge),
        NotInWishKnowledge: new KnowledgeStatusCountsResponse(knowledgeSummary.NotInWishKnowledge),
        Total: new KnowledgeStatusCountsResponse(knowledgeSummary.Total))
    {
    }
};
