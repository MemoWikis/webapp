
using System.Collections.Concurrent;
using CacheManager.Core;


public class BaseEntityCache
{
    protected static ICacheManager<object> _cache = CacheFactory.Build<object>(settings =>
    {
        settings.WithDictionaryHandle();
    });

    public static void IntoForeverCache<T>(string key, ConcurrentDictionary<int, T> objectToCache)
    {
        _cache.Add(key, objectToCache);
    }

    public static void IntoForeverCache<T>(string key, ConcurrentDictionary<(int, int), T> objectToCache)
    {
        _cache.Add(key, objectToCache);
    }
}

