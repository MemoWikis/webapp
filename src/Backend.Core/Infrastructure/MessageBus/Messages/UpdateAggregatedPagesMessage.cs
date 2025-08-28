using Rebus.Bus;

/// <summary>
/// Message for updating aggregated pages when questions are modified
/// </summary>
public class UpdateAggregatedPagesMessage : IMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public List<int> PageIds { get; set; } = new();
    public int UserId { get; set; }

    public UpdateAggregatedPagesMessage(List<int> pageIds, int userId = -1)
    {
        PageIds = pageIds;
        UserId = userId;
    }

    // Parameterless constructor for serialization
    public UpdateAggregatedPagesMessage() { }
}
