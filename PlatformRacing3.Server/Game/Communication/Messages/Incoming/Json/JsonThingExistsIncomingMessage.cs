using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonThingExistsIncomingMessage : JsonPacket
{
	[JsonPropertyName("thing_type")]
	public string ThingType { get; set; }

	[JsonPropertyName("thing_title")]
	public string ThingTitle { get; set; }

	[JsonPropertyName("thing_category")]
	public string ThingCategory { get; set; }
}