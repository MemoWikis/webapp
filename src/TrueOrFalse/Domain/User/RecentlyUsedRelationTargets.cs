using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class RecentlyUsedRelationTargets
{
    public static void Add(
        int userId,
        int topicId,
        UserWritingRepo userWritingRepo,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        var userCacheItem = EntityCache.GetUserById(userId);

        if (userCacheItem.StartTopicId == topicId)
            return;

        var recentlyUsedRelationTargetTopicIds =
            userCacheItem.RecentlyUsedRelationTargetTopicIds == null
                ? new List<int>()
                : new List<int>(userCacheItem.RecentlyUsedRelationTargetTopicIds);

        if (recentlyUsedRelationTargetTopicIds.Count >= 3)
            recentlyUsedRelationTargetTopicIds.RemoveAt(0);

        recentlyUsedRelationTargetTopicIds.Add(topicId);

        var recentlyUsedRelationTargetTopics = string.Join(",", recentlyUsedRelationTargetTopicIds);

        userCacheItem.RecentlyUsedRelationTargetTopics = recentlyUsedRelationTargetTopics;
        userWritingRepo.ApplyChangeAndUpdate(userId,
            user => { user.RecentlyUsedRelationTargetTopics = recentlyUsedRelationTargetTopics; });
    }
}