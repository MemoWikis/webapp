using System;
using System.Collections;

namespace Seedworks.Web.State
{    
    public class Cache
    {
        private static readonly ICache _cache;
        
        public static int Count{ get { return _cache.Count; } }

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
        public static void Add(string key, object obj, TimeSpan timeSpan)
        {
            _cache.Add(key, obj, timeSpan);
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

        public static IDictionaryEnumerator GetEnumerator()
        {
            return _cache.GetEnumerator();
        }
    }

    ///// <summary>
    ///// Convenience Name in case the "Cache" is used as MemberVariable. E.g: System.Web.Page.Cache
    ///// </summary>
    //public class CacheSf : Cache{}
    //NO! do this instead:
    //using CacheSf = SpeakFriend.Utilities.Web.Cache;
    
}
