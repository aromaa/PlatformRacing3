using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonThingExistsIncomingMessage : JsonPacket
{
	[JsonPropertyName("thing_type")]
	public required string ThingType { get; init; }

	[JsonPropertyName("thing_title")]
	public required string ThingTitle { get; init; }

	[JsonPropertyName("thing_category")]
	public required string ThingCategory { get; init; }
}