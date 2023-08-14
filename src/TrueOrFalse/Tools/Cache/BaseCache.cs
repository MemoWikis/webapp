
using System.Collections.Concurrent;
using CacheManager.Core;


public class BaseCache
{
    protected static ICacheManager<object> _cache;

    public BaseCache()
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

