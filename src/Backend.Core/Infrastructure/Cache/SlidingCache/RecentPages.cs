public class RecentPages()
{
    public readonly Queue<int> PagesQueue = new Queue<int>();
    private readonly int maxSize = 5;

    public RecentPages(int userId, PageViewRepo pageViewRepo) : this()
    {
        var recentPagesIds = pageViewRepo.GetRecentPagesForUser(userId);
        var reversedPagesIds = recentPagesIds.Reverse();

        foreach (var id in reversedPagesIds)
        {
            VisitPage(id);
        }
    }

    public void VisitPage(int pageId)
    {
        if (PagesQueue.Contains(pageId))
        {
            // Remove the pageId if it already exists
            var tempQueue = new Queue<int>(PagesQueue.Where(id => id != pageId));
            PagesQueue.Clear();
            foreach (var id in tempQueue)
            {
                PagesQueue.Enqueue(id);
            }
        }
        else if (PagesQueue.Count >= maxSize)
        {
            PagesQueue.Dequeue();
        }

        // Enqueue the pageId to the top
        PagesQueue.Enqueue(pageId);
    }

    public List<PageCacheItem> GetRecentPages()
    {
        var recentPages = new List<PageCacheItem>();
        foreach (var page in PagesQueue.Reverse())
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