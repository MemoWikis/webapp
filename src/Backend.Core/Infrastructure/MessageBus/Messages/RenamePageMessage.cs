/// <summary>
/// Message for renaming a page
/// </summary>
public class RenamePageMessage : IMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public int PageId { get; set; }
    public string NewName { get; set; } = string.Empty;

    public RenamePageMessage(int pageId, string newName)
    {
        PageId = pageId;
        NewName = newName;
    }

    // Parameterless constructor for serialization
    public RenamePageMessage() { }
}
