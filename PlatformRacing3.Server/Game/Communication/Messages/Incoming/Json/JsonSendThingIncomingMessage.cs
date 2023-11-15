using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonSendThingIncomingMessage : JsonPacket
{
	[JsonPropertyName("thing")]
	public required string Thing { get; init; }

	[JsonPropertyName("thing_id")]
	public required uint ThingId { get; init; }
        
	[JsonPropertyName("thing_title")]
	public required string ThingTitle { get; init; }

	[JsonPropertyName("user_id")]
	public required uint ToUserId { get; init; }
}