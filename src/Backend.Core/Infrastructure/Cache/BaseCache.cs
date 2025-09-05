using System.Collections.Concurrent;
using CacheManager.Core;

public static class MemoCache
{
    private static readonly ICacheManager<object> _mgr = CacheFactory.Build<object>(settings 
        => settings.WithDictionaryHandle());
    
    public static T Get<T>(string key) => _mgr.Get<T>(key);

    public static void Add<T>(string key, ConcurrentDictionary<int, T> objectToCache) => 
        _mgr.Add(key, objectToCache);

    public static void Add<T>(string key, ConcurrentDictionary<(int, int), T> objectToCache) => 
        _mgr.Add(key, objectToCache);

    /// <summary>
    /// Add an object to the cache with sliding expiration.
    /// </summary>
    /// <param name="key">Cache key</param>
    /// <param name="obj">Object to cache</param>
    /// <param name="expiration">Sliding expiration time</param>
    public static void AddWithSlidingExpiration<T>(string key, T obj, TimeSpan expiration)
    {
        if (obj == null) return;
        
        var cacheItem = new CacheItem<object>(key, obj, ExpirationMode.Sliding, expiration);
        _mgr.Add(cacheItem);
    }

    public static void Clear() => _mgr.Clear();

    public static void Remove(string key) => _mgr.Remove(key);
}

