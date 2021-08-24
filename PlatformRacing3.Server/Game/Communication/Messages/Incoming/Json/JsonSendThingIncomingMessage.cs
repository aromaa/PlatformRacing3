using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal sealed class JsonSendThingIncomingMessage : JsonPacket
    {
        [JsonPropertyName("thing")]
        public string Thing { get; set; }

        [JsonPropertyName("thing_id")]
        public uint ThingId { get; set; }
        
        [JsonPropertyName("thing_title")]
        public string ThingTitle { get; set; }

        [JsonPropertyName("user_id")]
        public uint ToUserId { get; set; }
    }
}
