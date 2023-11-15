using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonRateLevelIncomingMessage : JsonPacket
{
	[JsonPropertyName("level_id")]
	public required uint LevelId { get; init; }

	[JsonPropertyName("rating")]
	public required int Rating { get; init; }
}