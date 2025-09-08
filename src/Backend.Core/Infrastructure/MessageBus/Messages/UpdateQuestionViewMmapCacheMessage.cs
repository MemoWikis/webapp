/// <summary>
/// Message for QuestionView mmap cache updates
/// </summary>
public class UpdateQuestionViewMmapCacheMessage : IMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public QuestionViewMmapCacheUpdateType UpdateType { get; set; }
    public QuestionViewSummaryWithId? QuestionViewSummary { get; set; }
    public List<QuestionViewSummaryWithId>? QuestionViewSummaries { get; set; }
    public int? QuestionId { get; set; }

    /// <summary>
    /// Key used for debouncing - same key messages will be debounced together
    /// </summary>
    public string DebounceKey => UpdateType switch
    {
        QuestionViewMmapCacheUpdateType.AddView => $"questionview_add_{QuestionViewSummary?.QuestionId}_{QuestionViewSummary?.DateOnly:yyyyMMdd}",
        QuestionViewMmapCacheUpdateType.AddViews => $"questionview_add_batch_{QuestionViewSummaries?.FirstOrDefault().QuestionId}",
        QuestionViewMmapCacheUpdateType.DeleteViews => $"questionview_delete_{QuestionId}",
        QuestionViewMmapCacheUpdateType.RefreshMmap => "questionview_refresh",
        QuestionViewMmapCacheUpdateType.RebuildMmap => "questionview_rebuild",
        _ => $"questionview_unknown_{Id}"
    };

    // Single view
    public UpdateQuestionViewMmapCacheMessage(
        QuestionViewMmapCacheUpdateType updateType, 
        QuestionViewSummaryWithId? questionViewSummary = null)
    {
        UpdateType = updateType;
        QuestionViewSummary = questionViewSummary;
    }

    // Multiple views
    public UpdateQuestionViewMmapCacheMessage(
        QuestionViewMmapCacheUpdateType updateType, 
        List<QuestionViewSummaryWithId>? questionViewSummaries = null)
    {
        UpdateType = updateType;
        QuestionViewSummaries = questionViewSummaries;
    }

    // Delete by question ID
    public UpdateQuestionViewMmapCacheMessage(
        QuestionViewMmapCacheUpdateType updateType, 
        int? questionId = null)
    {
        UpdateType = updateType;
        QuestionId = questionId;
    }

    // Refresh/Rebuild (no additional data needed)
    public UpdateQuestionViewMmapCacheMessage(QuestionViewMmapCacheUpdateType updateType)
    {
        UpdateType = updateType;
    }

    // Parameterless constructor for serialization
    public UpdateQuestionViewMmapCacheMessage() { }
}

public enum QuestionViewMmapCacheUpdateType
{
    AddView,
    AddViews,
    DeleteViews,
    RefreshMmap,
    RebuildMmap
}
