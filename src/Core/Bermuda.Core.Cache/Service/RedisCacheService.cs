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
            var result = new Dictionary<string, T>();
            var endpoints = _connection?.GetEndPoints(true);
            if (endpoints is null) return result;
            var db = index.HasValue ? _connection?.GetDatabase(index.Value) : _database;
            if (db is null) return result;
            foreach (var endpoint in endpoints)
            {
                var server = _connection?.GetServer(endpoint);
                if (server is null) continue;
                // Read one key at a time: a multi-key StringGet (MGET) throws CROSSSLOT on a
                // Redis cluster (keys hash to different slots). Single-key GET is slot-safe.
                // Iterate every endpoint so all cluster nodes' slots are covered (mirrors RemoveAll).
                foreach (var key in server.Keys(db.Database, pattern))
                    result[key.ToString()] = ConvertRedisValue<T>(db.StringGet(key));
            }
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
            if (endpoints is null) return;
            foreach (var endpoint in endpoints)
            {
                var server = _connection?.GetServer(endpoint);
                var db = index.HasValue ? _connection?.GetDatabase(index.Value) : _database;
                if (server is null || db is null) continue;
                // Delete one key at a time: a multi-key KeyDelete throws CROSSSLOT on a
                // Redis cluster (keys hash to different slots). Single-key DEL is slot-safe.
                foreach (var key in server.Keys(db.Database, pattern))
                    db.KeyDelete(key);
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