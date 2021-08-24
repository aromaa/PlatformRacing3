using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing
{
    internal class RoomVarsOutgoingMessage : JsonOutgoingMessage<JsonRoomVarsOutgoingMessage>
    {
        internal RoomVarsOutgoingMessage(uint chatId, string roomName, IReadOnlyDictionary<string, object> vars) : base(new JsonRoomVarsOutgoingMessage(chatId, roomName, vars))
        {
        }
    }
}
