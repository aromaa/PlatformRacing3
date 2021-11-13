using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonCreateMatchIncomingMessage : JsonPacket
{
	[JsonPropertyName("level_id")]
	public uint LevelId { get; set; }

	[JsonPropertyName("version")]
	public uint Version { get; set; }

	[JsonPropertyName("min_rank")]
	public uint MinRank { get; set; }

	[JsonPropertyName("max_rank")]
	public uint MaxRank { get; set; }

	[JsonPropertyName("max_members")]
	public uint MaxMembers { get; set; }

	[JsonPropertyName("only_friends")]
	public bool OnlyFriends { get; set; }
}