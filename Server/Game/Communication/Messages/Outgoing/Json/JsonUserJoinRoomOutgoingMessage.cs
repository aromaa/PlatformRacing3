using Newtonsoft.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonUserJoinRoomOutgoingMessage : JsonPacket
    {
        internal override string Type => "userJoinRoom";

        [JsonProperty("roomName")]
        internal string RoomName { get; set; }

        [JsonProperty("socketID")]
        internal uint SocketId { get; set; }

        [JsonProperty("userID")]
        internal uint UserId { get; set; }

        [JsonProperty("userName")]
        internal string Username { get; set; }

        [JsonProperty("vars")]
        internal IReadOnlyDictionary<string, object> Vars { get; set; }

        internal JsonUserJoinRoomOutgoingMessage(string roomName, uint socketId, uint userId, string username, IReadOnlyDictionary<string, object> vars)
        {
            this.RoomName = roomName;
            this.SocketId = socketId;
            this.UserId = userId;
            this.Username = username;
            this.Vars = vars;
        }
    }
}
