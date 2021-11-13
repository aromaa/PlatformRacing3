using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;
using PlatformRacing3.Server.Game.Match;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing
{
	internal class CoinsOutgoingMessage : JsonOutgoingMessage<JsonCoinsOutgoingMessage>
    {
        internal CoinsOutgoingMessage(IReadOnlyCollection<MatchPlayer> matchPlayer) : base(new JsonCoinsOutgoingMessage(matchPlayer))
        {
        }
    }
}
