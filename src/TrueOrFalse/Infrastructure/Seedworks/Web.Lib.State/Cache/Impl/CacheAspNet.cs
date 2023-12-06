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
                    .WithExpiration(ExpirationMode.Sliding, TimeSpan.FromMinutes(10));
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

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                ExpirationTokens = { new CancellationChangeToken(_cacheResetToken.Token) }
            };

            if (expiration.HasValue)
            {
                if (slidingExpiration)
                {
                    cacheEntryOptions.SlidingExpiration = expiration.Value;
                }
                else
                {
                    cacheEntryOptions.AbsoluteExpirationRelativeToNow = expiration.Value;
                }
            }
            if (key.Contains("SessionUserCacheItem_2150"))
                Logg.r.Information("==Cache== SessionUserCacheItem add 2150, {SlidingExpiration}, {AbsoluteExpirationRelativeToNow}, {options}, {expiration}", cacheEntryOptions.SlidingExpiration, cacheEntryOptions.AbsoluteExpirationRelativeToNow, cacheEntryOptions, expiration.Value);

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
            Logg.r.Information("==Cache== cache clear, stackTrace {stackTrace}", Environment.StackTrace);
            _cacheResetToken.Cancel();
            _cacheResetToken.Dispose();
            _cacheResetToken = new CancellationTokenSource();
        }

        public static void Remove(string key)
        {
            if (key.Contains("SessionUserCacheItem_2150"))
                Logg.r.Information("==Cache== SessionUserCacheItem remove 2150");
            _cache.Remove(key);
        }
    }
}
