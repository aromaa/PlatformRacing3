using System.Text.Json.Serialization;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json
{
	internal sealed class JsonRoomVarsOutgoingMessage : JsonPacket
    {
        private protected override string InternalType => "receiveRoomVars";

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
