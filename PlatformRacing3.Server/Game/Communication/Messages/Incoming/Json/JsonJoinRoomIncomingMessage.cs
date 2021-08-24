using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal sealed class JsonJoinRoomIncomingMessage : JsonPacket
    {
        [JsonPropertyName("chatId")]
        public uint ChatId { get; set; }

        [JsonPropertyName("room_name")]
        public string RoomName { get; set; }

        [JsonPropertyName("room_type")]
        public string RoomType { get; set; }

        [JsonPropertyName("pass")]
        public string Pass { get; set; }

        [JsonPropertyName("note")] //Only sent when room is created
        public string Note { get; set; }
    }
}
