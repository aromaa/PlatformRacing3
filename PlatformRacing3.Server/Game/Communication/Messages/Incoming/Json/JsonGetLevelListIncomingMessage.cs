using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal sealed class JsonGetLevelListIncomingMessage : JsonPacket
    {
        [JsonPropertyName("mode")]
        public string Mode { get; set; }

        [JsonPropertyName("request_id")]
        public uint RequestId { get; set; }

        [JsonPropertyName("start")]
        public uint Start { get; set; }

        [JsonPropertyName("count")]
        public uint Count { get; set; }

        [JsonPropertyName("data")]
        public string Data { get; set; }
    }
}
