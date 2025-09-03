public class KnowledgeSummaryUpdate(
    KnowledgeSummaryLoader knowledgeSummaryLoader,
    UserSkillService userSkillService) : IRegisterAsInstancePerLifetime
{
    public void UpdateSkillsByPage(int pageId)
    {
        // Get all active extended users and find users who have a skill for this page
        var allActiveExtendedUsers = SlidingCache.GetAllActiveExtendedUsers();

        foreach (var userCache in allActiveExtendedUsers)
        {
            var existingSkill = userCache.GetSkill(pageId);
            if (existingSkill != null)
            {
                Log.Information("Updating skill for userId: {0}, pageId: {1}", userCache.Id, pageId);
                UpdateSkill(pageId, userCache.Id);
            }
        }
    }

    public void UpdateKnowledgeSummariesByPage(int pageId)
    {
        // Get all active extended users and find users who have a knowledge summary for this page
        var allActiveExtendedUsers = SlidingCache.GetAllActiveExtendedUsers();

        foreach (var userCache in allActiveExtendedUsers)
        {
            var existingKnowledgeSummary = userCache.GetKnowledgeSummary(pageId);
            if (existingKnowledgeSummary != null)
            {
                Log.Information("Updating knowledge summary for userId: {0}, pageId: {1}", userCache.Id, pageId);
                UpdateKnowledgeSummary(pageId, userCache.Id);
            }
        }
    }

    public void UpdateSkillsByUser(int userId)
    {
        var extendedUser = SlidingCache.GetExtendedUserById(userId);

        // Update all skills for the user
        var skills = extendedUser.GetAllSkills();

        foreach (var skill in skills)
            UpdateSkill(skill.PageId, userId);
    }

    public void UpdateKnowledgeSummariesByUser(int userId)
    {
        var extendedUser = SlidingCache.GetExtendedUserById(userId);

        // Update all knowledge summaries for the user
        var knowledgeSummaries = extendedUser.GetAllKnowledgeSummaries();

        foreach (var knowledgeSummary in knowledgeSummaries)
            UpdateKnowledgeSummary(knowledgeSummary.PageId, userId);
    }

    public void UpdateSkillForUserAndPage(int userId, int pageId)
    {
        Log.Information("UpdateSkillForUserAndPage: userId: {0}, pageId: {1}", userId, pageId);
        UpdateSkill(pageId, userId);
    }

    public void UpdateKnowledgeSummaryForUserAndPage(int userId, int pageId)
    {
        Log.Information("UpdateKnowledgeSummaryForUserAndPage: userId: {0}, pageId: {1}", userId, pageId);
        UpdateKnowledgeSummary(pageId, userId);
    }

    private void UpdateSkill(int pageId, int userId)
    {
        var knowledgeSummary = knowledgeSummaryLoader.Run(
            userId,
            pageId,
            onlyValuated: false);

        SlidingCache.UpdateUserSkill(userId, pageId, knowledgeSummary, userSkillService);
    }

    private void UpdateKnowledgeSummary(int pageId, int userId)
    {
        var knowledgeSummary = knowledgeSummaryLoader.Run(
            userId,
            pageId,
            onlyValuated: false);

        SlidingCache.UpdateKnowledgeSummary(userId, pageId, knowledgeSummary);
    }

    public void RunForUser(int userId, bool forProfilePage = false)
    {
        if (forProfilePage)
            UpdateSkillsByUser(userId);
        else
            UpdateKnowledgeSummariesByUser(userId);
    }

    public void RunForPage(int pageId, bool forProfilePage = false)
    {
        if (forProfilePage)
            UpdateSkillsByPage(pageId);
        else
            UpdateKnowledgeSummariesByPage(pageId);
    }

    public void RunForUserAndPage(int userId, int pageId, bool forProfilePage = false)
    {
        if (forProfilePage)
            UpdateSkillForUserAndPage(userId, pageId);
        else
            UpdateKnowledgeSummaryForUserAndPage(userId, pageId);
    }
}