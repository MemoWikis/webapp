using MessagePack;

[MessagePackObject]
public record struct PageViewSummaryWithId(
    [property: Key(0)] Int64 Count,
    [property: Key(1)] DateTime DateOnly,
    [property: Key(2)] int PageId,
    [property: Key(3)] DateTime LastPageViewCreatedAt);

public class PageViewMmapCache : IRegisterAsInstancePerLifetime
{
    private readonly string _pageViewsFilePath;
    private readonly object _pageViewLock = new();

    public PageViewMmapCache()
    {
        var cacheDirectory = Settings.MmapCachePath;
        Directory.CreateDirectory(cacheDirectory);

        _pageViewsFilePath = Path.Combine(cacheDirectory, "pageviews.mmap");
    }

    public List<PageViewSummaryWithId> LoadPageViews()
    {
        if (!File.Exists(_pageViewsFilePath))
        {
            return new List<PageViewSummaryWithId>();
        }

        try
        {
            lock (_pageViewLock)
            {
                using var fileStream =
                    new FileStream(_pageViewsFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var views = MessagePackSerializer.Deserialize<List<PageViewSummaryWithId>>(fileStream);

                Log.Information($"Loaded {views.Count} page views from mmap cache");
                return views;
            }
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Failed to load page views from mmap cache");
            // Delete corrupted file
            File.Delete(_pageViewsFilePath);
            return new List<PageViewSummaryWithId>();
        }
    }

    public void AppendPageView(PageViewSummaryWithId view)
    {
        try
        {
            lock (_pageViewLock)
            {
                // Load existing views
                var existingViews = new List<PageViewSummaryWithId>();
                if (File.Exists(_pageViewsFilePath))
                {
                    using var readStream = new FileStream(_pageViewsFilePath, FileMode.Open, FileAccess.Read,
                        FileShare.Read);
                    existingViews = MessagePackSerializer.Deserialize<List<PageViewSummaryWithId>>(readStream);
                }

                // Add the new view
                existingViews.Add(view);

                // Write back all views
                var tempFile = _pageViewsFilePath + ".tmp";
                File.WriteAllBytes(tempFile, MessagePackSerializer.Serialize(existingViews));
                File.Move(tempFile, _pageViewsFilePath, true);

                Log.Debug(
                    "Appended page view to mmap cache: PageId={PageId}, DateOnly={DateOnly}, Total views: {Count}",
                    view.PageId, view.DateOnly, existingViews.Count);
            }
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Failed to append page view to mmap cache: PageId={PageId}", view.PageId);
        }
    }

    public void AppendPageViews(IEnumerable<PageViewSummaryWithId> views)
    {
        var viewsList = views.ToList();
        if (!viewsList.Any())
        {
            Log.Debug("No page views to append to mmap cache");
            return;
        }

        try
        {
            lock (_pageViewLock)
            {
                // Load existing views
                var existingViews = new List<PageViewSummaryWithId>();
                if (File.Exists(_pageViewsFilePath))
                {
                    using var readStream = new FileStream(_pageViewsFilePath, FileMode.Open, FileAccess.Read,
                        FileShare.Read);
                    existingViews = MessagePackSerializer.Deserialize<List<PageViewSummaryWithId>>(readStream);
                }

                // Add all new views
                existingViews.AddRange(viewsList);

                // Write back all views
                var tempFile = _pageViewsFilePath + ".tmp";
                File.WriteAllBytes(tempFile, MessagePackSerializer.Serialize(existingViews));
                File.Move(tempFile, _pageViewsFilePath, true);

                Log.Information("Appended {AppendedCount} page views to mmap cache. Total views: {TotalCount}",
                    viewsList.Count, existingViews.Count);
            }
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Failed to append {Count} page views to mmap cache", viewsList.Count);
        }
    }

    public void DeletePageViews(int pageId)
    {
        try
        {
            lock (_pageViewLock)
            {
                if (!File.Exists(_pageViewsFilePath))
                {
                    Log.Debug("No mmap cache file exists for deleting page views: PageId={PageId}", pageId);
                    return;
                }

                // Load existing views
                using var readStream =
                    new FileStream(_pageViewsFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var existingViews = MessagePackSerializer.Deserialize<List<PageViewSummaryWithId>>(readStream);
                readStream.Close();

                // Count views to be deleted
                var viewsToDelete = existingViews.Count(v => v.PageId == pageId);

                if (viewsToDelete == 0)
                {
                    Log.Debug("No page views found to delete for PageId={PageId}", pageId);
                    return;
                }

                // Filter out views for the specified pageId
                var filteredViews = existingViews.Where(v => v.PageId != pageId).ToList();

                // Write back filtered views
                var tempFile = _pageViewsFilePath + ".tmp";
                File.WriteAllBytes(tempFile, MessagePackSerializer.Serialize(filteredViews));
                File.Move(tempFile, _pageViewsFilePath, true);

                Log.Information(
                    "Deleted {DeletedCount} page views for PageId={PageId}. Remaining views: {RemainingCount}",
                    viewsToDelete, pageId, filteredViews.Count);
            }
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Failed to delete page views from mmap cache: PageId={PageId}", pageId);
        }
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

    public void DeleteCacheFile()
    {
        lock (_pageViewLock)
        {
            File.Delete(_pageViewsFilePath);
        }
    }
}