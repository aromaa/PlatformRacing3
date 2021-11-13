using System.Text.Json.Serialization;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

internal sealed class JsonLegacyPingOutgoingMessage : JsonPacket
{
	private protected override string InternalType => "ping";

	[JsonPropertyName("time")]
	public ulong Time { get; set; }

	[JsonPropertyName("server_time")]
	public ulong ServerTime { get; set; }

	internal JsonLegacyPingOutgoingMessage(ulong time, ulong serverTime)
	{
		this.Time = time;
		this.ServerTime = serverTime;
	}
}