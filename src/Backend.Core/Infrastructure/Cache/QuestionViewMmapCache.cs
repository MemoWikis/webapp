using MessagePack;

[MessagePackObject]
public record struct QuestionViewSummaryWithId(
    [property: Key(0)] Int64 Count, 
    [property: Key(1)] DateTime DateOnly, 
    [property: Key(2)] int QuestionId, 
    [property: Key(3)] DateTime DateCreated);

public class QuestionViewMmapCache : IRegisterAsInstancePerLifetime
{
    private readonly string _questionViewsFilePath;
    private readonly object _questionViewLock = new();

    public QuestionViewMmapCache()
    {
        var cacheDirectory = Settings.MmapCachePath;
        Directory.CreateDirectory(cacheDirectory);

        _questionViewsFilePath = Path.Combine(cacheDirectory, "questionviews.mmap");
    }

    public List<QuestionViewSummaryWithId> LoadQuestionViews()
    {
        if (!File.Exists(_questionViewsFilePath))
        {
            return new List<QuestionViewSummaryWithId>();
        }

        try
        {
            lock (_questionViewLock)
            {
                using var fileStream = new FileStream(_questionViewsFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var views = MessagePackSerializer.Deserialize<List<QuestionViewSummaryWithId>>(fileStream);

                Log.Information($"Loaded {views.Count} question views from mmap cache");
                return views;
            }
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Failed to load question views from mmap cache");
            // Delete corrupted file
            File.Delete(_questionViewsFilePath);
            return new List<QuestionViewSummaryWithId>();
        }
    }

    public void AppendQuestionView(QuestionViewSummaryWithId view)
    {
        try
        {
            lock (_questionViewLock)
            {
                // Load existing views
                var existingViews = new List<QuestionViewSummaryWithId>();
                if (File.Exists(_questionViewsFilePath))
                {
                    using var readStream = new FileStream(_questionViewsFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    existingViews = MessagePackSerializer.Deserialize<List<QuestionViewSummaryWithId>>(readStream);
                }

                // Add the new view
                existingViews.Add(view);

                // Write back all views
                var tempFile = _questionViewsFilePath + ".tmp";
                File.WriteAllBytes(tempFile, MessagePackSerializer.Serialize(existingViews));
                File.Move(tempFile, _questionViewsFilePath, true);

                Log.Debug("Appended question view to mmap cache: QuestionId={QuestionId}, DateOnly={DateOnly}, Total views: {Count}",
                    view.QuestionId, view.DateOnly, existingViews.Count);
            }
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Failed to append question view to mmap cache: QuestionId={QuestionId}", view.QuestionId);
        }
    }

    public void AppendQuestionViews(IEnumerable<QuestionViewSummaryWithId> views)
    {
        var viewsList = views.ToList();
        if (!viewsList.Any())
        {
            Log.Debug("No question views to append to mmap cache");
            return;
        }

        try
        {
            lock (_questionViewLock)
            {
                // Load existing views
                var existingViews = new List<QuestionViewSummaryWithId>();
                if (File.Exists(_questionViewsFilePath))
                {
                    using var readStream = new FileStream(_questionViewsFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    existingViews = MessagePackSerializer.Deserialize<List<QuestionViewSummaryWithId>>(readStream);
                }

                // Add all new views
                existingViews.AddRange(viewsList);

                // Write back all views
                var tempFile = _questionViewsFilePath + ".tmp";
                File.WriteAllBytes(tempFile, MessagePackSerializer.Serialize(existingViews));
                File.Move(tempFile, _questionViewsFilePath, true);

                Log.Information("Appended {AppendedCount} question views to mmap cache. Total views: {TotalCount}",
                    viewsList.Count, existingViews.Count);
            }
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Failed to append {Count} question views to mmap cache", viewsList.Count);
        }
    }

    public void DeleteQuestionViews(int questionId)
    {
        try
        {
            lock (_questionViewLock)
            {
                if (!File.Exists(_questionViewsFilePath))
                {
                    Log.Debug("No mmap cache file exists for deleting question views: QuestionId={QuestionId}", questionId);
                    return;
                }

                // Load existing views
                using var readStream = new FileStream(_questionViewsFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var existingViews = MessagePackSerializer.Deserialize<List<QuestionViewSummaryWithId>>(readStream);
                readStream.Close();

                // Count views to be deleted
                var viewsToDelete = existingViews.Count(v => v.QuestionId == questionId);

                if (viewsToDelete == 0)
                {
                    Log.Debug("No question views found to delete for QuestionId={QuestionId}", questionId);
                    return;
                }

                // Filter out views for the specified questionId
                var filteredViews = existingViews.Where(v => v.QuestionId != questionId).ToList();

                // Write back filtered views
                var tempFile = _questionViewsFilePath + ".tmp";
                File.WriteAllBytes(tempFile, MessagePackSerializer.Serialize(filteredViews));
                File.Move(tempFile, _questionViewsFilePath, true);

                Log.Information("Deleted {DeletedCount} question views for QuestionId={QuestionId}. Remaining views: {RemainingCount}",
                    viewsToDelete, questionId, filteredViews.Count);
            }
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Failed to delete question views from mmap cache: QuestionId={QuestionId}", questionId);
        }
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

    public void DeleteCacheFile()
    {
        lock (_questionViewLock)
        {
            File.Delete(_questionViewsFilePath);
        }
    }
}
