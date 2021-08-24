using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal sealed class JsonGetMemberListIncomingMessage : JsonPacket
    {
        [JsonPropertyName("room_name")]
        public string RoomName { get; set; }
    }
}
