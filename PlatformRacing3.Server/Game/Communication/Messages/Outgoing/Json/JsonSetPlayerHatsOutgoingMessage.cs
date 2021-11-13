using System.Text.Json.Serialization;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;
using PlatformRacing3.Server.Game.Match;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

internal sealed class JsonSetPlayerHatsOutgoingMessage : JsonPacket
{
	private protected override string InternalType => "setPlayerHats";

	[JsonPropertyName("socketID")]
	public uint SocketId { get; set; }

	[JsonPropertyName("hatArray")]
	public IReadOnlyCollection<MatchPlayerHat> Hats { get; set; }

	internal JsonSetPlayerHatsOutgoingMessage(uint socketId, IReadOnlyCollection<MatchPlayerHat> hats)
	{
		this.SocketId = socketId;
		this.Hats = hats;
	}
}