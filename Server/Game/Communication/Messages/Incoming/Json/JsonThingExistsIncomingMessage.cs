using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal sealed class JsonThingExistsIncomingMessage : JsonPacket
    {
        [JsonPropertyName("thing_type")]
        public string ThingType { get; set; }

        [JsonPropertyName("thing_title")]
        public string ThingTitle { get; set; }

        [JsonPropertyName("thing_category")]
        public string ThingCategory { get; set; }
    }
}
