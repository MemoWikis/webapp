using Microsoft.Extensions.Hosting;
using System.Diagnostics;

/// <summary>
/// Background service that refreshes mmap cache files daily
/// </summary>
public class MmapCacheRefreshService(
    PageViewMmapCache pageViewMmapCache,
    QuestionViewMmapCache questionViewMmapCache,
    PageViewRepo pageViewRepo,
    QuestionViewRepository questionViewRepo)
    : BackgroundService, IRegisterAsInstancePerLifetime
{
    // Track first refresh after server restart
    private static bool _hasPerformedFirstRefresh = false;
    private static readonly object _firstRefreshLock = new();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Log.Information("MmapCacheRefreshService started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Calculate next refresh time (daily at 2 AM)
                var now = DateTime.Now;
                var nextRun = GetNextRefreshTime(now);
                var delay = nextRun - now;

                Log.Information("Next mmap cache refresh scheduled for {NextRun} (in {DelayHours:F1} hours)",
                    nextRun, delay.TotalHours);

                // Wait until it's time to refresh
                await Task.Delay(delay, stoppingToken);

                if (!stoppingToken.IsCancellationRequested)
                {
                    await RefreshMmapCaches();

                    // On first refresh after restart, update EntityCache views
                    lock (_firstRefreshLock)
                    {
                        if (!_hasPerformedFirstRefresh)
                        {
                            Log.Information("First refresh after restart - updating EntityCache views from mmap cache");
                            UpdateEntityCacheViewsFromMmap();
                            _hasPerformedFirstRefresh = true;
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Service is being stopped
                Log.Information("MmapCacheRefreshService stopping");
                break;
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Error in MmapCacheRefreshService. Will retry at next scheduled time.");

                // Wait 1 hour before retrying on error
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }

    /// <summary>
    /// Calculate the next refresh time (daily at 2 AM)
    /// </summary>
    private static DateTime GetNextRefreshTime(DateTime currentTime)
    {
        var refreshHour = 2; // 2 AM
        var today2Am = currentTime.Date.AddHours(refreshHour);

        // If it's already past 2 AM today, schedule for tomorrow at 2 AM
        if (currentTime >= today2Am)
        {
            return today2Am.AddDays(1);
        }

        // Otherwise, schedule for today at 2 AM
        return today2Am;
    }

    /// <summary>
    /// Refresh both PageView and QuestionView mmap caches from database
    /// </summary>
    private async Task RefreshMmapCaches()
    {
        var stopwatch = Stopwatch.StartNew();
        Log.Information("Starting daily mmap cache refresh");

        // Refresh PageView mmap cache
        await RefreshPageViewCache();

        // Refresh QuestionView mmap cache  
        await RefreshQuestionViewCache();

        stopwatch.Stop();
        Log.Information("Completed mmap cache refresh in {ElapsedMs} ms", stopwatch.ElapsedMilliseconds);
    }

    /// <summary>
    /// Refresh PageView mmap cache from database
    /// </summary>
    private async Task RefreshPageViewCache()
    {
        await Task.Run(() =>
        {
            var stopwatch = Stopwatch.StartNew();
            Log.Information("Refreshing PageView mmap cache from database");

            try
            {
                var allPageViews = pageViewRepo.GetAllEager();
                Log.Information("Refresh PageView pageViews loaded from db");
                pageViewMmapCache.SaveAllPageViews(allPageViews);

                stopwatch.Stop();
                Log.Information("Refreshed PageView mmap cache with {Count} views in {ElapsedMs} ms",
                    allPageViews.Count, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception exception)
            {
                stopwatch.Stop();
                Log.Error(exception, "Failed to refresh PageView mmap cache after {ElapsedMs} ms", stopwatch.ElapsedMilliseconds);
                throw;
            }
        });
    }

    /// <summary>
    /// Refresh QuestionView mmap cache from database
    /// </summary>
    private async Task RefreshQuestionViewCache()
    {
        await Task.Run(() =>
        {
            var stopwatch = Stopwatch.StartNew();
            Log.Information("Refreshing QuestionView mmap cache from database");

            try
            {
                var allQuestionViews = questionViewRepo.GetAllEager();
                questionViewMmapCache.SaveAllQuestionViews(allQuestionViews);

                stopwatch.Stop();
                Log.Information("Refreshed QuestionView mmap cache with {Count} views in {ElapsedMs} ms",
                    allQuestionViews.Count, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception exception)
            {
                stopwatch.Stop();
                Log.Error(exception, "Failed to refresh QuestionView mmap cache after {ElapsedMs} ms", stopwatch.ElapsedMilliseconds);
                throw;
            }
        });
    }

    /// <summary>
    /// Update only the view-related data in existing EntityCache from mmap cache
    /// Call this on first refresh after server restart to sync view data
    /// </summary>
    public void UpdateEntityCacheViewsFromMmap()
    {
        var stopwatch = Stopwatch.StartNew();
        Log.Information("Updating EntityCache views from mmap cache");

        try
        {
            // Update page views
            var cachedPageViews = pageViewMmapCache.LoadPageViews();
            if (cachedPageViews.Any())
            {
                UpdatePageViewsInEntityCache(cachedPageViews);
                Log.Information("Updated {PageCount} pages with view data from mmap cache",
                    cachedPageViews.GroupBy(v => v.PageId).Count());
            }

            // Update question views  
            var cachedQuestionViews = questionViewMmapCache.LoadQuestionViews();
            if (cachedQuestionViews.Any())
            {
                UpdateQuestionViewsInEntityCache(cachedQuestionViews);
                Log.Information("Updated {QuestionCount} questions with view data from mmap cache",
                    cachedQuestionViews.GroupBy(v => v.QuestionId).Count());
            }

            stopwatch.Stop();
            Log.Information("Updated EntityCache views from mmap cache in {ElapsedMs} ms", stopwatch.ElapsedMilliseconds);
        }
        catch (Exception exception)
        {
            stopwatch.Stop();
            Log.Error(exception, "Failed to update EntityCache views from mmap cache after {ElapsedMs} ms",
                stopwatch.ElapsedMilliseconds);
            throw;
        }
    }

    private void UpdatePageViewsInEntityCache(List<PageViewSummaryWithId> pageViews)
    {
        var pageViewsByPageId = pageViews.GroupBy(pv => pv.PageId).ToDictionary(g => g.Key, g => g.ToList());

        foreach (var (pageId, views) in pageViewsByPageId)
        {
            var pageCacheItem = EntityCache.GetPage(pageId);
            if (pageCacheItem != null)
            {
                // Convert to the format expected by SetPageViews
                var viewSummaries = views.Select(v => new PageViewSummaryWithId(
                    v.Count, v.DateOnly, v.PageId, v.DateCreated)).ToList();

                PageCacheItem.SetPageViews(pageCacheItem, viewSummaries);
            }
        }
    }

    private void UpdateQuestionViewsInEntityCache(List<QuestionViewSummaryWithId> questionViews)
    {
        var questionViewsByQuestionId = questionViews.GroupBy(qv => qv.QuestionId).ToDictionary(g => g.Key, g => g.ToList());

        foreach (var (questionId, views) in questionViewsByQuestionId)
        {
            var questionCacheItem = EntityCache.GetQuestion(questionId);
            if (questionCacheItem != null)
            {
                // Update TotalViews
                questionCacheItem.TotalViews = (int)views.Sum(qv => qv.Count);

                // Update ViewsOfPast90Days (same logic as in ToCacheQuestion)
                var startDate = DateTime.Now.Date.AddDays(-90);
                var endDate = DateTime.Now.Date;
                var dateRange = Enumerable.Range(0, (endDate - startDate).Days + 1)
                    .Select(d => startDate.AddDays(d));

                questionCacheItem.ViewsOfPast90Days = views
                    .Where(qv => dateRange.Contains(qv.DateOnly))
                    .Select(qv => new DailyViews { Date = qv.DateOnly, Count = qv.Count })
                    .OrderBy(v => v.Date)
                    .ToList();
            }
        }
    }

    /// <summary>
    /// Manually trigger a cache refresh (for testing or admin purposes)
    /// </summary>
    public async Task TriggerManualRefresh()
    {
        Log.Information("Manual mmap cache refresh triggered");
        await RefreshMmapCaches();
    }

    public void DeleteAllCacheFiles()
    {
        try
        {
            pageViewMmapCache.DeleteCacheFile();
            questionViewMmapCache.DeleteCacheFile();
            Log.Information("Deleted all mmap cache files");
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Failed to delete mmap cache files");
            throw;
        }
    }
}
