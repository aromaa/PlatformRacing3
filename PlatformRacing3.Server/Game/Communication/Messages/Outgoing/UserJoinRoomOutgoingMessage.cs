using System.Collections.Generic;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing
{
    internal class UserJoinRoomOutgoingMessage : JsonOutgoingMessage<JsonUserJoinRoomOutgoingMessage>
    {
        internal UserJoinRoomOutgoingMessage(string roomName, uint socketId, uint userId, string username, IReadOnlyDictionary<string, object> vars) : base(new JsonUserJoinRoomOutgoingMessage(roomName, socketId, userId, username, vars))
        {
        }
    }
}
