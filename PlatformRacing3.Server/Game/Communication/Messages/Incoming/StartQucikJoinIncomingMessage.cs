using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Lobby;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal sealed class StartQucikJoinIncomingMessage : IMessageIncomingJson
    {
        private readonly MatchListingManager matchListingManager;

        public StartQucikJoinIncomingMessage(MatchListingManager matchListingManager)
        {
            this.matchListingManager = matchListingManager;
        }

        public void Handle(ClientSession session, JsonPacket message)
        {
            if (!session.IsLoggedIn)
            {
                return;
            }

            this.matchListingManager.StartQuickJoin(session);
        }
    }
}
