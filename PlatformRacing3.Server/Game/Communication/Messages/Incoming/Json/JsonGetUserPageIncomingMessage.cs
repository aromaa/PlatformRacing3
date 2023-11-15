using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonGetUserPageIncomingMessage : JsonPacket
{
	[JsonPropertyName("socket_id")]
	public required uint SocketId { get; init; }

	[JsonPropertyName("user_id")]
	public required uint UserId { get; init; }
}