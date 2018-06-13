using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing
{
    internal class LoginErrorOutgoingMessage : JsonOutgoingMessage
    {
        internal LoginErrorOutgoingMessage(string error) : base(new JsonLoginErrorOutgoingMessage(error))
        {
        }
    }
}
