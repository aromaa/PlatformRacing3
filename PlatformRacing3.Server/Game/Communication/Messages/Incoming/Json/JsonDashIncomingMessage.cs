using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json
{
    internal sealed class JsonDashIncomingMessage : JsonPacket
    {
        [JsonPropertyName("dash")]
        public uint Dash { get; set; }
    }
}
