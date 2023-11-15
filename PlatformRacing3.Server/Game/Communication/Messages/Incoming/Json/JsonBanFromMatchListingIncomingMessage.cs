using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonBanFromMatchListingIncomingMessage : JsonPacket
{
	[JsonPropertyName("socket_id")]
	public required uint SocketId { get; init; }
}