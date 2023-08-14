
using System.Collections.Concurrent;
using CacheManager.Core;


public class BaseEntityCache
{
    protected static ICacheManager<object> _cache;

    public BaseEntityCache()
    {
        _cache = CacheFactory.Build<object>("entityCache", settings =>
        {
            settings.WithSystemRuntimeCacheHandle("handleName");
        });
    }
    public static void IntoForeverCache<T>(string key, ConcurrentDictionary<int, T> objectToCache)
    {
        _cache.Add(key, objectToCache);
    }
}

