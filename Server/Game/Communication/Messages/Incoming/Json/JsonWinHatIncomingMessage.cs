using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal class JsonWinHatIncomingMessage : JsonPacket
    {
        [JsonProperty("season")]
        internal string Season { get; set; }

        [JsonProperty("medals", Required = Required.Always)]
        internal uint Medals { get; set; }
    }
}
