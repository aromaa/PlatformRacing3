using System.Text.Json.Serialization;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

internal sealed class JsonAlertOutgoingMessage : JsonPacket
{
	private protected override string InternalType => "alert";

	[JsonPropertyName("message")]
	public string Message { get; set; }

	internal JsonAlertOutgoingMessage(string message)
	{
		this.Message = message;
	}
}