using Microsoft.Extensions.Caching.Memory;

namespace Bermuda.Core.Cache
{
    public class InMemoryCacheService : ICacheService
    {
        private IMemoryCache _memoryCache;

        public InMemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public bool CacheContains(string key)
        {
            return _memoryCache.TryGetValue(key, out _);
        }

        public T GetByKey<T>(string key)
        {
            if (_memoryCache.TryGetValue(key, out var value)) return (T)value;
            return default;
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
            _memoryCache.Remove(key);
        }

        public void RemoveAll()
        {
            _memoryCache.Dispose();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }
    }
}