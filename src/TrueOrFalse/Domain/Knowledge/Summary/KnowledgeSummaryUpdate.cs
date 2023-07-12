class KnowledgeSummaryUpdate
{
    public static void RunForCategory(int catgoryId, CategoryValuationRepo categoryValuationRepo, KnowledgeSummaryLoader knowledgeSummaryLoader)
    {
        foreach (var categoryValuation in categoryValuationRepo.GetByCategory(catgoryId))
        {
            Run(categoryValuation, categoryValuationRepo, knowledgeSummaryLoader);
        }
    }

    public static void RunForUser(int userId, CategoryValuationRepo categoryValuationRepo, KnowledgeSummaryLoader knowledgeSummaryLoader)
    {
        foreach (var categoryValuation in categoryValuationRepo.GetByUser(userId))
        {
            Run(categoryValuation, categoryValuationRepo,knowledgeSummaryLoader);
        }
    }

    private static void Run(CategoryValuation categoryValuation, CategoryValuationRepo categoryValuationRepo, KnowledgeSummaryLoader knowledgeSummaryLoader)
    {
        var knowledgeSummary = knowledgeSummaryLoader.Run(categoryValuation.UserId, categoryValuation.CategoryId, false);
        categoryValuation.CountNotLearned = knowledgeSummary.NotLearned;
        categoryValuation.CountNeedsLearning = knowledgeSummary.NeedsLearning;
        categoryValuation.CountNeedsConsolidation = knowledgeSummary.NeedsConsolidation;
        categoryValuation.CountSolid = knowledgeSummary.Solid;

        categoryValuationRepo.Update(categoryValuation);
    }

    public static void ScheduleForCategory(int categoryId, JobQueueRepo jobQueueRepo)
        => jobQueueRepo.Add(JobQueueType.RecalcKnowledgeSummaryForCategory, categoryId.ToString());
}
