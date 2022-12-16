using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RecentlyUsedRelationTargets
{
    public static void Add(int userId, int topicId)
    {
        var userCacheItem = EntityCache.GetUserById(userId);

        if (userCacheItem.StartTopicId == topicId)
            return;

        var recentlyUsedRelationTargetTopicIds = userCacheItem.RecentlyUsedRelationTargetTopicIds == null ? new List<int>() : new List<int>(userCacheItem.RecentlyUsedRelationTargetTopicIds);

        if (recentlyUsedRelationTargetTopicIds.Count >= 3)
            recentlyUsedRelationTargetTopicIds.RemoveAt(0);

        recentlyUsedRelationTargetTopicIds.Add(topicId);

        var recentlyUsedRelationTargetTopics = string.Join(",", recentlyUsedRelationTargetTopicIds);

        userCacheItem.RecentlyUsedRelationTargetTopics = recentlyUsedRelationTargetTopics;
        Sl.UserRepo.ApplyChangeAndUpdate(userId, user =>
        {
            user.RecentlyUsedRelationTargetTopics = recentlyUsedRelationTargetTopics;
        });
    }
}
