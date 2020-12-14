using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

 public class BaseCache
    {
        protected static void IntoForeverCache<T>(string key, ConcurrentDictionary<int, T> objectToCache)
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

