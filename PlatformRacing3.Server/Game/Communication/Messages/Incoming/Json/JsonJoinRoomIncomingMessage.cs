using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonJoinRoomIncomingMessage : JsonPacket
{
	[JsonPropertyName("chatId")]
	public required uint ChatId { get; init; }

	[JsonPropertyName("room_name")]
	public required string RoomName { get; init; }

	[JsonPropertyName("room_type")]
	public required string RoomType { get; init; }

	[JsonPropertyName("pass")]
	public required string Pass { get; init; }

	[JsonPropertyName("note")] //Only sent when room is created
	public string Note { get; init; }
}