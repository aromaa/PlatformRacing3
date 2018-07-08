using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal class JsonDashIncomingMessage : JsonPacket
    {
        [JsonProperty("dash", Required = Required.Always)]
        internal uint Dash { get; set; }
    }
}
