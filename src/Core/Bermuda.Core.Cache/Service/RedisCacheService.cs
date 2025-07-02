using StackExchange.Redis;
using System.Text.Json;

namespace Bermuda.Core.Cache
{
    public class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _connection;
        private readonly IDatabase _database;

        public RedisCacheService(string connectionString, int index = 0, bool ssl = true)
        {
            var configurationOptions = new ConfigurationOptions
            {
                EndPoints = { connectionString },
                Ssl = ssl
            };
            _connection = ConnectionMultiplexer.Connect(configurationOptions);
            _database = _connection.GetDatabase(index);
        }

        public bool CacheContains(string key)
        {
            return _database.KeyExists(key);
        }

        public T GetByKey<T>(string key)
        {
            var value = _database.StringGet(key);
            if (value.IsNullOrEmpty) return default;

            if (typeof(T).IsValueType || typeof(T) == typeof(string))
            {
                if (typeof(T).IsEnum) return (T)Enum.Parse(typeof(T), value, true);
                return (T)Convert.ChangeType(value.ToString(), typeof(T));
            }
                
            return JsonSerializer.Deserialize<T>(value);
        }

        public void Set<T>(string key, T data, DateTime expiryDate)
        {
            var expiry = expiryDate - DateTime.UtcNow;
            if (typeof(T).IsValueType || typeof(T) == typeof(string)) _database.StringSet(key, data.ToString(), expiry);
            else
            {
                var jsonValue = JsonSerializer.Serialize(data);
                _database.StringSet(key, jsonValue, expiry);
            }
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
                foreach (var key in server.Keys(_database.Database))
                {
                    _database.KeyDelete(key);
                }
            }
        }
    }
}