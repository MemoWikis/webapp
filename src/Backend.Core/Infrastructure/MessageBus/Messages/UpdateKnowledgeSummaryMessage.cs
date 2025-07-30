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
    public bool ForProfilePage { get; set; } = false;
    
    /// <summary>
    /// Key used for debouncing - same key messages will be debounced together
    /// </summary>
    public string DebounceKey => Type switch
    {
        UpdateType.Page => $"page_{PageId}{(ForProfilePage ? "_profile" : "")}",
        UpdateType.User => $"user_{UserId}{(ForProfilePage ? "_profile" : "")}",
        UpdateType.UserAndPage => $"user_{UserId}_page_{PageId}{(ForProfilePage ? "_profile" : "")}",
        _ => $"unknown_{PageId}_{UserId}{(ForProfilePage ? "_profile" : "")}"
    };

    public UpdateKnowledgeSummaryMessage(int pageId, bool forProfilePage = false)
    {
        PageId = pageId;
        Type = UpdateType.Page;
        ForProfilePage = forProfilePage;
    }

    public UpdateKnowledgeSummaryMessage(int? userId, int pageId, UpdateType type, bool forProfilePage = false)
    {
        UserId = userId;
        PageId = pageId;
        Type = type;
        ForProfilePage = forProfilePage;
    }

    /// <summary>
    /// Constructor for UserAndPage updates (specific user and page combination)
    /// </summary>
    public UpdateKnowledgeSummaryMessage(int userId, int pageId, bool forProfilePage = false)
    {
        UserId = userId;
        PageId = pageId;
        Type = UpdateType.UserAndPage;
        ForProfilePage = forProfilePage;
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
