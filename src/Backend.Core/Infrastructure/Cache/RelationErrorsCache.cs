public class RelationErrorsCache
{    
    // Cache the results so they can be accessed after analysis
    private static RelationErrorsResult? _cachedResults;
    private static DateTime _cacheTimestamp;

    public static void SetCache(RelationErrorsResult results)
    {
        _cachedResults = results;
        _cacheTimestamp = DateTime.UtcNow;
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