using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;
using PlatformRacing3.Server.Game.Match;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

internal class PrizeOutgoingMessage : JsonOutgoingMessage<JsonPrizeOutgoingMessage>
{
	internal PrizeOutgoingMessage(MatchPrize prize, string status) : base (new JsonPrizeOutgoingMessage(prize, status))
	{

	}
}