using StackExchange.Redis;

namespace Bermuda.Core.Cache
{
    public class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _connection;
        private readonly IDatabase _database;

        public RedisCacheService(string connectionString)
        {
            _connection = ConnectionMultiplexer.Connect(connectionString);
            _database = _connection.GetDatabase();
        }

        public bool CacheContains(string key)
        {
            return _database.KeyExists(key);
        }

        public T GetByKey<T>(string key)
        {
            var value = _database.StringGet(key);
            if (value.IsNullOrEmpty) return default;
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public void Set<T>(string key, T data, DateTime expiryDate)
        {
            _database.StringSet(key, data.ToString(), expiryDate - DateTime.Now);
        }

        public void Remove(string key)
        {
            _database.KeyDelete(key);
        }

        public void RemoveAll()
        {
            var endpoints = _connection.GetEndPoints(true);
            foreach (var endpoint in endpoints)
            {
                var server = _connection.GetServer(endpoint);
                foreach (var key in server.Keys())
                {
                    _database.KeyDelete(key);
                }
            }
        }
    }
}