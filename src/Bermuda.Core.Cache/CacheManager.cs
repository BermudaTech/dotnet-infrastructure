using Microsoft.Extensions.Caching.Memory;

namespace Bermuda.Core.Cache
{
    public class CacheManager : ICacheManager
    {
        private IMemoryCache _memoryCache;

        public CacheManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public bool CacheContains(string key)
        {
            object cacheValue;

            _memoryCache.TryGetValue(key, out cacheValue);

            if (cacheValue != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public T GetByKey<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public void Set<T>(string key, T data, DateTime expiryDate)
        {
            _memoryCache.Set(key, data, new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = expiryDate,
                Priority = CacheItemPriority.Normal
            });
        }

        public void Remove(string key)
        {
            if (CacheContains(key))
            {
                _memoryCache.Remove(key);
            }
        }

        public void RemoveAll()
        {
            _memoryCache.Dispose();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }
    }
}