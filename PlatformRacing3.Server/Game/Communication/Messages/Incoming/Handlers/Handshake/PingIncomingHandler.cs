using Net.Communication.Attributes;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Managers;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Packets.Handshake;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Handlers.Handshake;

[PacketManagerRegister(typeof(BytePacketManager))]
internal class PingIncomingHandler : AbstractIncomingClientSessionPacketHandler<PingIncomingPacket>
{
	internal override void Handle(ClientSession session, in PingIncomingPacket packet)
	{
		session.LastPing.Restart();
		session.SendPacket(PingOutgoingMessage.Instance);
	}
}