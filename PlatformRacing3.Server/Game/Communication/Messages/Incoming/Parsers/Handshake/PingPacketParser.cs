using Net.Buffers;
using Net.Communication.Attributes;
using Net.Communication.Incoming.Parser;
using PlatformRacing3.Server.Game.Communication.Managers;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Packets.Handshake;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Parsers.Handshake;

[PacketManagerRegister(typeof(BytePacketManager))]
[PacketParserId(15u)]
internal class PingPacketParser : IIncomingPacketParser<PingIncomingPacket>
{
	public PingIncomingPacket Parse(ref PacketReader reader)
	{
		return new PingIncomingPacket();
	}
}