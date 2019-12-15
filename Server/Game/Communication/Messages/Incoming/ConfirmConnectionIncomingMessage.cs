using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class ConfirmConnectionIncomingMessage : IMessageIncomingJson
    {
        public void Handle(ClientSession session, JsonPacket message)
        {
            if (session.UpgradeClientStatus(ClientStatus.ConnectionConfirmed))
            {
                session.SendPacket(new VersionOutgoingMessage(PlatformRacing3Server.PROTOCOL_VERSION));
                session.SendPacket(new SocketIdOutgoingMessage(session.SocketId));
            }
        }
    }
}
