using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing
{
    internal class LoginSuccessOutgoingMessage : JsonOutgoingMessage<JsonLoginSuccessOutgoingMessage>
    {
        internal LoginSuccessOutgoingMessage(uint socketID, uint userID, string username, IReadOnlyCollection<string> permissions, IReadOnlyDictionary<string, object> vars) : base(new JsonLoginSuccessOutgoingMessage(socketID, userID, username, permissions, vars))
        {
        }
    }
}
