using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json;
using Platform_Racing_3_Server.Game.Match;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing
{
    internal class PlayerFinishedOutgoingMessage : JsonOutgoingMessage<JsonPlayerFinishedOutgoingMessage>
    {
        internal PlayerFinishedOutgoingMessage(uint socketId, IReadOnlyCollection<MatchPlayer> players) : base(new JsonPlayerFinishedOutgoingMessage(socketId, players))
        {
        }
    }
}
