using System.Collections.Generic;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing
{
    internal class YouFinishedOutgoingMessage : JsonOutgoingMessage<JsonYouFinishedOutgoingMessage>
    {
        internal YouFinishedOutgoingMessage(uint rank, ulong curExp, ulong maxExp, ulong totExpGain, IReadOnlyCollection<object[]> expArray, int place) 
	        : base(new JsonYouFinishedOutgoingMessage(rank, curExp, maxExp, totExpGain, expArray, place))
        {
        }
    }
}
