using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing;
using PlatformRacing3.Server.Game.Lobby;
using PlatformRacing3.Server.Game.Match;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming;

internal sealed class JoinTournamentIncomingMessage : IMessageIncomingJson
{
	private readonly MatchListingManager matchListingManager;
	private readonly MatchManager matchManager;

	public JoinTournamentIncomingMessage(MatchListingManager matchListingManager, MatchManager matchManager)
	{
		this.matchListingManager = matchListingManager;
		this.matchManager = matchManager;
	}

	public void Handle(ClientSession session, JsonPacket message)
	{
		if (!session.IsLoggedIn)
		{
			return;
		}

		MatchListing matchListing = this.matchListingManager.GetTournament(session);
		if (matchListing != null)
		{
			session.SendPacket(new ForceMatchOutgoingMessage(matchListing));
		}
		else if (this.matchManager.HasOngoingTournaments)
		{
			session.SendMessage("Tournament is running, spectate coming \"soon\"!");
		}
		else
		{
			session.SendMessage("There are no tournaments running");
		}
	}
}