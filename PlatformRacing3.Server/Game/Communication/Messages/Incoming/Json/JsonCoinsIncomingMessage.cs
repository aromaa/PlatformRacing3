using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json
{
    internal sealed class JsonCoinsIncomingMessage : JsonPacket
    {
        [JsonPropertyName("coins")]
        public uint Coins { get; set; }
    }
}
