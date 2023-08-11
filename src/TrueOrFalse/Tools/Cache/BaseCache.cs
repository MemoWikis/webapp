using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;


public class BaseCache
{
    protected static IMemoryCache _cache;

    public BaseCache(IMemoryCache memoryCache)
    {
        _cache = memoryCache;
    }
    public static void IntoForeverCache<T>(string key, ConcurrentDictionary<int, T> objectToCache)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
            Priority = CacheItemPriority.NeverRemove
        };

        _cache.Set(key, objectToCache, cacheEntryOptions);
    }
}

