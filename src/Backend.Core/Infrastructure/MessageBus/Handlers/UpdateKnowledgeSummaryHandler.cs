using Autofac;
using Rebus.Handlers;
using System.Collections.Concurrent;

/// <summary>
/// Debounced handler for knowledge summary updates
/// </summary>
public class UpdateKnowledgeSummaryHandler : IHandleMessages<UpdateKnowledgeSummaryMessage>
{
    private readonly KnowledgeSummaryUpdate _knowledgeSummaryUpdate;
    private readonly ILogger _logger;
    private readonly IContainer _container;

    // Debounce state management
    private static readonly ConcurrentDictionary<string, DateTime> _lastMessageTime = new();
    private static readonly ConcurrentDictionary<string, Timer> _debounceTimers = new();
    private static readonly ConcurrentDictionary<string, UpdateKnowledgeSummaryMessage> _pendingMessages = new();

    // Configurable debounce delay (in milliseconds)
    private static readonly int DebounceDelayMs = 5000; // 5 seconds

    public UpdateKnowledgeSummaryHandler(
        KnowledgeSummaryUpdate knowledgeSummaryUpdate,
        ILogger logger,
        IContainer container)
    {
        _knowledgeSummaryUpdate = knowledgeSummaryUpdate;
        _logger = logger;
        _container = container;
    }

    public Task Handle(UpdateKnowledgeSummaryMessage message)
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
        var timer = new Timer(_ => ExecuteUpdate(debounceKey), null, DebounceDelayMs, Timeout.Infinite);
        _debounceTimers.AddOrUpdate(debounceKey, timer, (key, existing) =>
        {
            existing?.Dispose();
            return timer;
        });

        return Task.CompletedTask;
    }

    private void ExecuteUpdate(string debounceKey)
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

            // Create a new scope from the root container for database operations
            // The root container never gets disposed, so this is safe for background operations
            using (var scope = _container.BeginLifetimeScope())
            {
                var scopedKnowledgeSummaryUpdate = scope.Resolve<KnowledgeSummaryUpdate>();
                
                // Execute the actual knowledge summary update
                switch (message.Type)
                {
                    case UpdateType.Page:
                        scopedKnowledgeSummaryUpdate.RunForPage(message.PageId, message.ForProfilePage);
                        break;

                    case UpdateType.User when message.UserId.HasValue:
                        scopedKnowledgeSummaryUpdate.RunForUser(message.UserId.Value, message.ForProfilePage);
                        break;

                    case UpdateType.UserAndPage when message.UserId.HasValue:
                        scopedKnowledgeSummaryUpdate.RunForUserAndPage(message.UserId.Value, message.PageId, message.ForProfilePage);
                        break;

                    default:
                        _logger.Warning("Invalid update type or missing UserId for message: {Message}", message);
                        break;
                }
            }            _logger.Information("Successfully completed knowledge summary update for key: {DebounceKey}", debounceKey);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error executing debounced knowledge summary update for key: {DebounceKey}", debounceKey);
        }
    }
}