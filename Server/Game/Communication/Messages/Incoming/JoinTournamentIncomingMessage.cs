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
}
