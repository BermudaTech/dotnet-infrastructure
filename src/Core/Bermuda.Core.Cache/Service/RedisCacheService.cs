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

        public bool CacheContains(string key, int? index = null)
        {
            var db = index.HasValue ? _connection.GetDatabase(index.Value) : _database;
            return db.KeyExists(key);
        }

        public T GetByKey<T>(string key, int? index = null)
        {
            var db = index.HasValue ? _connection.GetDatabase(index.Value) : _database;
            var value = db.StringGet(key);
            return ConvertRedisValue<T>(value);
        }

        public Dictionary<string, T> GetList<T>(string pattern, int? index = null)
        {
            var endpoints = _connection.GetEndPoints(true);
            var server = _connection.GetServer(endpoints[0]);
            var db = index.HasValue ? _connection.GetDatabase(index.Value) : _database;
            var keys = server.Keys(db.Database, pattern).ToArray();
            var values = db.StringGet(keys);

            var result = keys
                .Select((key, i) => new { key, value = values[i] })
                .ToDictionary(kv => kv.key.ToString(), kv => ConvertRedisValue<T>(kv.value));
            return result;
        }

        public void Set<T>(string key, T data, DateTime expiryDate, int? index = null)
        {
            var db = index.HasValue ? _connection.GetDatabase(index.Value) : _database;
            var expiry = expiryDate - DateTime.UtcNow;
            if (typeof(T).IsValueType || typeof(T) == typeof(string)) db.StringSet(key, data.ToString(), expiry);
            else
            {
                var jsonValue = JsonSerializer.Serialize(data);
                db.StringSet(key, jsonValue, expiry);
            }
        }

        public void Remove(string key, int? index = null)
        {
            var db = index.HasValue ? _connection.GetDatabase(index.Value) : _database;
            db.KeyDelete(key);
        }

        public void RemoveAll(string pattern = "*", int? index = null)
        {
            var endpoints = _connection?.GetEndPoints(true);
            foreach (var endpoint in endpoints)
            {
                var server = _connection?.GetServer(endpoint);
                var db = index.HasValue ? _connection?.GetDatabase(index.Value) : _database;
                var keys = server?.Keys(db.Database, pattern)?.ToArray();
                db?.KeyDelete(keys);
            }
        }

        #region [[ Private ]]
        public T ConvertRedisValue<T>(RedisValue value)
        {
            if (value.IsNullOrEmpty) return default;

            if (typeof(T).IsValueType || typeof(T) == typeof(string))
            {
                if (typeof(T).IsEnum) return (T)Enum.Parse(typeof(T), value, true);
                return (T)Convert.ChangeType(value.ToString(), typeof(T));
            }

            return JsonSerializer.Deserialize<T>(value);
        }
        #endregion
    }
}