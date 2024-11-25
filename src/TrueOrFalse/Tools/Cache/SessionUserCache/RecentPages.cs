
public class RecentPages()
{
    public readonly Queue<int> PagesQueue = new Queue<int>();
    private readonly int maxSize = 5;

    public RecentPages(int userId, PageViewRepo pageViewRepo) : this()
    {
        var recentPagesIds = pageViewRepo.GetRecentPagesForUser(userId);

        foreach (var id in recentPagesIds)
        {
            VisitPage(id);
        }
    }

    public void VisitPage(int pageId)
    {
        if (PagesQueue.Count >= maxSize)
        {
            PagesQueue.Dequeue();
        }

        PagesQueue.Enqueue(pageId);
    }

    public List<PageCacheItem> GetRecentPages()
    {
        var recentPages = new List<PageCacheItem>();
        foreach (var page in PagesQueue)
        {
            var pageCacheItem = EntityCache.GetPage(page);
            if (pageCacheItem != null)
            {
                recentPages.Add(pageCacheItem);
            }
        }

        return recentPages;
    }
}