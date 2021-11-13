using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

internal class MessageOutgoingMessage : JsonOutgoingMessage<JsonMessageOutgoingMessage>
{
	internal MessageOutgoingMessage(string roomName, JsonMessageOutgoingMessage.RoomMessageData data) : base(new JsonMessageOutgoingMessage(roomName, data))
	{
	}

	internal MessageOutgoingMessage(uint socketId, JsonMessageOutgoingMessage.RoomMessageData data) : base(new JsonMessageOutgoingMessage(socketId, data))
	{
	}

	internal MessageOutgoingMessage(string roomName, uint socketId, JsonMessageOutgoingMessage.RoomMessageData data) : base(new JsonMessageOutgoingMessage(roomName, socketId, data))
	{
	}
}