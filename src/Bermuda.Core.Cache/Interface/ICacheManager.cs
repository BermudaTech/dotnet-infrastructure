namespace Bermuda.Core.Cache
{
    public interface ICacheManager
    {
        bool CacheContains(string key);
        T GetByKey<T>(string key);
        void Set<T>(string key, T data, DateTime expiryDate);
        void Remove(string key);
        void RemoveAll();
    }
}