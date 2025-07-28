/// <summary>
/// Message for updating knowledge summaries with debouncing support
/// </summary>
public class UpdateKnowledgeSummaryMessage : IMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public int PageId { get; set; }
    public int? UserId { get; set; }
    public UpdateType Type { get; set; }
    
    /// <summary>
    /// Key used for debouncing - same key messages will be debounced together
    /// </summary>
    public string DebounceKey => Type switch
    {
        UpdateType.Page => $"page_{PageId}",
        UpdateType.User => $"user_{UserId}",
        UpdateType.UserAndPage => $"user_{UserId}_page_{PageId}",
        _ => $"unknown_{PageId}_{UserId}"
    };

    public UpdateKnowledgeSummaryMessage(int pageId, UpdateType type = UpdateType.Page)
    {
        PageId = pageId;
        Type = type;
    }

    public UpdateKnowledgeSummaryMessage(int userId, int pageId, UpdateType type = UpdateType.User)
    {
        UserId = userId;
        PageId = pageId;
        Type = type;
    }

    /// <summary>
    /// Constructor for UserAndPage updates (specific user and page combination)
    /// </summary>
    public UpdateKnowledgeSummaryMessage(int userId, int pageId)
    {
        UserId = userId;
        PageId = pageId;
        Type = UpdateType.UserAndPage;
    }

    // Parameterless constructor for serialization
    public UpdateKnowledgeSummaryMessage() { }
}

public enum UpdateType
{
    Page,
    User,
    UserAndPage
}
