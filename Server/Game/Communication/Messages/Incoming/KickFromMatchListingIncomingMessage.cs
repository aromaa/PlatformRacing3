using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class KickFromMatchListingIncomingMessage : MessageIncomingJson<JsonKickFromMatchListingIncomingMessage>
    {
        internal override void Handle(ClientSession session, JsonKickFromMatchListingIncomingMessage message)
        {
            if (!session.IsLoggedIn)
            {
                return;
            }

            session.LobbySession?.MatchListing?.Kick(session, message.SocketId);
        }
    }
}
