using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

internal class SocketIdOutgoingMessage : JsonOutgoingMessage<JsonSocketIdOutgoingMessage>
{
	internal SocketIdOutgoingMessage(uint socketId) : base(new JsonSocketIdOutgoingMessage(socketId))
	{
	}
}