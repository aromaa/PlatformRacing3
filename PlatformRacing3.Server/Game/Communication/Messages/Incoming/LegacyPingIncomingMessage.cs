using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class LegacyPingIncomingMessage : MessageIncomingJson<JsonLegacyPingIncomingMessage>
    {
        internal override void Handle(ClientSession session, JsonLegacyPingIncomingMessage message)
        {
            session.LastPing.Restart();
            session.SendPacket(new LegacyPingOutgoingMessage(message.Time, (ulong)PlatformRacing3Server.Uptime.TotalSeconds));
        }
    }
}
