using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing
{
    internal class YouFinishedOutgoingMessage : JsonOutgoingMessage<JsonYouFinishedOutgoingMessage>
    {
        internal YouFinishedOutgoingMessage(uint rank, ulong curExp, ulong maxExp, ulong totExpGain, IReadOnlyCollection<object[]> expArray, int place) 
	        : base(new JsonYouFinishedOutgoingMessage(rank, curExp, maxExp, totExpGain, expArray, place))
        {
        }
    }
}
