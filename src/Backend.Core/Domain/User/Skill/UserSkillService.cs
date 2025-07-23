using Newtonsoft.Json;

/// <summary>
/// Service for managing user skills - automatically calculated based on question answering performance
/// </summary>
public class UserSkillService(UserSkillRepo userSkillRepo, PageRepository pageRepository)
{
    /// <summary>
    /// Calculate and update user skill based on their question performance for a specific page/wiki
    /// This should be called when a user answers questions or when we need to recalculate skills
    /// </summary>
    public void CalculateAndUpdateUserSkill(int userId, int pageId, KnowledgeSummary knowledgeSummary)
    {
        var existingSkillFromCache = EntityCache.GetSkillByUserAndPage(userId, pageId);
        
        if (existingSkillFromCache == null)
            CreateUserSkill(userId, pageId, knowledgeSummary);
        else
            UpdateUserSkill(userId, existingSkillFromCache, knowledgeSummary);
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
            AddedAt = now,
            LastUpdatedAt = now
        };
        
        userSkillRepo.Create(newSkill);
        UpdateSkillInCache(newSkill);
    }

    /// <summary>
    /// Update an existing user skill with new evaluation data
    /// </summary>
    public void UpdateUserSkill(int userId, UserSkillCacheItem existingSkillCache, KnowledgeSummary knowledgeSummary)
    {
        // Update the cache item first with new evaluation
        existingSkillCache.Evaluation = knowledgeSummary;
        existingSkillCache.LastUpdatedAt = DateTime.UtcNow;
        
        // Update both caches immediately
        EntityCache.AddOrUpdateSkill(userId, existingSkillCache);
        var extendedUser = EntityCache.GetExtendedUserByIdNullable(userId);
        if (extendedUser != null)
        {
            extendedUser.AddOrUpdateSkill(existingSkillCache);
        }
        
        // Update the database entity using the cached ID
        if (existingSkillCache.Id > 0)
        {
            var existingSkill = userSkillRepo.GetById(existingSkillCache.Id);
            if (existingSkill != null)
            {
                existingSkill.EvaluationJson = JsonConvert.SerializeObject(knowledgeSummary);
                existingSkill.LastUpdatedAt = existingSkillCache.LastUpdatedAt.Value;
                
                userSkillRepo.Update(existingSkill);
            }
            else
            {
                // Database entity was deleted but cache still exists - create new skill
                CreateUserSkill(userId, existingSkillCache.PageId, knowledgeSummary);
            }
        }
        else
        {
            // Cache item doesn't have database ID - fallback to composite key lookup
            var existingSkill = userSkillRepo.GetByUserAndPage(userId, existingSkillCache.PageId);
            if (existingSkill != null)
            {
                existingSkill.EvaluationJson = JsonConvert.SerializeObject(knowledgeSummary);
                existingSkill.LastUpdatedAt = existingSkillCache.LastUpdatedAt.Value;
                
                userSkillRepo.Update(existingSkill);
                
                // Update cache with the ID for future operations
                existingSkillCache.Id = existingSkill.Id;
                EntityCache.AddOrUpdateSkill(userId, existingSkillCache);
            }
            else
            {
                // Database and cache are out of sync - create new skill
                CreateUserSkill(userId, existingSkillCache.PageId, knowledgeSummary);
            }
        }
    }

    /// <summary>
    /// Update skill in both global cache and user's extended cache
    /// </summary>
    private void UpdateSkillInCache(UserSkill userSkill)
    {
        var page = pageRepository.GetById(userSkill.PageId);
        if (page != null)
        {
            var cacheItem = UserSkillCacheItem.FromUserSkill(userSkill, page.Name, page.IsWiki);
            EntityCache.AddOrUpdateSkill(userSkill.UserId, cacheItem);
            
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
        
        // Remove from cache
        EntityCache.RemoveSkill(userId, pageId);
        
        // Also remove from user's extended cache if it exists
        var extendedUser = EntityCache.GetExtendedUserByIdNullable(userId);
        if (extendedUser != null)
        {
            extendedUser.RemoveSkill(pageId);
        }
    }

    public IList<UserSkillCacheItem> GetUserSkills(int userId)
    {
        // Try cache first
        var cachedSkills = EntityCache.GetSkillsByUserId(userId);
        if (cachedSkills.Any())
        {
            return cachedSkills.ToList();
        }

        // Load from database and populate cache
        var dbSkills = userSkillRepo.GetByUserId(userId);
        var skillCacheItems = new List<UserSkillCacheItem>();

        foreach (var dbSkill in dbSkills)
        {
            var page = pageRepository.GetById(dbSkill.PageId);
            if (page != null)
            {
                var cacheItem = UserSkillCacheItem.FromUserSkill(dbSkill, page.Name, page.IsWiki);
                skillCacheItems.Add(cacheItem);
                EntityCache.AddOrUpdateSkill(userId, cacheItem);
            }
        }

        return skillCacheItems;
    }

    public UserSkillCacheItem? GetUserSkill(int userId, int pageId)
    {
        // Try cache first
        var cachedSkill = EntityCache.GetSkillByUserAndPage(userId, pageId);
        if (cachedSkill != null)
        {
            return cachedSkill;
        }

        // Load from database
        var dbSkill = userSkillRepo.GetByUserAndPage(userId, pageId);
        if (dbSkill != null)
        {
            var page = pageRepository.GetById(pageId);
            if (page != null)
            {
                var cacheItem = UserSkillCacheItem.FromUserSkill(dbSkill, page.Name, page.IsWiki);
                EntityCache.AddOrUpdateSkill(userId, cacheItem);
                return cacheItem;
            }
        }

        return null;
    }

    public int GetUserSkillCount(int userId)
    {
        return userSkillRepo.GetCountByUserId(userId);
    }
}
