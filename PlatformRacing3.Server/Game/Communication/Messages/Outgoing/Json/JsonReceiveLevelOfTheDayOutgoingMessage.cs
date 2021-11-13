using System.Text.Json.Serialization;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;
using PlatformRacing3.Server.Game.Lobby;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

internal sealed class JsonReceiveLevelOfTheDayOutgoingMessage : JsonPacket
{
	private protected override string InternalType => "receiveLOTD";

	[JsonPropertyName("lotd")]
	public IReadOnlyDictionary<string, object> Lotd { get; set; }

	internal JsonReceiveLevelOfTheDayOutgoingMessage(MatchListing matchListing)
	{
		this.Lotd = matchListing.GetVars("*");
	}
}