using Net.Buffers;
using Net.Communication.Attributes;
using Net.Communication.Incoming.Parser;
using PlatformRacing3.Server.Game.Communication.Managers;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Packets.Handshake;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Parsers.Handshake
{
    [PacketManagerRegister(typeof(BytePacketManager))]
    [PacketParserId(33u)]
    internal class RoundtripTimePacketParser : IIncomingPacketParser<RoundtripTimeIncomingPacket>
    {
        public RoundtripTimeIncomingPacket Parse(ref PacketReader reader)
        {
            return new RoundtripTimeIncomingPacket(
                lastRoundtripTime: reader.ReadUInt32()
            );
        }
    }
}
