using System.Collections.Generic;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing
{
    internal class UserVarsOutgoingMessage : JsonOutgoingMessage<JsonUserVarsOutgoingMessage>
    {
        internal UserVarsOutgoingMessage(uint socketId, IReadOnlyDictionary<string, object> vars) : base(new JsonUserVarsOutgoingMessage(socketId, vars))
        {
        }
    }
}
