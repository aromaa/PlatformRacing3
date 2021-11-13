using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing;
using PlatformRacing3.Server.Game.Lobby;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming;

internal sealed class GetLevelOfTheDayIncomingMessage : IMessageIncomingJson
{
	private readonly MatchListingManager matchListingManager;

	public GetLevelOfTheDayIncomingMessage(MatchListingManager matchListingManager)
	{
		this.matchListingManager = matchListingManager;
	}

	public void Handle(ClientSession session, JsonPacket message)
	{
		MatchListing matchListing = this.matchListingManager.GetLeveOfTheDay();
		if (matchListing != null)
		{
			session.SendPacket(new ReceiveLevelOfTheDayOutgoingMessage(matchListing));
			session.LobbySession.AddMatch(matchListing);
		}
	}
}