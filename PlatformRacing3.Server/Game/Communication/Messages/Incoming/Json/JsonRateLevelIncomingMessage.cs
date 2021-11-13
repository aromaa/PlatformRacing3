using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonRateLevelIncomingMessage : JsonPacket
{
	[JsonPropertyName("level_id")]
	public uint LevelId { get; set; }

	[JsonPropertyName("rating")]
	public int Rating { get; set; }
}