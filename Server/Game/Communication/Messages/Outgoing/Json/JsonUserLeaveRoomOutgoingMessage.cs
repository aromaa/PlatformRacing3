using System.Text.Json.Serialization;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonUserLeaveRoomOutgoingMessage : JsonPacket
    {
        public override string Type => "userLeaveRoom";

        [JsonPropertyName("roomName")]
        public string RoomName { get; set; }

        [JsonPropertyName("socketID")]
        public uint SocketId { get; set; }

        internal JsonUserLeaveRoomOutgoingMessage(string roomName, uint socketId)
        {
            this.RoomName = roomName;
            this.SocketId = socketId;
        }
    }
}
