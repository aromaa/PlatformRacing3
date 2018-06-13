using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class PingIncomingMessage : IMessageIncomingBytes
    {
        public void Handle(ClientSession session, IncomingMessage message)
        {
            session.LastPing.Restart();
            session.SendPacket(PingOutgoingMessage.Instance);
        }
    }
}
