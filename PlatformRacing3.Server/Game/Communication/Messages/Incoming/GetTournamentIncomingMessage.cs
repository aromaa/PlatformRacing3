using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing;
using PlatformRacing3.Server.Game.Lobby;
using PlatformRacing3.Server.Game.Match;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming
{
    internal sealed class GetTournamentIncomingMessage : IMessageIncomingJson
    {
	    private readonly MatchListingManager matchListingManager;
        private readonly MatchManager matchManager;

        public GetTournamentIncomingMessage(MatchListingManager matchListingManager, MatchManager matchManager)
        {
	        this.matchListingManager = matchListingManager;
            this.matchManager = matchManager;
        }

        public void Handle(ClientSession session, JsonPacket message)
        {
            MatchListing matchListing = this.matchListingManager.GetTournament(session);
            if (matchListing != null)
            {
                session.SendPacket(new TournamnetStatusOutgoingMessage(1));
            }
            else if (this.matchManager.HasOngoingTournaments)
            {
                session.SendPacket(new TournamnetStatusOutgoingMessage(2));
            }
            else
            {
                session.SendPacket(new TournamnetStatusOutgoingMessage(0));
            }
        }
    }
}
