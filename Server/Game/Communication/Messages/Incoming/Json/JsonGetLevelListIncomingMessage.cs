using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal class JsonGetLevelListIncomingMessage : JsonPacket
    {
        [JsonProperty("mode", Required = Required.Always)]
        internal string Mode { get; set; }

        [JsonProperty("request_id", Required = Required.Always)]
        internal uint RequestId { get; set; }

        [JsonProperty("start", Required = Required.Always)]
        internal uint Start { get; set; }

        [JsonProperty("count", Required = Required.Always)]
        internal uint Count { get; set; }

        [JsonProperty("data")]
        internal string Data { get; set; }
    }
}
