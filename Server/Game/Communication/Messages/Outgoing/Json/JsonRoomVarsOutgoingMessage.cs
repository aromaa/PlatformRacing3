using System.Text.Json.Serialization;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonRoomVarsOutgoingMessage : JsonPacket
    {
        public override string Type => "receiveRoomVars";

        [JsonPropertyName("chatId")]
        public uint ChatId { get; set; }

        [JsonPropertyName("roomName")]
        public string RoomName { get; set; }

        [JsonPropertyName("vars")]
        public IReadOnlyDictionary<string, object> Vars { get; set; }

        internal JsonRoomVarsOutgoingMessage(uint chatId, string roomName, IReadOnlyDictionary<string, object> vars)
        {
            this.ChatId = chatId;
            this.RoomName = roomName;
            this.Vars = vars;
        }
    }
}
