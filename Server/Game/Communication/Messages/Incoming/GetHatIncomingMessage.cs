using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class GetHatIncomingMessage : MessageIncomingJson<JsonGetHatIncomingMessage>
    {
        internal override void Handle(ClientSession session, JsonGetHatIncomingMessage message)
        {
            if (!session.IsLoggedIn)
            {
                return;
            }

            session.MultiplayerMatchSession?.Match.GetHat(session, message.Id);
        }
    }
}
