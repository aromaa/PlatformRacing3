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
    internal class RequestMatchesIncominggMessage : MessageIncomingJson<JsonRequestMatchesIncomingMessage>
    {
        internal override void Handle(ClientSession session, JsonRequestMatchesIncomingMessage message)
        {
            if (!session.IsLoggedIn)
            {
                return;
            }

            if (message.Num > 0)
            {
                List<MatchListing> matches = PlatformRacing3Server.MatchListingManager.RequestsMatches(session, message.Num);

                session.SendPacket(new MatchesOutgoingMessage(message.LobbyId, matches));
                session.LobbySession.AddMatches(matches);
            }
        }
    }
}
