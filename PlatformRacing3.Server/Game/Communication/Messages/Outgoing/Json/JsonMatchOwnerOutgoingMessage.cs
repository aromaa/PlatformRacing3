using System.Text.Json.Serialization;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

internal sealed class JsonMatchOwnerOutgoingMessage : JsonPacket
{
	private protected override string InternalType => "matchOwner";

	[JsonPropertyName("matchName")]
	public string Name { get; set; }

	[JsonPropertyName("play")]
	public bool Play { get; set; }
	[JsonPropertyName("kick")]
	public bool Kick { get; set; }
	[JsonPropertyName("ban")]
	public bool Ban { get; set; }

	internal JsonMatchOwnerOutgoingMessage(string name, bool play, bool kick, bool ban)
	{
		this.Name = name;

		this.Play = play;
		this.Kick = kick;
		this.Ban = ban;
	}
}