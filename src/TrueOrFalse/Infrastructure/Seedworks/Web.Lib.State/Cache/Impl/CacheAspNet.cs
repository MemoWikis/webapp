using CacheManager.Core;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Seedworks.Web.State
{
    //todo (DaMa) überarbeiten, dieser Cache muss nicht mehr alleine existieren und kann verallgemeinert werden, mir fehlt gerade der Überblick um das zu erledigen
    internal class CacheAspNet
    {
        private readonly ICacheManager<object> _cache;
        private CancellationTokenSource _cacheResetToken;

        public CacheAspNet()
        {
            _cache = CacheFactory.Build<object>("CacheAspNet", settings =>
            {
                settings.WithSystemRuntimeCacheHandle("handleName");
            }); ;
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
        public void Add(string key, object obj, TimeSpan? expiration = null, bool slidingExpiration = false)
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

            _cache.Add(key, obj);
        }

        public object Get(string key)
        {
            
            return _cache.Get<object>(key); ;
        }

        public T Get<T>(string key)
        {
            return _cache.Get<T>(key);
        }

        public void Clear()
        {
            _cacheResetToken.Cancel();
            _cacheResetToken.Dispose();
            _cacheResetToken = new CancellationTokenSource();
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}
