using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonWinHatIncomingMessage : JsonPacket
{
	[JsonPropertyName("season")]
	public required string Season { get; init; }

	[JsonPropertyName("medals")]
	public required uint Medals { get; init; }
}