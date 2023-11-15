using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonGetPmsIncomingMessage : JsonPacket
{
	[JsonPropertyName("request_id")]
	public required uint RequestId { get; init; }

	[JsonPropertyName("start")]
	public required uint Start { get; init; }

	[JsonPropertyName("count")]
	public required uint Count { get; init; }
}