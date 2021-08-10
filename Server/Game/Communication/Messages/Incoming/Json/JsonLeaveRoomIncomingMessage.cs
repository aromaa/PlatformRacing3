using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal sealed class JsonLeaveRoomIncomingMessage : JsonPacket
    {
        [JsonPropertyName("room_name")]
        public string RoomName { get; set; }

        [JsonPropertyName("room_type")]
        public string RoomType { get; set; }
    }
}
