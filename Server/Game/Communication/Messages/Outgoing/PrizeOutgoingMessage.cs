using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json;
using Platform_Racing_3_Server.Game.Match;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing
{
    internal class PrizeOutgoingMessage : JsonOutgoingMessage
    {
        internal PrizeOutgoingMessage(MatchPrize prize, string status) : base (new JsonPrizeOutgoingMessage(prize, status))
        {

        }
    }
}
