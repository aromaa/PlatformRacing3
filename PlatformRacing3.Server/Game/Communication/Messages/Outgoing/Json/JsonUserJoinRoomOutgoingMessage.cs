using System.Collections.Generic;
using System.Text.Json.Serialization;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonUserJoinRoomOutgoingMessage : JsonPacket
    {
        private protected override string InternalType => "userJoinRoom";

        [JsonPropertyName("roomName")]
        public string RoomName { get; set; }

        [JsonPropertyName("socketID")]
        public uint SocketId { get; set; }

        [JsonPropertyName("userID")]
        public uint UserId { get; set; }

        [JsonPropertyName("userName")]
        public string Username { get; set; }

        [JsonPropertyName("vars")]
        public IReadOnlyDictionary<string, object> Vars { get; set; }

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
