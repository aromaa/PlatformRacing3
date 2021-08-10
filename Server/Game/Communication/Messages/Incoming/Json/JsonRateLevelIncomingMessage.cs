using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal sealed class JsonRateLevelIncomingMessage : JsonPacket
    {
        [JsonPropertyName("level_id")]
        public uint LevelId { get; set; }

        [JsonPropertyName("rating")]
        public int Rating { get; set; }
    }
}
