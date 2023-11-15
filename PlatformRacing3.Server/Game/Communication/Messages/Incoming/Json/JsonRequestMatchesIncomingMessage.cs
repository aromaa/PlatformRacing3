using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonRequestMatchesIncomingMessage : JsonPacket
{
	[JsonPropertyName("num")]
	public required uint Num { get; init; }

	[JsonPropertyName("lobbyId")]
	public required uint LobbyId { get; init; }
}