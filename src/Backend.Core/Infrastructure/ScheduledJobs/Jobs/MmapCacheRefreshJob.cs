using Quartz;
using System.Diagnostics;

public class MmapCacheRefreshJob : IJob
{
    private readonly PageViewMmapCache _pageViewMmapCache;
    private readonly QuestionViewMmapCache _questionViewMmapCache;
    private readonly PageViewRepo _pageViewRepo;
    private readonly QuestionViewRepository _questionViewRepo;

    public MmapCacheRefreshJob(
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

    public Task Execute(IJobExecutionContext context)
    {
        JobExecute.Run(scope =>
        {
            RefreshMmapCaches();
        }, "MmapCacheRefreshJob");

        return Task.CompletedTask;
    }

    /// <summary>
    /// Refresh both PageView and QuestionView mmap caches from database
    /// </summary>
    private void RefreshMmapCaches()
    {
        var stopwatch = Stopwatch.StartNew();
        Log.Information("Starting daily mmap cache refresh");

        // Refresh PageView mmap cache
        RefreshPageViewCache();

        // Refresh QuestionView mmap cache  
        RefreshQuestionViewCache();

        stopwatch.Stop();
        Log.Information("Completed mmap cache refresh in {ElapsedMs} ms", stopwatch.ElapsedMilliseconds);
    }

    /// <summary>
    /// Refresh PageView mmap cache from database
    /// </summary>
    private void RefreshPageViewCache()
    {
        var stopwatch = Stopwatch.StartNew();
        Log.Information("Refreshing PageView mmap cache from database");

        try
        {
            var allPageViews = _pageViewRepo.GetAllEager();
            Log.Information("Refresh PageView pageViews loaded from db");
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
    }

    /// <summary>
    /// Refresh QuestionView mmap cache from database
    /// </summary>
    private void RefreshQuestionViewCache()
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
    }
}