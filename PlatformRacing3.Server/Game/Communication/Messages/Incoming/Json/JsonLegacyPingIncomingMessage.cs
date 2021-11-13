using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonLegacyPingIncomingMessage : JsonPacket
{
	[JsonPropertyName("time")]
	public ulong Time { get; set; }
}