using Meilisearch;

/// <summary>
/// Utility class to wait for Meilisearch indexing operations to complete.
/// Useful for testing scenarios where you need to ensure all indexing is done
/// before proceeding with verification.
/// </summary>
public class MeilisearchTaskWaiter
{
    private readonly MeilisearchClient _client;

    public MeilisearchTaskWaiter()
    {
        _client = new MeilisearchClient(Settings.MeilisearchUrl, Settings.MeilisearchMasterKey);
    }

    public MeilisearchTaskWaiter(string url, string masterKey)
    {
        _client = new MeilisearchClient(url, masterKey);
    }

    /// <summary>
    /// Waits for all pending Meilisearch tasks to complete.
    /// This includes indexing, updating, and deleting operations.
    /// </summary>
    /// <param name="timeoutMs">Maximum time to wait in milliseconds. Default is 30 seconds.</param>
    /// <returns>True if all tasks completed successfully, false if timeout or errors occurred</returns>
    public async Task<bool> WaitForAllTasksToComplete(int timeoutMs = 30000)
    {
        var startTime = DateTime.UtcNow;
        var timeout = TimeSpan.FromMilliseconds(timeoutMs);

        while (DateTime.UtcNow - startTime < timeout)
        {
            var tasks = await _client.GetTasksAsync();

            // Check if there are any tasks that are still processing or enqueued
            var pendingTasks = tasks.Results.Where(task =>
                    task.Status == TaskInfoStatus.Enqueued ||
                    task.Status == TaskInfoStatus.Processing)
                .ToList();

            if (!pendingTasks.Any())
            {
                // No pending tasks, all operations are complete
                return true;
            }

            // Wait a bit before checking again
            await Task.Delay(100);
        }

        // Timeout reached
        return false;
    }

    /// <summary>
    /// Waits for all pending tasks and returns detailed information about failed tasks.
    /// </summary>
    /// <param name="timeoutMs">Maximum time to wait in milliseconds</param>
    /// <returns>A result containing success status and any error details</returns>
    public async Task<WaitResult> WaitForAllTasksWithDetails(int timeoutMs = 30000)
    {
        var startTime = DateTime.UtcNow;
        var timeout = TimeSpan.FromMilliseconds(timeoutMs);
        var failedTasks = new List<TaskResource>();

        while (DateTime.UtcNow - startTime < timeout)
        {
            var tasks = await _client.GetTasksAsync();

            var pendingTasks = tasks.Results.Where(task =>
                    task.Status == TaskInfoStatus.Enqueued ||
                    task.Status == TaskInfoStatus.Processing)
                .ToList();

            var newFailedTasks = tasks.Results.Where(task =>
                    task.Status == TaskInfoStatus.Failed)
                .ToList();

            failedTasks.AddRange(newFailedTasks);

            if (!pendingTasks.Any())
            {
                return new WaitResult
                {
                    Success = !failedTasks.Any(),
                    TimedOut = false,
                    FailedTasks = failedTasks,
                    WaitTime = DateTime.UtcNow - startTime
                };
            }

            await Task.Delay(100);
        }

        return new WaitResult { Success = false, TimedOut = true, FailedTasks = failedTasks, WaitTime = timeout };
    }

    /// <summary>
    /// Gets the current status of all Meilisearch tasks.
    /// Useful for debugging what operations are currently running.
    /// </summary>
    public async Task<TaskStatusSummary> GetTaskStatusSummary()
    {
        var tasks = await _client.GetTasksAsync();

        return new TaskStatusSummary
        {
            Total = tasks.Results.Count(),
            Enqueued = tasks.Results.Count(t => t.Status == TaskInfoStatus.Enqueued),
            Processing = tasks.Results.Count(t => t.Status == TaskInfoStatus.Processing),
            Succeeded = tasks.Results.Count(t => t.Status == TaskInfoStatus.Succeeded),
            Failed = tasks.Results.Count(t => t.Status == TaskInfoStatus.Failed),
            Canceled = tasks.Results.Count(t => t.Status == TaskInfoStatus.Canceled),
            Tasks = tasks.Results.ToList()
        };
    }
}

public class WaitResult
{
    public bool Success { get; set; }
    public bool TimedOut { get; set; }
    public List<TaskResource> FailedTasks { get; set; } = new();
    public TimeSpan WaitTime { get; set; }
}

public class TaskStatusSummary
{
    public int Total { get; set; }
    public int Enqueued { get; set; }
    public int Processing { get; set; }
    public int Succeeded { get; set; }
    public int Failed { get; set; }
    public int Canceled { get; set; }
    public List<TaskResource> Tasks { get; set; } = new();

    public bool HasPendingTasks => Enqueued > 0 || Processing > 0;
}