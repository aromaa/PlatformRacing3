using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal class JsonJoinRoomIncomingMessage : JsonPacket
    {
        [JsonProperty("chatId")]
        internal uint ChatId { get; set; }

        [JsonProperty("room_name", Required = Required.Always)]
        internal string RoomName { get; set; }

        [JsonProperty("room_type", Required = Required.Always)]
        internal string RoomType { get; set; }

        [JsonProperty("pass", Required = Required.Always)]
        internal string Pass { get; set; }

        [JsonProperty("note")] //Only sent when room is created
        internal string Note { get; set; }
    }
}
