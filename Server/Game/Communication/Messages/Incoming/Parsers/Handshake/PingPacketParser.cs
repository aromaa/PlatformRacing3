using Net.Communication.Attributes;
using Net.Communication.Incoming.Helpers;
using Net.Communication.Incoming.Packet;
using Net.Communication.Incoming.Packet.Parser;
using Platform_Racing_3_Server.Game.Communication.Managers;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Packets.Handshake;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Parsers.Handshake
{
    [PacketManagerRegister(typeof(BytePacketManager))]
    [PacketParserId(15u)]
    internal class PingPacketParser : IIncomingPacketParser<PingIncomingPacket>
    {
        public PingIncomingPacket Parse(ref PacketReader reader)
        {
            return new PingIncomingPacket();
        }
    }
}
