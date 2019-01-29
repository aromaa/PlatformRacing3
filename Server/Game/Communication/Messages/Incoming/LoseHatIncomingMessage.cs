using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class LoseHatIncomingMessage : MessageIncomingJson<JsonLoseHatIncomingMessage>
    {
        internal override void Handle(ClientSession session, JsonLoseHatIncomingMessage message)
        {
            if (!session.IsLoggedIn)
            {
                return;
            }

            session.MultiplayerMatchSession?.MatchPlayer?.Match.LoseHat(session, message.X, message.Y, message.VelX, message.VelY);
        }
    }
}
