using CacheManager.Core;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Seedworks.Web.State
{
    //todo (DaMa) überarbeiten, dieser Cache muss nicht mehr alleine existieren und kann verallgemeinert werden, mir fehlt gerade der Überblick um das zu erledigen
    internal class CacheAspNet
    {
        private static readonly ICacheManager<object> _cache;
        private static CancellationTokenSource _cacheResetToken;

        static CacheAspNet()
        {
            _cache = CacheFactory.Build<object>(settings =>
            {
                settings.WithDictionaryHandle()
                    .EnablePerformanceCounters()
                    .WithExpiration(ExpirationMode.Sliding, TimeSpan.FromMinutes(Settings.SessionStateTimeoutInMin));
            });
            _cacheResetToken = new CancellationTokenSource();
        }

        /// <summary>
        /// Add an object to the Cache (overwrite if already existent).<br/>
        /// Remove the item from the cache after <paramref name="expiration"/> has elapsed.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiration"></param>
        /// <param name="slidingExpiration"></param>
        public static void Add(string key, object obj, TimeSpan? expiration = null, bool slidingExpiration = false)
        {
            //code block below does nothing
                //var cacheEntryOptions = new MemoryCacheEntryOptions
                //{
                //    ExpirationTokens = { new CancellationChangeToken(_cacheResetToken.Token) }
                //};

                //if (expiration.HasValue)
                //{
                //    if (slidingExpiration)
                //    {
                //        cacheEntryOptions.SlidingExpiration = expiration.Value;
                //    }
                //    else
                //    {
                //        cacheEntryOptions.AbsoluteExpirationRelativeToNow = expiration.Value;
                //    }
                //}

            _cache.Add(key, obj);
        }

        public static object? Get(string key)
        {
            return _cache.Get<object>(key); ;
        }

        public static T Get<T>(string key)
        {
            return _cache.Get<T>(key);
        }

        public static void Clear()
        {
            _cacheResetToken.Cancel();
            _cacheResetToken.Dispose();
            _cacheResetToken = new CancellationTokenSource();
        }

        public static void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}
