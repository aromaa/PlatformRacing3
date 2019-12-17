using Net.Communication.Attributes;
using Net.Communication.Incoming.Packet;
using Net.Communication.Pipeline;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Managers;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Packets.Match;
using Platform_Racing_3_Server.Game.Match;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Handlers.Match
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
