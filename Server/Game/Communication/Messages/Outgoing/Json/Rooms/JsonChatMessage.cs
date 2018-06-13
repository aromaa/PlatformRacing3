using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json.Rooms
{
    internal class JsonChatMessage : JsonMessageOutgoingMessage
    {
        internal JsonChatMessage(string roomName, string message, uint socketId, uint userId, string username, Color nameColor, bool highlight = false) : base(roomName, new RoomMessageData("chat", new ChatMessageData(message, socketId, userId, username, nameColor, highlight)))
        {
        }

        private class ChatMessageData
        {
            [JsonProperty("message", Required = Required.Always)]
            internal string Message { get; set; }

            [JsonProperty("socketID", Required = Required.Always)]
            internal uint SocketId { get; set; }

            [JsonProperty("userID", Required = Required.Always)]
            internal uint UserId { get; set; }

            [JsonProperty("name", Required = Required.Always)]
            internal string Username { get; set; }

            [JsonProperty("nameColor", Required = Required.Always)]
            internal uint NameColor { get; set; }

            [JsonProperty("highlight")]
            internal bool Highlight;

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
