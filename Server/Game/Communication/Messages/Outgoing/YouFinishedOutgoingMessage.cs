using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing
{
    internal class YouFinishedOutgoingMessage : JsonOutgoingMessage
    {
        internal YouFinishedOutgoingMessage(uint rank, ulong curExp, ulong maxExp, ulong totExpGain, IReadOnlyCollection<object[]> expArray) : base(new JsonYouFinishedOutgoingMessage(rank, curExp, maxExp, totExpGain, expArray))
        {
        }
    }
}
