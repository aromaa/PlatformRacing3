using System.Text.Json;
using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonSendToRoomIncomingMessage : JsonPacket
{
	[JsonPropertyName("room_name")]
	public required string RoomName { get; init; }

	[JsonPropertyName("room_type")]
	public required string RoomType { get; init; }

	[JsonPropertyName("send_to_self")]
	public required bool SendToSelf { get; init; }

	[JsonPropertyName("data")]
	public required RoomMessageData Data { get; init; }

	internal sealed class RoomMessageData
	{
		[JsonPropertyName("type")] //Optional
		public string Type { get; init; }

		[JsonPropertyName("data")]
		public required JsonElement Data { get; init; }
	}
}