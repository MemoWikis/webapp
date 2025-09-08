/// <summary>
/// Message for PageView mmap cache updates
/// </summary>
public class UpdatePageViewMmapCacheMessage : IMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public PageViewMmapCacheUpdateType UpdateType { get; set; }
    public PageViewSummaryWithId? PageViewSummary { get; set; }
    public List<PageViewSummaryWithId>? PageViewSummaries { get; set; }
    public int? PageId { get; set; }

    /// <summary>
    /// Key used for debouncing - same key messages will be debounced together
    /// </summary>
    public string DebounceKey => UpdateType switch
    {
        PageViewMmapCacheUpdateType.AddView => $"pageview_add_{PageViewSummary?.PageId}_{PageViewSummary?.DateOnly:yyyyMMdd}",
        PageViewMmapCacheUpdateType.AddViews => $"pageview_add_batch_{PageViewSummaries?.FirstOrDefault().PageId}",
        PageViewMmapCacheUpdateType.DeleteViews => $"pageview_delete_{PageId}",
        PageViewMmapCacheUpdateType.RefreshMmap => "pageview_refresh",
        PageViewMmapCacheUpdateType.RebuildMmap => "pageview_rebuild",
        _ => $"pageview_unknown_{Id}"
    };

    // Single view
    public UpdatePageViewMmapCacheMessage(
        PageViewMmapCacheUpdateType updateType, 
        PageViewSummaryWithId? pageViewSummary = null)
    {
        UpdateType = updateType;
        PageViewSummary = pageViewSummary;
    }

    // Multiple views
    public UpdatePageViewMmapCacheMessage(
        PageViewMmapCacheUpdateType updateType, 
        List<PageViewSummaryWithId>? pageViewSummaries = null)
    {
        UpdateType = updateType;
        PageViewSummaries = pageViewSummaries;
    }

    // Delete by page ID
    public UpdatePageViewMmapCacheMessage(
        PageViewMmapCacheUpdateType updateType, 
        int? pageId = null)
    {
        UpdateType = updateType;
        PageId = pageId;
    }

    // Refresh/Rebuild (no additional data needed)
    public UpdatePageViewMmapCacheMessage(PageViewMmapCacheUpdateType updateType)
    {
        UpdateType = updateType;
    }

    // Parameterless constructor for serialization
    public UpdatePageViewMmapCacheMessage() { }
}

public enum PageViewMmapCacheUpdateType
{
    AddView,
    AddViews,
    DeleteViews,
    RefreshMmap,
    RebuildMmap
}
