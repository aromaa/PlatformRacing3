using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json;
using Platform_Racing_3_Server.Game.Lobby;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing
{
    internal class ForceMatchOutgoingMessage : JsonOutgoingMessage<JsonForceMatchOutgoingMessage>
    {
        internal ForceMatchOutgoingMessage(MatchListing listing) : base(new JsonForceMatchOutgoingMessage(listing))
        {
        }
    }
}
