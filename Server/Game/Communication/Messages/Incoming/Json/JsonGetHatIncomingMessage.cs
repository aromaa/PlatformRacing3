using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal class JsonGetHatIncomingMessage : JsonPacket
    {
        [JsonProperty("id")]
        internal uint Id { get; set; }
    }
}
