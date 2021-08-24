using Net.Communication.Attributes;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Managers;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Packets.Match;
using PlatformRacing3.Server.Game.Match;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Handlers.Match
{
    [PacketManagerRegister(typeof(BytePacketManager))]
    internal class TryGetItemIncomingHandler : AbstractIncomingClientSessionPacketHandler<TryGetItemIncomingPacket>
    {
        internal override void Handle(ClientSession session, in TryGetItemIncomingPacket packet)
        {
            MultiplayerMatch match = session.MultiplayerMatchSession?.MatchPlayer?.Match;
            if (match != null)
            {
                match.TryGetItem(session, packet.X, packet.Y, packet.Side, packet.Item);
            }
        }
    }
}
