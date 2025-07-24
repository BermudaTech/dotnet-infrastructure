using Microsoft.Extensions.Caching.Memory;

namespace Bermuda.Core.Cache
{
    public class InMemoryCacheService : ICacheService
    {
        private IMemoryCache _memoryCache;
        private HashSet<string> _keys = new HashSet<string>();

        public InMemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public bool CacheContains(string key, int? index = null)
        {
            return _memoryCache.TryGetValue(key, out _);
        }

        public T GetByKey<T>(string key, int? index = null)
        {
            if (_memoryCache.TryGetValue(key, out var value)) return (T)value;
            return default;
        }

        public Dictionary<string, T> GetList<T>(string pattern, int? index = null)
        {
            var result = new Dictionary<string, T>();
            foreach(var key in _keys.Where(w => w.StartsWith(pattern.Replace("*", ""))))
            {
                var value = GetByKey<T>(key, index);
                result.Add(key, value);
            }
            return result;
        }

        public void Set<T>(string key, T data, DateTime expiryDate, int? index = null)
        {
            _memoryCache.Set(key, data, new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = expiryDate,
                Priority = CacheItemPriority.Normal
            });

            if (!_keys.Contains(key)) _keys.Add(key);
        }

        public void Remove(string key, int? index = null)
        {
            _memoryCache.Remove(key);
            _keys.Remove(key);
        }

        public void RemoveAll(int? index = null)
        {
            _memoryCache.Dispose();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _keys = new HashSet<string>();
        }
    }
}