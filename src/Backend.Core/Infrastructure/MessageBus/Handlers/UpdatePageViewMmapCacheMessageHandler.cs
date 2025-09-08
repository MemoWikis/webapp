using Rebus.Handlers;
public class UpdatePageViewMmapCacheMessageHandler(
    PageViewMmapCache _pageViewMmapCache,
    PageViewRepo _pageViewRepo) : IHandleMessages<UpdatePageViewMmapCacheMessage>
{
    public async Task Handle(UpdatePageViewMmapCacheMessage message)
    {
        try
        {
            switch (message.UpdateType)
            {
                case PageViewMmapCacheUpdateType.AddView:
                    HandleAddView(message);
                    break;

                case PageViewMmapCacheUpdateType.AddViews:
                    HandleAddViews(message);
                    break;

                case PageViewMmapCacheUpdateType.DeleteViews:
                    HandleDeleteViews(message);
                    break;

                case PageViewMmapCacheUpdateType.RefreshMmap:
                    await HandleRefreshMmapAsync();
                    break;

                case PageViewMmapCacheUpdateType.RebuildMmap:
                    await HandleRebuildMmapAsync();
                    break;

                default:
                    Log.Warning("Unknown PageViewMmapCacheUpdateType: {UpdateType}", message.UpdateType);
                    break;
            }
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Error handling PageView mmap cache update: {UpdateType}", message.UpdateType);
            throw;
        }
    }

    private void HandleAddView(UpdatePageViewMmapCacheMessage message)
    {
        if (message.PageViewSummary.HasValue)
        {
            _pageViewMmapCache.AppendPageView(message.PageViewSummary.Value);
            Log.Debug("Added single page view to mmap cache: PageId={PageId}", message.PageViewSummary.Value.PageId);
        }
        else
        {
            Log.Warning("AddView message missing PageViewSummary");
        }
    }

    private void HandleAddViews(UpdatePageViewMmapCacheMessage message)
    {
        if (message.PageViewSummaries?.Any() == true)
        {
            _pageViewMmapCache.AppendPageViews(message.PageViewSummaries);
            Log.Debug("Added {Count} page views to mmap cache", message.PageViewSummaries.Count);
        }
        else
        {
            Log.Warning("AddViews message missing or empty PageViewSummaries");
        }
    }

    private void HandleDeleteViews(UpdatePageViewMmapCacheMessage message)
    {
        if (message.PageId.HasValue)
        {
            _pageViewMmapCache.DeletePageViews(message.PageId.Value);
            Log.Debug("Deleted page views from mmap cache for PageId={PageId}", message.PageId.Value);
        }
        else
        {
            Log.Warning("DeleteViews message missing PageId");
        }
    }

    private async Task HandleRefreshMmapAsync()
    {
        Log.Information("Starting PageView mmap cache refresh");
        
        try
        {
            // Load current mmap cache to get the latest entry date
            var (cachedViews, lastEntryDate) = _pageViewMmapCache.LoadPageViews();
            
            if (lastEntryDate.HasValue)
            {
                // Get newer views from database since the last cached entry
                var newerViews = _pageViewRepo.GetAllEagerSince(lastEntryDate.Value);
                
                if (newerViews.Any())
                {
                    _pageViewMmapCache.AppendPageViews(newerViews);
                    Log.Information("Refreshed mmap cache with {Count} newer page views since {LastDate}", 
                        newerViews.Count, lastEntryDate.Value);
                }
                else
                {
                    Log.Information("No newer page views found since {LastDate}", lastEntryDate.Value);
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
            Log.Error(exception, "Error during PageView mmap cache refresh");
            throw;
        }
    }

    private async Task HandleRebuildMmapAsync()
    {
        Log.Information("Starting PageView mmap cache rebuild");
        
        await Task.Run(() =>
        {
            try
            {
                // Get all page views from database
                var allViews = _pageViewRepo.GetAllEager();
                
                // Save all views to mmap cache (this will overwrite existing cache)
                _pageViewMmapCache.SaveAllPageViews(allViews);
                
                Log.Information("Rebuilt mmap cache with {Count} page views", allViews.Count);
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Error during PageView mmap cache rebuild");
                throw;
            }
        });
    }
}
