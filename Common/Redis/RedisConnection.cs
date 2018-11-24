using Platform_Racing_3_Common.Config;
using Platform_Racing_3_Common.User;
using Renci.SshNet;
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
        private static SshClient SshClient = null;

        public static void Init(IRedisConfig redisConfig)
        {
            if (redisConfig.RedisUseSsh)
            {
                RedisConnection.SshClient = new SshClient(redisConfig.RedisHost, redisConfig.RedisSshUser, new PrivateKeyFile(redisConfig.RedisSshKey));
                RedisConnection.SshClient.Connect();

                ForwardedPortLocal forward = new ForwardedPortLocal("127.0.0.1", "127.0.0.1", redisConfig.RedisPort);

                RedisConnection.SshClient.AddForwardedPort(forward);

                forward.Start();

                RedisConnection.Redis = ConnectionMultiplexer.Connect(forward.BoundHost + ":" + forward.BoundPort);
            }
            else
            {
                RedisConnection.Redis = ConnectionMultiplexer.Connect(redisConfig.RedisHost + ":" + redisConfig.RedisPort);
            }
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
