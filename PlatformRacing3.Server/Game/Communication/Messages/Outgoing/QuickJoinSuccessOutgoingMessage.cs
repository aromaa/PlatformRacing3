using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;
using PlatformRacing3.Server.Game.Lobby;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing
{
    internal class QuickJoinSuccessOutgoingMessage : JsonOutgoingMessage<JsonQuickJoinSuccessOutgoingMessage>
    {
        internal QuickJoinSuccessOutgoingMessage(MatchListing matchListing) : base(new JsonQuickJoinSuccessOutgoingMessage(matchListing))
        {
        }
    }
}
