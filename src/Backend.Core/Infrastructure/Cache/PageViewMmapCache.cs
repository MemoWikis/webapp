using MessagePack;

[MessagePackObject]
public record struct PageViewSummaryWithId(
    [property: Key(0)] Int64 Count, 
    [property: Key(1)] DateTime DateOnly, 
    [property: Key(2)] int PageId, 
    [property: Key(3)] DateTime DateCreated);

public class PageViewMmapCache : IRegisterAsInstancePerLifetime, IDisposable
{
    private readonly string _pageViewsFilePath;
    private readonly object _pageViewLock = new();

    public PageViewMmapCache()
    {
        var cacheDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "viewcache");
        Directory.CreateDirectory(cacheDirectory);

        _pageViewsFilePath = Path.Combine(cacheDirectory, "pageviews.mmap");
    }

    public (List<PageViewSummaryWithId> views, DateTime? lastEntryDate) LoadPageViews()
    {
        if (!File.Exists(_pageViewsFilePath))
        {
            return (new List<PageViewSummaryWithId>(), null);
        }

        try
        {
            lock (_pageViewLock)
            {
                using var fileStream = new FileStream(_pageViewsFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var views = MessagePackSerializer.Deserialize<List<PageViewSummaryWithId>>(fileStream);

                var lastEntryDate = views.Any()
                    ? views.Max(view => view.DateCreated)
                    : (DateTime?)null;

                Log.Information($"Loaded {views.Count} page views from mmap cache. Last entry: {lastEntryDate}");
                return (views, lastEntryDate);
            }
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Failed to load page views from mmap cache");
            // Delete corrupted file
            File.Delete(_pageViewsFilePath);
            return (new List<PageViewSummaryWithId>(), null);
        }
    }

    public void AppendPageView(PageViewSummaryWithId view)
    {
        // TODO: Implement append functionality
        // For now, just log that we would append
        Log.Debug("Would append page view to mmap cache: PageId={PageId}, DateOnly={DateOnly}", view.PageId, view.DateOnly);
    }

    public void SaveAllPageViews(IList<PageViewSummaryWithId> views)
    {
        lock (_pageViewLock)
        {
            var tempFile = _pageViewsFilePath + ".tmp";
            File.WriteAllBytes(tempFile, MessagePackSerializer.Serialize(views));
            File.Move(tempFile, _pageViewsFilePath, true);
            Log.Information($"Saved {views.Count} page views to mmap cache");
        }
    }

    public void Dispose()
    {
        // Nothing to dispose explicitly for now
    }
}
