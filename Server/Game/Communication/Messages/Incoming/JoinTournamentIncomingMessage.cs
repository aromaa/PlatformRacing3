using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing;
using Platform_Racing_3_Server.Game.Lobby;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class JoinTournamentIncomingMessage : IMessageIncomingJson
    {
        public void Handle(ClientSession session, JsonPacket message)
        {
            MatchListing matchListing = PlatformRacing3Server.MatchListingManager.GetTournament(session);
            if (matchListing != null)
            {
                session.SendPacket(new ForceMatchOutgoingMessage(matchListing));
            }
            else if (PlatformRacing3Server.MatchManager.HasOngoingTournaments)
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
