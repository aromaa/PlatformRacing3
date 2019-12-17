using Net.Communication.Attributes;
using Net.Communication.Incoming.Helpers;
using Net.Communication.Incoming.Packet;
using Net.Communication.Incoming.Packet.Parser;
using Platform_Racing_3_Server.Game.Communication.Managers;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Packets.Match;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Parsers.Match
{
    [PacketManagerRegister(typeof(BytePacketManager))]
    [PacketParserId(14u)]
    internal class TryGetItemPacketParser : IIncomingPacketParser<TryGetItemIncomingPacket>
    {
        public TryGetItemIncomingPacket Parse(ref PacketReader reader)
        {
            return new TryGetItemIncomingPacket(
                x: reader.ReadInt32(),
                y: reader.ReadInt32(),

                side: reader.ReadFixedString(),
                item: reader.ReadFixedString()
            );
        }
    }
}
