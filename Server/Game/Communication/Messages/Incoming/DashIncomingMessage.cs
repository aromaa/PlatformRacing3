using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class DashIncomingMessage : MessageIncomingJson<JsonDashIncomingMessage>
    {
        internal override void Handle(ClientSession session, JsonDashIncomingMessage message)
        {
            session.MultiplayerMatchSession?.Match?.UpdateDash(session, message.Dash);
        }
    }
}
