using System.Collections.Generic;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;
using PlatformRacing3.Server.Game.Match;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing
{
    internal class PlayerFinishedOutgoingMessage : JsonOutgoingMessage<JsonPlayerFinishedOutgoingMessage>
    {
        internal PlayerFinishedOutgoingMessage(uint socketId, IReadOnlyCollection<MatchPlayer> players) : base(new JsonPlayerFinishedOutgoingMessage(socketId, players))
        {
        }
    }
}
