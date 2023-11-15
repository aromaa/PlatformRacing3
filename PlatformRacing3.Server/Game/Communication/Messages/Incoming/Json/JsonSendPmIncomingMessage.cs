using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonSendPmIncomingMessage : JsonPacket
{
	[JsonPropertyName("name")]
	public required string ReceiverUsername { get; init; }

	[JsonPropertyName("title")]
	public required string Title { get; init; }

	[JsonPropertyName("message")]
	public required string Message { get; init; }
}