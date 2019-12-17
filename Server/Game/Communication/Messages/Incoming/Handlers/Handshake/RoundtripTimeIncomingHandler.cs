using Net.Communication.Attributes;
using Net.Communication.Incoming.Packet;
using Net.Communication.Pipeline;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Managers;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Packets.Handshake;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Handlers.Handshake
{
    [PacketManagerRegister(typeof(BytePacketManager))]
    internal class RoundtripTimeIncomingHandler : AbstractIncomingClientSessionPacketHandler<RoundtripTimeIncomingPacket>
    {
        internal override void Handle(ClientSession session, in RoundtripTimeIncomingPacket packet)
        {
            session.LastRoundtripTime = packet.LastRoundtripTime;
        }
    }
}
