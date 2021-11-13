using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;
using PlatformRacing3.Server.Game.Lobby;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

internal class ForceMatchOutgoingMessage : JsonOutgoingMessage<JsonForceMatchOutgoingMessage>
{
	internal ForceMatchOutgoingMessage(MatchListing listing) : base(new JsonForceMatchOutgoingMessage(listing))
	{
	}
}