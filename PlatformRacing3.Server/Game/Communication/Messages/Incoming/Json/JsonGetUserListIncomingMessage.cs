using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal sealed class JsonGetUserListIncomingMessage : JsonPacket
    {
        [JsonPropertyName("list_type")]
        public string ListType { get; set; }

        [JsonPropertyName("request_id")]
        public uint RequestId { get; set; }

        [JsonPropertyName("start")]
        public uint Start { get; set; }

        [JsonPropertyName("count")]
        public uint Count { get; set; }
    }
}
