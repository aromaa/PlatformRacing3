using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;
using PlatformRacing3.Server.Game.Lobby;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing
{
    internal class MatchCreatedOutgoingMessage : JsonOutgoingMessage<JsonMatchCreatedOutgoingMessage>
    {
        internal MatchCreatedOutgoingMessage(MatchListing matchListing) : base(new JsonMatchCreatedOutgoingMessage(matchListing))
        {
        }
    }
}
