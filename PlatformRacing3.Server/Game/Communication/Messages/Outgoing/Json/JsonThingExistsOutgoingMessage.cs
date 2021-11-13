using System.Text.Json.Serialization;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

internal sealed class JsonThingExistsOutgoingMessage : JsonPacket
{
	private protected override string InternalType => "thingExits";

	[JsonPropertyName("exits")]
	public bool Exists { get; set; }

	internal JsonThingExistsOutgoingMessage(bool exists)
	{
		this.Exists = exists;
	}
}