using System.Diagnostics;

/// <summary>
/// Service for mmap cache operations and manual cache management
/// Daily recreate is now handled by MmapCacheRefreshJob scheduled via JobScheduler
/// </summary>
public class MmapCacheRefreshService(
    PageViewMmapCache pageViewMmapCache,
    QuestionViewMmapCache questionViewMmapCache,
    PageViewRepo pageViewRepo,
    QuestionViewRepository questionViewRepo)
    : IRegisterAsInstancePerLifetime
{
    /// <summary>
    /// Manually trigger a cache recreate (for testing or admin purposes)
    /// </summary>
    public void Refresh(string? jobTrackingId = null)
    {
        Log.Information("Manual mmap cache recreate triggered");
        RecreateMmapCaches(jobTrackingId);
    }

    /// <summary>
    /// Refresh both PageView and QuestionView mmap caches from database
    /// </summary>
    private void RecreateMmapCaches(string? jobTrackingId = null)
    {
        var stopwatch = Stopwatch.StartNew();
        Log.Information("Starting daily mmap cache recreate");

        JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running, "Refreshing PageView mmap cache...",
            "RefreshMmapCaches");

        // Refresh PageView mmap cache
        RecreatePageViewCache();

        JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running, "Refreshing QuestionView mmap cache...",
            "RefreshMmapCaches");

        // Refresh QuestionView mmap cache  
        RecreateQuestionViewCache();

        stopwatch.Stop();
        Log.Information("Completed mmap cache recreate in {ElapsedMs} ms", stopwatch.ElapsedMilliseconds);
    }

    /// <summary>
    /// Refresh PageView mmap cache from database
    /// </summary>
    private void RecreatePageViewCache()
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
            Log.Error(exception, "Failed to recreate PageView mmap cache after {ElapsedMs} ms",
                stopwatch.ElapsedMilliseconds);
            throw;
        }
    }

    /// <summary>
    /// Refresh QuestionView mmap cache from database
    /// </summary>
    private void RecreateQuestionViewCache()
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
            Log.Error(exception, "Failed to recreate QuestionView mmap cache after {ElapsedMs} ms",
                stopwatch.ElapsedMilliseconds);
            throw;
        }
    }

    /// <summary>
    /// Update only the view-related data in existing EntityCache from mmap cache
    /// NOTE: This is primarily used for testing since EntityCacheInitializer now loads 
    /// today's views directly from the database during startup
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
            Log.Information("Updated EntityCache views from mmap cache in {ElapsedMs} ms",
                stopwatch.ElapsedMilliseconds);
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
                    v.Count, v.DateOnly, v.PageId, v.LastPageViewCreatedAt)).ToList();

                PageCacheItem.SetPageViews(pageCacheItem, viewSummaries);
            }
        }
    }

    private void UpdateQuestionViewsInEntityCache(List<QuestionViewSummaryWithId> questionViews)
    {
        var questionViewsByQuestionId =
            questionViews.GroupBy(qv => qv.QuestionId).ToDictionary(g => g.Key, g => g.ToList());

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

    /// <summary>
    /// Load today's views in background and update EntityCache (called during startup)
    /// </summary>
    public void LoadTodaysViewsInBackground()
    {
        _ = Task.Run(async () =>
        {
            try
            {
                // Small delay to ensure EntityCache is fully initialized
                await Task.Delay(500);

                var today = DateTime.UtcNow.Date;
                Log.Information("Background: Loading today's views from database");

                // Load today's page views
                var todaysPageViews = pageViewRepo.GetAllEagerSince(today);
                if (todaysPageViews.Any())
                {
                    UpdatePageViewsInEntityCache(todaysPageViews.ToList());
                    Log.Information("Background: Updated EntityCache with {count} page view entries for today",
                        todaysPageViews.Count);
                }

                // Load today's question views
                var todaysQuestionViews = questionViewRepo.GetAllEagerSince(today);
                if (todaysQuestionViews.Any())
                {
                    UpdateQuestionViewsInEntityCache(todaysQuestionViews.ToList());
                    Log.Information("Background: Updated EntityCache with {count} question view entries for today",
                        todaysQuestionViews.Count);
                }

                Log.Information("Background: Completed loading today's views");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Background: Failed to load today's views");
            }
        });
    }
}