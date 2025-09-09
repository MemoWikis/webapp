using Microsoft.Extensions.Hosting;
using System.Diagnostics;

/// <summary>
/// Background service that refreshes mmap cache files daily
/// </summary>
public class MmapCacheRefreshService : BackgroundService, IRegisterAsInstancePerLifetime
{
    private readonly PageViewMmapCache _pageViewMmapCache;
    private readonly QuestionViewMmapCache _questionViewMmapCache;
    private readonly PageViewRepo _pageViewRepo;
    private readonly QuestionViewRepository _questionViewRepo;

    public MmapCacheRefreshService(
        PageViewMmapCache pageViewMmapCache,
        QuestionViewMmapCache questionViewMmapCache,
        PageViewRepo pageViewRepo,
        QuestionViewRepository questionViewRepo)
    {
        _pageViewMmapCache = pageViewMmapCache;
        _questionViewMmapCache = questionViewMmapCache;
        _pageViewRepo = pageViewRepo;
        _questionViewRepo = questionViewRepo;
    }

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
                var allPageViews = _pageViewRepo.GetAllEager();
                _pageViewMmapCache.SaveAllPageViews(allPageViews);

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
                var allQuestionViews = _questionViewRepo.GetAllEager();
                _questionViewMmapCache.SaveAllQuestionViews(allQuestionViews);

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
    /// Manually trigger a cache refresh (for testing or admin purposes)
    /// </summary>
    public async Task TriggerManualRefresh()
    {
        Log.Information("Manual mmap cache refresh triggered");
        await RefreshMmapCaches();
    }
}
