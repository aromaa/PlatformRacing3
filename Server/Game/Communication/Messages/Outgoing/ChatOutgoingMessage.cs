using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json.Rooms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing
{
    internal class ChatOutgoingMessage : JsonOutgoingMessage<JsonChatMessage>
    {
        internal ChatOutgoingMessage(string roomName, string message, uint socketId, uint userId, string username, Color nameColor, bool highlight = false) : base(new JsonChatMessage(roomName, message, socketId, userId, username, nameColor, highlight))
        {

        }
    }
}
