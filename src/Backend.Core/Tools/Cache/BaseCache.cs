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

    public static void Clear() => _mgr.Clear();
}

