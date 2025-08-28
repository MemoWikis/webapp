using Newtonsoft.Json;

/// <summary>
/// Service for managing user skills - automatically calculated based on question answering performance
/// </summary>
public class UserSkillService(UserSkillRepo userSkillRepo, KnowledgeSummaryUpdateService knowledgeSummaryUpdateService) : IRegisterAsInstancePerLifetime
{
    /// <summary>
    /// Calculate and update user skill based on their question performance for a specific page/wiki
    /// This should be called when a user answers questions or when we need to recalculate skills
    /// </summary>
    public void CalculateAndUpdateUserSkill(int userId, int pageId, KnowledgeSummary knowledgeSummary)
    {
        var existingSkillFromCache = SlidingCache.GetExtendedUserById(userId)?.GetSkill(pageId);

        if (existingSkillFromCache == null)
            CreateUserSkill(userId, pageId, knowledgeSummary);
        else
            UpdateUserSkill(existingSkillFromCache, knowledgeSummary);
    }

    /// <summary>
    /// Create a new user skill with the given evaluation
    /// </summary>
    public void CreateUserSkill(int userId, int pageId, KnowledgeSummary knowledgeSummary)
    {
        var now = DateTime.UtcNow;
        var newSkill = new UserSkill
        {
            UserId = userId,
            PageId = pageId,
            EvaluationJson = JsonConvert.SerializeObject(knowledgeSummary),
            DateCreated = now,
            LastUpdatedAt = now
        };

        userSkillRepo.Create(newSkill);
        var dbSkill = userSkillRepo.GetByUserAndPage(userId, pageId);
        AddNewSkillToCache(dbSkill);
        knowledgeSummaryUpdateService.ScheduleUserAndPageUpdateForProfilePage(userId, pageId);
    }

    /// <summary>
    /// Create a new user skill with default empty knowledge summary (KnowledgeSummary will be Updated using Rebus)
    /// </summary>
    public void CreateUserSkill(int userId, int pageId)
    {
        var defaultKnowledgeSummary = new KnowledgeSummary();

        CreateUserSkill(userId, pageId, defaultKnowledgeSummary);
    }

    private void AddNewSkillToCache(UserSkill newSkill)
    {
        var dbSkill = userSkillRepo.GetByUserAndPage(newSkill.UserId, newSkill.PageId);
        UpdateSkillInCache(dbSkill);
    }

    /// <summary>
    /// Update an existing user skill with new evaluation data
    /// </summary>
    public void UpdateUserSkill(KnowledgeEvaluationCacheItem knowledgeEvaluationCacheItem, KnowledgeSummary knowledgeSummary)
    {
        var userId = knowledgeEvaluationCacheItem.UserId;
        // Update the cache item first with new evaluation
        knowledgeEvaluationCacheItem.KnowledgeSummary = knowledgeSummary;
        knowledgeEvaluationCacheItem.LastUpdatedAt = DateTime.UtcNow;

        var extendedUser = EntityCache.GetExtendedUserByIdNullable(userId);
        if (extendedUser != null)
        {
            extendedUser.AddOrUpdateSkill(knowledgeEvaluationCacheItem);
        }

        var existingSkill = userSkillRepo.GetById(knowledgeEvaluationCacheItem.Id);
        if (existingSkill != null)
        {
            existingSkill.EvaluationJson = JsonConvert.SerializeObject(knowledgeSummary);
            existingSkill.LastUpdatedAt = knowledgeEvaluationCacheItem.LastUpdatedAt.Value;

            userSkillRepo.Update(existingSkill);
        }
    }

    /// <summary>
    /// Update skill in both global cache and user's extended cache
    /// </summary>
    private void UpdateSkillInCache(UserSkill userSkill)
    {
        var page = EntityCache.GetPage(userSkill.PageId);
        if (page != null)
        {
            var cacheItem = KnowledgeEvaluationCacheItem.FromUserSkill(userSkill, page.Name, page.IsWiki);

            // Also update user's extended cache if it exists
            var extendedUser = EntityCache.GetExtendedUserByIdNullable(userSkill.UserId);
            if (extendedUser != null)
            {
                extendedUser.AddOrUpdateSkill(cacheItem);
            }
        }
    }

    public void RemoveUserSkill(int userId, int pageId)
    {
        userSkillRepo.DeleteByUserAndPage(userId, pageId);

        // Also remove from user's extended cache if it exists
        var extendedUser = EntityCache.GetExtendedUserByIdNullable(userId);
        if (extendedUser != null)
        {
            extendedUser.RemoveSkill(pageId);
        }
    }

    public IList<KnowledgeEvaluationCacheItem> GetUserSkills(int userId)
    {
        // Try cache first
        var extendedUser = SlidingCache.GetExtendedUserByIdNullable(userId);
        var cachedSkills = SlidingCache.GetExtendedUserById(userId)?.GetAllSkills();

        if (cachedSkills?.Any() == true)
        {
            return cachedSkills.ToList();
        }

        // Load from database and populate cache
        var dbSkills = userSkillRepo.GetByUserId(userId);
        var skillCacheItems = new List<KnowledgeEvaluationCacheItem>();

        foreach (var dbSkill in dbSkills)
        {
            var page = EntityCache.GetPage(dbSkill.PageId);

            if (dbSkill != null && page != null)
            {
                var cacheItem = KnowledgeEvaluationCacheItem.FromUserSkill(dbSkill, page.Name, page.IsWiki);
                skillCacheItems.Add(cacheItem);
            }
        }

        if (extendedUser != null)
        {
            extendedUser.AddSkills(skillCacheItems);
            SlidingCache.AddOrUpdate(extendedUser);
        }


        return skillCacheItems;
    }

    public KnowledgeEvaluationCacheItem? GetUserSkill(int userId, int pageId)
    {
        // Try cache first
        var extendedUser = SlidingCache.GetExtendedUserByIdNullable(userId);
        var cachedSkill = SlidingCache.GetExtendedUserById(userId)?.GetSkill(pageId);

        if (cachedSkill != null)
            return cachedSkill;

        // Load from database
        var dbSkill = userSkillRepo.GetByUserAndPage(userId, pageId);
        var page = EntityCache.GetPage(pageId);

        if (dbSkill != null && page != null)
        {
            var cacheItem = KnowledgeEvaluationCacheItem.FromUserSkill(dbSkill, page.Name, page.IsWiki);
            if (extendedUser != null)
            {
                extendedUser.AddOrUpdateSkill(cacheItem);
                SlidingCache.AddOrUpdate(extendedUser);
            }

            return cacheItem;
        }

        return null;
    }
}
