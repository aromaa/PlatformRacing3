using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonGetPmIncomingMessage : JsonPacket
{
	[JsonPropertyName("message_id")]
	public required uint MessageId { get; init; }
}