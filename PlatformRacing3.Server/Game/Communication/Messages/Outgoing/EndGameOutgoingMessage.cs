using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

internal class EndGameOutgoingMessage : JsonOutgoingMessage<JsonEndGameOutgoingMessage>
{
	internal EndGameOutgoingMessage() : base(new JsonEndGameOutgoingMessage())
	{
	}
}