using System.Collections.Concurrent;

/// <summary>
/// Cache with automatic sliding expiration for session-specific data.
/// Items automatically expire after a period of inactivity.
/// </summary>
public static class SlidingCache
{
    /// <summary>
    /// Default sliding expiration time in seconds for extended user cache items
    /// </summary>
    private const int _defaultExpirationSeconds = 36000; // 10 hours (600 minutes), using seconds for easier testing

    /// <summary>
    /// Cache key prefix for extended user cache items
    /// </summary>
    private const string _extendedUserCacheKeyPrefix = "ExtendedUser_";

    /// <summary>
    /// Track active user IDs and their expiration times for enumeration
    /// </summary>
    private static readonly ConcurrentDictionary<int, DateTime> _activeUserIds = new();

    // Extended User Cache Methods with sliding expiration
    public static ExtendedUserCacheItem GetExtendedUserById(int userId)
    {
        var cacheKey = $"{_extendedUserCacheKeyPrefix}{userId}";
        var extendedUser = MemoCache.Get<ExtendedUserCacheItem>(cacheKey);

        if (extendedUser != null)
        {
            // Update tracking since this is a sliding expiration access
            var newExpirationTime = DateTime.UtcNow.AddSeconds(_defaultExpirationSeconds);
            _activeUserIds.AddOrUpdate(userId, newExpirationTime, (key, oldValue) => newExpirationTime);
            return extendedUser;
        }

        return new ExtendedUserCacheItem();
    }

    public static ExtendedUserCacheItem? GetExtendedUserByIdNullable(int userId)
    {
        var cacheKey = $"{_extendedUserCacheKeyPrefix}{userId}";
        var extendedUser = MemoCache.Get<ExtendedUserCacheItem>(cacheKey);

        if (extendedUser != null)
        {
            // Update tracking since this is a sliding expiration access
            var newExpirationTime = DateTime.UtcNow.AddSeconds(_defaultExpirationSeconds);
            _activeUserIds.AddOrUpdate(userId, newExpirationTime, (key, oldValue) => newExpirationTime);
        }

        return extendedUser;
    }

    public static List<ExtendedUserCacheItem> GetAllActiveExtendedUsers()
    {
        var activeUsers = new List<ExtendedUserCacheItem>();
        var now = DateTime.UtcNow;
        var expiredKeys = new List<int>();

        foreach (var kvp in _activeUserIds.ToList())
        {
            if (kvp.Value > now) // Not expired according to our tracking
            {
                var user = GetExtendedUserByIdNullable(kvp.Key);
                if (user != null)
                {
                    // Update the expiration time since this is a sliding expiration access
                    _activeUserIds.TryUpdate(kvp.Key, now.AddSeconds(_defaultExpirationSeconds), kvp.Value);
                    activeUsers.Add(user);
                }
                else
                {
                    // Cache item was removed but tracking wasn't updated
                    expiredKeys.Add(kvp.Key);
                }
            }
            else
            {
                // Expired according to our tracking
                expiredKeys.Add(kvp.Key);
            }
        }

        // Clean up expired tracking entries
        foreach (var key in expiredKeys)
        {
            _activeUserIds.TryRemove(key, out _);
        }

        return activeUsers;
    }

    public static void AddOrUpdate(ExtendedUserCacheItem extendedUser, int expirationSeconds = _defaultExpirationSeconds)
    {
        var cacheKey = $"{_extendedUserCacheKeyPrefix}{extendedUser.Id}";
        // Use AddWithSlidingExpiration for sliding expiration
        MemoCache.AddWithSlidingExpiration(cacheKey, extendedUser, TimeSpan.FromSeconds(expirationSeconds));

        // Track the active user ID with expiration time
        var expirationTime = DateTime.UtcNow.AddSeconds(expirationSeconds);
        _activeUserIds.AddOrUpdate(extendedUser.Id, expirationTime, (key, oldValue) => expirationTime);
    }

    public static void Remove(ExtendedUserCacheItem extendedUser)
    {
        var cacheKey = $"{_extendedUserCacheKeyPrefix}{extendedUser.Id}";
        MemoCache.Remove(cacheKey);
        // Remove from tracking
        _activeUserIds.TryRemove(extendedUser.Id, out _);
    }

    public static void RemoveExtendedUser(int userId)
    {
        var cacheKey = $"{_extendedUserCacheKeyPrefix}{userId}";
        MemoCache.Remove(cacheKey);
        // Remove from tracking
        _activeUserIds.TryRemove(userId, out _);
    }

    public static void RemovePage(int pageId)
    {
        var allActiveUsers = GetAllActiveExtendedUsers();

        foreach (var user in allActiveUsers)
        {
            user.RemoveSkill(pageId);
            user.RemoveKnowledgeSummary(pageId);
        }
    }

    /// <summary>
    /// Update a user's skill in cache if the user exists
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="pageId">Page ID</param>
    /// <param name="knowledgeSummary">Knowledge summary to update with</param>
    /// <param name="userSkillService">Service to perform the update</param>
    public static void UpdateUserSkill(int userId, int pageId, KnowledgeSummary knowledgeSummary, UserSkillService userSkillService)
    {
        var extendedUser = GetExtendedUserByIdNullable(userId);
        if (extendedUser == null)
            return;

        var existingSkill = extendedUser.GetSkill(pageId);
        if (existingSkill == null)
            return;

        userSkillService.UpdateUserSkill(existingSkill, knowledgeSummary);
    }

    /// <summary>
    /// Update a user's knowledge summary in cache if the user exists
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="pageId">Page ID</param>
    /// <param name="knowledgeSummary">Knowledge summary to update with</param>
    public static void UpdateKnowledgeSummary(int userId, int pageId, KnowledgeSummary knowledgeSummary)
    {
        var extendedUser = GetExtendedUserByIdNullable(userId);
        if (extendedUser == null)
            return;

        var knowledgeEvaluationCacheItem = new KnowledgeEvaluationCacheItem
        {
            UserId = userId,
            PageId = pageId,
            KnowledgeSummary = knowledgeSummary,
            DateCreated = DateTime.Now,
            LastUpdatedAt = DateTime.Now
        };
        
        extendedUser.AddOrUpdateKnowledgeSummary(knowledgeEvaluationCacheItem);
    }
}
