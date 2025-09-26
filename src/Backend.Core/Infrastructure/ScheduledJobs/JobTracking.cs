using System.Collections.Concurrent;

public enum JobStatus
{
    Running,
    Completed,
    Failed,
    NotFound
}

public static class JobTracking
{
    // Using ConcurrentDictionary for thread safety since multiple background tasks might update jobs simultaneously
    private static readonly ConcurrentDictionary<string, JobWithTimestamp> _jobStatuses = new();
    private const int COMPLETED_JOB_LINGER_SECONDS = 15;

    public static string CreateJob(string operationName)
    {
        var jobTrackingId = Guid.NewGuid().ToString();
        var job = new JobStatusResponse(jobTrackingId, JobStatus.Running, $"Starting {operationName}...", operationName);
        _jobStatuses[jobTrackingId] = new JobWithTimestamp(job, DateTime.UtcNow);
        return jobTrackingId;
    }

    public static void UpdateJobStatus(string jobTrackingId, JobStatus status, string message, string operationName)
    {
        var job = new JobStatusResponse(jobTrackingId, status, message, operationName);
        var timestamp = (status == JobStatus.Completed || status == JobStatus.Failed) ? DateTime.UtcNow : DateTime.UtcNow;
        _jobStatuses[jobTrackingId] = new JobWithTimestamp(job, timestamp);
    }

    public static JobStatusResponse GetJobStatus(string jobTrackingId)
    {
        CleanupExpiredJobs();

        if (_jobStatuses.TryGetValue(jobTrackingId, out var jobWithTimestamp))
        {
            return jobWithTimestamp.Job;
        }

        return new JobStatusResponse(jobTrackingId, JobStatus.NotFound, "Job not found", "Unknown");
    }

    public static IEnumerable<JobStatusResponse> GetAllActiveJobs()
    {
        CleanupExpiredJobs();

        // Return all jobs that haven't been cleaned up yet (including completed/failed jobs that are still lingering)
        return _jobStatuses.Values
            .Select(jobWithTimestamp => jobWithTimestamp.Job)
            .ToList();
    }

    public static bool ClearJob(string jobTrackingId)
    {
        return _jobStatuses.TryRemove(jobTrackingId, out _);
    }

    public static int ClearAllJobs()
    {
        var count = _jobStatuses.Count;
        _jobStatuses.Clear();
        return count;
    }

    private static void CleanupExpiredJobs()
    {
        var cutoffTime = DateTime.UtcNow.AddSeconds(-COMPLETED_JOB_LINGER_SECONDS);
        var jobsToRemove = new List<string>();

        foreach (var keyValuePair in _jobStatuses)
        {
            var jobWithTimestamp = keyValuePair.Value;

            // Remove completed/failed jobs that are older than the linger time
            if ((jobWithTimestamp.Job.Status == JobStatus.Completed || jobWithTimestamp.Job.Status == JobStatus.Failed) &&
                jobWithTimestamp.CompletionTime < cutoffTime)
            {
                jobsToRemove.Add(keyValuePair.Key);
            }
        }

        foreach (var jobTrackingId in jobsToRemove)
        {
            _jobStatuses.TryRemove(jobTrackingId, out _);
        }
    }

    private record JobWithTimestamp(JobStatusResponse Job, DateTime CompletionTime);
}

public readonly record struct JobStatusResponse(
    string JobTrackingId,
    JobStatus Status,
    string Message,
    string OperationName);