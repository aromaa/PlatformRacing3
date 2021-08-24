using System.Text.Json.Serialization;
using PlatformRacing3.Common.Config;

namespace PlatformRacing3.MobileImport.Config
{
    internal class DatabaseConfig : IDatabaseConfig
    {
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
    }
}
