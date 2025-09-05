using Rebus.Handlers;
using System.Collections.Concurrent;

/// <summary>
/// Debounced handler for knowledge summary updates
/// </summary>
public class UpdateKnowledgeSummaryHandler(KnowledgeSummaryUpdate knowledgeSummaryUpdate) : IHandleMessages<UpdateKnowledgeSummaryMessage>
{
    // Debounce state management using CancellationTokens
    private static readonly ConcurrentDictionary<string, CancellationTokenSource> _debounceCancellations = new();

    // Configurable debounce delay (in milliseconds)
    private static readonly int DebounceDelayMs = 5000; // 5 seconds

    public async Task Handle(UpdateKnowledgeSummaryMessage message)
    {
        var debounceKey = message.DebounceKey;

        Log.Information("Received knowledge summary update message for key: {DebounceKey}", debounceKey);

        // Cancel any existing debounce for this key
        if (_debounceCancellations.TryGetValue(debounceKey, out var existingCts))
        {
            existingCts.Cancel();
            existingCts.Dispose();
        }

        // Create new cancellation token for this debounce
        var newCts = new CancellationTokenSource();
        _debounceCancellations[debounceKey] = newCts;

        try
        {
            // Wait for the debounce delay
            await Task.Delay(DebounceDelayMs, newCts.Token);

            // If we reach here, we weren't cancelled - execute the actual work
            Log.Information("Executing debounced knowledge summary update for key: {DebounceKey}", debounceKey);

            await ExecuteKnowledgeSummaryUpdate(message);
        }
        catch (OperationCanceledException)
        {
            // This is expected when debouncing cancels the delay
            Log.Debug("Knowledge summary update cancelled by debouncing for key: {DebounceKey}", debounceKey);
        }
        finally
        {
            // Clean up the cancellation token
            _debounceCancellations.TryRemove(debounceKey, out _);
            newCts.Dispose();
        }
    }

    private Task ExecuteKnowledgeSummaryUpdate(UpdateKnowledgeSummaryMessage message)
    {
        try
        {
            Log.Information("Executing knowledge summary update for message: {Message}", message);

            // Execute the actual knowledge summary update using injected service
            switch (message.Type)
            {
                case UpdateType.Page:
                    knowledgeSummaryUpdate.RunForPage(message.PageId, message.ForProfilePage);
                    break;

                case UpdateType.User when message.UserId.HasValue:
                    knowledgeSummaryUpdate.RunForUser(message.UserId.Value, message.ForProfilePage);
                    break;

                case UpdateType.UserAndPage when message.UserId.HasValue:
                    knowledgeSummaryUpdate.RunForUserAndPage(message.UserId.Value, message.PageId, message.ForProfilePage);
                    break;

                default:
                    Log.Warning("Invalid update type or missing UserId for message: {Message}", message);
                    break;
            }

            Log.Information("Successfully completed knowledge summary update for message: {Message}", message);
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error executing knowledge summary update for message: {Message}", message);
            throw; // Re-throw to allow proper error handling
        }
    }
}