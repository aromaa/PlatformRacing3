using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal class JsonCoinsIncomingMessage : JsonPacket
    {
        [JsonProperty("coins", Required = Required.Always)]
        internal uint Coins { get; set; }
    }
}
