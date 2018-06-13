using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing
{
    internal class MatchOwnerOutgoingMessage : JsonOutgoingMessage
    {
        internal MatchOwnerOutgoingMessage(string name, bool play, bool kick, bool ban) : base(new JsonMatchOwnerOutgoingMessage(name, play, kick, ban))
        {
        }
    }
}
