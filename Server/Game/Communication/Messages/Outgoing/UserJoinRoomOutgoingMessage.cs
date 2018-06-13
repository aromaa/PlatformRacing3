using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing
{
    internal class UserJoinRoomOutgoingMessage : JsonOutgoingMessage
    {
        internal UserJoinRoomOutgoingMessage(string roomName, uint socketId, uint userId, string username, IReadOnlyDictionary<string, object> vars) : base(new JsonUserJoinRoomOutgoingMessage(roomName, socketId, userId, username, vars))
        {
        }
    }
}
