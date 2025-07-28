using Rebus.Handlers;
using System.Collections.Concurrent;

/// <summary>
/// Debounced handler for knowledge summary updates
/// </summary>
public class UpdateKnowledgeSummaryHandler : IHandleMessages<UpdateKnowledgeSummaryMessage>
{
    private readonly KnowledgeSummaryUpdate _knowledgeSummaryUpdate;
    private readonly ILogger _logger;

    // Debounce state management
    private static readonly ConcurrentDictionary<string, DateTime> _lastMessageTime = new();
    private static readonly ConcurrentDictionary<string, Timer> _debounceTimers = new();
    private static readonly ConcurrentDictionary<string, UpdateKnowledgeSummaryMessage> _pendingMessages = new();

    // Configurable debounce delay (in milliseconds)
    private static readonly int DebounceDelayMs = 5000; // 5 seconds

    public UpdateKnowledgeSummaryHandler(
        KnowledgeSummaryUpdate knowledgeSummaryUpdate,
        ILogger logger)
    {
        _knowledgeSummaryUpdate = knowledgeSummaryUpdate;
        _logger = logger;
    }

    public async Task Handle(UpdateKnowledgeSummaryMessage message)
    {
        var debounceKey = message.DebounceKey;
        var now = DateTime.UtcNow;

        _logger.Information("Received knowledge summary update message for key: {DebounceKey}", debounceKey);

        // Update the last message time and store the latest message
        _lastMessageTime.AddOrUpdate(debounceKey, now, (key, existing) => now);
        _pendingMessages.AddOrUpdate(debounceKey, message, (key, existing) => message);

        // Cancel existing timer if it exists
        if (_debounceTimers.TryGetValue(debounceKey, out var existingTimer))
        {
            existingTimer?.Dispose();
        }

        // Create new timer for debounced execution
        var timer = new Timer(async _ => await ExecuteUpdate(debounceKey), null, DebounceDelayMs, Timeout.Infinite);
        _debounceTimers.AddOrUpdate(debounceKey, timer, (key, existing) =>
        {
            existing?.Dispose();
            return timer;
        });
    }

    private async Task ExecuteUpdate(string debounceKey)
    {
        try
        {
            // Get the pending message and clean up debounce state
            if (!_pendingMessages.TryRemove(debounceKey, out var message))
            {
                _logger.Warning("No pending message found for debounce key: {DebounceKey}", debounceKey);
                return;
            }

            // Clean up timers and timestamps
            if (_debounceTimers.TryRemove(debounceKey, out var timer))
            {
                timer?.Dispose();
            }
            _lastMessageTime.TryRemove(debounceKey, out _);

            _logger.Information("Executing debounced knowledge summary update for key: {DebounceKey}", debounceKey);

            // Execute the actual knowledge summary update
            switch (message.Type)
            {
                case UpdateType.Page:
                    _knowledgeSummaryUpdate.RunForPage(message.PageId);
                    break;

                case UpdateType.User when message.UserId.HasValue:
                    _knowledgeSummaryUpdate.RunForUser(message.UserId.Value);
                    break;

                case UpdateType.UserAndPage when message.UserId.HasValue:
                    _knowledgeSummaryUpdate.RunForUserAndPage(message.UserId.Value, message.PageId);
                    break;

                default:
                    _logger.Warning("Invalid update type or missing UserId for message: {Message}", message);
                    break;
            }

            _logger.Information("Successfully completed knowledge summary update for key: {DebounceKey}", debounceKey);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error executing debounced knowledge summary update for key: {DebounceKey}", debounceKey);
        }
    }
}
