using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json
{
    internal sealed class JsonEditUserListIncomingMessage : JsonPacket
    {
        [JsonPropertyName("user_id")]
        public uint UserId { get; set; }

        [JsonPropertyName("list_type")]
        public string ListType { get; set; }
        
        [JsonPropertyName("action")]
        public string Action { get; set; }
    }
}
