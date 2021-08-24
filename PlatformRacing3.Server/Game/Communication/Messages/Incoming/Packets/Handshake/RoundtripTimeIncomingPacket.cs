using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Packets.Handshake
{
    internal readonly struct RoundtripTimeIncomingPacket
    {
        internal readonly uint LastRoundtripTime;

        internal RoundtripTimeIncomingPacket(uint lastRoundtripTime)
        {
            this.LastRoundtripTime = lastRoundtripTime;
        }
    }
}
