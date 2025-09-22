namespace Bermuda.Core.Cache
{
    public interface ICacheService
    {
        bool CacheContains(string key, int? index = null);
        T GetByKey<T>(string key, int? index = null);
        Dictionary<string, T> GetList<T>(string pattern, int? index = null);
        void Set<T>(string key, T data, DateTime expiryDate, int? index = null);
        void Remove(string key, int? index = null);
        void RemoveAll(int? index = null);
    }
}