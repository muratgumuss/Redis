using StackExchange.Redis;

namespace RedisExchangeApi.Web.Services
{
    public class RedisService
    {
        private readonly string _redisHost;
        private readonly string _redisPort;
        public ConnectionMultiplexer _redis;
        public IDatabase db { get; set; }
        public RedisService(IConfiguration configuration)
        {
            _redisHost = configuration["Redis:Host"] ?? "localhost";
            _redisPort = configuration["Redis:Port"] ?? "6379";
        }

        public void Connect()
        {
            var configurationOptions = new ConfigurationOptions
            {
                EndPoints = { $"{_redisHost}:{_redisPort}" }
            };
            _redis = ConnectionMultiplexer.Connect(configurationOptions);
            db = _redis.GetDatabase();
        }

        public IDatabase GetDatabase(int db) { 
        
            return _redis.GetDatabase(db);
        }
    }
}
