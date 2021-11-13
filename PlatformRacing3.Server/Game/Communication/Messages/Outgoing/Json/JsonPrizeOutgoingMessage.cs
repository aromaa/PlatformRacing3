using System.Text.Json.Serialization;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;
using PlatformRacing3.Server.Game.Match;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

internal sealed class JsonPrizeOutgoingMessage : JsonPacket
{
	private protected override string InternalType => "prize";

	[JsonPropertyName("category")]
	public string Category { get; set; }

	[JsonPropertyName("id")]
	public uint Id { get; set; }

	[JsonPropertyName("status")]
	public string Status { get; set; }

	internal JsonPrizeOutgoingMessage(MatchPrize prize, string status)
	{
		this.Category = prize.Category;
		this.Id = prize.Id;
		this.Status = status;
	}
}