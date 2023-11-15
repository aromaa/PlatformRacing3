using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonEditUserListIncomingMessage : JsonPacket
{
	[JsonPropertyName("user_id")]
	public required uint UserId { get; init; }

	[JsonPropertyName("list_type")]
	public required string ListType { get; init; }
        
	[JsonPropertyName("action")]
	public required string Action { get; init; }
}