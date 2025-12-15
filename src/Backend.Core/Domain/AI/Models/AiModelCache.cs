using System.Collections.Concurrent;

/// <summary>
/// Cache for AI model whitelist to reduce database calls
/// </summary>
public static class AiModelCache
{
    public const string CacheKey = "aiModelWhitelist_Cache";

    private static ConcurrentDictionary<int, AiModelWhitelist>? Models =>
        MemoCache.Get<ConcurrentDictionary<int, AiModelWhitelist>>(CacheKey);

    /// <summary>
    /// Initialize cache from database. Should be called during app startup.
    /// </summary>
    public static void Initialize(List<AiModelWhitelist> models)
    {
        var dictionary = models.ToConcurrentDictionary();
        MemoCache.Add(CacheKey, dictionary);
        Log.Information("AiModelCache initialized with {Count} models", models.Count);
    }

    /// <summary>
    /// Check if cache is initialized
    /// </summary>
    public static bool IsInitialized => Models != null;

    /// <summary>
    /// Get all models from cache
    /// </summary>
    public static List<AiModelWhitelist> GetAll()
    {
        return Models?.Values
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Provider)
            .ToList() ?? new List<AiModelWhitelist>();
    }

    /// <summary>
    /// Get only enabled models from cache
    /// </summary>
    public static List<AiModelWhitelist> GetEnabled()
    {
        return Models?.Values
            .Where(x => x.IsEnabled)
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Provider)
            .ToList() ?? new List<AiModelWhitelist>();
    }

    /// <summary>
    /// Get model by ModelId string (e.g., "claude-sonnet-4-latest")
    /// </summary>
    public static AiModelWhitelist? GetByModelId(string modelId)
    {
        return Models?.Values.FirstOrDefault(x => x.ModelId == modelId);
    }

    /// <summary>
    /// Get model by database Id
    /// </summary>
    public static AiModelWhitelist? GetById(int id)
    {
        if (Models == null) return null;
        Models.TryGetValue(id, out var model);
        return model;
    }

    /// <summary>
    /// Get the default model
    /// </summary>
    public static AiModelWhitelist? GetDefault()
    {
        return Models?.Values.FirstOrDefault(x => x.IsDefault && x.IsEnabled);
    }

    /// <summary>
    /// Add or update a model in cache
    /// </summary>
    public static void AddOrUpdate(AiModelWhitelist model)
    {
        Models?.AddOrUpdate(model.Id, model, (k, v) => model);
    }

    /// <summary>
    /// Remove a model from cache
    /// </summary>
    public static void Remove(int id)
    {
        Models?.TryRemove(id, out _);
    }

    /// <summary>
    /// Clear the default flag on all models in cache
    /// </summary>
    public static void ClearDefaultFlag()
    {
        if (Models == null) return;
        foreach (var model in Models.Values.Where(m => m.IsDefault))
        {
            model.IsDefault = false;
        }
    }

    /// <summary>
    /// Refresh cache from database
    /// </summary>
    public static void Refresh(List<AiModelWhitelist> models)
    {
        Initialize(models);
    }
}
