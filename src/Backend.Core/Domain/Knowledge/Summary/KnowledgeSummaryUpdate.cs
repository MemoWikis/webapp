public class KnowledgeSummaryUpdate(
    PageValuationReadingRepository pageValuationReadingRepository,
    PageValuationWritingRepo pageValuationWritingRepo,
    KnowledgeSummaryLoader knowledgeSummaryLoader,
    UserSkillService userSkillService) : IRegisterAsInstancePerLifetime
{
    public void RunForPage(int pageId, bool forProfilePage = false)
    {
        foreach (var pageValuation in pageValuationReadingRepository.GetByPage(pageId))
        {
            Run(pageValuation, forProfilePage);
        }
    }

    public void RunForUser(int userId, bool forProfilePage = false)
    {
        // Try to get from cache first
        var extendedUser = SlidingCache.GetExtendedUserByIdNullable(userId);
        if (extendedUser?.PageValuations != null && extendedUser.PageValuations.Any())
        {
            // Use cached page valuations if available
            foreach (var pageValuation in extendedUser.PageValuations.Values)
            {
                Run(pageValuation, forProfilePage);
            }
        }
        else
        {
            // Fallback to database if not in cache
            foreach (var pageValuation in pageValuationReadingRepository.GetByUser(userId))
            {
                Run(pageValuation, forProfilePage);
            }
        }
    }

    public void RunForUserAndPage(int userId, int pageId, bool forProfilePage = false)
    {
        Log.Information("RunForUserAndPage: userId: {0}, {1}", userId, pageId);
        // Try to get from cache first
        var extendedUser = SlidingCache.GetExtendedUserByIdNullable(userId);
        var cachedPageValuation = extendedUser?.PageValuations?.GetValueOrDefault(pageId);

        if (cachedPageValuation != null)
        {
            // Use cached page valuation if available
            Run(cachedPageValuation, forProfilePage);
        }
        else
        {
            // Fallback to database if not in cache
            var pageValuation = pageValuationReadingRepository.GetBy(pageId, userId);
            if (pageValuation != null)
            {
                Run(pageValuation, forProfilePage);
            }
        }
    }

    private void Run(PageValuation pageValuation, bool forProfilePage = false)
    {
        var knowledgeSummary = forProfilePage
            ? knowledgeSummaryLoader.RunForProfilePage(
                pageValuation.UserId,
                pageValuation.PageId,
                onlyValuated: false)
            : knowledgeSummaryLoader.Run(
                pageValuation.UserId,
                pageValuation.PageId,
                onlyValuated: false);

        pageValuation.CountNotLearned = knowledgeSummary.NotLearned;
        pageValuation.CountNeedsLearning = knowledgeSummary.NeedsLearning;
        pageValuation.CountNeedsConsolidation = knowledgeSummary.NeedsConsolidation;
        pageValuation.CountSolid = knowledgeSummary.Solid;

        pageValuationWritingRepo.Update(pageValuation);

        // Update user skills in cache if the page exists and the user has an extended cache
        var existingSkillFromCache = SlidingCache
            .GetExtendedUserById(pageValuation.UserId)?
            .GetSkill(pageValuation.PageId);

        if (existingSkillFromCache != null)
            userSkillService.UpdateUserSkill(existingSkillFromCache, knowledgeSummary);
    }
}