using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonSendPmIncomingMessage : JsonPacket
{
	[JsonPropertyName("name")]
	public string ReceiverUsername { get; set; }

	[JsonPropertyName("title")]
	public string Title { get; set; }

	[JsonPropertyName("message")]
	public string Message { get; set; }
}