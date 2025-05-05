class KnowledgeSummaryUpdate
{
    public static void RunForPage(int pageId,
        PageValuationReadingRepository pageValuationReadingRepository,
        PageValuationWritingRepo pageValuationWritingRepo,
        KnowledgeSummaryLoader knowledgeSummaryLoader)
    {
        foreach (var pageValuation in pageValuationReadingRepository.GetByPage(pageId))
        {
            Run(pageValuation, pageValuationWritingRepo, knowledgeSummaryLoader);
        }
    }

    public static void RunForUser(int userId,
        PageValuationReadingRepository pageValuationReadingRepository,
        PageValuationWritingRepo pageValuationWritingRepo,
        KnowledgeSummaryLoader knowledgeSummaryLoader)
    {
        foreach (var pageValuation in pageValuationReadingRepository.GetByUser(userId))
        {
            Run(pageValuation, pageValuationWritingRepo, knowledgeSummaryLoader);
        }
    }

    private static void Run(PageValuation pageValuation, PageValuationWritingRepo pageValuationWritingRepo, KnowledgeSummaryLoader knowledgeSummaryLoader)
    {
        var knowledgeSummary = knowledgeSummaryLoader.Run(pageValuation.UserId, pageValuation.PageId, false);
        pageValuation.CountNotLearned = knowledgeSummary.NotLearned;
        pageValuation.CountNeedsLearning = knowledgeSummary.NeedsLearning;
        pageValuation.CountNeedsConsolidation = knowledgeSummary.NeedsConsolidation;
        pageValuation.CountSolid = knowledgeSummary.Solid;

        pageValuationWritingRepo.Update(pageValuation);
    }

    public static void ScheduleForPage(int pageId, JobQueueRepo jobQueueRepo)
        => jobQueueRepo.Add(JobQueueType.RecalcKnowledgeSummaryForPage, pageId.ToString());
}
