using System.Text.Json.Serialization;
using Platform_Racing_3_Common.Config;
using Platform_Racing_3_Common.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Config
{
    internal class ServerConfig : IDatabaseConfig, IRedisConfig
    {
        [JsonPropertyName("server_id")]
        public uint ServerId { get; set; }

        [JsonPropertyName("discord_chat_webhook_id")]
        public ulong DiscordChatWebhookId { get; set; }
        [JsonPropertyName("discord_chat_webhook_token")]
        public string DiscordChatWebhookToken { get; set; }

        [JsonPropertyName("discord_notifications_webhook_id")]
        public ulong DiscordNotificationsWebhookId { get; set; }
        [JsonPropertyName("discord_notifications_webhook_token")]
        public string DiscordNotificationsWebhookToken { get; set; }

        [JsonPropertyName("database_host")]
        public string DatabaseHost { get; set; }
        [JsonPropertyName("database_port")]
        public uint DatabasePort { get; set; }

        [JsonPropertyName("database_user")]
        public string DatabaseUser { get; set; }
        [JsonPropertyName("database_pass")]
        public string DatabasePass { get; set; }
        [JsonPropertyName("database_name")]
        public string DatabaseName { get; set; }

        [JsonPropertyName("redis_host")]
        public string RedisHost { get; set; }
        [JsonPropertyName("redis_port")]
        public uint RedisPort { get; set; }

        [JsonPropertyName("bind_ip")]
        public string BindIp { get; set; }
        [JsonPropertyName("bind_port")]
        public ushort BindPort { get; set; }
    }
}
