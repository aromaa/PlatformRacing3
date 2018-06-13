using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal class JsonLegacyPingIncomingMessage : JsonPacket
    {
        [JsonProperty("time", Required = Required.Always)]
        public ulong Time { get; set; }
    }
}
