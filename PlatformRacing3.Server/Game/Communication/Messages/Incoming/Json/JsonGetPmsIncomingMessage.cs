using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonGetPmsIncomingMessage : JsonPacket
{
	[JsonPropertyName("request_id")]
	public uint RequestId { get; set; }

	[JsonPropertyName("start")]
	public uint Start { get; set; }

	[JsonPropertyName("count")]
	public uint Count { get; set; }
}