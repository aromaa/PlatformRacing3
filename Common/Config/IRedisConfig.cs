using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Common.Config
{
    public interface IRedisConfig
    {
        string RedisHost { get; }
        uint RedisPort { get; }

        bool RedisUseSsh { get; }
        string RedisSshKey { get; }
        string RedisSshUser { get; }
    }
}
