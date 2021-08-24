using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class BanFromMatchListingIncomingMessage : MessageIncomingJson<JsonBanFromMatchListingIncomingMessage>
    {
        internal override void Handle(ClientSession session, JsonBanFromMatchListingIncomingMessage message)
        {
            session.LobbySession?.MatchListing?.Ban(session, message.SocketId);
        }
    }
}
