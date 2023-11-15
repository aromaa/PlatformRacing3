using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonLeaveRoomIncomingMessage : JsonPacket
{
	[JsonPropertyName("room_name")]
	public required string RoomName { get; init; }

	[JsonPropertyName("room_type")]
	public required string RoomType { get; init; }
}