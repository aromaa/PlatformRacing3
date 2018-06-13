using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal class JsonSendThingIncomingMessage : JsonPacket
    {
        [JsonProperty("thing", Required = Required.Always)]
        internal string Thing { get; set; }

        [JsonProperty("thing_id", Required = Required.Always)]
        internal uint ThingId { get; set; }
        
        [JsonProperty("thing_title", Required = Required.Always)]
        internal string ThingTitle { get; set; }

        [JsonProperty("user_id", Required = Required.Always)]
        internal uint ToUserId { get; set; }
    }
}
