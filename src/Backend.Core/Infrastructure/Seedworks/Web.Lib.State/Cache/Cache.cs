public class Cache 
{
    private static CacheAspNet _cache;
    private static readonly object _lock = new();
  

    public static void Add(string key, object obj)
    {
        CacheAspNet.Add(key, obj);
    }

    /// <summary>
    /// Add an object to the Cache (overwrite if already existent).<br/>
    /// Remove the item from the cache after <paramref name="timeSpan"/> has elapsed.
    /// </summary>
    public static void Add(string key, object obj, TimeSpan timeSpan, bool slidingExpiration = false)
    {
        CacheAspNet.Add(key, obj, timeSpan, slidingExpiration);
    }

    public static object? Get(string key)
    {
        return CacheAspNet.Get(key);
    }

    public static T? Get<T>(string key)
    {
        return CacheAspNet.Get<T>(key);
    }

    public static void Clear()
    {
        CacheAspNet.Clear();
    }

    public static void Remove(string key)
    {
        CacheAspNet.Remove(key);
    }

    public static bool Contains(string key)
    {
        return CacheAspNet.Get(key) != null;
    }
}