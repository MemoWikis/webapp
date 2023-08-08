using System.Collections.Concurrent;


 public class BaseCache
    {
        public static void IntoForeverCache<T>(string key, ConcurrentDictionary<int, T> objectToCache)
        {
            HttpRuntime.Cache.Insert(
                key,
                objectToCache,
                null,
                Cache.NoAbsoluteExpiration,
                Cache.NoSlidingExpiration,
                CacheItemPriority.NotRemovable,
                null);
        }
}

