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
        var jobId = Guid.NewGuid().ToString();
        var job = new JobStatusResponse(jobId, JobStatus.Running, $"Starting {operationName}...", operationName);
        _jobStatuses[jobId] = new JobWithTimestamp(job, DateTime.UtcNow);
        return jobId;
    }

    public static void UpdateJobStatus(string jobId, JobStatus status, string message, string operationName)
    {
        var job = new JobStatusResponse(jobId, status, message, operationName);
        var timestamp = (status == JobStatus.Completed || status == JobStatus.Failed) ? DateTime.UtcNow : DateTime.UtcNow;
        _jobStatuses[jobId] = new JobWithTimestamp(job, timestamp);
    }

    public static JobStatusResponse GetJobStatus(string jobId)
    {
        CleanupExpiredJobs();
        
        if (_jobStatuses.TryGetValue(jobId, out var jobWithTimestamp))
        {
            return jobWithTimestamp.Job;
        }

        return new JobStatusResponse(jobId, JobStatus.NotFound, "Job not found", "Unknown");
    }

    public static IEnumerable<JobStatusResponse> GetAllActiveJobs()
    {
        CleanupExpiredJobs();
        
        // Return all jobs that haven't been cleaned up yet (including completed/failed jobs that are still lingering)
        return _jobStatuses.Values
            .Select(jobWithTimestamp => jobWithTimestamp.Job)
            .ToList();
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

        foreach (var jobId in jobsToRemove)
        {
            _jobStatuses.TryRemove(jobId, out _);
        }
    }

    private record JobWithTimestamp(JobStatusResponse Job, DateTime CompletionTime);
}

public readonly record struct JobStatusResponse(
    string JobId,
    JobStatus Status,
    string Message,
    string OperationName);