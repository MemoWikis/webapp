using NHibernate;

/// <summary>
/// Repository for AI model whitelist stored in database.
/// Uses AiModelCache for read operations; writes update both DB and cache.
/// </summary>
public class AiModelWhitelistRepo(ISession _session) : RepositoryDbBase<AiModelWhitelist>(_session)
{
    /// <summary>
    /// Get all models. Uses cache if available, falls back to DB.
    /// </summary>
    public new List<AiModelWhitelist> GetAll()
    {
        if (AiModelCache.IsInitialized)
            return AiModelCache.GetAll();

        return GetAllFromDb();
    }

    /// <summary>
    /// Get all models directly from database (for cache initialization)
    /// </summary>
    public List<AiModelWhitelist> GetAllFromDb()
    {
        return _session.QueryOver<AiModelWhitelist>()
            .OrderBy(x => x.Provider).Asc
            .List()
            .ToList();
    }

    /// <summary>
    /// Get enabled models. Uses cache if available.
    /// </summary>
    public List<AiModelWhitelist> GetEnabled()
    {
        if (AiModelCache.IsInitialized)
            return AiModelCache.GetEnabled();

        return _session.QueryOver<AiModelWhitelist>()
            .Where(x => x.IsEnabled)
            .ThenBy(x => x.Provider).Asc
            .List()
            .ToList();
    }

    /// <summary>
    /// Get model by ModelId string. Uses cache if available.
    /// </summary>
    public AiModelWhitelist? GetByModelId(string modelId)
    {
        if (AiModelCache.IsInitialized)
            return AiModelCache.GetByModelId(modelId);

        return _session.QueryOver<AiModelWhitelist>()
            .Where(x => x.ModelId == modelId)
            .SingleOrDefault();
    }

    /// <summary>
    /// Save or update a model (creates if ModelId doesn't exist, updates if it does)
    /// </summary>
    public void SaveModel(AiModelWhitelist model)
    {
        var existing = _session.QueryOver<AiModelWhitelist>()
            .Where(x => x.ModelId == model.ModelId)
            .SingleOrDefault();

        if (existing != null)
        {
            existing.Provider = model.Provider;
            existing.TokenCostMultiplier = model.TokenCostMultiplier;
            existing.IsEnabled = model.IsEnabled;
            Update(existing);
            Flush();
            AiModelCache.AddOrUpdate(existing);
        }
        else
        {
            Create(model);
            Flush();
            AiModelCache.AddOrUpdate(model);
        }
    }

    /// <summary>
    /// Delete a model by Id
    /// </summary>
    public void DeleteModel(int id)
    {
        var model = GetById(id);
        if (model != null)
        {
            Delete(model);
            Flush();
            AiModelCache.Remove(id);
        }
    }

    /// <summary>
    /// Initialize the cache from database
    /// </summary>
    public void InitializeCache()
    {
        var allModels = GetAllFromDb();
        AiModelCache.Initialize(allModels);
    }
}
