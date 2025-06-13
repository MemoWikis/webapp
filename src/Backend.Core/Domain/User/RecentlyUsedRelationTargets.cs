using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class RecentlyUsedRelationTargets
{
    public static void Add(
        int userId,
        int pageId,
        UserWritingRepo userWritingRepo,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        var userCacheItem = EntityCache.GetUserById(userId);

        if (userCacheItem.FirstWikiId == pageId)
            return;

        var recentlyUsedRelationTargetPageIds =
            userCacheItem.RecentlyUsedRelationTargetPageIds == null
                ? new List<int>()
                : new List<int>(userCacheItem.RecentlyUsedRelationTargetPageIds);

        if (recentlyUsedRelationTargetPageIds.Count >= 3)
            recentlyUsedRelationTargetPageIds.RemoveAt(0);

        recentlyUsedRelationTargetPageIds.Add(pageId);

        var recentlyUsedRelationTargetPages = string.Join(",", recentlyUsedRelationTargetPageIds);

        userCacheItem.RecentlyUsedRelationTargetPages = recentlyUsedRelationTargetPages;
        userWritingRepo.ApplyChangeAndUpdate(userId,
            user => { user.RecentlyUsedRelationTargetPages = recentlyUsedRelationTargetPages; });
    }
}