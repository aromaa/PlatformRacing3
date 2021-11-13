using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

internal class FinishDrawingOutgoingMessage : JsonOutgoingMessage<JsonFinishDrawingOutgoingMessage>
{
	internal FinishDrawingOutgoingMessage(uint socketId) : base(new JsonFinishDrawingOutgoingMessage(socketId))
	{
	}
}