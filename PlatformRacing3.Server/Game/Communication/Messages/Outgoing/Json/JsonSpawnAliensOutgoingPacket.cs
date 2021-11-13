using System.Text.Json.Serialization;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

internal sealed class JsonSpawnAliensOutgoingPacket : JsonPacket
{
	private protected override string InternalType => "spawnAliens";

	[JsonPropertyName("count")]
	public uint Id { get; set; }
	[JsonPropertyName("seed")]
	public int Seed { get; set; }

	public JsonSpawnAliensOutgoingPacket(uint id, int seed)
	{
		this.Id = id;
		this.Seed = seed;
	}
}