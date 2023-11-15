using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonGetHatIncomingMessage : JsonPacket
{
	[JsonPropertyName("id")]
	public required uint Id { get; init; }
}