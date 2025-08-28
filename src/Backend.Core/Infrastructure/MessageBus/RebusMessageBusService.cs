using Rebus.Bus;

/// <summary>
/// Rebus implementation of the message bus service
/// </summary>
public class RebusMessageBusService : IMessageBusService
{
    private readonly IBus _bus;

    public RebusMessageBusService(IBus bus)
    {
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    }

    public async Task SendAsync<T>(T message) where T : class, IMessage
    {
        // Use SendLocal for in-memory transport to avoid routing configuration
        await _bus.SendLocal(message);
    }
}
