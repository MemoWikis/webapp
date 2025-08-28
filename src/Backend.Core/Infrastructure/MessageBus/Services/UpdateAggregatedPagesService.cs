/// <summary>
/// Service for updating aggregated pages when questions are modified
/// </summary>
public class UpdateAggregatedPagesService
{
    private readonly IMessageBusService _messageBusService;

    public UpdateAggregatedPagesService(IMessageBusService messageBusService)
    {
        _messageBusService = messageBusService;
    }

    /// <summary>
    /// Update aggregated pages for question changes (fire-and-forget)
    /// </summary>
    /// <param name="pageIds">List of page IDs affected by question changes</param>
    /// <param name="userId">User ID performing the action</param>
    public void UpdateAggregatedPages(List<int> pageIds, int userId = -1)
    {
        var message = new UpdateAggregatedPagesMessage(pageIds, userId);
        _messageBusService.SendAsync(message);
    }

    /// <summary>
    /// Update aggregated pages for question changes (awaitable)
    /// </summary>
    /// <param name="pageIds">List of page IDs affected by question changes</param>
    /// <param name="userId">User ID performing the action</param>
    public async Task UpdateAggregatedPagesAsync(List<int> pageIds, int userId = -1)
    {
        var message = new UpdateAggregatedPagesMessage(pageIds, userId);
        await _messageBusService.SendAsync(message);
    }
}
