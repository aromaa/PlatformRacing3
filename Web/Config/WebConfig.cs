using Newtonsoft.Json;
using Platform_Racing_3_Common.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform_Racing_3_Web.Config
{
    public class WebConfig : IDatabaseConfig
    {
        [JsonProperty("database_host")]
        public string DatabaseHost { get; set; }
        [JsonProperty("database_user")]
        public string DatabaseUser { get; set; }
        [JsonProperty("database_pass")]
        public string DatabasePass { get; set; }
        [JsonProperty("database_name")]
        public string DatabaseName { get; set; }
    }
}
