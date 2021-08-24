using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing;
using Platform_Racing_3_Server.Game.Lobby;
using Platform_Racing_3_Server.Game.Match;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
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
