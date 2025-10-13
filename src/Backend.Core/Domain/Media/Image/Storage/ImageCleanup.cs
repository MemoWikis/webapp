public static class ImageCleanup
{
    public static void Schedule(int pageId, string[] previousImages, string[] currentImages)
    {
        var previousImageSet = previousImages.ToHashSet();
        var currentImageSet = currentImages.ToHashSet();
        var removedImages = previousImageSet.Except(currentImageSet).ToArray();

        if (removedImages.Length > 0)
        {
            var delay = Settings.Environment == "develop" ? TimeSpan.FromMinutes(5) : TimeSpan.FromHours(24);
            JobScheduler.ScheduleDelayedImageCleanup(pageId, removedImages, delay);
        }
    }

    public static void Run(int pageId, string[] imageUrlsToCheck, DateTime scheduledAt)
    {
        Log.Information(
            "ImageCleanup.Run: Processing cleanup for page {PageId} with {ImageCount} images to check, scheduled at {ScheduledAt}",
            pageId, imageUrlsToCheck.Length, scheduledAt);

        // Get current state of the page
        var pageCacheItem = EntityCache.GetPage(pageId);
        if (pageCacheItem == null)
        {
            Log.Warning("ImageCleanup.Run: Page {PageId} not found, skipping cleanup", pageId);
            return;
        }

        var currentImages = pageCacheItem.CurrentImageUrls?.ToHashSet() ?? new HashSet<string>();
        var imagesToCheck = imageUrlsToCheck.ToHashSet();

        // Find images that were scheduled for deletion AND are still not in current content
        var imagesToDelete = imagesToCheck.Except(currentImages).ToList();

        if (imagesToDelete.Count == 0)
        {
            Log.Information(
                "ImageCleanup.Run: No images to delete for page {PageId} - all images are back in use",
                pageId);
            return;
        }

        Log.Information("ImageCleanup.Run: Found {ImageCount} images to delete for page {PageId}: {Images}",
            imagesToDelete.Count, pageId, string.Join(", ", imagesToDelete));

        // Perform the actual cleanup using settings directly
        var basePath = Settings.PageContentImageBasePath;
        var directory = Path.Combine(Settings.ImagePath, basePath);

        if (!Directory.Exists(directory))
        {
            Log.Information("ImageCleanup.Run: Directory {Directory} does not exist, cleanup complete",
                directory);
            return;
        }

        // Convert URLs to filenames and delete
        var filenamesToDelete = imagesToDelete.Select(url => Path.GetFileName(url)).ToList();
        var deleteImage = new DeleteImage();
        deleteImage.Run(basePath, filenamesToDelete);

        Log.Information(
            "ImageCleanup.Run: Successfully deleted {DeletedCount} orphaned images for page {PageId}",
            filenamesToDelete.Count, pageId);
    }
}