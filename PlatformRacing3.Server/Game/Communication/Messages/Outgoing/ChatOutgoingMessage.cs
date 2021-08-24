using System.Drawing;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json.Rooms;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing
{
    internal class ChatOutgoingMessage : JsonOutgoingMessage<JsonChatMessage>
    {
        internal ChatOutgoingMessage(string roomName, string message, uint socketId, uint userId, string username, Color nameColor, bool highlight = false) : base(new JsonChatMessage(roomName, message, socketId, userId, username, nameColor, highlight))
        {

        }
    }
}
