using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal class JsonRequestMatchesIncomingMessage : JsonPacket
    {
        [JsonProperty("num")]
        public uint Num { get; set; }
    }
}
