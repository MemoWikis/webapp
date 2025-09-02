/// <summary>
/// Service for scheduling debounced knowledge summary updates via Rebus
/// </summary>
public class KnowledgeSummaryUpdateDispatcher(IMessageBusService _messageBusService)
{
    /// <summary>
    /// Schedule a debounced knowledge summary update for a page (awaitable)
    /// </summary>
    /// <param name="pageId">The page ID to update knowledge summaries for</param>
    public async Task SchedulePageUpdateAsync(int pageId)
    {
        var message = new UpdateKnowledgeSummaryMessage(pageId);
        await _messageBusService.SendAsync(message);
    }

    /// <summary>
    /// Schedule a debounced knowledge summary update for a user (awaitable)
    /// </summary>
    /// <param name="userId">The user ID to update knowledge summaries for</param>
    /// <param name="pageId">Optional page ID context</param>
    public async Task ScheduleUserUpdateAsync(int userId, int pageId = 0)
    {
        var message = new UpdateKnowledgeSummaryMessage(userId, pageId, UpdateType.User);
        await _messageBusService.SendAsync(message);
    }

    /// <summary>
    /// Schedule multiple page updates efficiently (awaitable)
    /// </summary>
    /// <param name="pageIds">List of page IDs to update</param>
    public async Task SchedulePageUpdatesAsync(IEnumerable<int> pageIds)
    {
        var tasks = pageIds.Select(SchedulePageUpdateAsync);
        await Task.WhenAll(tasks);
    }

    public async Task SchedulePageUpdatesAsync(IEnumerable<PageCacheItem> pages)
    {
        var tasks = pages.Select(page => SchedulePageUpdateAsync(page.Id));
        await Task.WhenAll(tasks);
    }

    public async Task SchedulePageUpdatesAsync(IEnumerable<Page> pages)
    {
        var tasks = pages.Select(page => SchedulePageUpdateAsync(page.Id));
        await Task.WhenAll(tasks);
    }

    public async Task ScheduleUserAndPageUpdateForProfilePageAsync(int userId, int pageId)
    {
        var message = new UpdateKnowledgeSummaryMessage(userId, pageId, forProfilePage: true);
        await _messageBusService.SendAsync(message);
    }

    /// <summary>
    /// Schedule a debounced knowledge summary update for a specific user and page combination (awaitable)
    /// </summary>
    /// <param name="userId">The user ID to update knowledge summaries for</param>
    /// <param name="pageId">The page ID to update knowledge summaries for</param>
    public async Task ScheduleUserAndPageUpdateAsync(int userId, int pageId)
    {
        var message = new UpdateKnowledgeSummaryMessage(userId, pageId);
        await _messageBusService.SendAsync(message);
    }
}