using Platform_Racing_3_Common.Config;
using Platform_Racing_3_Common.User;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Platform_Racing_3_Common.Redis
{
    public class RedisConnection
    {
        private static ConnectionMultiplexer Redis;

        public static void Init(IRedisConfig redisConfig)
        {
            RedisConnection.Redis = ConnectionMultiplexer.Connect(redisConfig.RedisHost + ":" + redisConfig.RedisPort);
        }

        public static ConnectionMultiplexer GetConnectionMultiplexer() => RedisConnection.Redis;
        public static IDatabase GetDatabase() => RedisConnection.GetConnectionMultiplexer().GetDatabase();

        /*public static Task<RedisResult> HashExchangeAsync(RedisKey key, RedisKey field, RedisValue value)
        {
            return RedisConnection.GetDatabase().ScriptEvaluateAsync(@"
local serverId = redis.call('hget', KEYS[1], KEYS[2])
if serverId != ARGV[1] then
    redis.call('hset', KEYS[1], KEYS[2], ARGV[1])
end

return serverId", new RedisKey[]{ key, field }, new RedisValue[] { value });
        }*/
    }
}
