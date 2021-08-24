using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json
{
    internal sealed class JsonDeletePmsIncomingMessage : JsonPacket
    {
        [JsonPropertyName("pm_array")]
        public IReadOnlyCollection<uint> PMs { get; set; }
    }
}
