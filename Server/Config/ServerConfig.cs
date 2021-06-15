using Newtonsoft.Json;
using Platform_Racing_3_Common.Config;
using Platform_Racing_3_Common.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Config
{
    internal class ServerConfig : IDatabaseConfig, IRedisConfig
    {
        [JsonProperty("server_id", Required = Required.Always)]
        internal uint ServerId { get; set; }

        [JsonProperty("discord_chat_webhook_id")]
        internal ulong DiscordChatWebhookId { get; set; }
        [JsonProperty("discord_chat_webhook_token")]
        internal string DiscordChatWebhookToken { get; set; }

        [JsonProperty("discord_notifications_webhook_id")]
        internal ulong DiscordNotificationsWebhookId { get; set; }
        [JsonProperty("discord_notifications_webhook_token")]
        internal string DiscordNotificationsWebhookToken { get; set; }

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

        [JsonProperty("redis_host", Required = Required.Always)]
        public string RedisHost { get; set; }
        [JsonProperty("redis_port", Required = Required.Always)]
        public uint RedisPort { get; set; }

        [JsonProperty("bind_ip", Required = Required.Always)]
        internal string BindIp { get; set; }
        [JsonProperty("bind_port", Required = Required.Always)]
        internal ushort BindPort { get; set; }
    }
}
