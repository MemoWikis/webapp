using Rebus.Handlers;

public class UpdateQuestionViewMmapCacheMessageHandler(
    QuestionViewMmapCache _questionViewMmapCache,
    QuestionViewRepository _questionViewRepo) : IHandleMessages<UpdateQuestionViewMmapCacheMessage>
{
    public async Task Handle(UpdateQuestionViewMmapCacheMessage message)
    {
        try
        {
            switch (message.UpdateType)
            {
                case QuestionViewMmapCacheUpdateType.AddView:
                    HandleAddView(message);
                    break;

                case QuestionViewMmapCacheUpdateType.AddViews:
                    HandleAddViews(message);
                    break;

                case QuestionViewMmapCacheUpdateType.DeleteViews:
                    HandleDeleteViews(message);
                    break;

                case QuestionViewMmapCacheUpdateType.RefreshMmap:
                    await HandleRefreshMmapAsync();
                    break;

                case QuestionViewMmapCacheUpdateType.RebuildMmap:
                    await HandleRebuildMmapAsync();
                    break;

                default:
                    Log.Warning("Unknown QuestionViewMmapCacheUpdateType: {UpdateType}", message.UpdateType);
                    break;
            }
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Error handling QuestionView mmap cache update: {UpdateType}", message.UpdateType);
            throw;
        }
    }

    private void HandleAddView(UpdateQuestionViewMmapCacheMessage message)
    {
        if (message.QuestionViewSummary.HasValue)
        {
            _questionViewMmapCache.AppendQuestionView(message.QuestionViewSummary.Value);
            Log.Debug("Added single question view to mmap cache: QuestionId={QuestionId}", message.QuestionViewSummary.Value.QuestionId);
        }
        else
        {
            Log.Warning("AddView message missing QuestionViewSummary");
        }
    }

    private void HandleAddViews(UpdateQuestionViewMmapCacheMessage message)
    {
        if (message.QuestionViewSummaries?.Any() == true)
        {
            _questionViewMmapCache.AppendQuestionViews(message.QuestionViewSummaries);
            Log.Debug("Added {Count} question views to mmap cache", message.QuestionViewSummaries.Count);
        }
        else
        {
            Log.Warning("AddViews message missing or empty QuestionViewSummaries");
        }
    }

    private void HandleDeleteViews(UpdateQuestionViewMmapCacheMessage message)
    {
        if (message.QuestionId.HasValue)
        {
            _questionViewMmapCache.DeleteQuestionViews(message.QuestionId.Value);
            Log.Debug("Deleted question views from mmap cache for QuestionId={QuestionId}", message.QuestionId.Value);
        }
        else
        {
            Log.Warning("DeleteViews message missing QuestionId");
        }
    }

    private async Task HandleRefreshMmapAsync()
    {
        Log.Information("Starting QuestionView mmap cache refresh");
        
        try
        {
            // Load current mmap cache to get the latest entry date
            var (cachedViews, lastEntryDate) = _questionViewMmapCache.LoadQuestionViews();
            
            if (lastEntryDate.HasValue)
            {
                // Get newer views from database since the last cached entry
                var newerViews = _questionViewRepo.GetAllEagerSince(lastEntryDate.Value);
                
                if (newerViews.Any())
                {
                    _questionViewMmapCache.AppendQuestionViews(newerViews);
                    Log.Information("Refreshed mmap cache with {Count} newer question views since {LastDate}", 
                        newerViews.Count, lastEntryDate.Value);
                }
                else
                {
                    Log.Information("No newer question views found since {LastDate}", lastEntryDate.Value);
                }
            }
            else
            {
                // No cached data, perform full rebuild
                Log.Information("No cached data found, performing full rebuild");
                await HandleRebuildMmapAsync();
            }
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Error during QuestionView mmap cache refresh");
            throw;
        }
    }

    private async Task HandleRebuildMmapAsync()
    {
        Log.Information("Starting QuestionView mmap cache rebuild");
        
        await Task.Run(() =>
        {
            try
            {
                // Get all question views from database
                var allViews = _questionViewRepo.GetAllEager();
                
                // Save all views to mmap cache (this will overwrite existing cache)
                _questionViewMmapCache.SaveAllQuestionViews(allViews);
                
                Log.Information("Rebuilt mmap cache with {Count} question views", allViews.Count);
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Error during QuestionView mmap cache rebuild");
                throw;
            }
        });
    }
}
