using System.Text.Json.Serialization;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

internal sealed class JsonTournamentStatusOutgoingMessage : JsonPacket
{
	private protected override string InternalType => "tournament_status";

	[JsonPropertyName("status")]
	public byte Status { get; set; }

	internal JsonTournamentStatusOutgoingMessage(byte status)
	{
		this.Status = status;
	}
}