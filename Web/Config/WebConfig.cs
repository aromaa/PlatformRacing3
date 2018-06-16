using Newtonsoft.Json;
using Platform_Racing_3_Common.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform_Racing_3_Web.Config
{
    public class WebConfig : IDatabaseConfig, IRedisConfig
    {
        [JsonProperty("database_host", Required = Required.Always)]
        public string DatabaseHost { get; set; }
        [JsonProperty("database_port", Required = Required.Always)]
        public uint DatabasePort { get; set; }

        [JsonProperty("database_user", Required = Required.Always)]
        public string DatabaseUser { get; set; }
        [JsonProperty("database_pass", Required = Required.Always)]
        public string DatabasePass { get; set; }
        [JsonProperty("database_name", Required = Required.Always)]
        public string DatabaseName { get; set; }

        [JsonProperty("database_use_ssh")]
        public bool DatabaseUseSsh { get; set; }
        [JsonProperty("database_ssh_key")]
        public string DatabaseSshKey { get; set; }
        [JsonProperty("database_ssh_user")]
        public string DatabaseSshUser { get; set; }

        [JsonProperty("redis_host", Required = Required.Always)]
        public string RedisHost { get; set; }
        [JsonProperty("redis_port", Required = Required.Always)]
        public uint RedisPort { get; set; }

        [JsonProperty("redis_use_ssh")]
        public bool RedisUseSsh { get; set; }
        [JsonProperty("redis_ssh_key")]
        public string RedisSshKey { get; set; }
        [JsonProperty("redis_ssh_user")]
        public string RedisSshUser { get; set; }
    }
}
