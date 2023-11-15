using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonDeletePmsIncomingMessage : JsonPacket
{
	[JsonPropertyName("pm_array")]
	public required IReadOnlyCollection<uint> PMs { get; init; }
}