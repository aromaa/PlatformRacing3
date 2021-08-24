using System.Text.Json.Serialization;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonMessageOutgoingMessage : JsonPacket
    {
        private protected override string InternalType => "receiveMessage";

        [JsonPropertyName("roomName")]
        public string RoomName { get; set; }

        [JsonPropertyName("socketID")]
        public uint SocketId { get; set; }

        [JsonPropertyName("data")]
        public object Data { get; set; }

        internal JsonMessageOutgoingMessage(string roomName, RoomMessageData data)
        {
            this.RoomName = roomName;
            this.Data = data;
        }

        internal JsonMessageOutgoingMessage(uint socketId, RoomMessageData data)
        {
            this.SocketId = socketId;
            this.Data = data;
        }

        internal JsonMessageOutgoingMessage(string roomName, uint socketId, RoomMessageData data)
        {
            this.RoomName = roomName;
            this.SocketId = socketId;
            this.Data = data;
        }

        internal class RoomMessageData
        {
            [JsonPropertyName("type")]
            public string Type { get; set; }

            [JsonPropertyName("data")]
            public object Data { get; set; }

            internal RoomMessageData(string type, object data)
            {
                this.Type = type;
                this.Data = data;
            }

            internal RoomMessageData(object data)
            {
                this.Data = data;
            }
        }
    }
}
