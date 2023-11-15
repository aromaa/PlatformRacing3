using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonAcceptThingTransferIncomingMessage : JsonPacket
{
	[JsonPropertyName("transfer_id")]
	public required uint TransferId { get; init; }

	[JsonPropertyName("title")]
	public required string Title { get; init; }

	[JsonPropertyName("comment")]
	public required string Description { get; init; }

	[JsonPropertyName("category")]
	public required string Category { get; init; }

	[JsonPropertyName("publish")]
	public required bool Publish { get; init; }
}