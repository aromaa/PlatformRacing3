using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

internal class ForfietOutgoingMessage : JsonOutgoingMessage<JsonForfietOutgoingMessage>
{
	internal ForfietOutgoingMessage(uint socketId) : base(new JsonForfietOutgoingMessage(socketId))
	{
	}
}