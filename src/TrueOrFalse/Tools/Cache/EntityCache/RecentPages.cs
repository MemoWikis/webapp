
class RecentPages()
{
    private readonly Queue<int> pagesQueue = new Queue<int>();
    private readonly int maxSize = 5;

    public RecentPages(int userId, PageViewRepo pageViewRepo) : this()
    {
        var recentPagesId = pageViewRepo.GetRecentPagesForUser(userId);
    }

    public void VisitPage(int pageId)
    {
        if (pagesQueue.Count >= maxSize)
        {
            pagesQueue.Dequeue();
        }

        pagesQueue.Enqueue(pageId);
    }

    public void DisplayRecentPages()
    {
        Console.WriteLine("Recent Pages:");
        foreach (var page in pagesQueue)
        {
            Console.WriteLine(page);
        }
    }
}