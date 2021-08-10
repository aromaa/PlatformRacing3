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
}
