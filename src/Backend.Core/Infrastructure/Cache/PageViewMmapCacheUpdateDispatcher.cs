/// <summary>
/// Service for scheduling background PageView mmap cache updates via Rebus
/// </summary>
public class PageViewMmapCacheUpdateDispatcher(IMessageBusService _messageBusService)
{
    /// <summary>
    /// Schedule adding a single page view to the mmap cache (awaitable)
    /// </summary>
    /// <param name="pageViewSummary">The page view to add</param>
    public async Task ScheduleAddViewAsync(PageViewSummaryWithId pageViewSummary)
    {
        var message = new UpdatePageViewMmapCacheMessage(
            PageViewMmapCacheUpdateType.AddView, 
            pageViewSummary: pageViewSummary);
        await _messageBusService.SendAsync(message);
    }

    /// <summary>
    /// Schedule adding multiple page views to the mmap cache (awaitable)
    /// </summary>
    /// <param name="pageViewSummaries">The page views to add</param>
    public async Task ScheduleAddViewsAsync(IEnumerable<PageViewSummaryWithId> pageViewSummaries)
    {
        var message = new UpdatePageViewMmapCacheMessage(
            PageViewMmapCacheUpdateType.AddViews, 
            pageViewSummaries: pageViewSummaries.ToList());
        await _messageBusService.SendAsync(message);
    }

    /// <summary>
    /// Schedule deletion of all page views for a specific page ID (awaitable)
    /// </summary>
    /// <param name="pageId">The page ID to delete views for</param>
    public async Task ScheduleDeleteViewsAsync(int pageId)
    {
        var message = new UpdatePageViewMmapCacheMessage(
            PageViewMmapCacheUpdateType.DeleteViews, 
            pageId: pageId);
        await _messageBusService.SendAsync(message);
    }

    /// <summary>
    /// Schedule a refresh/sync of the mmap cache with the database (awaitable)
    /// This will check for newer views in the database and append them to the mmap cache
    /// </summary>
    public async Task ScheduleRefreshMmapAsync()
    {
        var message = new UpdatePageViewMmapCacheMessage(
            PageViewMmapCacheUpdateType.RefreshMmap);
        await _messageBusService.SendAsync(message);
    }

    /// <summary>
    /// Schedule a full rebuild of the mmap cache from the database (awaitable)
    /// </summary>
    public async Task ScheduleRebuildMmapAsync()
    {
        var message = new UpdatePageViewMmapCacheMessage(
            PageViewMmapCacheUpdateType.RebuildMmap);
        await _messageBusService.SendAsync(message);
    }
}
