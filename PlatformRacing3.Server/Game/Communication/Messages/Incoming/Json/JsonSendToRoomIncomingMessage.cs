using System.Text.Json;
using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonSendToRoomIncomingMessage : JsonPacket
{
	[JsonPropertyName("room_name")]
	public string RoomName { get; set; }

	[JsonPropertyName("room_type")]
	public string RoomType { get; set; }

	[JsonPropertyName("send_to_self")]
	public bool SendToSelf { get; set; }

	[JsonPropertyName("data")]
	public RoomMessageData Data { get; set; }

	internal sealed class RoomMessageData
	{
		[JsonPropertyName("type")] //Optional
		public string Type { get; set; }

		[JsonPropertyName("data")]
		public JsonElement Data { get; set; }
	}
}