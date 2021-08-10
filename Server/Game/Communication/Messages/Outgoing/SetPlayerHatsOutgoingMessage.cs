using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json;
using Platform_Racing_3_Server.Game.Match;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing
{
    internal class SetPlayerHatsOutgoingMessage : JsonOutgoingMessage<JsonSetPlayerHatsOutgoingMessage>
    {
        internal SetPlayerHatsOutgoingMessage(uint socketId, IReadOnlyCollection<MatchPlayerHat> hats) : base(new JsonSetPlayerHatsOutgoingMessage(socketId, hats))
        {
        }
    }
}
