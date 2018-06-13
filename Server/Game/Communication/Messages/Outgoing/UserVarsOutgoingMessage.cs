using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing
{
    internal class UserVarsOutgoingMessage : JsonOutgoingMessage
    {
        internal UserVarsOutgoingMessage(uint socketId, IReadOnlyDictionary<string, object> vars) : base(new JsonUserVarsOutgoingMessage(socketId, vars))
        {
        }
    }
}
