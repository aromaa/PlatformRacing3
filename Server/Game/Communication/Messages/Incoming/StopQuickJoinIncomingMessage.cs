using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class StopQuickJoinIncomingMessage : IMessageIncomingJson
    {
        public void Handle(ClientSession session, JsonPacket message)
        {
            if (session.IsLoggedIn)
            {
                return;
            }

            PlatformRacing3Server.MatchListingManager.StopQuickJoin(session);
        }
    }
}
