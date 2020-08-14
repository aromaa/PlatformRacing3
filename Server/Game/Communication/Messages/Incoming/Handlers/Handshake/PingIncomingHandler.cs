using Net.Communication.Attributes;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Managers;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Packets.Handshake;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Handlers.Handshake
{
    [PacketManagerRegister(typeof(BytePacketManager))]
    internal class PingIncomingHandler : AbstractIncomingClientSessionPacketHandler<PingIncomingPacket>
    {
        internal override void Handle(ClientSession session, in PingIncomingPacket packet)
        {
            session.LastPing.Restart();
            session.SendPacket(PingOutgoingMessage.Instance);
        }
    }
}
