using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;
using PlatformRacing3.Server.Game.Match;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing
{
	internal class SetPlayerHatsOutgoingMessage : JsonOutgoingMessage<JsonSetPlayerHatsOutgoingMessage>
    {
        internal SetPlayerHatsOutgoingMessage(uint socketId, IReadOnlyCollection<MatchPlayerHat> hats) : base(new JsonSetPlayerHatsOutgoingMessage(socketId, hats))
        {
        }
    }
}
