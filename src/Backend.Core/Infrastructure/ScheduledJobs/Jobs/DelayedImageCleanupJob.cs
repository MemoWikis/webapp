using Quartz;

/// <summary>
/// Quartz job for delayed image cleanup - processes after 24h delay to allow undo/revert
/// </summary>
public class DelayedImageCleanupJob : IJob
{

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            var dataMap = context.JobDetail.JobDataMap;
            var pageId = dataMap.GetInt("pageId");
            var imageUrlsToCheck = dataMap.GetString("imageUrlsToCheck")?.Split(',') ?? Array.Empty<string>();
            var scheduledAt = dataMap.GetDateTime("scheduledAt");

            Log.Information("DelayedImageCleanupJob: Processing cleanup for page {PageId} with {ImageCount} images to check, scheduled at {ScheduledAt}", 
                pageId, imageUrlsToCheck.Length, scheduledAt);

            // Get current state of the page
            var pageCacheItem = EntityCache.GetPage(pageId);
            if (pageCacheItem == null)
            {
                Log.Warning("DelayedImageCleanupJob: Page {PageId} not found, skipping cleanup", pageId);
                return;
            }

            var currentImages = pageCacheItem.CurrentImageUrls?.ToHashSet() ?? new HashSet<string>();
            var imagesToCheck = imageUrlsToCheck.ToHashSet();

            // Find images that were scheduled for deletion AND are still not in current content
            var imagesToDelete = imagesToCheck.Except(currentImages).ToList();

            if (imagesToDelete.Count == 0)
            {
                Log.Information("DelayedImageCleanupJob: No images to delete for page {PageId} - all images are back in use", 
                    pageId);
                return;
            }

            Log.Information("DelayedImageCleanupJob: Found {ImageCount} images to delete for page {PageId}: {Images}", 
                imagesToDelete.Count, pageId, string.Join(", ", imagesToDelete));

            // Perform the actual cleanup using settings directly
            var basePath = Settings.PageContentImageBasePath;
            var directory = Path.Combine(Settings.ImagePath, basePath);

            if (!Directory.Exists(directory))
            {
                Log.Information("DelayedImageCleanupJob: Directory {Directory} does not exist, cleanup complete", directory);
                return;
            }

            // Convert URLs to filenames and delete
            var filenamesToDelete = imagesToDelete.Select(url => Path.GetFileName(url)).ToList();
            var deleteImage = new DeleteImage();
            deleteImage.Run(basePath, filenamesToDelete);

            Log.Information("DelayedImageCleanupJob: Successfully deleted {DeletedCount} orphaned images for page {PageId}", 
                filenamesToDelete.Count, pageId);

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "DelayedImageCleanupJob: Error processing delayed cleanup");
            throw; // Re-throw to allow Quartz to handle retry logic
        }
    }
}