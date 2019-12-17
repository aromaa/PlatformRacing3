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
    internal class UpdateIncomingHandler : AbstractIncomingClientSessionPacketHandler<UpdatePacketIncomingPacket>
    {
        internal override void Handle(ClientSession session, in UpdatePacketIncomingPacket packet)
        {
            MatchPlayer matchPlayer = session.MultiplayerMatchSession?.MatchPlayer;
            if (matchPlayer != null)
            {
                matchPlayer.Match.SendUpdateIfRequired(session, matchPlayer);
            }
        }
    }
}
