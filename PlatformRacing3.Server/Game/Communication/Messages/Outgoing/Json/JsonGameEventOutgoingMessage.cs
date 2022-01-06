using System.Text.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

internal class JsonGameEventOutgoingMessage : JsonMessageOutgoingMessage
{
	internal JsonGameEventOutgoingMessage(string roomName, uint socketId, JsonElement data) : base(roomName, socketId, new RoomMessageData("gameEvent", data))
	{
	}
}
