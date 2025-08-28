using Rebus.Handlers;
using Serilog;

/// <summary>
/// Simple handler for test messages
/// </summary>
public class TestMessageHandler : IHandleMessages<TestMessage>
{
    private readonly ILogger _logger;

    public TestMessageHandler(ILogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(TestMessage message)
    {
        _logger.Information("Received test message: {Content} (ID: {MessageId})", 
            message.Content, message.Id);
        
        // Simulate some work
        await Task.Delay(100);
        
        _logger.Information("Finished processing test message: {MessageId}", message.Id);
    }
}
