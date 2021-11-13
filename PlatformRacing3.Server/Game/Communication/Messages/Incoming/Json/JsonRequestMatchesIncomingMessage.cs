using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonRequestMatchesIncomingMessage : JsonPacket
{
	[JsonPropertyName("num")]
	public uint Num { get; set; }

	[JsonPropertyName("lobbyId")]
	public uint LobbyId { get; set; }
}