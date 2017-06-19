using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace Seedworks.Web.State
{
    internal class CacheAspNet : ICache
    {
        public int Count { get { return HttpRuntime.Cache.Count; } }

        public IDictionaryEnumerator GetEnumerator()
        {
            return HttpRuntime.Cache.GetEnumerator();
        }

        /// <summary>
        /// Add an object to the Cache (overwrite if already existent).<br/>
        /// Remove the item from the cache after <paramref name="expiration"/> has elapsed.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiration"></param>
        public void Add(string key, object obj, TimeSpan expiration)
        {
            // The first approach removes an item from the cache if it has not been accessed for the given TimeSpan.
            // HttpRuntime.Cache.Insert(key, obj, null, System.Web.Caching.Cache.NoAbsoluteExpiration, expiration);

            // The second approach removes an item from the cache at a given point in time, here after <expiration> has elapsed.
            HttpRuntime.Cache.Insert(key, obj, null, DateTime.Now + expiration, System.Web.Caching.Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// Add an object to the Cache (overwrite if already existent).
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void Add(string key, object obj)
        {
            HttpRuntime.Cache.Insert(key, obj);  
        }

        public object Get(string key)
        {
            return HttpRuntime.Cache.Get(key);
        }

        public T Get<T>(string key)
        {
            return (T)HttpRuntime.Cache.Get(key);
        }

        public void Clear()
        {
            foreach (DictionaryEntry item in HttpRuntime.Cache)
                HttpRuntime.Cache.Remove(item.Key.ToString());
        }

        public void Remove(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }
    }
}
