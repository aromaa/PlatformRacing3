using Net.Communication.Attributes;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Managers;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Packets.Handshake;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Handlers.Handshake
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
