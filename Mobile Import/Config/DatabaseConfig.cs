using Platform_Racing_3_Common.Config;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Mobile_Import.Config
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
