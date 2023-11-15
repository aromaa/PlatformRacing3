using System.Drawing;
using System.Text.Json.Serialization;
using PlatformRacing3.Common.Customization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonSetAccountSettingsMessage : JsonPacket
{
	[JsonPropertyName("hat")]
	public required Hat Hat { get; init; }
	[JsonPropertyName("hatColor")]
	public required Color HatColor { get; init; }

	[JsonPropertyName("head")]
	public required Part Head { get; init; }
	[JsonPropertyName("headColor")]
	public required Color HeadColor { get; init; }

	[JsonPropertyName("body")]
	public required Part Body { get; init; }
	[JsonPropertyName("bodyColor")]
	public required Color BodyColor { get; init; }

	[JsonPropertyName("feet")]
	public required Part Feet { get; init; }
	[JsonPropertyName("feetColor")]
	public required Color FeetColor { get; init; }

	[JsonPropertyName("speed")]
	public required uint Speed { get; init; }
	[JsonPropertyName("accel")]
	public required uint Accel { get; init; }
	[JsonPropertyName("jump")]
	public required uint Jump { get; init; }
}