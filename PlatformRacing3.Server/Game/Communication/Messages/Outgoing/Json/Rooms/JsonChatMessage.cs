using System.Drawing;
using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json.Rooms
{
    internal sealed class JsonChatMessage : JsonMessageOutgoingMessage
    {
        internal JsonChatMessage(string roomName, string message, uint socketId, uint userId, string username, Color nameColor, bool highlight = false) : base(roomName, new RoomMessageData("chat", new ChatMessageData(message, socketId, userId, username, nameColor, highlight)))
        {
        }

        private sealed class ChatMessageData
        {
            [JsonPropertyName("message")]
            public string Message { get; set; }

            [JsonPropertyName("socketID")]
            public uint SocketId { get; set; }

            [JsonPropertyName("userID")]
            public uint UserId { get; set; }

            [JsonPropertyName("name")]
            public string Username { get; set; }

            [JsonPropertyName("nameColor")]
            public uint NameColor { get; set; }

            [JsonPropertyName("highlight")]
            public bool Highlight;

            internal ChatMessageData(string message, uint socketId, uint userId, string username, Color nameColor, bool highlight = false)
            {
                this.Message = message;
                this.SocketId = socketId;
                this.UserId = userId;
                this.Username = username;
                this.NameColor = (uint)nameColor.ToArgb();
                this.Highlight = highlight;
            }
        }
    }
}
