using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonManageVarsIncomingMessage : JsonPacket
{
	[JsonPropertyName("user_vars")]
	public required HashSet<string> UserVars { get; init; }

	[JsonPropertyName("location")]
	public required string Location { get; init; }

	[JsonPropertyName("action")]
	public required string Action { get; init; }

	[JsonPropertyName("id")]
	public required string Id { get; init; }
}