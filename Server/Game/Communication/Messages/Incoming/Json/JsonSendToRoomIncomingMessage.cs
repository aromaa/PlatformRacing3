using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal sealed class JsonSendToRoomIncomingMessage : JsonPacket
    {
        [JsonPropertyName("room_name")]
        public string RoomName { get; set; }

        [JsonPropertyName("room_type")]
        public string RoomType { get; set; }

        [JsonPropertyName("send_to_self")]
        public bool SendToSelf { get; set; }

        [JsonPropertyName("data")]
        public RoomMessageData Data { get; set; }

        internal sealed class RoomMessageData
        {
            [JsonPropertyName("type")] //Optional
            public string Type { get; set; }

            [JsonPropertyName("data")]
            public JsonElement Data { get; set; }
        }
    }
}
