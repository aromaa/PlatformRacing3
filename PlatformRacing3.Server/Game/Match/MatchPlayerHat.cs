using System.Drawing;
using System.Text.Json.Serialization;
using PlatformRacing3.Common.Customization;

namespace PlatformRacing3.Server.Game.Match;

internal sealed class MatchPlayerHat
{
	[JsonPropertyName("id")]
	public uint Id { get; }

	[JsonPropertyName("num")]
	public Hat Hat { get; }

	[JsonPropertyName("color")]
	public Color Color { get; }

	[JsonIgnore]
	internal bool Spawned { get; }

	internal MatchPlayerHat(uint id, Hat hat, Color color, bool spawned = true)
	{
		this.Id = id;
		this.Hat = hat;
		this.Color = color;
		this.Spawned = spawned;
	}
}