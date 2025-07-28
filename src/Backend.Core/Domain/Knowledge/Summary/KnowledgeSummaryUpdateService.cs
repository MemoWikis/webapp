/// <summary>
/// Service for scheduling debounced knowledge summary updates via Rebus
/// </summary>
public class KnowledgeSummaryUpdateService
{
    private readonly IMessageBusService _messageBusService;

    public KnowledgeSummaryUpdateService(IMessageBusService messageBusService)
    {
        _messageBusService = messageBusService;
    }

    /// <summary>
    /// Schedule a debounced knowledge summary update for a page (fire-and-forget)
    /// </summary>
    /// <param name="pageId">The page ID to update knowledge summaries for</param>
    public void SchedulePageUpdate(int pageId)
    {
        var message = new UpdateKnowledgeSummaryMessage(pageId, UpdateType.Page);
        _messageBusService.SendAsync(message);
    }

    /// <summary>
    /// Schedule a debounced knowledge summary update for a page (awaitable)
    /// </summary>
    /// <param name="pageId">The page ID to update knowledge summaries for</param>
    public async Task SchedulePageUpdateAsync(int pageId)
    {
        var message = new UpdateKnowledgeSummaryMessage(pageId, UpdateType.Page);
        await _messageBusService.SendAsync(message);
    }

    /// <summary>
    /// Schedule a debounced knowledge summary update for a user (fire-and-forget)
    /// </summary>
    /// <param name="userId">The user ID to update knowledge summaries for</param>
    /// <param name="pageId">Optional page ID context</param>
    public void ScheduleUserUpdate(int userId, int pageId = 0)
    {
        var message = new UpdateKnowledgeSummaryMessage(userId, pageId, UpdateType.User);
        _messageBusService.SendAsync(message);
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
    /// Schedule multiple page updates efficiently (fire-and-forget)
    /// </summary>
    /// <param name="pageIds">List of page IDs to update</param>
    public void SchedulePageUpdates(IEnumerable<int> pageIds)
    {
        foreach (var pageId in pageIds)
        {
            SchedulePageUpdate(pageId);
        }
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

    /// <summary>
    /// Schedule a debounced knowledge summary update for a specific user and page combination (fire-and-forget)
    /// </summary>
    /// <param name="userId">The user ID to update knowledge summaries for</param>
    /// <param name="pageId">The page ID to update knowledge summaries for</param>
    public void ScheduleUserAndPageUpdate(int userId, int pageId)
    {
        var message = new UpdateKnowledgeSummaryMessage(userId, pageId);
        _messageBusService.SendAsync(message);
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
