using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json;
using Platform_Racing_3_Server.Game.Lobby;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing
{
    internal class QuickJoinSuccessOutgoingMessage : JsonOutgoingMessage
    {
        internal QuickJoinSuccessOutgoingMessage(MatchListing matchListing) : base(new JsonQuickJoinSuccessOutgoingMessage(matchListing))
        {
        }
    }
}
