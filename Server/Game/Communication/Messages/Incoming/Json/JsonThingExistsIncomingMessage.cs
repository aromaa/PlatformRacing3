using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal class JsonThingExistsIncomingMessage : JsonPacket
    {
        [JsonProperty("thing_type", Required = Required.Always)]
        internal string ThingType { get; set; }

        [JsonProperty("thing_title", Required = Required.Always)]
        internal string ThingTitle { get; set; }

        [JsonProperty("thing_category")]
        internal string ThingCategory { get; set; }
    }
}
