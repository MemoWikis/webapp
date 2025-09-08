/// <summary>
/// Service for scheduling background QuestionView mmap cache updates via Rebus
/// </summary>
public class QuestionViewMmapCacheUpdateDispatcher(IMessageBusService _messageBusService)
{
    /// <summary>
    /// Schedule adding a single question view to the mmap cache (awaitable)
    /// </summary>
    /// <param name="questionViewSummary">The question view to add</param>
    public async Task ScheduleAddViewAsync(QuestionViewSummaryWithId questionViewSummary)
    {
        var message = new UpdateQuestionViewMmapCacheMessage(
            QuestionViewMmapCacheUpdateType.AddView, 
            questionViewSummary: questionViewSummary);
        await _messageBusService.SendAsync(message);
    }

    /// <summary>
    /// Schedule adding multiple question views to the mmap cache (awaitable)
    /// </summary>
    /// <param name="questionViewSummaries">The question views to add</param>
    public async Task ScheduleAddViewsAsync(IEnumerable<QuestionViewSummaryWithId> questionViewSummaries)
    {
        var message = new UpdateQuestionViewMmapCacheMessage(
            QuestionViewMmapCacheUpdateType.AddViews, 
            questionViewSummaries: questionViewSummaries.ToList());
        await _messageBusService.SendAsync(message);
    }

    /// <summary>
    /// Schedule deletion of all question views for a specific question ID (awaitable)
    /// </summary>
    /// <param name="questionId">The question ID to delete views for</param>
    public async Task ScheduleDeleteViewsAsync(int questionId)
    {
        var message = new UpdateQuestionViewMmapCacheMessage(
            QuestionViewMmapCacheUpdateType.DeleteViews, 
            questionId: questionId);
        await _messageBusService.SendAsync(message);
    }

    /// <summary>
    /// Schedule a refresh/sync of the mmap cache with the database (awaitable)
    /// This will check for newer views in the database and append them to the mmap cache
    /// </summary>
    public async Task ScheduleRefreshMmapAsync()
    {
        var message = new UpdateQuestionViewMmapCacheMessage(
            QuestionViewMmapCacheUpdateType.RefreshMmap);
        await _messageBusService.SendAsync(message);
    }

    /// <summary>
    /// Schedule a full rebuild of the mmap cache from the database (awaitable)
    /// </summary>
    public async Task ScheduleRebuildMmapAsync()
    {
        var message = new UpdateQuestionViewMmapCacheMessage(
            QuestionViewMmapCacheUpdateType.RebuildMmap);
        await _messageBusService.SendAsync(message);
    }
}


