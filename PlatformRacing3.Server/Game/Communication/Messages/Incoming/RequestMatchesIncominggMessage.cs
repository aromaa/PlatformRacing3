using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing;
using PlatformRacing3.Server.Game.Lobby;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming;

internal sealed class RequestMatchesIncominggMessage : MessageIncomingJson<JsonRequestMatchesIncomingMessage>
{
	private readonly MatchListingManager matchListingManager;

	public RequestMatchesIncominggMessage(MatchListingManager matchListingManager)
	{
		this.matchListingManager = matchListingManager;
	}

	internal override void Handle(ClientSession session, JsonRequestMatchesIncomingMessage message)
	{
		if (!session.IsLoggedIn)
		{
			return;
		}

		if (message.Num > 0)
		{
			List<MatchListing> matches = this.matchListingManager.RequestsMatches(session, message.Num);

			session.SendPacket(new MatchesOutgoingMessage(message.LobbyId, matches));
			session.LobbySession.AddMatches(matches);
		}
	}
}