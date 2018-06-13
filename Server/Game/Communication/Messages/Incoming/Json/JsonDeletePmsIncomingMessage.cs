using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal class JsonDeletePmsIncomingMessage : JsonPacket
    {
        [JsonProperty("pm_array")]
        internal IReadOnlyCollection<uint> PMs { get; set; }
    }
}
