/// <summary>
/// Simple test message for verifying the message bus works
/// </summary>
public class TestMessage : IMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Content { get; set; } = string.Empty;

    public TestMessage(string content)
    {
        Content = content;
    }

    // Parameterless constructor for serialization
    public TestMessage() { }
}
