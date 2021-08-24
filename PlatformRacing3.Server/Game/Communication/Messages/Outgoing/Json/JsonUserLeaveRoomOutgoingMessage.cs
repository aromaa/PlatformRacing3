using System.Text.Json.Serialization;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonUserLeaveRoomOutgoingMessage : JsonPacket
    {
        private protected override string InternalType => "userLeaveRoom";

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
