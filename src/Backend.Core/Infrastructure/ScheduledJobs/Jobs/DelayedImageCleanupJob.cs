using Quartz;

/// <summary>
/// Quartz job for delayed image cleanup - processes after 24h delay to allow undo/revert
/// </summary>
public class DelayedImageCleanupJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var dataMap = context.JobDetail.JobDataMap;
        var pageId = dataMap.GetInt("pageId");
        var imageUrlsToCheck = dataMap.GetString("imageUrlsToCheck")?.Split(',') ?? Array.Empty<string>();
        var scheduledAt = dataMap.GetDateTime("scheduledAt");

        ImageCleanup.Run(pageId, imageUrlsToCheck, scheduledAt);

        await Task.CompletedTask;
    }
}