using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

internal sealed class JsonReportPmIncomingMessage : JsonPacket
{
	[JsonPropertyName("message_id")]
	public uint MessageId { get; set; }
}