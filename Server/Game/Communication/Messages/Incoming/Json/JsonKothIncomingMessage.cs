using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal class JsonKothIncomingMessage : JsonPacket
    {
        [JsonProperty("time", Required = Required.Always)]
        internal string Time { get; set; }
    }
}
