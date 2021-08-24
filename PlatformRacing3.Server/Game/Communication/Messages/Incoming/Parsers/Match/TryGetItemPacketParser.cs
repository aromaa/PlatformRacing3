using Net.Buffers;
using Net.Communication.Attributes;
using Net.Communication.Incoming.Parser;
using PlatformRacing3.Server.Game.Communication.Managers;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Packets.Match;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Parsers.Match
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
