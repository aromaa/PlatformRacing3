using System.Collections.Generic;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;
using PlatformRacing3.Server.Game.Lobby;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing
{
    internal class MatchesOutgoingMessage : JsonOutgoingMessage<JsonMatchesOutgoingMessage>
    {
        internal MatchesOutgoingMessage(uint lobbyId, IReadOnlyCollection<MatchListing> matchListings) : base(new JsonMatchesOutgoingMessage(lobbyId, matchListings))
        {
        }
    }
}
