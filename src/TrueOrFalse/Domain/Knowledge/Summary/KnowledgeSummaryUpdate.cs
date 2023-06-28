class KnowledgeSummaryUpdate
{
    public static void RunForCategory(int catgoryId, CategoryValuationRepo categoryValuationRepo)
    {
        foreach (var categoryValuation in categoryValuationRepo.GetByCategory(catgoryId))
        {
            Run(categoryValuation, categoryValuationRepo);
        }
    }

    public static void RunForUser(int userId, CategoryValuationRepo categoryValuationRepo)
    {
        foreach (var categoryValuation in categoryValuationRepo.GetByUser(userId))
        {
            Run(categoryValuation, categoryValuationRepo);
        }
    }

    private static void Run(CategoryValuation categoryValuation, CategoryValuationRepo categoryValuationRepo)
    {
        var knowledgeSummary = KnowledgeSummaryLoader.Run(categoryValuation.UserId, categoryValuation.CategoryId, false);
        categoryValuation.CountNotLearned = knowledgeSummary.NotLearned;
        categoryValuation.CountNeedsLearning = knowledgeSummary.NeedsLearning;
        categoryValuation.CountNeedsConsolidation = knowledgeSummary.NeedsConsolidation;
        categoryValuation.CountSolid = knowledgeSummary.Solid;

        categoryValuationRepo.Update(categoryValuation);
    }

    public static void ScheduleForCategory(int categoryId)
        => Sl.JobQueueRepo.Add(JobQueueType.RecalcKnowledgeSummaryForCategory, categoryId.ToString());
}
