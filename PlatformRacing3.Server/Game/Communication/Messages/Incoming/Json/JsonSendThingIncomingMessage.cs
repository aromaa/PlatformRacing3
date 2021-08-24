using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json
{
    internal sealed class JsonSendThingIncomingMessage : JsonPacket
    {
        [JsonPropertyName("thing")]
        public string Thing { get; set; }

        [JsonPropertyName("thing_id")]
        public uint ThingId { get; set; }
        
        [JsonPropertyName("thing_title")]
        public string ThingTitle { get; set; }

        [JsonPropertyName("user_id")]
        public uint ToUserId { get; set; }
    }
}
