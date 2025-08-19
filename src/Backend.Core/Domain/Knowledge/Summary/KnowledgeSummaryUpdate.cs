public class KnowledgeSummaryUpdate(
    KnowledgeSummaryLoader knowledgeSummaryLoader,
    UserSkillService userSkillService,
    ExtendedUserCache extendedUserCache) : IRegisterAsInstancePerLifetime
{
    public void RunForPage(int pageId, bool forProfilePage = false)
    {
        //foreach (var pageValuation in pageValuationReadingRepository.GetByPage(pageId))
        //{
        //    Run(pageValuation, forProfilePage);
        //}
    }

    public void RunForUser(int userId, bool forProfilePage = false)
    {
        // Try to get from cache first
        var extendedUser = SlidingCache.GetExtendedUserByIdNullable(userId);
        var skills = extendedUser.GetAllSkills();

        if (skills.Any())
        {
            foreach (var skill in skills)
                Run(skill.PageId, userId, forProfilePage);
        }
    }

    public void RunForUserAndPage(int userId, int pageId, bool forProfilePage = false)
    {
        Log.Information("RunForUserAndPage: userId: {0}, {1}", userId, pageId);
        // Try to get from cache first
        //var extendedUser = extendedUserCache.GetUser(userId);
        Run(pageId, userId, forProfilePage);
    }

    private void Run(int pageId, int userId, bool forProfilePage = false)
    {
        var knowledgeSummary = forProfilePage
            ? knowledgeSummaryLoader.RunForProfilePage(
                userId,
                pageId,
                onlyValuated: false)
            : knowledgeSummaryLoader.Run(
                userId,
                pageId,
                onlyValuated: false);

        // Update user skills in cache if the page exists and the user has an extended cache
        var existingSkillFromCache = SlidingCache
            .GetExtendedUserById(userId)?
            .GetSkill(pageId);

        if (existingSkillFromCache != null)
            userSkillService.UpdateUserSkill(existingSkillFromCache, knowledgeSummary);
    }
}