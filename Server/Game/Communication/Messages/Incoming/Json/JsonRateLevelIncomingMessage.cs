using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal class JsonRateLevelIncomingMessage : JsonPacket
    {
        [JsonProperty("level_id")]
        internal uint LevelId { get; set; }

        [JsonProperty("rating")]
        internal int Rating { get; set; }
    }
}
