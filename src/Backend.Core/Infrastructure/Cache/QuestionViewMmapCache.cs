using MessagePack;

[MessagePackObject]
public record struct QuestionViewSummaryWithId(
    [property: Key(0)] Int64 Count, 
    [property: Key(1)] DateTime DateOnly, 
    [property: Key(2)] int QuestionId, 
    [property: Key(3)] DateTime DateCreated);

public class QuestionViewMmapCache : IRegisterAsInstancePerLifetime, IDisposable
{
    private readonly string _questionViewsFilePath;
    private readonly object _questionViewLock = new();

    public QuestionViewMmapCache()
    {
        var cacheDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "viewcache");
        Directory.CreateDirectory(cacheDirectory);

        _questionViewsFilePath = Path.Combine(cacheDirectory, "questionviews.mmap");
    }

    public (List<QuestionViewSummaryWithId> views, DateTime? lastEntryDate) LoadQuestionViews()
    {
        if (!File.Exists(_questionViewsFilePath))
        {
            return (new List<QuestionViewSummaryWithId>(), null);
        }

        try
        {
            lock (_questionViewLock)
            {
                using var fileStream = new FileStream(_questionViewsFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var views = MessagePackSerializer.Deserialize<List<QuestionViewSummaryWithId>>(fileStream);

                var lastEntryDate = views.Any()
                    ? views.Max(view => view.DateCreated)
                    : (DateTime?)null;

                Log.Information($"Loaded {views.Count} question views from mmap cache. Last entry: {lastEntryDate}");
                return (views, lastEntryDate);
            }
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Failed to load question views from mmap cache");
            // Delete corrupted file
            File.Delete(_questionViewsFilePath);
            return (new List<QuestionViewSummaryWithId>(), null);
        }
    }

    public void AppendQuestionView(QuestionViewSummaryWithId view)
    {
        // TODO: Implement append functionality
        // For now, just log that we would append
        Log.Debug("Would append question view to mmap cache: QuestionId={QuestionId}, DateOnly={DateOnly}", view.QuestionId, view.DateOnly);
    }

    public void SaveAllQuestionViews(IList<QuestionViewSummaryWithId> views)
    {
        lock (_questionViewLock)
        {
            var tempFile = _questionViewsFilePath + ".tmp";
            File.WriteAllBytes(tempFile, MessagePackSerializer.Serialize(views));
            File.Move(tempFile, _questionViewsFilePath, true);
            Log.Information($"Saved {views.Count} question views to mmap cache");
        }
    }

    public void Dispose()
    {
        // Nothing to dispose explicitly for now
    }
}
