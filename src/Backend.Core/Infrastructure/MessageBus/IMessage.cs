/// <summary>
/// Base interface for all messages in the system
/// </summary>
public interface IMessage
{
    /// <summary>
    /// Unique identifier for the message
    /// </summary>
    Guid Id { get; }
    
    /// <summary>
    /// Timestamp when the message was created
    /// </summary>
    DateTime CreatedAt { get; }
}
