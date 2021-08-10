using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal sealed class JsonAcceptThingTransferIncomingMessage : JsonPacket
    {
        [JsonPropertyName("transfer_id")]
        public uint TransferId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("comment")]
        public string Description { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("publish")]
        public bool Publish { get; set; }
    }
}
