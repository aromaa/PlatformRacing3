using System.Text.Json;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

internal class SendGameEventOutgoingMessage : JsonOutgoingMessage<JsonGameEventOutgoingMessage>
{
	public SendGameEventOutgoingMessage(string roomName, uint socketId, JsonElement data) : base(new JsonGameEventOutgoingMessage(roomName, socketId, data))
	{
	}
}
