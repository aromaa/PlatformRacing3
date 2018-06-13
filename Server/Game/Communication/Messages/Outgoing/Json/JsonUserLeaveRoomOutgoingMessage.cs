using Newtonsoft.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonUserLeaveRoomOutgoingMessage : JsonPacket
    {
        internal override string Type => "userLeaveRoom";

        [JsonProperty("roomName")]
        internal string RoomName { get; set; }

        [JsonProperty("socketID")]
        internal uint SocketId { get; set; }

        internal JsonUserLeaveRoomOutgoingMessage(string roomName, uint socketId)
        {
            this.RoomName = roomName;
            this.SocketId = socketId;
        }
    }
}
