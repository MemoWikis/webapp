public class ShowRelationErrorsMaintenanceOperation : IMaintenanceOperation, IRegisterAsInstancePerLifetime
{
    private readonly RelationErrors _relationErrors;
    private readonly IMaintenanceJobService _jobService;
    
    // Cache the results so they can be accessed after analysis
    private static RelationErrorsResult? _cachedResults;
    private static DateTime _cacheTimestamp;

    public string OperationName => "ShowRelationErrors";

    public ShowRelationErrorsMaintenanceOperation(
        RelationErrors relationErrors,
        IMaintenanceJobService jobService)
    {
        _relationErrors = relationErrors;
        _jobService = jobService;
    }

    public Task Run(string jobId)
    {
        try
        {
            _jobService.UpdateJobStatus(jobId, JobStatus.Running, "Analyzing relation errors...", OperationName);
            
            var errors = _relationErrors.GetErrors();
            
            _jobService.UpdateJobStatus(jobId, JobStatus.Running, "Processing error data...", OperationName);
            
            // Cache the results for later retrieval
            _cachedResults = errors;
            _cacheTimestamp = DateTime.UtcNow;
            
            var errorCount = errors.Data?.Count ?? 0;
            var message = errorCount > 0 
                ? $"Analysis complete. Found {errorCount} pages with relation errors."
                : "Analysis complete. No relation errors found.";
            
            _jobService.UpdateJobStatus(jobId, JobStatus.Completed, message, OperationName);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to execute {OperationName} with jobId {JobId}", OperationName, jobId);
            _jobService.UpdateJobStatus(jobId, JobStatus.Failed, $"Error: {ex.Message}", OperationName);
        }
        
        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets cached results from the last async analysis, or null if no cached results available
    /// </summary>
    public static RelationErrorsResult? GetCachedResults()
    {
        return _cachedResults;
    }

    /// <summary>
    /// Gets the timestamp when the results were cached
    /// </summary>
    public static DateTime GetCacheTimestamp()
    {
        return _cacheTimestamp;
    }

    /// <summary>
    /// Checks if cached results are available
    /// </summary>
    public static bool HasCachedResults()
    {
        return _cachedResults.HasValue;
    }

    /// <summary>
    /// Clears the cached results
    /// </summary>
    public static void ClearCache()
    {
        _cachedResults = null;
        _cacheTimestamp = default;
    }
}