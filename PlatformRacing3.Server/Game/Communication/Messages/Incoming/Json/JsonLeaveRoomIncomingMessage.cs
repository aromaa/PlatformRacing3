using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json
{
    internal sealed class JsonLeaveRoomIncomingMessage : JsonPacket
    {
        [JsonPropertyName("room_name")]
        public string RoomName { get; set; }

        [JsonPropertyName("room_type")]
        public string RoomType { get; set; }
    }
}
