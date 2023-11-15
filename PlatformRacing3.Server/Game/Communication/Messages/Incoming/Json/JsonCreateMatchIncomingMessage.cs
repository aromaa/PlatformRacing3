using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonCreateMatchIncomingMessage : JsonPacket
{
	[JsonPropertyName("level_id")]
	public required uint LevelId { get; init; }

	[JsonPropertyName("version")]
	public required uint Version { get; init; }

	[JsonPropertyName("min_rank")]
	public required uint MinRank { get; init; }

	[JsonPropertyName("max_rank")]
	public required uint MaxRank { get; init; }

	[JsonPropertyName("max_members")]
	public required uint MaxMembers { get; init; }

	[JsonPropertyName("only_friends")]
	public required bool OnlyFriends { get; init; }
}