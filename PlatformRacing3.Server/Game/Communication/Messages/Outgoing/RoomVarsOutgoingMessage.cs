using System.Collections.Generic;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing
{
    internal class RoomVarsOutgoingMessage : JsonOutgoingMessage<JsonRoomVarsOutgoingMessage>
    {
        internal RoomVarsOutgoingMessage(uint chatId, string roomName, IReadOnlyDictionary<string, object> vars) : base(new JsonRoomVarsOutgoingMessage(chatId, roomName, vars))
        {
        }
    }
}
