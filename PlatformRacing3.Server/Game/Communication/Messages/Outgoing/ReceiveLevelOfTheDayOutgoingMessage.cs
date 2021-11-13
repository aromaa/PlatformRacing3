using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;
using PlatformRacing3.Server.Game.Lobby;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

internal class ReceiveLevelOfTheDayOutgoingMessage : JsonOutgoingMessage<JsonReceiveLevelOfTheDayOutgoingMessage>
{
	internal ReceiveLevelOfTheDayOutgoingMessage(MatchListing listing) : base(new JsonReceiveLevelOfTheDayOutgoingMessage(listing))
	{
	}
}