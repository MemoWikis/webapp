using System.Collections;

namespace Seedworks.Web.State
{    
    public class Cache 
    {
        private static readonly ICache _cache;
        
        public static int Count => _cache.Count;

        static Cache()
        {
            _cache = new CacheAspNet();
        }

        public static void Add(string key, object obj)
        {
            _cache.Add(key, obj);
        }

        /// <summary>
        /// Add an object to the Cache (overwrite if already existent).<br/>
        /// Remove the item from the cache after <paramref name="timeSpan"/> has elapsed.
        /// </summary>
        public static void Add(string key, object obj, TimeSpan timeSpan, bool slidingExpiration = false)
        {
            _cache.Add(key, obj, timeSpan, slidingExpiration);
        }

        public static object Get(string key)
        {
            return _cache.Get(key);
        }

        public static T Get<T>(string key)
        {
            return _cache.Get<T>(key);
        }

        public static void Clear()
        {
            _cache.Clear();
        }

        public static void Remove(string key)
        {
            _cache.Remove(key);
        }

        public static bool Contains(string key)
        {
            return _cache.Get(key) != null;
        }
    }
}
