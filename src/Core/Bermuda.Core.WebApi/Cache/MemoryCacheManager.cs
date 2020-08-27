using Bermuda.Core.CacheManager;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace Bermuda.Core.WebApi.Cache
{
    public class MemoryCacheManager : ICacheManager
    {
        private readonly IMemoryCache memoryCache;

        public MemoryCacheManager(
            IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public bool CacheContains(string key)
        {
            object cacheValue;

            memoryCache.TryGetValue(key, out cacheValue);

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
            return memoryCache.Get<T>(key);
        }

        public void Set<T>(string key, T data, DateTime expiryDate)
        {
            memoryCache.Set(key, data, new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = expiryDate,
                Priority = CacheItemPriority.Normal
            });
        }

        public void Remove(string key)
        {
            if (CacheContains(key))
            {
                memoryCache.Remove(key);
            }
        }
    }
}
