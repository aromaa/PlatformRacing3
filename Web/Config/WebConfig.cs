using Newtonsoft.Json;
using Platform_Racing_3_Common.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform_Racing_3_Web.Config
{
    public class WebConfig : IDatabaseConfig, IRedisConfig, IEmailConfig
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
        
        [JsonProperty("smtp_host", Required = Required.Always)]
        public string SmtpHost { get; set; }
        [JsonProperty("smtp_port", Required = Required.Always)]
        public ushort SmtpPort { get; set; }

        [JsonProperty("smtp_user", Required = Required.Always)]
        public string SmtpUser { get; set; }
        [JsonProperty("smtp_pass", Required = Required.Always)]
        public string SmtpPass { get; set; }

        [JsonProperty("discord_client_id", Required = Required.Always)]
        public string DiscordClientId { get; set; }
        [JsonProperty("discord_client_secret", Required = Required.Always)]
        public string DiscordClientSecret { get; set; }
    }
}
