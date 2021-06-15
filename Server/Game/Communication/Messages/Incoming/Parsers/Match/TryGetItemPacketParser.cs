using Net.Communication.Attributes;
using Platform_Racing_3_Server.Game.Communication.Managers;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Packets.Match;
using System;
using System.Collections.Generic;
using System.Text;
using Net.Buffers;
using Net.Communication.Incoming.Parser;

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

                side: reader.ReadFixedUInt16String(),
                item: reader.ReadFixedUInt16String()
            );
        }
    }
}
