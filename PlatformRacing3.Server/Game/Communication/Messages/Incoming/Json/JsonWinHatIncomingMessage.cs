using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json
{
    internal sealed class JsonWinHatIncomingMessage : JsonPacket
    {
        [JsonPropertyName("season")]
        public string Season { get; set; }

        [JsonPropertyName("medals")]
        public uint Medals { get; set; }
    }
}
