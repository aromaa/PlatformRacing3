using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonGetLevelListIncomingMessage : JsonPacket
{
	[JsonPropertyName("mode")]
	public required string Mode { get; init; }

	[JsonPropertyName("request_id")]
	public required uint RequestId { get; init; }

	[JsonPropertyName("start")]
	public required uint Start { get; init; }

	[JsonPropertyName("count")]
	public required uint Count { get; init; }

	[JsonPropertyName("data")]
	public required string Data { get; init; }
}