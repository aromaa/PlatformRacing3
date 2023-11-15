using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonLoseHatIncomingMessage : JsonPacket
{
	[JsonPropertyName("x")]
	public required double X { get; init; }
	[JsonPropertyName("y")]
	public required double Y {get; set;}

	[JsonPropertyName("vel_x")]
	public required float VelX { get; init; }
	[JsonPropertyName("vel_y")]
	public required float VelY { get; init; }
}