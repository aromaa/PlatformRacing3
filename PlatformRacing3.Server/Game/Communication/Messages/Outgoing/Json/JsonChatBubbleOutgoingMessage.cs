using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

internal class JsonChatBubbleOutgoingMessage : JsonMessageOutgoingMessage
{
	internal JsonChatBubbleOutgoingMessage(string roomName, uint socketId, int bubbleId) : base(roomName, socketId, new RoomMessageData("chatBubble", new ChatBubbleData(bubbleId)))
	{
	}

	internal sealed class ChatBubbleData
	{
		[JsonPropertyName("id")]
		public int BubbleId { get; set; }

		internal ChatBubbleData(int bubbleId)
		{
			this.BubbleId = bubbleId;
		}
	}
}
