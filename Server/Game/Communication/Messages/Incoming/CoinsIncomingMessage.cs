using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class CoinsIncomingMessage : MessageIncomingJson<JsonCoinsIncomingMessage>
    {
        internal override void Handle(ClientSession session, JsonCoinsIncomingMessage message)
        {
            if (!session.IsLoggedIn)
            {
                return;
            }

            session.MultiplayerMatchSession?.Match.UpdateCoins(session, message.Coins);
        }
    }
}
