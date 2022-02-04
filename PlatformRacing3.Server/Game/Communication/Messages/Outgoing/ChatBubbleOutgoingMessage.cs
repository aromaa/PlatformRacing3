using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

internal class ChatBubbleOutgoingMessage : JsonOutgoingMessage<JsonChatBubbleOutgoingMessage>
{
	public ChatBubbleOutgoingMessage(string roomName, uint socketId, int bubbleId) : base(new JsonChatBubbleOutgoingMessage(roomName, socketId, bubbleId))
	{

	}
}
