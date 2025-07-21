/// <summary>
/// Service for sending messages through the message bus
/// </summary>
public interface IMessageBusService
{
    /// <summary>
    /// Sends a message to the bus
    /// </summary>
    Task SendAsync<T>(T message) where T : class, IMessage;
}
