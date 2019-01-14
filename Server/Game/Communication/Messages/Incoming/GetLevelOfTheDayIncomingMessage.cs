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
    internal class GetLevelOfTheDayIncomingMessage : IMessageIncomingJson
    {
        public void Handle(ClientSession session, JsonPacket message)
        {
            MatchListing matchListing = PlatformRacing3Server.MatchListingManager.GetLeveOfTheDay();
            if (matchListing != null)
            {
                session.SendPacket(new ReceiveLevelOfTheDayOutgoingMessage(matchListing));
                session.LobbySession.AddMatch(matchListing);
            }
        }
    }
}
