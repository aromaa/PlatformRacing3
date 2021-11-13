using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

class MatchFailedOutgoingMessage : JsonOutgoingMessage<JsonMatchFailedOutgoingMessage>
{
	internal MatchFailedOutgoingMessage() : base(new JsonMatchFailedOutgoingMessage())
	{
	}
}