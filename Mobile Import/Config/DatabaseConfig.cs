using Newtonsoft.Json;
using Platform_Racing_3_Common.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile_Import.Config
{
    internal class DatabaseConfig : IDatabaseConfig
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
    }
}
