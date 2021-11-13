using System.Text.Json.Serialization;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

internal sealed class JsonVersionOutgoingMessage : JsonPacket
{
	private protected override string InternalType => "receiveVersion";

	[JsonPropertyName("version")]
	public uint Version { get; set; }

	internal JsonVersionOutgoingMessage(uint version)
	{
		this.Version = version;
	}
}