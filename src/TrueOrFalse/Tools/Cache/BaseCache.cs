using System.Collections.Concurrent;
using System.Web;
using System.Web.Caching;

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

