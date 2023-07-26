class KnowledgeSummaryUpdate
{
    public static void RunForCategory(int catgoryId,
        CategoryValuationReadingRepo categoryValuationReadingRepo, 
        CategoryValuationWritingRepo categoryValuationWritingRepo, 
        KnowledgeSummaryLoader knowledgeSummaryLoader)
    {
        foreach (var categoryValuation in categoryValuationReadingRepo.GetByCategory(catgoryId))
        {
            Run(categoryValuation, categoryValuationWritingRepo, knowledgeSummaryLoader);
        }
    }

    public static void RunForUser(int userId,
        CategoryValuationReadingRepo categoryValuationReadingRepo,
        CategoryValuationWritingRepo categoryValuationWritingRepo,
        KnowledgeSummaryLoader knowledgeSummaryLoader)
    {
        foreach (var categoryValuation in categoryValuationReadingRepo.GetByUser(userId))
        {
            Run(categoryValuation, categoryValuationWritingRepo, knowledgeSummaryLoader);
        }
    }

    private static void Run(CategoryValuation categoryValuation, CategoryValuationWritingRepo categoryValuationWritingRepo, KnowledgeSummaryLoader knowledgeSummaryLoader)
    {
        var knowledgeSummary = knowledgeSummaryLoader.Run(categoryValuation.UserId, categoryValuation.CategoryId, false);
        categoryValuation.CountNotLearned = knowledgeSummary.NotLearned;
        categoryValuation.CountNeedsLearning = knowledgeSummary.NeedsLearning;
        categoryValuation.CountNeedsConsolidation = knowledgeSummary.NeedsConsolidation;
        categoryValuation.CountSolid = knowledgeSummary.Solid;

        categoryValuationWritingRepo.Update(categoryValuation);
    }

    public static void ScheduleForCategory(int categoryId, JobQueueRepo jobQueueRepo)
        => jobQueueRepo.Add(JobQueueType.RecalcKnowledgeSummaryForCategory, categoryId.ToString());
}
