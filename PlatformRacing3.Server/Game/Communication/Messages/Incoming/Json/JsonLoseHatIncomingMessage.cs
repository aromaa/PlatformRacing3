using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonLoseHatIncomingMessage : JsonPacket
{
	[JsonPropertyName("x")]
	public double X { get; set; }
	[JsonPropertyName("y")]
	public double Y {get; set;}

	[JsonPropertyName("vel_x")]
	public float VelX { get; set; }
	[JsonPropertyName("vel_y")]
	public float VelY { get; set; }
}