using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class KothIncomingMessage : MessageIncomingJson<JsonKothIncomingMessage>
    {
        internal override void Handle(ClientSession session, JsonKothIncomingMessage message)
        {
            session.MultiplayerMatchSession?.MatchPlayer?.Match.KothTime(session, message.Time);
        }
    }
}
